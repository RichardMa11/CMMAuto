using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CMMAuto.Common;
using CMMAuto.CommonHelp;
using CMMAuto.Config;
using log4net;
using log4net.Appender;

namespace CMMAuto
{
    public partial class MainForm : Form
    {
        private readonly CMMVisionHelp _cmmVisionHelp = new CMMVisionHelp();
        private readonly KeyboardSimulatorHelp _simulator = new KeyboardSimulatorHelp();
        private readonly object _lock = new object();

        private static string FullFileName = "";
        private static bool IsSingle = false;
        private static bool IsCycle = false;
        private static bool IsTheSame = false;

        private static readonly ILog Log = LogManager.GetLogger(typeof(MainForm));
        private SQLiteHelper SQLiteHelpers = null;

        public MainForm()
        {
            InitializeComponent();
            InitSqliteHelps();
            InitScreenImgPath();
        }

        private void InitSqliteHelps()
        {
            var sqlBasePath = GetSqliteBasePath();
            if (!Directory.Exists(sqlBasePath))
                Directory.CreateDirectory(sqlBasePath);

            string dbAddress = Path.Combine(sqlBasePath, $"CMM-{DateTime.Now.ToString("yyyy")}.db3");
            SQLiteHelpers = new SQLiteHelper(dbAddress);
            SQLiteHelpers.Open();
        }

        private string GetSqliteBasePath()
        {
            string path;
            string project = "SQLiteData";
            try
            {
                path = Path.Combine(project);
            }
            catch (Exception ex)
            {
                Log.Error($"[Save][SQL] - check path error: {ex}");
                throw new Exception($"save sql error: {ex.Message}", ex);
            }
            Log.Info($"[Path] - get path: {path}");
            return path;
        }

        private void InitScreenImgPath()
        {
            var basePath = GetImageBasePath();
            if (!Directory.Exists(basePath))
                Directory.CreateDirectory(basePath);

            //var fullFileName = Path.Combine(basePath, $"{DateTime.Now.ToString("yyyyMMddHHmmss")}.{CommonConstant.IMAGE_SUFFIX}");
            FullFileName = Path.Combine(basePath, $"AutoScreenflash.{CommonConstant.IMAGE_SUFFIX}");
        }

        private string GetImageBasePath()
        {
            var imagePath = AppSettings.ReadSysValue(CommonConstant.APP_IMAGE_PATH);
            var project = "CMM-TEMP";
            var image = CommonConstant.APP_IMAGE;
            string path;
            try
            {
                path = Path.Combine(imagePath, project, image);
            }
            catch (Exception ex)
            {
                Log.Error($"[Save][Image] - check path error: {ex}");
                throw new Exception($"save image error: {ex.Message}", ex);
            }
            Log.Info($"[Path] - get path: {path}");
            return path;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadLogTxt();
            LoadTreeView();
            LoadMeasureData();
        }

        private void LoadLogTxt()
        {
            this.txtLog.ScrollBars = ScrollBars.Vertical;
            this.txtLog.Text = SyncLog();
            this.txtLog.Select(this.txtLog.TextLength, 0);
            this.txtLog.ScrollToCaret();
        }

        private string SyncLog()
        {
            var appender = LogManager.GetRepository().GetAppenders().FirstOrDefault(a => a is FileAppender) as FileAppender;
            var logPath = appender.File;
            if (!File.Exists(logPath))
            {
                MessageBox.Show("日志文件未找到", "错误");
                return "";
            }
            var logLength = new FileInfo(logPath).Length;
            var bakPath = logPath + ".bak";
            File.Copy(logPath, bakPath, true);

            var bufferSize = 5120 * 5120;
            var buffer = new byte[bufferSize];
            if (logLength <= bufferSize)
            {
                buffer = File.ReadAllBytes(bakPath);
            }
            else
            {
                using (FileStream stream = new FileStream(bakPath, FileMode.Open, FileAccess.Read))
                {
                    stream.Seek(bufferSize, SeekOrigin.End);
                    stream.Read(buffer, 0, bufferSize);
                }
            }
            return System.Text.Encoding.Default.GetString(buffer);
        }

