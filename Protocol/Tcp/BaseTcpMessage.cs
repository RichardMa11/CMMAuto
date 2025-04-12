using System;
using System.Threading;
using CMMAuto.Protocol.Common;

namespace CMMAuto.Protocol.Tcp
{
    /// <summary>
    /// TCP协议报文抽象基类
    /// </summary>
    public abstract class BaseTcpMessage : BaseMessage
    {
        /// <summary>
        /// 交互标识
        /// </summary>
        //public ushort InteractiveFlag { get; set; } = (ushort)Random.Shared.Next(ushort.MaxValue);
        [ThreadStatic]
        private static Random _threadRandom;
        /// <summary>
        /// 交互标识
        /// </summary>
        public ushort InteractiveFlag { get; set; }
        protected BaseTcpMessage()
        {
            if (_threadRandom == null)
            {
                // 基于时间种子生成唯一性更强的随机数
                _threadRandom = new Random(Environment.TickCount ^ Thread.CurrentThread.ManagedThreadId);
            }
            InteractiveFlag = (ushort)_threadRandom.Next(ushort.MaxValue);
        }


        /// <summary>
        /// 协议标识
        /// </summary>
        public ushort ProtocolFlag => 0;

        /// <summary>
        /// 数据域长度
        /// </summary>
        public ushort DomainLength { get; set; }
    }
}