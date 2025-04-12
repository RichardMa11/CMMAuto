namespace CMMAuto.Extension
{
    /// <summary>
    /// 通用判断扩展类
    /// </summary>
    public static class DistinguishExtension
    {
        /// <summary>
        /// 字符串是否为字节
        /// </summary>
        /// <param name="value">字符串</param>
        /// <returns>字符串是字节</returns>
        public static bool IsByte(this string value) => byte.TryParse(value, out _);

        /// <summary>
        /// 字符串是否为非零字节
        /// </summary>
        /// <param name="value">字符串</param>
        /// <returns>字符串是非零字节</returns>
        public static bool IsByteNonZero(this string value) => byte.TryParse(value, out byte number) && number != 0;

        /// <summary>
        /// 字符串是否为Ushort
        /// </summary>
        /// <param name="value">字符串</param>
        /// <returns>字符串是Ushort数字</returns>
        public static bool IsUshort(this string value) => ushort.TryParse(value, out _);

        /// <summary>
        /// 字符串是否为非零Ushort
        /// </summary>
        /// <param name="value">字符串</param>
        /// <returns>字符串是非零Ushort数字</returns>
        public static bool IsUshortNonZero(this string value) => ushort.TryParse(value, out ushort number) && number != 0;
    }
}