        private void LoadTreeView()
        {
            trvTestPrgChoose.Nodes.Clear();
            string sql = "SELECT PrgName FROM MeaSurePrgCfg";
            DataSet dataSet = SQLiteHelpers.ExecuteDataSet(sql, null);
            if (dataSet != null)
            {
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    TreeNode node = new TreeNode(row["PrgName"].ToString()); // 使用ToString方法获取显示文本
                    trvTestPrgChoose.Nodes.Add(node); // 添加到TreeView中
                }
            }
        }

        private void LoadMeasureData()
        {
            drvCmmLog.DataSource = null;
            string sql = $@"SELECT PrgName,PrgPath,CMMState,CMMResult,CMMTime,Remark FROM MeaSureData WHERE strftime('%Y-%m-%d %H:%M:%S', CMMTime)  >= '{DateTime.Now.AddMinutes(-10).ToString("yyyy-MM-dd HH:mm:ss")}' order by CMMTime desc ";
            //string sql = $@"SELECT PrgName,PrgPath,CMMState,CMMResult,CMMTime,Remark FROM MeaSureData ";

            DataSet dataSet = SQLiteHelpers.ExecuteDataSet(sql, null);
            if (dataSet != null)
            {
                drvCmmLog.DataSource = dataSet.Tables[0];
            }
        }

        private void timerlog_Tick(object sender, EventArgs e)
        {
            LoadLogTxt();
            lock (_lock)
            {
                GetCmmState();
                LoadMeasureData();
                MeasurePrg();
            }
        }

