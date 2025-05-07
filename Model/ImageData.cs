using System;
using System.Collections.Generic;

namespace CMMAuto.Model
{
    public struct ImageData
    {
        public IntPtr Image;
        public int Width;
        public int Height;
        public int Channels;
    }

    public class PlcInfo
    {
        public string PlcName { get; set; }

        public int Address { get; set; }

        public int Count { get; set; }
    }

    public class CfgInfo
    {
        public string Key { get; set; }

        public string Value { get; set; }
    }

    public class Global
    {
        public static List<PlcInfo> PlcInfos = new List<PlcInfo>();

        public static List<CfgInfo> CfgInfos = new List<CfgInfo>();
    }

    public class Request
    {
        public int Status { get; set; }
        public string Ip { get; set; }
        public ImageData ImageData { get; set; }
    }

    public class Response
    {
        public string ResultStatus { get; set; }
        public string ReceivedMessage { get; set; }
        public DateTime Timestamp { get; set; }
    }
}