namespace CMMAuto.Model
{
    /// <summary>
    /// 网络连接参数
    /// </summary>
    public class NetworkConnection
    {
        /// <summary>
        /// IP地址
        /// </summary>
        public string Ip { get; }

        /// <summary>
        /// 端口号
        /// </summary>
        public ushort Port { get; }

        /// <summary>
        /// 网络连接参数构造
        /// </summary>
        /// <param name="ip">IPV4地址</param>
        /// <param name="port">端口号</param>
        public NetworkConnection(string ip, ushort port)
        {
            Ip = ip;
            Port = port;
        }
    }
}