        private void GetCmmState()
        {
            try
            {
                var imageBitmap = ScreenShotHelp.GetImage();
                imageBitmap.Save(FullFileName, ImageFormat.Jpeg);

                if (_cmmVisionHelp.CheckCmmIsClosed(FullFileName) == 0)
                    SetState(4);
                else
                {
                    switch (_cmmVisionHelp.CheckCmmRunState(FullFileName))
                    {
                        case 1:
                            SetState(1);
                            break;
                        case 2:
                            SetState(2);
                            break;
                        case 3:
                            SetState(3);
                            break;
                        default:
                            SetState(0);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error($"获取图像状态失败: {ex}");
            }
        }

        private void SetState(int stateValue)
        {
            switch (stateValue)
            {
                case 1:
                    txtOther.BackColor = System.Drawing.Color.White;
                    txtRun.BackColor = System.Drawing.Color.LimeGreen;
                    txtPause.BackColor = System.Drawing.Color.White;
                    txtExit.BackColor = System.Drawing.Color.White;
                    txtPreOrEnd.BackColor = System.Drawing.Color.White;
                    break;
                case 2:
                    txtOther.BackColor = System.Drawing.Color.White;
                    txtRun.BackColor = System.Drawing.Color.White;
                    txtPause.BackColor = System.Drawing.Color.LimeGreen;
                    txtExit.BackColor = System.Drawing.Color.White;
                    txtPreOrEnd.BackColor = System.Drawing.Color.White;
                    break;
                case 3:
                    txtOther.BackColor = System.Drawing.Color.White;
                    txtRun.BackColor = System.Drawing.Color.White;
                    txtPause.BackColor = System.Drawing.Color.White;
                    txtExit.BackColor = System.Drawing.Color.White;
                    txtPreOrEnd.BackColor = System.Drawing.Color.LimeGreen;
                    break;
                case 4:
                    txtOther.BackColor = System.Drawing.Color.White;
                    txtRun.BackColor = System.Drawing.Color.White;
                    txtPause.BackColor = System.Drawing.Color.White;
                    txtExit.BackColor = System.Drawing.Color.LimeGreen;
                    txtPreOrEnd.BackColor = System.Drawing.Color.White;
                    break;
                default:
                    txtOther.BackColor = System.Drawing.Color.LimeGreen;
                    txtRun.BackColor = System.Drawing.Color.White;
                    txtPause.BackColor = System.Drawing.Color.White;
                    txtExit.BackColor = System.Drawing.Color.White;
                    txtPreOrEnd.BackColor = System.Drawing.Color.White;
                    break;
            }
        }

        private void MeasurePrg()
        {
            Log.Info("测量任务: " + DateTime.Now); // 执行你的任务
            if (IsCycle)
                OpenTestRun();
            else
            {
                if (IsSingle)
                    OpenTestRun();
            }
        }

        private void OpenTestRun()
        {
            if (txtExit.BackColor == System.Drawing.Color.LimeGreen)
            {
                //获取文件位置
                if (_cmmVisionHelp.GetCmmFilePos(FullFileName, out float x, out float y) == 0)
                {
                    //鼠标点击
                    Log.Info($"[Auto][Run] 弹出文件窗口 - Start Button: X: {x}, Y: {y}");
                    //NativeWindowHelp.Click(Convert.ToInt32(x), Convert.ToInt32(y));
                    _simulator.SimiuCrtlO();
                    var imageBitmap = ScreenShotHelp.GetImage();
                    imageBitmap.Save(FullFileName, ImageFormat.Jpeg);

                    if (_cmmVisionHelp.GetCmmOpenFilePos(FullFileName, out float x0, out float y0, out float x1, out float y1) == 0)
                    {
                        Log.Info($"[Auto][Run] 打开量测程式 - Start Button: X: {x0}, Y: {y0}");
                        //NativeWindowHelp.Click(Convert.ToInt32(x0), Convert.ToInt32(y0));
                        // 将文本放入剪贴板
                        Clipboard.SetText(txtMeasureProgram.Text.Trim());
                        // 模拟Ctrl+V
                        //SendKeys.SendWait("^v");
                        _simulator.SimiuCrtlV();
                        Thread.Sleep(2000);
                        NativeWindowHelp.Click(Convert.ToInt32(x1), Convert.ToInt32(y1));
                        //判断打开但没有运行状态
                        Thread.Sleep(3000);
                        imageBitmap = ScreenShotHelp.GetImage();
                        imageBitmap.Save(FullFileName, ImageFormat.Jpeg);
                        if (_cmmVisionHelp.CheckCmmRunState(FullFileName) == 3)//check是否打开
                        {
                            //SendKeys.SendWait("^Q");
                            _simulator.SimiuCrtlQ();

                            Thread.Sleep(2000);
                            imageBitmap = ScreenShotHelp.GetImage();
                            imageBitmap.Save(FullFileName, ImageFormat.Jpeg);
                            if (_cmmVisionHelp.CheckCmmRunState(FullFileName) == 1 || _cmmVisionHelp.CheckCmmRunState(FullFileName) == 2)//check是否打开
                            {
                                Log.Info($"开始运行。。。");
                                //写入数据库
                                RecordMeasure("开始", 1);
                                IsTheSame = true;
                            }
                            else
                            { Log.Error("测量程式运行失败。"); }

                            //if (!IsCycle)
                            //    IsSingle = false;
                        }
                        else
                        { Log.Error("打开测量程式失败。"); }
                    }
                    else
                    {
                        //RecordMeasure("开始", 0);
                        Log.Error("获取打开测量程式位置失败。");
                    }
                }
                else
                {
                    //RecordMeasure("开始", 0);
                    Log.Error("获取文件位置失败。");
                }
            }
            else
            {
                //RecordMeasure("开始", 0);                
                if (txtRun.BackColor == System.Drawing.Color.LimeGreen || txtPause.BackColor == System.Drawing.Color.LimeGreen)
                {
                    Log.Error("运行中不能打开程式。");
                }
                else
                {
                    if (txtPreOrEnd.BackColor == System.Drawing.Color.LimeGreen)
                    {
                        if (_cmmVisionHelp.CheckCmmRunState(FullFileName) == 3)
                        {
                            Thread.Sleep(3000);
                            //simulator.SimiuCrtlQ();
                            if (IsTheSame)
                            {
                                //-----判断结束，并退出；
                                var imageBitmap = ScreenShotHelp.GetImage();
                                imageBitmap.Save(FullFileName, ImageFormat.Jpeg);

                                if (_cmmVisionHelp.GetCmmFilePos(FullFileName, out float x, out float y) == 0)
                                {
                                    NativeWindowHelp.Click(Convert.ToInt32(x), Convert.ToInt32(y));
                                    Thread.Sleep(1000);
                                    imageBitmap = ScreenShotHelp.GetImage();
                                    imageBitmap.Save(FullFileName, ImageFormat.Jpeg);
                                    if (_cmmVisionHelp.GetCmmClosedPos(FullFileName, out float x1, out float y1) == 0)
                                    {
                                        NativeWindowHelp.Click(Convert.ToInt32(x1), Convert.ToInt32(y1));

                                        Thread.Sleep(2000);
                                        imageBitmap = ScreenShotHelp.GetImage();
                                        imageBitmap.Save(FullFileName, ImageFormat.Jpeg);

                                        if (_cmmVisionHelp.CheckCmmIsClosed(FullFileName) == 0)
                                        {
                                            //是同一个就关闭
                                            Log.Info($"退出成功。。。");
                                            //写入数据库
                                            RecordMeasure("结束", 1);
                                            IsTheSame = false;

                                            if (!IsCycle)
                                                IsSingle = false;
                                        }
                                        else
                                        { Log.Error("程式退出失败。"); }
                                    }
                                    else
                                    { Log.Error("退出获取退出位置失败。"); }
                                }
                                else
                                { Log.Error("退出获取文件位置失败。"); }
                            }
                            else
                            {
                                _simulator.SimiuCrtlQ();
                                //判断是否运行成功
                                Thread.Sleep(2000);
                                var imageBitmap = ScreenShotHelp.GetImage();
                                imageBitmap.Save(FullFileName, ImageFormat.Jpeg);
                                if (_cmmVisionHelp.CheckCmmRunState(FullFileName) == 1 || _cmmVisionHelp.CheckCmmRunState(FullFileName) == 2)
                                {
                                    Log.Info($"开始运行。。。");
                                    //写入数据库
                                    RecordMeasure("开始", 1);
                                    IsTheSame = true;
                                }
                                else
                                { Log.Error("测量程式运行失败。"); }
                            }
                        }
                    }
                    else
                    {
                        Log.Error("程序没有打开或者其他问题。");
                    }
                }
            }
        }

        private async void btnOpenFile_Click(object sender, EventArgs e)
        {
            //if (DialogResult.OK == openFileDialog.ShowDialog())
            //{
            //    var filePath = openFileDialog.FileName;
            //    //_cMMVisionHelp.CheckCmmIsClosed(filePath);
            //    //_cMMVisionHelp.GetCmmFilePos(filePath, out float x, out float y);
            //    int h=_cMMVisionHelp.GetCmmOpenFilePos(filePath, out float x0, out float y0, out float x1, out float y1);
            //    //_cMMVisionHelp.CheckCmmRunState(filePath);
            //}

            if (string.IsNullOrEmpty(txtMeasureProgram.Text))
            {
                MessageBox.Show("量测程式不能为空！");
                return;
            }

            this.WindowState = FormWindowState.Minimized;
            await TestOpenPrgAsync();
        }

        public async Task TestOpenPrgAsync()
        {
            await Task.Delay(3000);

            //获取文件位置
            if (_cmmVisionHelp.GetCmmFilePos(FullFileName, out float x, out float y) == 0)
            {
                //鼠标点击
                Log.Info($"[Auto][Run] 弹出文件窗口 - Start Button: X: {x}, Y: {y}");
                //NativeWindowHelp.Click(Convert.ToInt32(x), Convert.ToInt32(y));
                _simulator.SimiuCrtlO();
                await Task.Delay(4000);

                if (_cmmVisionHelp.GetCmmOpenFilePos(FullFileName, out float x0, out float y0, out float x1, out float y1) == 0)
                {
                    Log.Info($"[Auto][Run] 打开量测程序 - Start Button: X: {x0}, Y: {y0}");
                    //NativeWindowHelp.Click(Convert.ToInt32(x0), Convert.ToInt32(y0));
                    // 将文本放入剪贴板
                    Clipboard.SetText(txtMeasureProgram.Text.Trim());
                    // 模拟Ctrl+V
                    //SendKeys.SendWait("^v");
                    _simulator.SimiuCrtlV();
                    await Task.Delay(2000);
                    NativeWindowHelp.Click(Convert.ToInt32(x1), Convert.ToInt32(y1));

                    await Task.Delay(3000);
                    //判断打开但没有运行状态  
                    if (_cmmVisionHelp.CheckCmmRunState(FullFileName) == 3)
                    {
                        //SendKeys.SendWait("^Q");
                        _simulator.SimiuCrtlQ();
                        //判断是否运行成功
                        await Task.Delay(2000);
                        var imageBitmap = ScreenShotHelp.GetImage();
                        imageBitmap.Save(FullFileName, ImageFormat.Jpeg);
                        if (_cmmVisionHelp.CheckCmmRunState(FullFileName) == 1 || _cmmVisionHelp.CheckCmmRunState(FullFileName) == 2)
                        {
                            Log.Info($"开始运行。。。");
                            //写入数据库
                            RecordMeasure("开始", 1);
                        }
                        else
                        { Log.Error("测量程式运行失败。"); }
                    }
                    else
                    { Log.Error("打开测量程式失败。"); }
                }
                else
                {
                    //RecordMeasure("开始", 0);
                    Log.Error("获取打开测量程序位置失败。");
                }
            }
            else
            {
                //RecordMeasure("开始", 0);
                Log.Error("获取文件位置失败。");
            }
        }

        private async void btnSaveImg_Click(object sender, EventArgs e)
        {
            await Test1Async();
            //GetCmmCurState();
        }

        public async Task Test1Async()
        {
            Log.Info($"11111111");
            await Task.Delay(5000); // 等待5秒
            Log.Info($"2222222");
            await Task.Delay(5000); // 等待5秒
            Log.Info($"333333");
            await Task.Delay(5000); // 等待5秒
            Log.Info($"444444444");
            await Task.Delay(5000); // 等待5秒
            Log.Info($"55555555");
            await Task.Delay(5000); // 等待5秒
            Log.Info($"6666666666");
        }

        private void GetCmmCurState()
        {
            try
            {
                var basePath = GetImageBasePath();
                if (!Directory.Exists(basePath))
                    Directory.CreateDirectory(basePath);

                //var fullFileName = Path.Combine(basePath, $"{DateTime.Now.ToString("yyyyMMddHHmmss")}.{CommonConstant.IMAGE_SUFFIX}");
                string fullFileName = Path.Combine(basePath, $"Screenflash.{CommonConstant.IMAGE_SUFFIX}");
                var imageBitmap = ScreenShotHelp.GetImage();
                imageBitmap.Save(fullFileName, ImageFormat.Jpeg);
                _cmmVisionHelp.CheckCmmIsClosed(fullFileName);
            }
            catch (Exception ex)
            {
                Log.Error($"[Save][Image] - save image error: {ex}");
            }
        }

        private void btnEnd_Click(object sender, EventArgs e)
        {
            IsCycle = false;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            txtAgvConnect.BackColor = System.Drawing.Color.LimeGreen;
            txtAgvDisconnect.BackColor = System.Drawing.Color.White;
            txtHeartBeat.BackColor = System.Drawing.Color.LimeGreen;
            lblCommInfo.Text = "AGV连接正常";
            lblCommInfo.ForeColor = System.Drawing.Color.LimeGreen;
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            txtAgvConnect.BackColor = System.Drawing.Color.White;
            txtAgvDisconnect.BackColor = System.Drawing.Color.Red;
            txtHeartBeat.BackColor = System.Drawing.Color.White;
            lblCommInfo.Text = "AGV通讯中断";
            lblCommInfo.ForeColor = System.Drawing.Color.Red;
        }

        private void btnManual_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMeasureProgram.Text))
            {
                MessageBox.Show("量测程序不能为空！");
                return;
            }

