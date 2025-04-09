using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using log4net;

namespace CMMAuto.CommonHelp
{
    public class ScreenShotHelp
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ScreenShotHelp));

        private static Bitmap CropBitmap(Bitmap source, int x, int y, int width, int height)
        {
            if (width == 0 || height == 0) return null;
            Bitmap cropped = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            using (Graphics g = Graphics.FromImage(cropped))
            {
                g.DrawImage(source, new Rectangle(0, 0, width, height), new Rectangle(x, y, width, height), GraphicsUnit.Pixel);
            }
            return cropped;
        }

        public static Bitmap GetImage(string type = null)
        {
            using (Bitmap bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format24bppRgb))
            {
                Rectangle rect;
                if (type == null)
                {
                    rect = new Rectangle(0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                }
                else
                {
                    rect = UtilHelp.GetRectangle(type);
                }
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    graphics.CopyFromScreen(0, 0, 0, 0, bitmap.Size);
                    var cropped = CropBitmap(bitmap, rect.X, rect.Y, rect.Width, rect.Height);
                    //log.Info($"[Screenshot] - cur screenshot X: {rect.X}, Y: {rect.Y}, Width: {rect.Width}, Height: {rect.Height}");
                    return cropped;
                }
            }
        }

        private static void CheckPathExist(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        //internal static void SaveCursorImage(Bitmap cursorBitmap)
        //{
        //    var cursorPath = Path.Combine(AppSettings.ReadSysValue(CommonConstant.APP_IMAGE_PATH), CommonConstant.APP_SCREENSHOT);
        //    CheckPathExist(cursorPath);
        //    var cursorFileName = $"{CommonConstant.CURSOR}.{CommonConstant.IMAGE_SUFFIX}";
        //    cursorBitmap.Save(Path.Combine(cursorPath, cursorFileName));
        //}

        //internal static void SaveTagImage(Bitmap tagBitmap)
        //{
        //    var imagePath = Path.Combine(AppSettings.ReadSysValue(CommonConstant.APP_IMAGE_PATH), CommonConstant.APP_SCREENSHOT);
        //    CheckPathExist(imagePath);
        //    var imageFileName = $"{CommonConstant.TAG}.{CommonConstant.IMAGE_SUFFIX}";
        //    tagBitmap.Save(Path.Combine(imagePath, imageFileName));
        //}
    }
}
