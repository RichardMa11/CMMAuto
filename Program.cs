using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace CMMAuto
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            log4net.Config.XmlConfigurator.Configure();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new MainForm());

            bool createdNew; // 检查是否创建了新的实例变量
            using (Mutex mutex = new Mutex(true, "MainForm", out createdNew)) // 使用互斥锁确保单实例运行
            {
                if (createdNew) // 如果创建了新的实例，则继续启动窗体；否则只激活已存在的窗体实例并退出。
                {
                    Application.Run(new MainForm()); // 启动窗体实例
                }
                else
                {
                    var notifyIcon = new NotifyIcon();
                    notifyIcon.Icon = SystemIcons.Application;
                    notifyIcon.BalloonTipText = "应用程序已在运行！";
                    notifyIcon.Visible = true;
                    notifyIcon.ShowBalloonTip(1000); // 显示气球提示信息
                    notifyIcon.Dispose();
                    // 激活已存在的窗体实例（如果有的话）并退出当前实例的启动过程。
                    // 你可以通过发送通知等方式来告诉用户应用程序已经运行。 例如： MessageBox.Show("应用程序已在运行！");
                    // 也可以使用其他方法如闪烁任务栏图标等。 这里的重点是避免重复启动。 你可以根据需要选择适当的方法。
                    // 例如，使用通知图标闪烁等方法提示用户。
                    // 例如： var notifyIcon = new NotifyIcon(); notifyIcon.Icon = SystemIcons.Application;
                    // notifyIcon.Visible = true; notifyIcon.ShowBalloonTip(1000); // 显示气球提示信息 notifyIcon.Dispose(); 
                    // 使用完毕后释放资源 这种方法可以有效地避免重复启动，并给用户一个应用程序已在运行的提示。
                }
            }
        }
    }
}
