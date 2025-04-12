using System.Collections.Generic;
using CMMAuto.Const;
using CMMAuto.Extension;

namespace CMMAuto.Protocol.Tcp
{
    /// <summary>
    /// TCP写单个寄存器报文
    /// </summary>
    public class TcpWriteSingleMessage : BaseTcpMessage
    {
        /// <summary>
        /// 寄存器地址
        /// </summary>
        public ushort Address { get; set; }

        /// <summary>
        /// 待写入的值
        /// </summary>
        public ushort Value { get; set; }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="slave">从机地址</param>
        /// <param name="address">寄存器地址</param>
        /// <param name="value">待写入的值</param>
        public TcpWriteSingleMessage(byte slave, ushort address, ushort value)
        {
            Slave = slave;
            Function = EnumFunction.WriteSingle;
            Address = address;
            Value = value;
        }

        /// <summary>
        /// 写单个寄存器报文
        /// </summary>
        public override byte[] Message
        {
            get
            {
                //List<byte> domain = [Slave, (byte)Function];
                List<byte> domain = new List<byte> { Slave, (byte)Function };
                domain.AddRange(Address.ToBytes());
                domain.AddRange(Value.ToBytes());
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
    }
}