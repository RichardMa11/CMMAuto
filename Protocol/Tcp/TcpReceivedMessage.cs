using System;
using System.Collections.Generic;
using System.Linq;
using CMMAuto.Const;
using CMMAuto.Extension;
using CMMAuto.Protocol.Common;

namespace CMMAuto.Protocol.Tcp
{
    /// <summary>
    /// TCP响应报文
    /// </summary>
    public class TcpReceivedMessage : BaseTcpMessage, IReceivedMessage
    {
        /// <summary>
        /// 数据字节数组长度
        /// </summary>
        public byte DataLength { get; set; }

        /// <summary>
        /// 寄存器数据
        /// </summary>
        //public List<byte[]> RegisterData { get; set; } = [];
        public List<byte[]> RegisterData { get; set; } = new List<byte[]>();

        /// <summary>
        /// 报文
        /// </summary>
        public override byte[] Message { get; }

        /// <summary>
        /// 解析报文
        /// </summary>
        /// <param name="message">报文字节数组</param>
        /// <exception cref="Exception">收到故障信息</exception>
        public TcpReceivedMessage(byte[] message)
        {
            Message = message;
            //InteractiveFlag = message[..2].ToUshort();
            //DomainLength = message[4..6].ToUshort();

            InteractiveFlag = message.GetRange(0, 2).ToUshort();
            DomainLength = message.GetRange(4, 2).ToUshort();
            Slave = message[6];
            Function = (EnumFunction)message[7];
            if (message[7] >= 80)
            {
                EnumFaultCode code = (EnumFaultCode)message[8];
                throw new Exception($"收到故障信息：{code.ToString()}");
            }

            switch (Function)
            {
                case EnumFunction.ReadHolding:
                case EnumFunction.ReadInput:
                    DataLength = message[8];
                    RegisterData = Enumerable.Range(0, DataLength / 2).Select(index => message.GetRange(9 + index * 2, 2)).ToList();
                    //RegisterData = Enumerable.Range(0, DataLength / 2).Select(index => message[(9 + index * 2)..(11 + index * 2)]).ToList();
                    break;
                case EnumFunction.WriteSingle:
                case EnumFunction.WriteMultiple:
                default:
                    break;
            }
        }

        // 提取字节数组范围（替代范围操作符 `..`）
        private byte[] GetRange(byte[] source, int startIndex, int length)
        {
            if (startIndex < 0 || length < 0 || startIndex + length > source.Length)
                throw new ArgumentOutOfRangeException();

            byte[] result = new byte[length];
            Array.Copy(source, startIndex, result, 0, length);
            return result;
        }
    }
}