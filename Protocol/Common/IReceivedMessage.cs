using System.Collections.Generic;

namespace CMMAuto.Protocol.Common
{
    /// <summary>
    /// 接收到的数据
    /// </summary>
    public interface IReceivedMessage
    {
        /// <summary>
        /// 寄存器数据
        /// </summary>
        List<byte[]> RegisterData { get; }
    }
}