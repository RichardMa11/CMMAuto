using ModbusDebugging.Extension;

namespace ModbusDebugging.Model;

/// <summary>
/// 寄存器值
/// </summary>
public class RegisterValue
{
    /// <summary>
    /// 寄存器十进制地址
    /// </summary>
    public ushort AddressDecimal { get; set; }

    /// <summary>
    /// 寄存器字节数组值
    /// </summary>
    public byte[] Bytes { get; set; }

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="address">寄存器地址</param>
    /// <param name="bytes">寄存器数据</param>
    public RegisterValue(ushort address, byte[] bytes)
    {
        AddressDecimal = address;
        Bytes = bytes;
    }

    /// <summary>
    /// 更新时间
    /// </summary>
    public string UpdateTime { get; } = DateTime.Now.ToString("HH:mm:ss");

    /// <summary>
    /// 寄存器十六进制地址
    /// </summary>
    public string AddressHexadecimal => AddressDecimal.ToString("X4");

    /// <summary>
    /// 寄存器十进制值
    /// </summary>
    public ushort Decimal => Bytes.ToUshort();

    /// <summary>
    /// 寄存器十六进制值
    /// </summary>
    public string Hexadecimal => Bytes.ToHex();

    /// <summary>
    /// 寄存器二进制值
    /// </summary>
    public string Binary => Bytes.ToBinary();
}