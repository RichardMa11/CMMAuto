namespace CMMAuto.Const
{
    /// <summary>
    /// 故障码
    /// </summary>
    public enum EnumFaultCode : byte
    {
        功能码不支持 = 0x01,
        非法数据地址 = 0x02,
        非法数据值 = 0x03,
        从站设备故障 = 0x04,
        硬件故障 = 0x05,
        设备忙 = 0x06,
        命令超时 = 0x08,
        非法确认 = 0x0A,
        不能用于写操作 = 0x0B,
        从机故障 = 0x0C
    }
}