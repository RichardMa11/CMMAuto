namespace CMMAuto.Const
{
    /// <summary>
    /// 功能方法
    /// </summary>
    public enum EnumFunction : byte
    {
        /// <summary>
        /// 读保持寄存器
        /// </summary>
        ReadHolding = 0X03,

        /// <summary>
        /// 读输入寄存器
        /// </summary>
        ReadInput = 0X04,

        /// <summary>
        /// 预置单个寄存器
        /// </summary>
        WriteSingle = 0X06,

        /// <summary>
        /// 预置多个寄存器
        /// </summary>
        WriteMultiple = 0X10
    }
}