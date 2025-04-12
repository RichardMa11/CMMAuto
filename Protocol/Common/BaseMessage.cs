using CMMAuto.Const;

namespace CMMAuto.Protocol.Common
{
    /// <summary>
    /// 报文抽象基类
    /// </summary>
    public abstract class BaseMessage
    {
        /// <summary>
        /// 从机地址
        /// </summary>
        public byte Slave { get; set; }

        /// <summary>
        /// 功能码
        /// </summary>
        public EnumFunction Function { get; set; }

        /// <summary>
        /// 报文
        /// </summary>
        public abstract byte[] Message { get; }
    }
}