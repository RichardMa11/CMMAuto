using ModbusDebugging.Const;
using ModbusDebugging.Protocol.Common;

namespace ModbusDebugging.Protocol.Rtu;

/// <summary>
/// RTU协议接收到报文
/// </summary>
public class RtuReceivedMessage : BaseMessage, IReceivedMessage
{
    /// <summary>
    /// 数据
    /// </summary>
    public byte DataLength { get; set; }

    /// <summary>
    /// 寄存器数据
    /// </summary>
    public List<byte[]> RegisterData { get; set; } = [];
    
    /// <summary>
    /// 报文
    /// </summary>
    public override byte[] Message { get; }

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="message">接收到的报文</param>
    /// <exception cref="Exception">收到故障信息</exception>
    public RtuReceivedMessage(byte[] message)
    {
        Message = message;
        Slave = message[0];
        Function = (EnumFunction)message[1];
        if (message[1] >= 80)
        {
            EnumFaultCode code = (EnumFaultCode)message[2];
            throw new Exception($"收到故障信息：{code.ToString()}");
        }

        switch (Function)
        {
            case EnumFunction.ReadHolding:
            case EnumFunction.ReadInput:
                DataLength = message[2];
                RegisterData = Enumerable.Range(0, DataLength / 2).Select(index => message[(3 + index * 2)..(5 + index * 2)]).ToList();
                break;
            case EnumFunction.WriteSingle:
            case EnumFunction.WriteMultiple:
            default:
                break;
        }
    }
}