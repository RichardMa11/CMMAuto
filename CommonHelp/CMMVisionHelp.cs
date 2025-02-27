using System;
using System.Runtime.InteropServices;
using CMMAuto.Model;
using log4net;
using OpenCvSharp;

namespace CMMAuto.CommonHelp
{
    public class CMMVisionHelp
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CMMVisionHelp));

        public ImageData GetPicImageData(string sourceImagePath)
        {
            var mat = Cv2.ImRead(sourceImagePath, ImreadModes.Color);
            var imageData = new ImageData
            {
                Image = mat.Data,
                Width = mat.Width,
                Height = mat.Height,
                Channels = mat.Channels()
            };
            log.Info($"原图信息 width: {imageData.Width}, height: {imageData.Height}, channels: {imageData.Channels}");

            return imageData;
        }

        /// <summary>
        /// 确定CMM软件运行状态
        /// </summary>
        /// <param name="sourceImagePath">图像路径</param>
        /// <returns>int,1：表示运行状态，2：表示暂停状态，3：表示没有启动测量或测量完成之后；-1：表示无法打开当前目录，
        /// -2：表示运行模板图片为空；-3：表示输入图像为空或错误；-4：表示错误模板图片为空; -5: 表示其他错误；</returns>
        public int CheckCmmRunState(string sourceImagePath)
        {
            try
            {
                log.Info("确定CMM软件运行状态");
                var res = CmmVisionInterface.CMMRunOrPauseCheck(GetPicImageData(sourceImagePath));
                log.Info($"CMM软件运行状态: { res }");

                return res;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return -1;
            }
        }

        /// <summary>
        /// 确认输入制程名称的位置
        /// </summary>
        /// <param name="sourceImagePath">图像路径</param>
        /// <param name="x0">返回输入程序名称的横坐标</param>
        /// <param name="y0">返回输入程序名称的纵坐标</param>
        /// <param name="x1">返回输入程序名称右边打开横坐标</param>
        /// <param name="y1">返回输入程序名称右边打开纵坐标</param>
        /// <returns>int, 0:表示运行成功；-1：表示运行失败； -2：表示识别阈值较低；-3:表示输入图片或模板为空； </returns>

        public int GetCmmOpenFilePos(string sourceImagePath, out float x0, out float y0, out float x1, out float y1)
        {
            try
            {
                log.Info("获取CMM软件打开程序的位置");
                var res = CmmVisionInterface.CMMOpenFilePos(GetPicImageData(sourceImagePath), out x0, out y0, out x1, out y1);
                log.Info($"获取CMM软件打开程序的位置结果: { res }");
                log.Info($"输出CMM软件打开程序的位置信息: x0: { x0 }, y0: { y0 } x1: { x1 }, y1: { y1 }");

                return res;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                x0 = 0;
                y0 = 0;
                x1 = 0;
                y1 = 0;
                return -1;
            }
        }

        /// <summary>
        /// 确定【文件】位置用于打开或关闭文件
        /// </summary>
        /// <param name="sourceImagePath">图像路径</param>
        /// <param name="x">返回【文件】位置的x坐标</param>
        /// <param name="y">返回【文件】位置的y坐标</param>
        /// <returns>int, 0:表示运行成功；-1：表示运行失败；-2：表示识别阈值较低； </returns>
        public int GetCmmFilePos(string sourceImagePath, out float x, out float y)
        {
            try
            {
                log.Info("获取CMM软件文件图标的位置");
                var res = CmmVisionInterface.CMMFilePos(GetPicImageData(sourceImagePath), out x, out y);
                log.Info($"获取CMM软件文件图标的位置结果: { res }");
                log.Info($"输出CMM软件文件图标信息: x0: { x }, y0: { y }");

                return res;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                x = 0;
                y = 0; ;
                return -1;
            }
        }

        /// <summary>
        /// 确定当前制程是否正确关闭
        /// </summary>
        /// <param name="sourceImagePath">图像路径</param>
        /// <returns>int,0:表示制程正常关闭；-1：表示该制程没有关闭；</returns>
        public int CheckCmmIsClosed(string sourceImagePath)
        {
            try
            {
                log.Info("确定CMM软件是否关闭");
                var res = CmmVisionInterface.CMMFileIsClosed(GetPicImageData(sourceImagePath));
                log.Info($"CMM软件运行状态: { res }");

                return res;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return -1;
            }
        }

        private static class CmmVisionInterface
        {
            //*******************CMM API***********************//

            /// <summary>
            /// 确定CMM软件运行状态
            /// </summary>
            /// <param name="inImage">输入图像</param>
            /// <param name="score">识别分数阈值</param>
            /// <returns>int,1：表示运行状态，2：表示暂停状态，3：表示没有启动测量或测量完成之后；-1：表示无法打开当前目录，
            /// 2：表示运行模板图片为空；-3：表示输入图像为空或错误；-4：表示错误模板图片为空; -5: 表示其他错误；</returns>
            [DllImport(@"dll\CMMMiddlewareImg.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern int CMMRunOrPauseCheck(ImageData inImage, float score = 80.0f);

            /// <summary>
            /// 确认输入制程名称的位置
            /// </summary>
            /// <param name="inImage">输入图像</param>
            /// <param name="x0">返回输入程序名称的横坐标</param>
            /// <param name="y0">返回输入程序名称的纵坐标</param>
            /// <param name="x1">返回输入程序名称右边打开横坐标</param>
            /// <param name="y1">返回输入程序名称右边打开纵坐标</param>
            /// <param name="score">识别分数阈值</param>
            /// <returns>int, 0:表示运行成功；-1：表示运行失败； -2：表示识别阈值较低；-3:表示输入图片或模板为空； </returns>
            [DllImport(@"dll\CMMMiddlewareImg.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern int CMMOpenFilePos(ImageData inImage, out float x0, out float y0, out float x1, out float y1, float score = 80.0f);

            /// <summary>
            /// 确定【文件】位置用于打开或关闭文件
            /// </summary>
            /// <param name="inImage">输入图像</param>
            /// <param name="x">返回【文件】位置的x坐标</param>
            /// <param name="y">返回【文件】位置的y坐标</param>
            /// <param name="score">识别分数阈值</param>
            /// <returns>int, 0:表示运行成功；-1：表示运行失败；-2：表示识别阈值较低； </returns>
            [DllImport(@"dll\CMMMiddlewareImg.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern int CMMFilePos(ImageData inImage, out float x, out float y, float score = 80.0f);

            /// <summary>
            /// 确定当前制程是否正确关闭
            /// </summary>
            /// <param name="inImage">输入图像</param>
            /// <returns>int,0:表示制程正常关闭；-1：表示该制程没有关闭；</returns>
            [DllImport(@"dll\CMMMiddlewareImg.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern int CMMFileIsClosed(ImageData inImage);
        }

    }
}

