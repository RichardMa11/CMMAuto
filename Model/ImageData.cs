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

    public class Global
    {
        public static List<PlcInfo> PlcInfos = new List<PlcInfo>();
    }
}