using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using log4net;

namespace CMMAuto.CommonHelp
{
    public class NativeWindowHelp
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(MainForm));

        private const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const uint MOUSEEVENTF_LEFTUP = 0x0004;
        public const int WM_HOTKEY = 0x0312;
        public const int HOTKEY_ID = 9001;
        private const int LOGPIXELSX = 88;
        private const int LOGPIXELSY = 90;

        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        public enum WindowSateEnum
        {
            Maximized = 3,
            Minimized = 6,
            Nomal = 9,
        }

        [DllImport("user32.dll")]
        static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, UIntPtr dwExtraInfo);

        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll")]
        public static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public static double GetScaleFactor()
        {
            Graphics graphics = Graphics.FromHwnd(IntPtr.Zero);
            IntPtr desktop = graphics.GetHdc();
            int logicalScreenWidth = GetDeviceCaps(desktop, 8);
            int physicalScreenWidth = GetDeviceCaps(desktop, 88);  // Horizontal DPI
            int physicalScreenHeight = GetDeviceCaps(desktop, 90); // Vertical DPI            
            graphics.ReleaseHdc(desktop);

            double horizontalScaleFactor = (double)physicalScreenWidth / logicalScreenWidth;
            double verticalScaleFactor = (double)physicalScreenHeight / 96;  // Assuming default vertical DPI is 96

            return Math.Min(horizontalScaleFactor, verticalScaleFactor);
        }

        public Rectangle GetWindowRectangle(IntPtr hWnd)
        {
            RECT rect;
            GetWindowRect(hWnd, out rect);
            return new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
        }

        public Bitmap CaptureExternalWindowScreenShot(IntPtr hWnd)
        {
            Rectangle windowRectangle = GetWindowRectangle(hWnd);
            Bitmap screenshot = new Bitmap(windowRectangle.Width, windowRectangle.Height, PixelFormat.Format32bppArgb);
            using (Graphics gs = Graphics.FromHwndInternal(hWnd))
            {
                gs.CopyFromScreen(0, 0, 0, 0, screenshot.Size);
                //gs.CopyFromScreen(windowRectangle.Left, windowRectangle.Top, 0, 0, screenshot.Size, CopyPixelOperation.SourceCopy);
            }
            return screenshot;
        }

        public static void Click(int x, int y)
        {
            log.Info($"[Mouse] click on x: {x}, y: {y}");
            Cursor.Position = new System.Drawing.Point(x, y);
            mouse_event(MOUSEEVENTF_LEFTDOWN, (uint)x, (uint)y, 0, UIntPtr.Zero);
            mouse_event(MOUSEEVENTF_LEFTUP, (uint)x, (uint)y, 0, UIntPtr.Zero);
        }
    }
}
