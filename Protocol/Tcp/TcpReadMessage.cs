using System.Collections.Generic;
using CMMAuto.Const;
using CMMAuto.Extension;

namespace CMMAuto.Protocol.Tcp
{
    /// <summary>
    /// TCP协议读寄存器报文
    /// </summary>
    public class TcpReadMessage : BaseTcpMessage
    {
        /// <summary>
        /// 寄存器起始地址
        /// </summary>
        public ushort Address { get; set; }

        /// <summary>
        /// 寄存器数量
        /// </summary>
        public ushort Count { get; set; }

        /// <summary>
        /// 请求报文
        /// </summary>
        public override byte[] Message
        {
            get
            {
                //List<byte> domain = [Slave, (byte)Function];
                List<byte> domain = new List<byte> { Slave, (byte)Function };
                domain.AddRange(Address.ToBytes());
                domain.AddRange(Count.ToBytes());
                DomainLength = (ushort)domain.Count;
                //List<byte> message = [];
                List<byte> message = new List<byte>();
                message.AddRange(InteractiveFlag.ToBytes());
                message.AddRange(ProtocolFlag.ToBytes());
                message.AddRange(DomainLength.ToBytes());
                message.AddRange(domain);
                return message.ToArray();
            }
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="slave">从机地址</param>
        /// <param name="function">功能码</param>
        /// <param name="address">寄存器地址</param>
        /// <param name="count">寄存器数量</param>
        public TcpReadMessage(byte slave, EnumFunction function, ushort address, ushort count)
        {
            Slave = slave;
            Function = function;
            Address = address;
            Count = count;
        }
    }
}