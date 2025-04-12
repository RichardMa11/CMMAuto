using ModbusDebugging.Const;
using ModbusDebugging.Extension;
using ModbusDebugging.Protocol.Common;

namespace ModbusDebugging.Protocol.Rtu;

/// <summary>
/// RTU协议读寄存器报文
/// </summary>
public class RtuReadMessage : BaseMessage
{
    /// <summary>
    /// 寄存器地址
    /// </summary>
    public ushort Address { get; set; }
    
    /// <summary>
    /// 寄存器数量
    /// </summary>
    public ushort Count { get; set; }

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="slave">从机地址</param>
    /// <param name="function">功能码</param>
    /// <param name="address">寄存器地址</param>
    /// <param name="count">寄存器数量</param>
    public RtuReadMessage(byte slave, EnumFunction function, ushort address, ushort count)
    {
        Slave = slave;
        Function = function;
        Address = address;
        Count = count;
    }

    /// <summary>
    /// 读寄存器报文
    /// </summary>
    public override byte[] Message
    {
        get
        {
            List<byte> message = [Slave, (byte)Function];
            message.AddRange(Address.ToBytes());
            message.AddRange(Count.ToBytes());
            message.AddRange(message.ToArray().CalculateCrc());
            return message.ToArray();
        }
    }
}