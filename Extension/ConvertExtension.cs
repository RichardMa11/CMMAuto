using System;
using System.Linq;

namespace CMMAuto.Extension
{
    /// <summary>
    /// 进制/类型转换扩展类
    /// </summary>
    public static class ConvertExtension
    {
        /// <summary>
        /// 字节数组转16进制串
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <returns>16进制串</returns>
        public static string ToHex(this byte[] bytes) => BitConverter.ToString(bytes).Replace("-", string.Empty);

        /// <summary>
        /// 16进制串转字节数组
        /// </summary>
        /// <param name="hex">16进制串</param>
        /// <returns>字节数组</returns>
        /// <exception cref="ArgumentException">只能将偶数位的16进制串转为字节数组</exception>
        public static byte[] ToBytes(this string hex) => hex.Length % 2 == 0
            ? Enumerable.Range(0, hex.Length / 2).Select(index => Convert.ToByte(hex.Substring(index * 2, 2), 16)).ToArray()
            : throw new ArgumentException("只能将偶数位的16进制串转为字节数组", nameof(hex));

        /// <summary>
        /// 大端字节数组转short数字
        /// </summary>
        /// <param name="bytes">大端字节数组</param>
        /// <returns>short数字</returns>
        /// <exception cref="ArgumentException">字节数组不足2位，无法转为short类型</exception>
        public static short ToShort(this byte[] bytes) => bytes.Length < 2
            ? throw new ArgumentException("字节数组不足2位，无法转为short类型")
            : BitConverter.ToInt16(BitConverter.IsLittleEndian ? bytes.Reverse().ToArray() : bytes, 0);

        /// <summary>
        /// short转大端字节数组
        /// </summary>
        /// <param name="value">short数字</param>
        /// <returns>大端字节数组</returns>
        public static byte[] ToBytes(this short value) => BitConverter.IsLittleEndian
            ? BitConverter.GetBytes(value).Reverse().ToArray()
            : BitConverter.GetBytes(value);

        /// <summary>
        /// 小端字节数组转short数字
        /// </summary>
        /// <param name="bytes">小端字节数组</param>
        /// <returns>short数字</returns>
        /// <exception cref="ArgumentException">字节数组不足2位，无法转为short类型</exception>
        public static short LittleEndianToShort(this byte[] bytes) => bytes.Length < 2
            ? throw new ArgumentException("字节数组不足2位，无法转为short类型")
            : BitConverter.ToInt16(BitConverter.IsLittleEndian ? bytes : bytes.Reverse().ToArray(), 0);

        /// <summary>
        /// 大端字节数组转ushort数字
        /// </summary>
        /// <param name="bytes">大端字节数组</param>
        /// <returns>ushort数字</returns>
        /// <exception cref="ArgumentException">字节数组不足2位，无法转为ushort类型</exception>
        public static ushort ToUshort(this byte[] bytes) => bytes.Length < 2
            ? throw new ArgumentException("字节数组不足2位，无法转为short类型")
            : BitConverter.ToUInt16(BitConverter.IsLittleEndian ? bytes.Reverse().ToArray() : bytes, 0);

        /// <summary>
        /// ushort转大端字节数组
        /// </summary>
        /// <param name="value">ushort数字</param>
        /// <returns>大端字节数组</returns>
        public static byte[] ToBytes(this ushort value) => BitConverter.IsLittleEndian
            ? BitConverter.GetBytes(value).Reverse().ToArray()
            : BitConverter.GetBytes(value);

        /// <summary>
        /// 小端字节数组转ushort数字
        /// </summary>
        /// <param name="bytes">小端字节数组</param>
        /// <returns>ushort数字</returns>
        /// <exception cref="ArgumentException">字节数组不足2位，无法转为ushort类型</exception>
        public static ushort LittleEndianToUshort(this byte[] bytes) => bytes.Length < 2
            ? throw new ArgumentException("字节数组不足2位，无法转为short类型")
            : BitConverter.ToUInt16(BitConverter.IsLittleEndian ? bytes : bytes.Reverse().ToArray(), 0);

        /// <summary>
        /// ushort转小端字节数组
        /// </summary>
        /// <param name="value">ushort数字</param>
        /// <returns>小端字节数组</returns>
        public static byte[] LittleEndianToBytes(this ushort value) => BitConverter.IsLittleEndian
            ? BitConverter.GetBytes(value)
            : BitConverter.GetBytes(value).Reverse().ToArray();

        /// <summary>
        /// 将字节数组转为二进制串
        /// </summary>
        /// <param name="bytes">2位字节数组</param>
        /// <returns>二进制串</returns>
        public static string ToBinary(this byte[] bytes) => string.Join("", bytes.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')));

        /// <summary>
        /// ModbusRTU的CRC校验码计算
        /// </summary>
        /// <param name="data">数据域数据</param>
        /// <returns>CRC校验码</returns>
        public static byte[] CalculateCrc(this byte[] data)
        {
            ushort crc = 0xFFFF; // 初始化CRC值
            foreach (byte b in data)
            {
                crc ^= b; // 将当前字节与CRC值进行异或

                for (int i = 0; i < 8; i++) // 对每个比特进行处理
                {
                    bool carry = (crc & 0x0001) == 0x0001; // 检查最低位
                    crc >>= 1; // 右移一位
                    if (carry) // 如果最低位是1，则与多项式进行异或  
                    {
                        crc ^= 0xA001;
                    }
                }
            }

            return crc.LittleEndianToBytes(); // 返回计算得到的CRC值
        }

        // 提取范围// 提取字节数组范围（替代范围操作符 `..`）
        public static byte[] GetRange(this byte[] source, int startIndex, int length)
        {
            if (startIndex < 0 || length < 0 || startIndex + length > source.Length)
                throw new ArgumentOutOfRangeException();

            byte[] result = new byte[length];
            Array.Copy(source, startIndex, result, 0, length);
            return result;
        }

        public static int StrToInt(this string text)
        {
            return int.TryParse(text, out var result) ? result : new int();
        }
    }
}