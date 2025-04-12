using System;
using System.Text;
using CMMAuto.Const;
using CMMAuto.Extension;

namespace CMMAuto.Model
{
    /// <summary>
    /// 报文传输
    /// </summary>
    public class MessageTransmit
    {
        /// <summary>
        /// 协议类型
        /// </summary>
        public EnumProtocol Protocol { get; set; }

        /// <summary>
        /// 传输方向
        /// </summary>
        public EnumTransmitWay TransmitWay { get; set; }

        /// <summary>
        /// 寄存器地址
        /// </summary>
        public ushort Address { get; set; }

        /// <summary>
        /// 报文数据
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="way">传输方向</param>
        /// <param name="protocol">协议类型</param>
        /// <param name="address">寄存器地址</param>
        /// <param name="data">报文数据</param>
        public MessageTransmit(EnumProtocol protocol, EnumTransmitWay way, ushort address, byte[] data)
        {
            Protocol = protocol;
            TransmitWay = way;
            Address = address;
            Data = data;
        }

        /// <summary>
        /// 将报文解析为行消息
        /// </summary>
        public string Message
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                string way = TransmitWay == EnumTransmitWay.Request ? "发送：" : "接收：";
                builder.Append($"【{DateTime.Now:HH:mm:ss}】{way}");
                builder.Append(Data.ToHex());
                return builder.ToString();
            }
        }
    }
}