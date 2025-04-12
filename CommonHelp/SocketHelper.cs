using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using CMMAuto.Const;
using CMMAuto.Model;

namespace CMMAuto.CommonHelp
{
    /// <summary>
    /// 套接字帮助类
    /// </summary>
    public class SocketHelper
    {
        #region 单例构造

        /// <summary>
        /// 单例锁
        /// </summary>
        private static readonly object Padlock = new object();

        /// <summary>
        /// 私有化单例对象
        /// </summary>
        private static SocketHelper _instance;

        /// <summary>
        /// 获取单例对象
        /// </summary>
        public static SocketHelper Instance
        {
            get
            {
                lock (Padlock)
                {
                    return _instance ?? (_instance = new SocketHelper());
                }
            }
        }

        /// <summary>
        /// 私有化构造方法
        /// </summary>
        private SocketHelper()
        {
        }

        #endregion 单例构造

        #region 属性

        /// <summary>
        /// 套接字
        /// </summary>
        private Socket _socket;

        /// <summary>
        /// 套接字状态改变处理
        /// </summary>
        public delegate void SocketStatusChangedHandler(EnumSocketStatus status);

        /// <summary>
        /// 套接字状态改变事件
        /// </summary>
        public event SocketStatusChangedHandler SocketStatusChangedEvent;

        /// <summary>
        /// 套接字接收到数据处理
        /// </summary>
        public delegate void ReceivedHandler(MessageTransmit transmit);

        /// <summary>
        /// 套接字接收到数据事件
        /// </summary>
        public event ReceivedHandler ReceivedEvent;

        /// <summary>
        /// 报文已发送处理
        /// </summary>
        public delegate void SentHandler(MessageTransmit transmit);

        /// <summary>
        /// 报文已发送事件
        /// </summary>
        public event SentHandler SentEvent;

        /// <summary>
        /// 判断套接字是否已连接
        /// </summary>
        public bool Connected
        {
            get
            {
                if (_socket is null)
                {
                    return false;
                }

                // 备份套接字阻塞状态
                bool blockingState = _socket.Blocking;
                try
                {
                    //尝试发送数据包
                    byte[] tmp = new byte[1];
                    _socket.Blocking = false;
                    _socket.Send(tmp, 0, 0);
                    return true;
                }
                catch (SocketException e)
                {
                    // 连接被阻止
                    return e.NativeErrorCode == 10035;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return false;
                }
                finally
                {
                    // 恢复套接字状态
                    _socket.Blocking = blockingState;
                }
            }
        }

        #endregion 属性

        #region 方法

        /// <summary>
        /// 套接字连接
        /// </summary>
        /// <param name="connection">网络连接参数</param>
        public async Task Connect(NetworkConnection connection)
        {
            try
            {
                // 当套接字不为空，则需要先将其回收
                _socket?.Dispose();
                // 初始化套接字：1、支持IPV4 2、支持可靠的、双向的、基于连接的字节流，不重复数据，也不保留边界 3、TCP传输控制协议
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Task connectAsync = _socket.ConnectAsync(connection.Ip, connection.Port);
                SocketStatusChangedEvent?.Invoke(EnumSocketStatus.Connecting);
                await connectAsync;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                SocketStatusChangedEvent?.Invoke(_socket != null && _socket.Connected && Connected ? EnumSocketStatus.Connected : EnumSocketStatus.Normal);
            }
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public void Disconnect()
        {
            try
            {
                _socket?.Disconnect(false);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                SocketStatusChangedEvent?.Invoke(_socket != null && _socket.Connected && Connected ? EnumSocketStatus.Connected : EnumSocketStatus.Normal);
            }
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="transmit">报文传输</param>
        public async void Send(MessageTransmit transmit)
        {
            try
            {
                if (_socket is null || Connected == false)
                {
                    return;
                }

                //await _socket.SendAsync(transmit.Data);
                await _socket.SendTaskAsync(transmit.Data, 0, transmit.Data.Length, SocketFlags.None);
                SentEvent?.Invoke(transmit);
                Receive(transmit);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                SocketStatusChangedEvent?.Invoke(_socket != null && _socket.Connected && Connected ? EnumSocketStatus.Connected : EnumSocketStatus.Normal);
            }
        }

        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="transmit">报文传输</param>
        private async void Receive(MessageTransmit transmit)
        {
            try
            {
                if (_socket is null || Connected == false)
                {
                    return;
                }

                byte[] buffer = new byte[ushort.MaxValue];
                //int seek = await _socket.ReceiveAsync(buffer, SocketFlags.None);
                int seek = await _socket.ReceiveTaskAsync(buffer, 0, buffer.Length, SocketFlags.None);
                if (seek == 0)
                {
                    return;
                }

                byte[] received = new byte[seek];
                Array.Copy(buffer, received, seek);
                MessageTransmit message = new MessageTransmit(transmit.Protocol, EnumTransmitWay.Receive, transmit.Address, received);
                ReceivedEvent?.Invoke(message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        #endregion 方法
    }


    public static class SocketExtensions
    {
        public static Task<int> SendTaskAsync(this Socket socket, byte[] buffer, int offset, int size, SocketFlags flags)
        {
            return Task.Factory.FromAsync(
                (callback, state) => socket.BeginSend(buffer, offset, size, flags, callback, state),
                asyncResult => socket.EndSend(asyncResult),
                null
            );
        }

        public static Task<int> ReceiveTaskAsync(this Socket socket, byte[] buffer, int offset, int size, SocketFlags flags)
        {
            return Task.Factory.FromAsync(
                (callback, state) => socket.BeginReceive(buffer, offset, size, flags, callback, state),
                asyncResult => socket.EndReceive(asyncResult),
                null
            );
        }
    }
}