using ModbusDebugging.Const;
using ModbusDebugging.Extension;
using ModbusDebugging.Protocol.Common;

namespace ModbusDebugging.Protocol.Rtu;

/// <summary>
/// RTU协议写多个寄存器报文
/// </summary>
public class RtuWriteMultipleMessage : BaseMessage
{
    /// <summary>
    /// 寄存器地址
    /// </summary>
    public ushort Address { get; set; }
    
    /// <summary>
    /// 寄存器个数
    /// </summary>
    public ushort Count => (ushort)Values.Length;
    
    /// <summary>
    /// 数据字节数组长度
    /// </summary>
    public byte DataLength => (byte)(Count * 2);
    
    /// <summary>
    /// 预置值
    /// </summary>
    public ushort[] Values { get; set; }

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="slave">从机地址</param>
    /// <param name="address">寄存器地址</param>
    /// <param name="values">预置值</param>
    public RtuWriteMultipleMessage(byte slave, ushort address, params ushort[] values)
    {
        Slave = slave;
        Function = EnumFunction.WriteMultiple;
        Address = address;
        Values = values;
    }

    /// <summary>
    /// 写多个寄存器报文
    /// </summary>
    public override byte[] Message
    {
        get
        {
            List<byte> message = [Slave, (byte)Function];
            message.AddRange(Address.ToBytes());
            message.AddRange(Count.ToBytes());
            message.Add(DataLength);
            foreach (ushort value in Values)
            {
                message.AddRange(value.ToBytes());
            }
            
            message.AddRange(message.ToArray().CalculateCrc());
            return message.ToArray();
        }
    }
}