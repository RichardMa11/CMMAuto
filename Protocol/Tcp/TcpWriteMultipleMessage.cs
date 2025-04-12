using System.Collections.Generic;
using CMMAuto.Const;
using CMMAuto.Extension;

namespace CMMAuto.Protocol.Tcp
{
    /// <summary>
    /// TCP协议写多个寄存器报文
    /// </summary>
    public class TcpWriteMultipleMessage : BaseTcpMessage
    {
        /// <summary>
        /// 寄存器地址
        /// </summary>
        public ushort Address { get; set; }

        /// <summary>
        /// 寄存器数量
        /// </summary>
        public ushort Count => (ushort)Values.Length;

        /// <summary>
        /// 数据字节长度
        /// </summary>
        public byte DataLength => (byte)(Count * 2);

        /// <summary>
        /// 待写入值
        /// </summary>
        public ushort[] Values { get; set; }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="slave">从机地址</param>
        /// <param name="address">寄存器地址</param>
        /// <param name="values">待写入值</param>
        public TcpWriteMultipleMessage(byte slave, ushort address, params ushort[] values)
        {
            Slave = slave;
            Function = EnumFunction.WriteMultiple;
            Address = address;
            Values = values;
        }

        /// <summary>
        /// 写入多个寄存器报文
        /// </summary>
        public override byte[] Message
        {
            get
            {
                //List<byte> domain = [Slave, (byte)Function];
                List<byte> domain = new List<byte> { Slave, (byte)Function };
                domain.AddRange(Address.ToBytes());
                domain.AddRange(Count.ToBytes());
                domain.Add(DataLength);
                foreach (ushort value in Values)
                {
                    domain.AddRange(value.ToBytes());
                }

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