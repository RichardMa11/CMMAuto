using System.Text.RegularExpressions;

namespace CMMAuto.Extension
{
    /// <summary>
    /// 正则表达式扩展类
    /// </summary>
    public static partial class RegexExtension
    {
        /// <summary>
        /// IPV4正则表达式
        /// </summary>
        /// <returns>IPV4正则</returns>
        //[GeneratedRegex(@"^((2(5[0-5]|[0-4]\d))|[0-1]?\d{1,2})(\.((2(5[0-5]|[0-4]\d))|[0-1]?\d{1,2})){3}$")]
        //public static partial Regex Ipv4Regex();

        /// <summary>
        /// 1-125正则表达式
        /// </summary>
        /// <returns>IPV4正则</returns>
        //[GeneratedRegex("^(?:[1-9]|[1-9][0-9]|1[01][0-9]|12[0-5])$")]
        //public static partial Regex RegisterCountRegex();


        #region IPv4 正则
        /// <summary>
        /// IPV4正则表达式
        /// </summary>
        /// <returns>IPV4正则</returns>
        private static readonly string _ipv4Pattern =
            @"^(?:(?:25[0-5]|2[0-4][0-9]|1[0-9]{2}|[1-9]?[0-9])\.){3}(?:25[0-5]|2[0-4][0-9]|1[0-9]{2}|[1-9]?[0-9])$";

        public static Regex Ipv4Regex => new Regex(_ipv4Pattern, RegexOptions.Compiled);
        #endregion

        #region 1-125 数值范围正则
        /// <summary>
        /// 1-125正则表达式
        /// </summary>
        /// <returns>IPV4正则</returns>
        private static readonly string _registerCountPattern =
            @"^(?:[1-9]|[1-9][0-9]|1[0-1][0-9]|12[0-5])$";

        public static Regex RegisterCountRegex => new Regex(_registerCountPattern, RegexOptions.Compiled);
        #endregion
    }
}