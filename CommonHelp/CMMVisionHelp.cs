﻿using System;
using System.Drawing;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using CMMAuto.Model;
using log4net;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace CMMAuto.CommonHelp
{
    public class CMMVisionHelp
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CMMVisionHelp));
        private readonly object _lock = new object();
        private readonly object _lockReadImg = new object();

        public ImageData GetPicImageData(string sourceImagePath)
        {
            try
            {
                ImageData imageData;
                lock (_lockReadImg)
                {
                    var mat = Cv2.ImRead(sourceImagePath, ImreadModes.Color);
                    imageData = new ImageData
                    {
                        Image = mat.Data,
                        Width = mat.Width,
                        Height = mat.Height,
                        Channels = mat.Channels()
                    };
                }
                //log.Info($"原图信息 width: {imageData.Width}, height: {imageData.Height}, channels: {imageData.Channels}");

                return imageData;
            }
            catch (Exception ex)
            {
                log.Error($"GetPicImageData出错：{ex.Message + ex.StackTrace}");
                throw;
            }
        }

        public ImageData GetPicImageData(Bitmap sourceImage)
        {
            try
            {
                var mat = new Bitmap(sourceImage).ToMat();
                //Cv2.CvtColor(mat,mat,ColorConversionCodes.BGRA2GRAY);
                Cv2.CvtColor(mat, mat, ColorConversionCodes.BGRA2BGR);
                ImageData imageData = new ImageData
                {
                    Image = mat.Data,
                    Width = mat.Width,
                    Height = mat.Height,
                    Channels = mat.Channels()
                };

                return imageData;
            }
            catch (Exception ex)
            {
                log.Error($"GetPicImageData出错：{ex.Message + ex.StackTrace}");
                throw;
            }
        }

        /// <summary>
        /// 确定CMM软件运行状态
        /// </summary>
        /// <param name="sourceImagePath">图像路径</param>
        /// <returns>int,1：表示运行状态，2：表示暂停状态，3：表示没有启动测量或测量完成之后；-1：表示无法打开当前目录，
        /// -2：表示运行模板图片为空；-3：表示输入图像为空或错误；-4：表示错误模板图片为空; -5: 表示其他错误；</returns>
        [HandleProcessCorruptedStateExceptions]
        public int CheckCmmRunState(Bitmap sourceImagePath)
        {
            try
            {
                lock (_lock)
                {
                    //log.Info("确定CMM软件运行状态");
                    var res = CmmVisionInterface.CMMRunOrPauseCheck(GetPicImageData(sourceImagePath));
                    //log.Info($"CMM软件运行状态: { res }");
                    return res;
                }
            }
            catch (AccessViolationException ex)
            {
                log.Error($"确定CMM软件运行状态出错：{ex.Message + ex.StackTrace}");
                return -1;
            }
            catch (Exception ex)
            {
                log.Error($"确定CMM软件运行状态出错：{ex.Message + ex.StackTrace}");
                return -1;
            }
        }


        [HandleProcessCorruptedStateExceptions]
        public int CheckCmmRunState(string sourceImagePath)
        {
            try
            {
                lock (_lock)
                {
                    //log.Info("确定CMM软件运行状态");
                    var res = CmmVisionInterface.CMMRunOrPauseCheck(GetPicImageData(sourceImagePath));
                    //log.Info($"CMM软件运行状态: { res }");
                    return res;
                }
            }
            catch (AccessViolationException ex)
            {
                log.Error($"确定CMM软件运行状态出错：{ex.Message + ex.StackTrace}");
                return -1;
            }
            catch (Exception ex)
            {
                log.Error($"确定CMM软件运行状态出错：{ex.Message + ex.StackTrace}");
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
        [HandleProcessCorruptedStateExceptions]
        public int GetCmmOpenFilePos(Bitmap sourceImagePath, out float x0, out float y0, out float x1, out float y1)
        {
            try
            {
                lock (_lock)
                {
                    //log.Info("获取CMM软件打开程序的位置");
                    var res = CmmVisionInterface.CMMOpenFilePos(GetPicImageData(sourceImagePath), out x0, out y0, out x1, out y1);
                    //log.Info($"获取CMM软件打开程序的位置结果: { res }");
                    //log.Info($"输出CMM软件打开程序的位置信息: x0: { x0 }, y0: { y0 } x1: { x1 }, y1: { y1 }");

                    return res;
                }
            }
            catch (AccessViolationException ex)
            {
                log.Error($"获取CMM软件【打开】位置信息出错：{ex.Message + ex.StackTrace}");
                x0 = 0;
                y0 = 0;
                x1 = 0;
                y1 = 0;
                return -1;
            }
            catch (Exception ex)
            {
                log.Error($"获取CMM软件【打开】位置信息出错：{ex.Message + ex.StackTrace}");
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
        [HandleProcessCorruptedStateExceptions]
        public int GetCmmFilePos(Bitmap sourceImagePath, out float x, out float y)
        {
            try
            {
                lock (_lock)
                {
                    //log.Info("获取CMM软件文件图标的位置");
                    var res = CmmVisionInterface.CMMFilePos(GetPicImageData(sourceImagePath), out x, out y);
                    //log.Info($"获取CMM软件文件图标的位置结果: { res }");
                    //log.Info($"输出CMM软件文件图标信息: x0: { x }, y0: { y }");

                    return res;
                }
            }
            catch (AccessViolationException ex)
            {
                log.Error($"获取CMM软件【文件】位置信息出错：{ex.Message + ex.StackTrace}");
                x = 0;
                y = 0; ;
                return -1;
            }
            catch (Exception ex)
            {
                log.Error($"获取CMM软件【文件】位置信息出错：{ex.Message + ex.StackTrace}");
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
        [HandleProcessCorruptedStateExceptions]
        public int CheckCmmIsClosed(Bitmap sourceImagePath)
        {
            try
            {
                lock (_lock)
                {
                    //log.Info("确定CMM软件是否关闭");
                    var res = CmmVisionInterface.CMMFileIsClosed(GetPicImageData(sourceImagePath));
                    //log.Info($"CMM软件运行状态: { res }");

                    return res;
                }
            }
            catch (AccessViolationException ex)
            {
                log.Error($"确定CMM软件是否关闭出错：{ex.Message + ex.StackTrace}");
                return -1;
            }
            catch (Exception ex)
            {
                log.Error($"确定CMM软件是否关闭出错：{ex.Message + ex.StackTrace}");
                return -1;
            }
        }

        /// <summary>
        /// 确定【关闭】按钮位置用于关闭当前制程，点击【文件】后使用
        /// </summary>
        /// <param name="sourceImagePath">图像路径</param>
        /// <param name="x">返回【关闭】位置的x坐标</param>
        /// <param name="y">返回【关闭】位置的y坐标</param>
        /// <returns>返回int, 0:表示运行成功；-1：表示运行失败；-2：表示识别阈值较低； </returns>
        [HandleProcessCorruptedStateExceptions]
        public int GetCmmClosedPos(Bitmap sourceImagePath, out float x, out float y)
        {
            try
            {
                lock (_lock)
                {
                    //log.Info("获取CMM软件退出图标的位置");
                    var res = CmmVisionInterface.CMMCloseBtnPos(GetPicImageData(sourceImagePath), out x, out y);
                    //log.Info($"获取CMM软件退出图标的位置结果: { res }");
                    //log.Info($"输出CMM软件退出图标信息: x0: { x }, y0: { y }");

                    return res;
                }
            }
            catch (AccessViolationException ex)
            {
                log.Error($"获取CMM软件【退出】位置信息出错：{ex.Message + ex.StackTrace}");
                x = 0;
                y = 0; ;
                return -1;
            }
            catch (Exception ex)
            {
                log.Error($"获取CMM软件【退出】位置信息出错：{ex.Message + ex.StackTrace}");
                x = 0;
                y = 0; ;
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
            public static extern int CMMRunOrPauseCheck(ImageData inImage, float score = 75.0f);

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
            public static extern int CMMOpenFilePos(ImageData inImage, out float x0, out float y0, out float x1, out float y1, float score = 75.0f);

            /// <summary>
            /// 确定【文件】位置用于打开或关闭文件
            /// </summary>
            /// <param name="inImage">输入图像</param>
            /// <param name="x">返回【文件】位置的x坐标</param>
            /// <param name="y">返回【文件】位置的y坐标</param>
            /// <param name="score">识别分数阈值</param>
            /// <returns>int, 0:表示运行成功；-1：表示运行失败；-2：表示识别阈值较低； </returns>
            [DllImport(@"dll\CMMMiddlewareImg.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern int CMMFilePos(ImageData inImage, out float x, out float y, float score = 75.0f);

            /// <summary>
            /// 确定当前制程是否正确关闭
            /// </summary>
            /// <param name="inImage">输入图像</param>
            /// <returns>int,0:表示制程正常关闭；-1：表示该制程没有关闭；</returns>
            [DllImport(@"dll\CMMMiddlewareImg.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern int CMMFileIsClosed(ImageData inImage);

            /// <summary>
            /// 确定【关闭】按钮位置用于关闭当前制程，点击【文件】后使用
            /// </summary>
            /// <param name="inImage">输入图像</param>
            /// <param name="x">返回【关闭】位置的x坐标</param>
            /// <param name="y">返回【关闭】位置的y坐标</param>
            /// <param name="score">识别分数阈值</param>
            /// <returns>返回int, 0:表示运行成功；-1：表示运行失败；-2：表示识别阈值较低；</returns>

            [DllImport(@"dll\CMMMiddlewareImg.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern int CMMCloseBtnPos(ImageData inImage, out float x, out float y, float score = 75.0f);
        }

    }
}

