using ModbusDebugging.Const;
using ModbusDebugging.Extension;
using ModbusDebugging.Protocol.Common;

namespace ModbusDebugging.Protocol.Rtu;

/// <summary>
/// RTU协议写单个寄存器报文
/// </summary>
public class RtuWriteSingleMessage : BaseMessage
{
    /// <summary>
    /// 寄存器地址
    /// </summary>
    public ushort Address { get; set; }
    
    /// <summary>
    /// 预置值
    /// </summary>
    public ushort Value { get; set; }

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="slave">从机地址</param>
    /// <param name="address">寄存器地址</param>
    /// <param name="value">预置值</param>
    public RtuWriteSingleMessage(byte slave, ushort address, ushort value)
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
            List<byte> message = [Slave, (byte)Function];
            message.AddRange(Address.ToBytes());
            message.AddRange(Value.ToBytes());
            message.AddRange(message.ToArray().CalculateCrc());
            return message.ToArray();
        }
    }
}