            IsCycle = false;
            IsSingle = true;
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnAutoRun_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMeasureProgram.Text))
            {
                MessageBox.Show("量测程序不能为空！");
                return;
            }

            IsCycle = true;
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnInputTestPrg_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMeasureName.Text.Trim()) || string.IsNullOrEmpty(txtMeasureProgram.Text.Trim()))
            {
                MessageBox.Show("量测程序和节点不能为空！");
                return;
            }

            // check 是否已经存在
            if (SQLiteHelpers.QueryOne("MeaSurePrgCfg", "PrgName", txtMeasureName.Text.Trim()) != null)
            {
                MessageBox.Show("该量测节点已经存在！");
                return;
            }

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("PrgName", txtMeasureName.Text.Trim());
            dic.Add("PrgPath", txtMeasureProgram.Text.Trim());
            dic.Add("CreateDate", DateTime.Now);

            int result = SQLiteHelpers.InsertData("MeaSurePrgCfg", dic);
            LoadTreeView();
            MessageBox.Show("录入成功！");
        }

        private void btnClearInfo_Click(object sender, EventArgs e)
        {
            ClearInfo();
        }

        private void ClearInfo()
        {
            txtMeasureProgram.Text = "";
            txtMeasureName.Text = "";
            LoadTreeView();
        }

        private void trvTestPrgChoose_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.ByMouse)
            {
                if (e.Node.Checked)
                {
                    trvTestPrgChoose.SelectedNode = e.Node;
                    CancelCheckedExceptOne(trvTestPrgChoose.Nodes, e.Node);

                    //赋值txt
                    SQLiteParameter[] parameter = new SQLiteParameter[] { new SQLiteParameter("PrgName", e.Node.Text) };
                    string sql = "SELECT PrgName,PrgPath FROM MeaSurePrgCfg WHERE PrgName = @PrgName";
                    DataSet dataSet = SQLiteHelpers.ExecuteDataSet(sql, parameter);
                    txtMeasureName.Text = e.Node.Text;
                    txtMeasureProgram.Text = dataSet.Tables[0].Rows[0]["PrgPath"].ToString();
                }
                else
                {
                    ClearInfo();
                }
            }
        }

        private static void CancelCheckedExceptOne(TreeNodeCollection tnc, TreeNode tn)
        {
            foreach (TreeNode item in tnc)
            {
                if (item != tn)
                    item.Checked = false;
                if (item.Nodes.Count > 0)
                    CancelCheckedExceptOne(item.Nodes, tn);
            }
        }

        private void btnCmmTestLogQuery_Click(object sender, EventArgs e)
        {
            LoadMeasureData();
        }

        private void RecordMeasure(string state, int result)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("PrgName", txtMeasureName.Text.Trim());
            dic.Add("PrgPath", txtMeasureProgram.Text.Trim());
            dic.Add("CMMState", state);
            dic.Add("CMMResult", result);
            dic.Add("CMMTime", DateTime.Now);
            dic.Add("CreateDate", DateTime.Now);

            SQLiteHelpers.InsertData("MeaSureData", dic);
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            SQLiteHelpers.Close();
        }

        private async void btnTestExit_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

            await TestExitPrgAsync();
        }

        public async Task TestExitPrgAsync()
        {
            await Task.Delay(3000);

            if (_cmmVisionHelp.CheckCmmRunState(FullFileName) == 3)
            {
                //log.Info($"结束运行。。。");
                ////写入数据库
                ////RecordMeasure("结束", 1);              
                ////-----判断结束，并退出；
                //var imageBitmap = ScreenShotHelp.GetImage();
                //imageBitmap.Save(_fullFileName, ImageFormat.Jpeg);

                //if (_cMMVisionHelp.GetCmmFilePos(_fullFileName, out float x, out float y) == 0)
                //{
                //    NativeWindowHelp.Click(Convert.ToInt32(x), Convert.ToInt32(y));
                //    Thread.Sleep(1000);
                //    imageBitmap = ScreenShotHelp.GetImage();
                //    imageBitmap.Save(_fullFileName, ImageFormat.Jpeg);
                //    if (_cMMVisionHelp.GetCmmClosedPos(_fullFileName, out float x1, out float y1) == 0)
                //    {
                //        NativeWindowHelp.Click(Convert.ToInt32(x1), Convert.ToInt32(y1));
                //        log.Error("退出成功。");
                //    }
                //    else
                //    { log.Error("退出获取退出位置失败。"); }
                //}
                //else
                //{ log.Error("退出获取文件位置失败。"); }
                //-----判断结束，并退出；
                var imageBitmap = ScreenShotHelp.GetImage();
                imageBitmap.Save(FullFileName, ImageFormat.Jpeg);

                if (_cmmVisionHelp.GetCmmFilePos(FullFileName, out float x, out float y) == 0)
                {
                    NativeWindowHelp.Click(Convert.ToInt32(x), Convert.ToInt32(y));
                    await Task.Delay(1000);
                    imageBitmap = ScreenShotHelp.GetImage();
                    imageBitmap.Save(FullFileName, ImageFormat.Jpeg);
                    if (_cmmVisionHelp.GetCmmClosedPos(FullFileName, out float x1, out float y1) == 0)
                    {
                        NativeWindowHelp.Click(Convert.ToInt32(x1), Convert.ToInt32(y1));

                        await Task.Delay(2000);
                        imageBitmap = ScreenShotHelp.GetImage();
                        imageBitmap.Save(FullFileName, ImageFormat.Jpeg);

                        if (_cmmVisionHelp.CheckCmmIsClosed(FullFileName) == 0)
                        {
                            //是同一个就关闭
                            Log.Info($"退出成功。。。");
                        }
                        else
                        { Log.Error("程式退出失败。"); }
                    }
                    else
                    { Log.Error("退出获取退出位置失败。"); }
                }
                else
                { Log.Error("退出获取文件位置失败。"); }
            }
            else
            { Log.Error("程式不是完成状态。"); }
        }

        private void BindToTreeView<T>(TreeView treeView, IEnumerable<T> dataSource)
        {
            foreach (var item in dataSource)
            {
                TreeNode node = new TreeNode(item.ToString()); // 使用ToString方法获取显示文本
                treeView.Nodes.Add(node); // 添加到TreeView中
            }
        }

        private void Crtlcv()
        {
            //// 将文本放入剪贴板
            Clipboard.SetText(txtMeasureProgram.Text.Trim());
            //// 模拟Ctrl+C
            //SendKeys.SendWait("^c");
            //// 模拟Ctrl+V
            SendKeys.SendWait("^v");

            //KeyboardSimulatorHelp simulator = new KeyboardSimulatorHelp();
            //simulator.CopyText(txtTestProgram.Text.Trim()); // 模拟复制文本到剪贴板并执行复制操作（实际上只是设置了剪贴板）
            //simulator.PasteText();
        }

        public async Task OpenTestPrgAsync() // 方法标记为async，允许使用await关键字
        {
            Log.Info("异步线程-测量执行线程已开启。");

            while (true) // 无限循环直到程序被外部方式终止（例如，用户按Ctrl+C）
            {
                Log.Info("测量任务: " + DateTime.Now); // 执行你的任务
                if (IsCycle)
                    OpenTestRun();
                else
                {
                    if (IsSingle)
                        OpenTestRun();
                }

                await Task.Delay(4000); // 暂停2秒，注意这里使用的是await，使得方法变为异步的，适合在UI或异步环境中使用。
            }
        }
    }
}
