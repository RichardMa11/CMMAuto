﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using CMMAuto.Common;
using CMMAuto.CommonHelp;
using CMMAuto.Config;
using CMMAuto.Extension;
using CMMAuto.Model;
using EasyModbus;
using KENDLL.Common;
using log4net;
using log4net.Appender;
using Panuon.UI.Silver;

namespace CMMAuto
{
    public partial class MainForm : Form
    {
        private readonly CMMVisionHelp _cmmVisionHelp = new CMMVisionHelp();
        private readonly KeyboardSimulatorHelp _simulator = new KeyboardSimulatorHelp();
        private readonly object _lock = new object();

        //private static string _fullFileName = "";
        private static bool _isStart = false;
        private static bool _isCycle = false;
        private static bool _isTheSame = false;
        private static bool _isStop = false;
        private static double _refreshTime = 2.0;
        private static string _ip = "127.0.0.1";
        private static ApiClient _apiClient;
        private static int _status = 0;
        //private static string _httpUrl = "http://localhost:8200/autolink";

        private static readonly ILog Log = LogManager.GetLogger(typeof(MainForm));
        private SQLiteHelper _sqLiteHelpers = null;
        private FrmConfig _frmConfig;
        private FrmQueryPlc _frmQueryPlc;
        private FrmDicConfig _frmDicConfig;
        private ModbusUitl _modbusUitl;

        private delegate void LogTxtDelegate();
        //private delegate void SyncLogDelegate();
        private delegate void MeasureDelegate();

        private delegate void GetCmmStateDelegate();
        private delegate void SetStateDelegate();
        private delegate void MeasurePrgDelegate();
        private delegate void OpenTestRunDelegate();

        public MainForm()
        {
            InitializeComponent();
            InitSqliteHelps();
            //InitScreenImgPath();
            InitApiClient();
        }

        private void InitSqliteHelps()
        {
            var sqlBasePath = GetSqliteBasePath();
            if (!Directory.Exists(sqlBasePath))
                Directory.CreateDirectory(sqlBasePath);

            string dbAddress = Path.Combine(sqlBasePath, $"CMM-BaseData.db3");
            _sqLiteHelpers = new SQLiteHelper(dbAddress);
            _sqLiteHelpers.Open();
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
            //_fullFileName = Path.Combine(basePath, $"AutoScreenflash.png");
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

        private void InitApiClient()
        {
            _ip = UtilHelp.GetLocalIPv4Addresses().FirstOrDefault();

            //SQLiteParameter[] parameter = new SQLiteParameter[] { new SQLiteParameter("Key", "Http") };
            //string sql = "SELECT * FROM Cfg WHERE Key=@Key";
            //DataSet dataSet = _sqLiteHelpers.ExecuteDataSet(sql, parameter);
            //if (dataSet.Tables[0].Rows.Count != 0)
            //    _httpUrl = dataSet.Tables[0].Rows[0]["Value"].ToString();

            DataSet dataSet = _sqLiteHelpers.ExecuteDataSet("SELECT * FROM Cfg", null);
            if (dataSet != null)
            {
                foreach (DataRow r in dataSet.Tables[0].Rows)
                {
                    Global.CfgInfos.Add(new CfgInfo
                    {
                        Key = r["Key"].ToString(),
                        Value = r["Value"].ToString()
                    });
                }
            }

            _apiClient = new ApiClient(Global.CfgInfos.Count(p => p.Key == "Http") != 0 ? Global.CfgInfos.First(p => p.Key == "Http").Value : "http://localhost:8200/autolink")
            {
                LogRequestResponse = msg =>
                {
                    Log.Info($"[API] {DateTime.Now:HH:mm:ss} {msg}");
                    //File.AppendAllText("api.log", $"{msg}\n\n");
                }
            };
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //LoadLogTxt();
            LoadTreeView();
            //LoadMeasureData();
            LoadPlcTxt();

            PoolUi();
            PoolMeasure();
            PoolGetPlc();
            PoolSetPlc();
            PoolPostUrl();
        }

        private void LoadTreeView()
        {
            trvTestPrgChoose.Nodes.Clear();
            string sql = "SELECT PrgName FROM MeaSurePrgCfg";
            DataSet dataSet = _sqLiteHelpers.ExecuteDataSet(sql, null);
            if (dataSet != null)
            {
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    TreeNode node = new TreeNode(row["PrgName"].ToString()); // 使用ToString方法获取显示文本
                    trvTestPrgChoose.Nodes.Add(node); // 添加到TreeView中
                }
            }
        }

        private void LoadPlcTxt()
        {
            //SQLiteParameter[] parameter = new SQLiteParameter[] { new SQLiteParameter("Key", "PLCIp") };
            //string sql = "SELECT * FROM Cfg WHERE Key=@Key";
            //DataSet dataSet = _sqLiteHelpers.ExecuteDataSet(sql, parameter);
            ////var ip = dataSet.Tables[0].Rows.Count == 0 ? txtIp.Text.Trim() : dataSet.Tables[0].Rows[0]["Value"].ToString();
            //if (dataSet.Tables[0].Rows.Count != 0)
            //    txtIp.Text = dataSet.Tables[0].Rows[0]["Value"].ToString();

            //parameter = new SQLiteParameter[] { new SQLiteParameter("Key", "PLCPort") };
            //dataSet = _sqLiteHelpers.ExecuteDataSet(sql, parameter);
            ////var port = dataSet.Tables[0].Rows.Count == 0 ? txtPort.Text.Trim() : dataSet.Tables[0].Rows[0]["Value"].ToString();
            //if (dataSet.Tables[0].Rows.Count != 0)
            //    txtPort.Text = dataSet.Tables[0].Rows[0]["Value"].ToString();

            //parameter = new SQLiteParameter[] { new SQLiteParameter("Key", "RefreshTime") };
            //dataSet = _sqLiteHelpers.ExecuteDataSet(sql, parameter);
            //if (dataSet.Tables[0].Rows.Count != 0)
            //    _refreshTime = Convert.ToDouble(dataSet.Tables[0].Rows[0]["Value"].ToString());

            if (Global.CfgInfos.Count(p => p.Key == "PLCIp") != 0)
                txtIp.Text = Global.CfgInfos.First(p => p.Key == "PLCIp").Value;

            if (Global.CfgInfos.Count(p => p.Key == "PLCPort") != 0)
                txtPort.Text = Global.CfgInfos.First(p => p.Key == "PLCPort").Value;

            if (Global.CfgInfos.Count(p => p.Key == "RefreshTime") != 0)
                _refreshTime = Convert.ToDouble(Global.CfgInfos.First(p => p.Key == "RefreshTime").Value);

            DataSet dataSet = _sqLiteHelpers.ExecuteDataSet("SELECT * FROM PLCCfg", null);
            if (dataSet != null)
            {
                foreach (DataRow r in dataSet.Tables[0].Rows)
                {
                    Global.PlcInfos.Add(new PlcInfo
                    {
                        PlcName = r["Name"].ToString(),
                        Address = r["Address"].ToString().StrToInt(),
                        Count = r["Count"].ToString().StrToInt()
                    });
                }
            }

            ConnPlc();
        }

        public void ConnPlc()
        {
            try
            {
                if (string.IsNullOrEmpty(txtIp.Text.Trim()))
                {
                    MessageBoxX.Show("PLC 的IP地址不能为空！", "提示");
                    return;
                }

                if (string.IsNullOrEmpty(txtPort.Text.Trim()))
                {
                    MessageBoxX.Show("PLC 的端口号不能为空！", "提示");
                    return;
                }

                _modbusUitl = ModbusUitl.getInstanceConn(txtIp.Text.Trim(), txtPort.Text.Trim().StrToInt());

                if (MessageBoxX.Show($"连接PLC成功", "提示") == MessageBoxResult.OK)
                    btnSavePLC_Click(null, null);

            }
            catch (Exception ex)
            {
                MessageBoxX.Show($"连接PLC失败: {ex.Message}", "提示");
                Log.Error($"连接PLC失败: {ex.Message}");
            }
        }

        private void PoolUi()
        {
            var pollingService = new PollingService(
                pollingInterval: TimeSpan.FromSeconds(1),
                checkAction: async () =>
                {
                    await Task.Run(LoadLogTxt);
                    await Task.Run(LoadMeasureData);
                    await Task.Run(GetCmmState);

                    return true; // 始终继续轮询
                }
            );

            // 订阅错误事件
            pollingService.OnError += ex =>
                Log.Error($"PoolUi error: {ex.Message}");

            // 启动轮询
            pollingService.Start();
        }

        private void LoadLogTxt()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new LogTxtDelegate(LoadLogTxt));
                return;
            }
            // 具体操作代码
            this.txtLog.ScrollBars = ScrollBars.Vertical;
            this.txtLog.Text = SyncLog();
            this.txtLog.Select(this.txtLog.TextLength, 0);
            this.txtLog.ScrollToCaret();
        }

        private string SyncLog()
        {
            var appender = LogManager.GetRepository().GetAppenders().FirstOrDefault(a => a is FileAppender) as FileAppender;
            var logPath = appender?.File;
            if (File.Exists(logPath)) return ReadFileTail(logPath, encoding: Encoding.GetEncoding("GBK"));
            //MessageBoxX.Show("日志文件未找到！", "提示");
            Log.Error("日志文件未找到！");
            return "";

            //var logLength = new FileInfo(logPath).Length;
            //var bakPath = logPath + ".bak";
            //File.Copy(logPath, bakPath, true);

            //var bufferSize = 5120 * 5120;
            //var buffer = new byte[bufferSize];
            //if (logLength <= bufferSize)
            //{
            //    buffer = File.ReadAllBytes(bakPath);
            //}
            //else
            //{
            //    using (FileStream stream = new FileStream(bakPath, FileMode.Open, FileAccess.Read))
            //    {
            //        stream.Seek(bufferSize, SeekOrigin.End);
            //        stream.Read(buffer, 0, bufferSize);
            //    }
            //}
            //return System.Text.Encoding.Default.GetString(buffer);
        }

        private void LoadMeasureData()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MeasureDelegate(LoadMeasureData));
                return;
            }
            //Log.Info("UI " + DateTime.Now.ToString("O")); // 执行你的任务
            drvCmmLog.DataSource = null;
            //string sql = $@"SELECT PieceId,Type,PrgName,PrgPath,CMMState,CMMResult,CMMTime,Remark FROM MeaSureData WHERE strftime('%Y-%m-%d %H:%M:%S', CMMTime)  >= '{DateTime.Now.AddMinutes(-10).ToString("yyyy-MM-dd HH:mm:ss")}' order by CMMTime desc ";
            //string sql = $@"SELECT PieceId,Type,PrgName,PrgPath,CMMState,CMMResult,CMMTime,Remark FROM MeaSureData ";
            //string sql = $@"SELECT * FROM MeaSurePrgCfg ";
            string sql = $@"SELECT PieceId AS '工件码',Type as '类型码' ,PrgName as '程式节点',PrgPath as '程式路径',CMMState as '运行状态',
  CASE 
       WHEN CMMResult = 0 THEN '失败' 
       WHEN CMMResult = 1 THEN '成功' 
       ELSE 'Unknown' 
   END as '结果',strftime('%Y-%m-%d %H:%M:%S', CMMTime) as '运行时间',Remark as '备注' FROM MeaSureData WHERE strftime('%Y-%m-%d %H:%M:%S', CMMTime)  >= '{DateTime.Now.AddMinutes(-(_refreshTime * 60)).ToString("yyyy-MM-dd HH:mm:ss")}' order by CMMTime desc ";


            DataSet dataSet = _sqLiteHelpers.ExecuteDataSet(sql, null);
            if (dataSet != null)
            {
                drvCmmLog.DataSource = dataSet.Tables[0];
            }
        }

        private async void GetCmmState()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new GetCmmStateDelegate(GetCmmState));
                return;
            }
            try
            {
                if (chkIsStatusCheck.Checked)
                {
                    //var imageBitmap = ScreenShotHelp.GetImage();
                    //imageBitmap.Save(_fullFileName, ImageFormat.Png);

                    if (_cmmVisionHelp.CheckCmmIsClosed(ScreenShotHelp.GetImage()) == 0)
                        SetState(4);
                    else
                    {
                        switch (_cmmVisionHelp.CheckCmmRunState(ScreenShotHelp.GetImage()))
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
            }
            catch (Exception ex)
            {
                Log.Error($"获取图像状态失败: {ex}");
            }
            // 具体操作代码
            // 在后台线程执行耗时操作
            await Task.Run(() =>
            {
                //Thread.Sleep(3000);  // 在后台线程阻塞（不影响 UI）
                //try
                //{
                //    var imageBitmap = ScreenShotHelp.GetImage();
                //    imageBitmap.Save(_fullFileName, ImageFormat.Jpeg);

                //    if (_cmmVisionHelp.CheckCmmIsClosed(_fullFileName) == 0)
                //        SetState(4);
                //    else
                //    {
                //        switch (_cmmVisionHelp.CheckCmmRunState(_fullFileName))
                //        {
                //            case 1:
                //                SetState(1);
                //                break;
                //            case 2:
                //                SetState(2);
                //                break;
                //            case 3:
                //                SetState(3);
                //                break;
                //            default:
                //                SetState(0);
                //                break;
                //        }
                //    }
                //}
                //catch (Exception ex)
                //{
                //    Log.Error($"获取图像状态失败: {ex}");
                //}
            });
        }

        private void SetState(int stateValue)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new SetStateDelegate(() => SetState(stateValue)));
                return;
            }
            // 具体操作代码
            switch (stateValue)
            {
                case 1:
                    txtOther.BackColor = System.Drawing.Color.White;
                    txtRun.BackColor = System.Drawing.Color.LimeGreen;
                    txtPause.BackColor = System.Drawing.Color.White;
                    txtExit.BackColor = System.Drawing.Color.White;
                    txtPreOrEnd.BackColor = System.Drawing.Color.White;
                    _status = 1;
                    break;
                case 2:
                    txtOther.BackColor = System.Drawing.Color.White;
                    txtRun.BackColor = System.Drawing.Color.White;
                    txtPause.BackColor = System.Drawing.Color.LimeGreen;
                    txtExit.BackColor = System.Drawing.Color.White;
                    txtPreOrEnd.BackColor = System.Drawing.Color.White;
                    _status = 2;
                    break;
                case 3:
                    txtOther.BackColor = System.Drawing.Color.White;
                    txtRun.BackColor = System.Drawing.Color.White;
                    txtPause.BackColor = System.Drawing.Color.White;
                    txtExit.BackColor = System.Drawing.Color.White;
                    txtPreOrEnd.BackColor = System.Drawing.Color.LimeGreen;
                    _status = 3;
                    break;
                case 4:
                    txtOther.BackColor = System.Drawing.Color.White;
                    txtRun.BackColor = System.Drawing.Color.White;
                    txtPause.BackColor = System.Drawing.Color.White;
                    txtExit.BackColor = System.Drawing.Color.LimeGreen;
                    txtPreOrEnd.BackColor = System.Drawing.Color.White;
                    _status = 4;
                    break;
                default:
                    txtOther.BackColor = System.Drawing.Color.LimeGreen;
                    txtRun.BackColor = System.Drawing.Color.White;
                    txtPause.BackColor = System.Drawing.Color.White;
                    txtExit.BackColor = System.Drawing.Color.White;
                    txtPreOrEnd.BackColor = System.Drawing.Color.White;
                    _status = 0;
                    break;
            }
        }

        private void PoolMeasure()
        {
            var pollingService = new PollingService(
                pollingInterval: TimeSpan.FromSeconds(15),
                checkAction: async () =>
                {
                    //await Task.Run(GetCmmState);
                    await Task.Run(MeasurePrg);
                    //lock (_lock)
                    //{
                    //    this.BeginInvoke(new Action(GetCmmState));
                    //    this.BeginInvoke(new Action(MeasurePrg));
                    //}

                    return true; // 始终继续轮询
                }
            );

            // 订阅错误事件
            pollingService.OnError += ex =>
                Log.Error($"PoolMeasure error: {ex.Message}");

            // 启动轮询
            pollingService.Start();
        }

        private void MeasurePrg()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MeasurePrgDelegate(MeasurePrg));
                return;
            }
            // 具体操作代码
            Log.Info("测量任务: " + DateTime.Now); // 执行你的任务
            if (_isCycle)
                OpenTestRun();
            else
            {
                if (!_isStart) return;
                //SetId();
                //SetType();
                SetPlc(ModbusClient.ConvertStringToRegisters(txtWorkPiece.Text.Trim()), "CMM_AckPartID");
                SetPlc(ModbusClient.ConvertStringToRegisters(txtType.Text.Trim()), "CMM_AckParType");
                OpenTestRun();
            }
        }

        private async void OpenTestRun()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new OpenTestRunDelegate(OpenTestRun));
                return;
            }

            //#if DEBUG
            //            await Task.Delay(3000);
            //            SetPlc(new int[] { 1 }, "CMM_MeasureCompleted");
            //            await Task.Delay(3000);
            //            SetPlc(new int[] { 1 }, "CMM_Alarm");
            //#endif

            //具体操作
            if (txtExit.BackColor == System.Drawing.Color.LimeGreen)
            {
                //获取文件位置
                if (_cmmVisionHelp.GetCmmFilePos(ScreenShotHelp.GetImage(), out float x, out float y) == 0)
                {
                    //鼠标点击
                    Log.Info($"[Auto][Run] 弹出文件窗口 - Start Button: X: {x}, Y: {y}");
                    //NativeWindowHelp.Click(Convert.ToInt32(x), Convert.ToInt32(y));
                    _simulator.SimiuCrtlO();
                    await Task.Delay(2000);
                    //var imageBitmap = ScreenShotHelp.GetImage();
                    //imageBitmap.Save(_fullFileName, ImageFormat.Png);

                    if (_cmmVisionHelp.GetCmmOpenFilePos(ScreenShotHelp.GetImage(), out float x0, out float y0, out float x1, out float y1) == 0)
                    {
                        Log.Info($"[Auto][Run] 打开量测程式 - Start Button: X: {x0}, Y: {y0}");
                        //NativeWindowHelp.Click(Convert.ToInt32(x0), Convert.ToInt32(y0));
                        // 将文本放入剪贴板
                        Clipboard.SetText(txtMeasureProgram.Text.Trim());
                        // 模拟Ctrl+V
                        //SendKeys.SendWait("^v");
                        _simulator.SimiuCrtlV();
                        await Task.Delay(2000);
                        NativeWindowHelp.Click(Convert.ToInt32(x1), Convert.ToInt32(y1));
                        //判断打开但没有运行状态
                        await Task.Delay(3000);
                        //imageBitmap = ScreenShotHelp.GetImage();
                        //imageBitmap.Save(_fullFileName, ImageFormat.Png);
                        if (_cmmVisionHelp.CheckCmmRunState(ScreenShotHelp.GetImage()) == 3)//check是否打开
                        {
                            //SendKeys.SendWait("^Q");
                            _simulator.SimiuCrtlQ();

                            await Task.Delay(2000);
                            //imageBitmap = ScreenShotHelp.GetImage();
                            //imageBitmap.Save(_fullFileName, ImageFormat.Png);
                            if (_cmmVisionHelp.CheckCmmRunState(ScreenShotHelp.GetImage()) == 1 || _cmmVisionHelp.CheckCmmRunState(ScreenShotHelp.GetImage()) == 2)//check是否打开
                            {
                                Log.Info($"开始运行1。。。");
                                //写入数据库
                                RecordMeasure("开始", 1);
                                _isTheSame = true;
                            }
                            else
                            {
                                Log.Error("测量程式运行失败1。");
                                //SetError(1);
                                SetPlc(new int[] { 1 }, "CMM_Alarm");
                            }

                            //if (!IsCycle)
                            //    IsSingle = false;
                        }
                        else
                        { Log.Error("打开测量程式失败。"); SetPlc(new int[] { 1 }, "CMM_Alarm"); }
                    }
                    else
                    {
                        //RecordMeasure("开始", 0);
                        Log.Error("获取【打开测量程式】位置失败。"); SetPlc(new int[] { 1 }, "CMM_Alarm");
                    }
                }
                else
                {
                    //RecordMeasure("开始", 0);
                    Log.Error("获取【文件】位置失败。"); SetPlc(new int[] { 1 }, "CMM_Alarm");
                }
            }
            else
            {
                //RecordMeasure("开始", 0);                
                if (txtRun.BackColor == System.Drawing.Color.LimeGreen || txtPause.BackColor == System.Drawing.Color.LimeGreen)
                {
                    Log.Error("运行中不能打开程式。");
                    //SetPlc(new int[] { 1 }, "CMM_Alarm");
                }
                else
                {
                    if (txtPreOrEnd.BackColor == System.Drawing.Color.LimeGreen)
                    {
                        if (_cmmVisionHelp.CheckCmmRunState(ScreenShotHelp.GetImage()) == 3)
                        {
                            await Task.Delay(3000);
                            //simulator.SimiuCrtlQ();
                            if (_isTheSame)
                            {
                                //-----判断结束，并退出；
                                //var imageBitmap = ScreenShotHelp.GetImage();
                                //imageBitmap.Save(_fullFileName, ImageFormat.Png);

                                if (_cmmVisionHelp.GetCmmFilePos(ScreenShotHelp.GetImage(), out float x, out float y) == 0)
                                {
                                    NativeWindowHelp.Click(Convert.ToInt32(x), Convert.ToInt32(y));
                                    await Task.Delay(1000);
                                    //imageBitmap = ScreenShotHelp.GetImage();
                                    //imageBitmap.Save(_fullFileName, ImageFormat.Png);
                                    if (_cmmVisionHelp.GetCmmClosedPos(ScreenShotHelp.GetImage(), out float x1, out float y1) == 0)
                                    {
                                        NativeWindowHelp.Click(Convert.ToInt32(x1), Convert.ToInt32(y1));

                                        await Task.Delay(2000);
                                        //imageBitmap = ScreenShotHelp.GetImage();
                                        //imageBitmap.Save(_fullFileName, ImageFormat.Png);

                                        if (_cmmVisionHelp.CheckCmmIsClosed(ScreenShotHelp.GetImage()) == 0)
                                        {
                                            //是同一个就关闭
                                            Log.Info($"退出成功。。。");
                                            //写入数据库
                                            RecordMeasure("结束", 1);
                                            //SetEnd(1);
                                            SetPlc(new int[] { 1 }, "CMM_MeasureCompleted");
                                            _isTheSame = false;

                                            if (!_isCycle)
                                                _isStart = false;
                                        }
                                        else
                                        { Log.Error("程式退出失败。"); SetPlc(new int[] { 1 }, "CMM_Alarm"); }
                                    }
                                    else
                                    { Log.Error("退出获取【退出】位置失败。"); SetPlc(new int[] { 1 }, "CMM_Alarm"); }
                                }
                                else
                                { Log.Error("退出获取【文件】位置失败。"); SetPlc(new int[] { 1 }, "CMM_Alarm"); }
                            }
                            else
                            {
                                _simulator.SimiuCrtlQ();
                                //判断是否运行成功
                                await Task.Delay(2000);
                                //var imageBitmap = ScreenShotHelp.GetImage();
                                //imageBitmap.Save(_fullFileName, ImageFormat.Png);
                                if (_cmmVisionHelp.CheckCmmRunState(ScreenShotHelp.GetImage()) == 1 || _cmmVisionHelp.CheckCmmRunState(ScreenShotHelp.GetImage()) == 2)
                                {
                                    Log.Info($"开始运行2。。。");
                                    //写入数据库
                                    RecordMeasure("开始", 1);
                                    _isTheSame = true;
                                }
                                else
                                { Log.Error("测量程式运行失败2。"); SetPlc(new int[] { 1 }, "CMM_Alarm"); }
                            }
                        }
                    }
                    else
                    {
                        Log.Error("程序没有打开或者其他问题。"); SetPlc(new int[] { 1 }, "CMM_Alarm");
                    }
                }
            }
        }

        private void PoolGetPlc()
        {
            var pollingService = new PollingService(
                pollingInterval: TimeSpan.FromSeconds(3),
                checkAction: async () =>
                {
                    await Task.Run(GetPlcHeart);
                    if (chkIsConnPLC.Checked)
                    {
                        //bool isConnected = await CheckNetworkConnection();
                        //await Task.Run(GetPlcHeart);
                        _isStop = await Task.Run(GetStop);
                        await Task.Run(GetProductType);
                        await Task.Run(GetProductId);
                        _isStart = await Task.Run(GetStart);
                        //if (_isStart)
                        //    _isTheSame = false;
                    }

                    return true; // 始终继续轮询
                }
            );

            // 订阅错误事件
            pollingService.OnError += ex =>
                Log.Error($"PoolGetPlc error: {ex.Message}");

            // 启动轮询
            pollingService.Start();

            // 停止轮询（如点击停止按钮时调用）
            // pollingService.Stop();
        }

        private void PoolSetPlc()
        {
            var pollingService = new PollingService(
                pollingInterval: TimeSpan.FromSeconds(3),
                checkAction: async () =>
                {
                    await Task.Run(() => SetPlc(new int[] { 1 }, "CMM_Live"));
                    if (_isStart)
                    {
                        await Task.Run(() => SetPlc(new int[] { 1 }, "CMM_Busy"));
                        //await Task.Run(() => SetReady(0));
                        await Task.Run(() => SetPlc(new int[] { 0 }, "CMM_Ready"));
                    }
                    else
                    {
                        await Task.Run(() => SetPlc(new int[] { 0 }, "CMM_Busy"));
                        await Task.Run(() => SetPlc(new int[] { 1 }, "CMM_Ready"));
                    }

                    if (chkIsConnPLC.Checked)
                        await Task.Run(() => SetPlc(new int[] { 1 }, "CMM_Auto"));
                    else
                        await Task.Run(() => SetPlc(new int[] { 0 }, "CMM_Auto"));

                    return true; // 始终继续轮询
                }
            );

            // 订阅错误事件
            pollingService.OnError += ex =>
                Log.Error($"PoolSetPlc error: {ex.Message}");

            // 启动轮询
            pollingService.Start();

            // 停止轮询（如点击停止按钮时调用）
            // pollingService.Stop();
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
                MessageBoxX.Show("量测程式不能为空！", "提示");
                return;
            }

            this.WindowState = FormWindowState.Minimized;
            await TestOpenPrgAsync();
        }

        public async Task TestOpenPrgAsync()
        {
            await Task.Delay(3000);
            //var imageBitmap = ScreenShotHelp.GetImage();
            //imageBitmap.Save(_fullFileName, ImageFormat.Png);

            //获取文件位置
            if (_cmmVisionHelp.GetCmmFilePos(ScreenShotHelp.GetImage(), out float x, out float y) == 0)
            {
                //鼠标点击
                Log.Info($"[Auto][Run] 弹出文件窗口 - Start Button: X: {x}, Y: {y}");
                //NativeWindowHelp.Click(Convert.ToInt32(x), Convert.ToInt32(y));
                _simulator.SimiuCrtlO();
                await Task.Delay(4000);
                //imageBitmap = ScreenShotHelp.GetImage();
                //imageBitmap.Save(_fullFileName, ImageFormat.Png);

                if (_cmmVisionHelp.GetCmmOpenFilePos(ScreenShotHelp.GetImage(), out float x0, out float y0, out float x1, out float y1) == 0)
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
                    //imageBitmap = ScreenShotHelp.GetImage();
                    //imageBitmap.Save(_fullFileName, ImageFormat.Png);
                    //判断打开但没有运行状态  
                    if (_cmmVisionHelp.CheckCmmRunState(ScreenShotHelp.GetImage()) == 3)
                    {
                        //SendKeys.SendWait("^Q");
                        _simulator.SimiuCrtlQ();
                        //判断是否运行成功
                        await Task.Delay(2000);
                        //imageBitmap = ScreenShotHelp.GetImage();
                        //imageBitmap.Save(_fullFileName, ImageFormat.Png);
                        if (_cmmVisionHelp.CheckCmmRunState(ScreenShotHelp.GetImage()) == 1 || _cmmVisionHelp.CheckCmmRunState(ScreenShotHelp.GetImage()) == 2)
                        {
                            Log.Info($"开始运行3。。。");
                            //写入数据库
                            RecordMeasure("开始", 1);
                        }
                        else
                        { Log.Error("测量程式运行失败3。"); }
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

        private void btnSaveImg_Click(object sender, EventArgs e)
        {
            //await Test1Async();
            this.WindowState = FormWindowState.Minimized;
            GetCmmCurState();
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
                Thread.Sleep(4000);
                var basePath = GetImageBasePath();
                if (!Directory.Exists(basePath))
                    Directory.CreateDirectory(basePath);

                //var fullFileName = Path.Combine(basePath, $"{DateTime.Now.ToString("yyyyMMddHHmmss")}.{CommonConstant.IMAGE_SUFFIX}");
                //string fullFileName = Path.Combine(basePath, $"Screenflash.png");
                //var imageBitmap = ScreenShotHelp.GetImage();
                //imageBitmap.Save(fullFileName, ImageFormat.Png);
                //_cmmVisionHelp.CheckCmmIsClosed(fullFileName);
                var temp = _cmmVisionHelp.GetCmmOpenFilePos(ScreenShotHelp.GetImage(), out float x0, out float y0, out float x1, out float y1);
                Log.Info($"输出CMM软件打开程序的位置结果: {temp},位置信息: x0: {x0}, y0: {y0} x1: {x1}, y1: {y1}");
            }
            catch (Exception ex)
            {
                Log.Error($"[Save][Image] - save image error: {ex}");
            }
        }

        private void btnEnd_Click(object sender, EventArgs e)
        {
            if (chkIsConnPLC.Checked)
            {
                MessageBoxX.Show("联机状态下，不允许手动操作！", "提示");
                return;
            }

            _isStart = false;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            ConnPlc();
        }

        private void btnManual_Click(object sender, EventArgs e)
        {
            if (chkIsConnPLC.Checked)
            {
                MessageBoxX.Show("联机状态下，不允许手动操作！", "提示");
                return;
            }

            if (string.IsNullOrEmpty(txtMeasureProgram.Text)
                || string.IsNullOrEmpty(txtWorkPiece.Text.Trim())
                || string.IsNullOrEmpty(txtType.Text.Trim()))
            {
                MessageBoxX.Show("量测程序、类型码和工件码不能为空！", "提示");
                return;
            }

            _isCycle = false;
            _isStart = true;
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnAutoRun_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMeasureProgram.Text))
            {
                MessageBoxX.Show("量测程序不能为空！", "提示");
                return;
            }

            _isCycle = true;
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnInputTestPrg_Click(object sender, EventArgs e)
        {
            if (Global.CfgInfos.Count(p => p.Key == "Password") == 0)
            {
                MessageBoxX.Show("密码没有设置，请去数据字典设置！", "提示");
                return;
            }

            using (FrmPassword pwdForm = new FrmPassword())
            {
                if (pwdForm.ShowDialog() == DialogResult.OK)
                {
                    // 验证密码（示例密码为"1234"）
                    if (pwdForm.EnteredPassword == Global.CfgInfos.First(p => p.Key == "Password").Value)
                    {
                        FrmMeasure opForm = new FrmMeasure(_sqLiteHelpers);
                        opForm.Show(); // 打开操作窗体
                    }
                    else
                    {
                        MessageBoxX.Show("密码错误，请重试！", "提示");
                    }
                }
            }

            //if (string.IsNullOrEmpty(txtMeasureName.Text.Trim())
            //    || string.IsNullOrEmpty(txtMeasureProgram.Text.Trim())
            //    || string.IsNullOrEmpty(txtTypeKey.Text.Trim()))
            //{
            //    MessageBoxX.Show("量测程序、节点以及类型码不能为空！", "提示");
            //    return;
            //}

            //// check 是否已经存在
            //if (_sqLiteHelpers.QueryOne("MeaSurePrgCfg", "PrgName", txtMeasureName.Text.Trim()) != null)
            //{
            //    MessageBoxX.Show("该量测节点已经存在！", "提示");
            //    return;
            //}

            //// check 是否已经存在
            //if (_sqLiteHelpers.QueryOne("MeaSurePrgCfg", "Type", txtTypeKey.Text.Trim()) != null)
            //{
            //    MessageBoxX.Show("该类型码已经存在！", "提示");
            //    return;
            //}

            //Dictionary<string, object> dic = new Dictionary<string, object>
            //{
            //    {"PrgName", txtMeasureName.Text.Trim()},
            //    {"PrgPath", txtMeasureProgram.Text.Trim()},
            //    {"Type", txtTypeKey.Text.Trim()},
            //    {"CreateDate", DateTime.Now}
            //};

            //int result = _sqLiteHelpers.InsertData("MeaSurePrgCfg", dic);
            //LoadTreeView();
            //MessageBoxX.Show("录入成功！", "提示");
        }

        private void btnClearInfo_Click(object sender, EventArgs e)
        {
            ClearInfo();
        }

        private void ClearInfo()
        {
            txtMeasureProgram.Text = "";
            txtMeasureName.Text = "";
            txtTypeKey.Text = "";
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
                    string sql = "SELECT PrgName,PrgPath,Type FROM MeaSurePrgCfg WHERE PrgName = @PrgName";
                    DataSet dataSet = _sqLiteHelpers.ExecuteDataSet(sql, parameter);
                    txtMeasureName.Text = e.Node.Text;
                    txtMeasureProgram.Text = dataSet.Tables[0].Rows[0]["PrgPath"].ToString();
                    txtTypeKey.Text = dataSet.Tables[0].Rows[0]["Type"].ToString();
                    txtType.Text = dataSet.Tables[0].Rows[0]["Type"].ToString();
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
            Dictionary<string, object> dic = new Dictionary<string, object>
            {
                {"PieceId", txtWorkPiece.Text.Trim()},
                {"Type", txtType.Text.Trim()},
                {"PrgName", txtMeasureName.Text.Trim()},
                {"PrgPath", txtMeasureProgram.Text.Trim()},
                {"CMMState", state},
                {"CMMResult", result},
                {"CMMTime", DateTime.Now},
                {"CreateDate", DateTime.Now}
            };

            _sqLiteHelpers.InsertData("MeaSureData", dic);
        }

        //FormClosing是在窗体即将关闭但还未关闭时触发，这时候还可以取消关闭操作，比如弹出确认对话框，用户点击取消，那么窗体就不会关闭。
        //而FormClosed是在窗体已经关闭之后触发，这时候窗体已经不可见了，只能执行一些清理工作，比如释放资源或者记录日志，但无法阻止关闭。
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            var result = MessageBoxX.Show("是否退出？", "提示", null, MessageBoxButton.YesNo);

            if (result == MessageBoxResult.No)
                e.Cancel = true; // 阻止关闭

            //if (!isClosingByAnimation && e.CloseReason != CloseReason.ApplicationExitCall)
            //{
            //    e.Cancel = true;  // 阻止直接关闭

            //    var result = MessageBoxX.Show("是否退出？", "提示", null, MessageBoxButton.YesNo);
            //    if (result == MessageBoxResult.Yes)
            //        CloseWindow();     // 启动动画
            //}
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _sqLiteHelpers.Close();
            Log.Info($"CMMAuto被关闭！！！");
        }

        private async void btnTestExit_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

            await TestExitPrgAsync();
        }

        public async Task TestExitPrgAsync()
        {
            await Task.Delay(3000);

            if (_cmmVisionHelp.CheckCmmRunState(ScreenShotHelp.GetImage()) == 3)
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
                //var imageBitmap = ScreenShotHelp.GetImage();
                //imageBitmap.Save(_fullFileName, ImageFormat.Png);

                if (_cmmVisionHelp.GetCmmFilePos(ScreenShotHelp.GetImage(), out float x, out float y) == 0)
                {
                    NativeWindowHelp.Click(Convert.ToInt32(x), Convert.ToInt32(y));
                    await Task.Delay(1000);
                    //imageBitmap = ScreenShotHelp.GetImage();
                    //imageBitmap.Save(_fullFileName, ImageFormat.Png);
                    if (_cmmVisionHelp.GetCmmClosedPos(ScreenShotHelp.GetImage(), out float x1, out float y1) == 0)
                    {
                        NativeWindowHelp.Click(Convert.ToInt32(x1), Convert.ToInt32(y1));

                        await Task.Delay(2000);
                        //imageBitmap = ScreenShotHelp.GetImage();
                        //imageBitmap.Save(_fullFileName, ImageFormat.Png);

                        if (_cmmVisionHelp.CheckCmmIsClosed(ScreenShotHelp.GetImage()) == 0)
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

        private async void ConnectAgv()
        {
            await Task.Run(() =>
            {
                try
                {
                    //txtAgvConnect.BackColor = _tcpServer.IsConnectionAlive(new TCPServerContext { Port = 1234 }) ? System.Drawing.Color.LimeGreen : System.Drawing.Color.Red;
                    //lblAGV1.Text = $@"1号小车（{_tcpServer.Ip}）";
                }
                catch (Exception ex)
                {
                    // 异常时更新UI提示错误
                    this.BeginInvoke(new Action(() =>
                    {

                    }));
                    Log.Error($"连接失败: {ex.Message}");
                }
            });


            // 服务端测试
            //var server = new TcpServer(8888);
            //server.Start();
            //server.Clients[0].OnClientDisconnected += ex => Console.WriteLine($"Error: {ex.ToString()}");

            // 客户端测试
            //var client = new TcpClients("127.0.0.1", 8888);
            //client.OnDataReceived += Console.WriteLine;
            //await client.ConnectAsync();

            // 测试大数据包（1MB）
            var bigData = new string('A', 1024 * 1024);
            //await client.SendAsync(bigData);

            // 测试连续发送
            //await client.SendAsync("Hello");
            //await client.SendAsync("World");
        }

        public async Task OpenTestPrgAsync() // 方法标记为async，允许使用await关键字
        {
            Log.Info("异步线程-测量执行线程已开启。");

            // 读取最后50KB的日志（使用GBK编码）
            string logContent1 = ReadFileTail(
                @"C:\logs\app.log",
                bufferSize: 50 * 1024,
                encoding: Encoding.GetEncoding("GBK")
            );

            // 读取最后1MB日志（异步操作）
            var logContent = await ReadFileTailAsync(
                @"C:\logs\app.log",
                bufferSize: 1024 * 1024,
                encoding: Encoding.UTF8
            );

            // 带取消功能的读取
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5)); // 5秒超时
            try
            {
                var content = await ReadFileTailAsync("large.log", cancellationToken: cts.Token);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("读取操作超时取消");
            }

            while (true) // 无限循环直到程序被外部方式终止（例如，用户按Ctrl+C）
            {
                Log.Info("测量任务: " + DateTime.Now); // 执行你的任务
                if (_isCycle)
                    OpenTestRun();
                else
                {
                    if (_isStart)
                        OpenTestRun();
                }

                await Task.Delay(4000); // 暂停2秒，注意这里使用的是await，使得方法变为异步的，适合在UI或异步环境中使用。
            }
        }

        public string ReadFileTail(string filePath, int bufferSize = 1024 * 15, Encoding encoding = null)
        {
            // 参数校验
            if (!File.Exists(filePath)) throw new FileNotFoundException("文件不存在", filePath);
            if (bufferSize <= 0) throw new ArgumentException(@"缓冲区大小必须大于0", nameof(bufferSize));

            // 设置编码（默认UTF-8）
            //encoding ??= Encoding.UTF8;
            if (encoding == null)
                encoding = Encoding.UTF8;

            try
            {
                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    long fileLength = stream.Length;

                    // 处理小文件：直接全量读取
                    if (fileLength <= bufferSize)
                    {
                        byte[] fullBuffer = new byte[fileLength];
                        stream.Read(fullBuffer, 0, (int)fileLength);
                        return encoding.GetString(fullBuffer);
                    }

                    // 处理大文件：读取末尾内容
                    else
                    {
                        byte[] buffer = new byte[bufferSize];
                        // 定位到文件末尾前 bufferSize 字节的位置（需确保不越界）
                        stream.Seek(-bufferSize, SeekOrigin.End);
                        int bytesRead = stream.Read(buffer, 0, bufferSize);

                        // 处理实际读取字节数（可能小于bufferSize）
                        return encoding.GetString(buffer, 0, bytesRead);
                    }
                }
            }
            catch (IOException ex)
            {
                Log.Error($"读取文件失败: {ex.Message + ex.StackTrace}");
                throw new IOException($"读取文件失败: {ex.Message}", ex);
            }
        }

        public async Task<string> ReadFileTailAsync(string filePath, int bufferSize = 1024 * 15, Encoding encoding = null, CancellationToken cancellationToken = default)
        {
            // 参数校验
            if (!File.Exists(filePath)) throw new FileNotFoundException("文件不存在", filePath);
            if (bufferSize <= 0) throw new ArgumentException(@"缓冲区大小必须大于0", nameof(bufferSize));

            //encoding ??= Encoding.UTF8; // 默认编码
            if (encoding == null)
                encoding = Encoding.UTF8;

            try
            {
                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, bufferSize: 4096, // 系统默认页大小
                    useAsync: true))  // 启用异步IO
                {
                    long fileLength = stream.Length;

                    // 小文件直接全量读取
                    if (fileLength <= bufferSize)
                    {
                        byte[] fullBuffer = new byte[fileLength];
                        await stream.ReadAsync(fullBuffer, 0, (int)fileLength, cancellationToken)
                                    .ConfigureAwait(false);
                        return encoding.GetString(fullBuffer);
                    }

                    // 大文件读取末尾
                    else
                    {
                        byte[] buffer = new byte[bufferSize];
                        stream.Seek(-bufferSize, SeekOrigin.End); // 定位到末尾前 bufferSize

                        int totalBytesRead = 0;
                        while (totalBytesRead < bufferSize)
                        {
                            int bytesRead = await stream.ReadAsync(
                                buffer,
                                totalBytesRead,
                                bufferSize - totalBytesRead,
                                cancellationToken
                            ).ConfigureAwait(false);

                            if (bytesRead == 0) break; // 流结束
                            totalBytesRead += bytesRead;
                        }

                        return encoding.GetString(buffer, 0, totalBytesRead);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                throw; // 取消操作直接抛出
            }
            catch (Exception ex)
            {
                Log.Error($"读取文件失败: {ex.Message + ex.StackTrace}");
                throw new IOException($"读取文件失败: {ex.Message}", ex);
            }
        }

        public void ReadPlc()
        {
            _modbusUitl.ReadHoldingRegisters(262, 1);
        }

        public void WritePlc()
        {
            int[] productCode = ModbusClient.ConvertStringToRegisters("你好呀");
            _modbusUitl.WriteMultipleRegisters(262, productCode); // 告知PLC终止任务//25个字符  占13位地址
            _modbusUitl.WriteMultipleRegisters(262, new int[] { 200 }); // 告知PLC终止任务
            _modbusUitl.WriteMultipleRegisters(262, new int[] { 0 });//操作指令初始化
        }

        /// <summary>
        /// 寄存器读取
        /// </summary>
        public async void ReadRegister()
        {
            try
            {
                if (SocketHelper.Instance.Connected == false)
                {
                    //_option.SocketStatus = EnumSocketStatus.Connecting;
                    await SocketHelper.Instance.Connect(Connection);
                }

                //BaseMessage message = new TcpReadMessage(txtSlaveId.Text, _option.Function.Key, _option.Address, _option.Count);

                //MessageTransmit transmit = new MessageTransmit(_option.Protocol.Key, EnumTransmitWay.Request, _option.Address, message.Message);
                //SocketHelper.Instance.Send(transmit);
            }
            catch (Exception ex)
            {
                //Growl.Warning(exception.Message);
                Log.Error($"和PLC交互失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 属性修改事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 网络连接配置
        /// </summary>
        private NetworkConnection _connection = new NetworkConnection("127.0.0.1", 502);

        /// <summary>
        /// 网络连接配置
        /// </summary>
        public NetworkConnection Connection
        {
            get => _connection;
            set
            {
                _connection = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Connection)));
            }
        }

        private void txtPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\b') return; // 允许退格键
            int len = txtPort.Text.Length;
            if (len < 1 && e.KeyChar == '0')
            { // 禁止首位输入0
                e.Handled = true;
            }
            else if (!char.IsDigit(e.KeyChar))
            { // 仅允许数字
                e.Handled = true;
            }
        }

        private void btnConfigPlcAddress_Click(object sender, EventArgs e)
        {
            if (_frmConfig == null || _frmConfig.IsDisposed)
            {
                _frmConfig = new FrmConfig(_sqLiteHelpers);
                _frmConfig.FormClosed += (s, args) => { _frmConfig = null; };
            }
            _frmConfig.Show();
            _frmConfig.BringToFront();  // 激活并置顶窗体
        }

        private void btnSavePLC_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtIp.Text.Trim()))
            {
                MessageBoxX.Show("PLC 的IP地址不能为空！", "提示");
                return;
            }

            if (string.IsNullOrEmpty(txtPort.Text.Trim()))
            {
                MessageBoxX.Show("PLC 的端口号不能为空！", "提示");
                return;
            }

            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    {"Key", "PLCIp"}, {"Value", txtIp.Text.Trim()}, {"CreateDate", DateTime.Now}
                },
                new Dictionary<string, object>
                {
                    {"Key", "PLCPort"}, {"Value", txtPort.Text.Trim()}, {"CreateDate", DateTime.Now}
                }
            };

            SQLiteParameter[] parameter = new SQLiteParameter[] { new SQLiteParameter("Key", "PLCIp") };
            _sqLiteHelpers.Delete("Cfg", "Key=@Key", parameter);

            parameter = new SQLiteParameter[] { new SQLiteParameter("Key", "PLCPort") };
            _sqLiteHelpers.Delete("Cfg", "Key=@Key", parameter);

            foreach (var dic in data)
            {
                _sqLiteHelpers.InsertData("Cfg", dic);
            }

            MessageBoxX.Show($"保存PLCIp成功", "提示");
        }

        #region 与PLC交互

        #region GET

        private async void GetPlcHeart()
        {
            //await Task.Run(() =>
            //{
            //});
            await Task.Run(() =>
            {
                if (Global.PlcInfos.Count(p => p.PlcName == "Load_Live") == 0) return;
                if (_modbusUitl == null) return;

                int temp = 0;
                lock (_lock)
                {
                    temp = _modbusUitl.ReadHoldingRegisters
                    (Global.PlcInfos.First(p => p.PlcName == "Load_Live").Address,
                        Global.PlcInfos.First(p => p.PlcName == "Load_Live").Count).StrToInt();
                }
                Log.Info($"心跳信号：{temp}");
                this.BeginInvoke(new Action(() =>
                {
                    txtPlcConnect.BackColor = temp != 1 ? Color.Red : Color.LimeGreen;
                    lblPlcState.Text = temp != 1 ? "PLC连接断开......" : "";
                }));
            });
        }
        //plc收到结束信息时置0，开始和结束。表示这个流程结束(plc置0，或者我这边结束写0)
        private bool GetStart()
        {
            if (Global.PlcInfos.Count(p => p.PlcName == "Load_Start") == 0) return false;
            if (_modbusUitl == null) return false;

            int temp = 0;
            lock (_lock)
            {
                temp = _modbusUitl.ReadHoldingRegisters(
                    Global.PlcInfos.First(p => p.PlcName == "Load_Start").Address,
                    Global.PlcInfos.First(p => p.PlcName == "Load_Start").Count).StrToInt();
            }
            Log.Info($"启动信号:{temp}");
            return temp == 1;
        }

        private async void GetProductType()
        {
            await Task.Run(() =>
            {
                if (Global.PlcInfos.Count(p => p.PlcName == "Load_PartType") == 0) return;
                if (_modbusUitl == null) return;

                string type = "";
                lock (_lock)
                {
                    type = _modbusUitl.ReadHoldingRegistersConverString
                    (Global.PlcInfos.First(p => p.PlcName == "Load_PartType").Address,
                        Global.PlcInfos.First(p => p.PlcName == "Load_PartType").Count,
                        Global.PlcInfos.First(p => p.PlcName == "Load_PartType").Count).Replace("\0", "");

                }
                Log.Info($"收到类型码：{type}");

                if (txtTypeKey.Text.Trim() != type && !string.IsNullOrEmpty(type))
                {
                    SQLiteParameter[] parameter = new SQLiteParameter[] { new SQLiteParameter("Type", type) };
                    string sql = "SELECT PrgName,PrgPath,Type FROM MeaSurePrgCfg WHERE Type = @Type";
                    DataSet dataSet = _sqLiteHelpers.ExecuteDataSet(sql, parameter);
                    if (dataSet.Tables[0].Rows.Count == 0)
                    {
                        Log.Error($"类型码:{type}的程式没有维护，无法启动！！！");
                    }
                    else
                    {
                        this.BeginInvoke(new Action(() =>
                        {
                            txtTypeKey.Text = type;
                            txtType.Text = type;
                            txtMeasureName.Text = dataSet.Tables[0].Rows[0]["PrgName"].ToString();
                            txtMeasureProgram.Text = dataSet.Tables[0].Rows[0]["PrgPath"].ToString();
                        }));

                        if (!_isStart)
                            SetPlc(new int[] { 1 }, "CMM_Ready");
                        else
                            Log.Error("运行中！！！");
                    }
                }
            });
        }

        private async void GetProductId()
        {
            await Task.Run(() =>
            {
                if (Global.PlcInfos.Count(p => p.PlcName == "Load_PartID") == 0) return;
                if (_modbusUitl == null) return;

                string temp = "";
                lock (_lock)
                {
                    temp = _modbusUitl.ReadHoldingRegistersConverString
                    (Global.PlcInfos.First(p => p.PlcName == "Load_PartID").Address,
                        Global.PlcInfos.First(p => p.PlcName == "Load_PartID").Count,
                        Global.PlcInfos.First(p => p.PlcName == "Load_PartID").Count).Replace("\0", "");
                }
                Log.Info($"收到工件码：{temp}");
                this.BeginInvoke(new Action(() =>
                {
                    txtWorkPiece.Text = temp;
                }));
            });
        }

        private bool GetStop()
        {
            //await Task.Run(() =>
            //{
            //});
            if (Global.PlcInfos.Count(p => p.PlcName == "Load_EStop") == 0) return false;
            if (_modbusUitl == null) return false;

            int temp = 0;
            lock (_lock)
            {
                temp = _modbusUitl.ReadHoldingRegisters
                (Global.PlcInfos.First(p => p.PlcName == "Load_EStop").Address,
                    Global.PlcInfos.First(p => p.PlcName == "Load_EStop").Count).StrToInt();
            }
            Log.Info($"急停信号：{temp}");
            return temp == 1;
        }

        #endregion

        #region SET
        //需要PLC置位
        private void SetEnd(int value)
        {
            if (Global.PlcInfos.Count(p => p.PlcName == "CMM_MeasureCompleted") == 0)
                return;

            if (_modbusUitl != null)
                _modbusUitl.WriteMultipleRegisters
                (Global.PlcInfos.First(p => p.PlcName == "CMM_MeasureCompleted").Address,
                    new int[] { value }
                    );
        }

        private void SetReady(int value)
        {
            if (Global.PlcInfos.Count(p => p.PlcName == "CMM_Ready") == 0)
                return;

            if (_modbusUitl != null)
                _modbusUitl.WriteMultipleRegisters
                (Global.PlcInfos.First(p => p.PlcName == "CMM_Ready").Address,
                    new int[] { value }
                );
        }
        //需要PLC置位
        private void SetError(int value)
        {
            if (Global.PlcInfos.Count(p => p.PlcName == "CMM_Alarm") == 0)
                return;

            if (_modbusUitl != null)
                _modbusUitl.WriteMultipleRegisters
                (Global.PlcInfos.First(p => p.PlcName == "CMM_Alarm").Address,
                    new int[] { value }
                );
        }

        private void SetBusy(int value)
        {
            if (Global.PlcInfos.Count(p => p.PlcName == "CMM_Busy") == 0)
                return;

            if (_modbusUitl != null)
                _modbusUitl.WriteMultipleRegisters
                (Global.PlcInfos.First(p => p.PlcName == "CMM_Busy").Address,
                    new int[] { value }
                );
        }

        private void SetConnectPlc(int value)
        {
            if (Global.PlcInfos.Count(p => p.PlcName == "CMM_Live") == 0)
                return;

            if (_modbusUitl != null)
                _modbusUitl.WriteMultipleRegisters
                (Global.PlcInfos.First(p => p.PlcName == "CMM_Live").Address,
                    new int[] { value }
                );
        }

        private void SetType()
        {
            if (Global.PlcInfos.Count(p => p.PlcName == "CMM_AckParType") == 0)
                return;

            if (_modbusUitl != null)
                _modbusUitl.WriteMultipleRegisters
                (Global.PlcInfos.First(p => p.PlcName == "CMM_AckParType").Address,
                    ModbusClient.ConvertStringToRegisters(txtType.Text.Trim())
                );
        }

        private void SetId()
        {
            if (Global.PlcInfos.Count(p => p.PlcName == "CMM_AckPartID") == 0)
                return;

            if (_modbusUitl != null)
                _modbusUitl.WriteMultipleRegisters
                (Global.PlcInfos.First(p => p.PlcName == "CMM_AckPartID").Address,
                    ModbusClient.ConvertStringToRegisters(txtWorkPiece.Text.Trim())
                );
        }

        private void SetSafetyPos(int value)
        {
            if (Global.PlcInfos.Count(p => p.PlcName == "CMM_SafetyPos") == 0)
                return;

            if (_modbusUitl != null)
                _modbusUitl.WriteMultipleRegisters
                (Global.PlcInfos.First(p => p.PlcName == "CMM_SafetyPos").Address,
                    new int[] { value }
                );
        }

        private void SetAuto(int value)
        {
            if (Global.PlcInfos.Count(p => p.PlcName == "CMM_Auto") == 0)
                return;

            if (_modbusUitl != null)
                _modbusUitl.WriteMultipleRegisters
                (Global.PlcInfos.First(p => p.PlcName == "CMM_Auto").Address,
                    new int[] { value }
                );
        }

        private async void SetPlc(int[] value, string type)
        {
            await Task.Run(() =>
            {
                try
                {
                    if (Global.PlcInfos.Count(p => p.PlcName == type) == 0) return;
                    if (_modbusUitl == null) return;

                    Log.Info($"写入PLC,写入值：{string.Join(", ", value ?? Array.Empty<int>())}，" + $"写入类型：{type}");
                    lock (_lock)
                    {
                        _modbusUitl?.WriteMultipleRegisters(Global.PlcInfos.First(p => p.PlcName == type).Address, value);
                    }
                }
                catch (Exception e)
                {
                    Log.Error($"写入PLC失败,写入值：{string.Join(", ", value ?? Array.Empty<int>())}，" +
                              $"写入类型：{type}，失败原因：{e.Message + e.StackTrace}");
                }
            });
        }

        #endregion

        private DataTable GetPlcAddressInfo(string name)
        {
            DataTable dt = new DataTable();
            SQLiteParameter[] parameter = new SQLiteParameter[] { new SQLiteParameter("Name", name) };
            string sql = "SELECT * FROM PLCCfg WHERE Name=@Name";
            DataSet dataSet = _sqLiteHelpers.ExecuteDataSet(sql, parameter);
            if (dataSet.Tables[0].Rows.Count != 0)
                dt = dataSet.Tables[0];

            return dt;
        }

        #endregion

        #region 关闭窗体动画

        // 可选：声明动画完成事件
        public event EventHandler AnimationCompleted;
        private System.Windows.Forms.Timer closeTimer;
        private DateTime animationStart;
        private double initialHeight;
        private double initialWidth;
        private bool isClosingByAnimation = false;

        private void CloseWindow()
        {
            // 初始化定时器
            closeTimer = new System.Windows.Forms.Timer();
            closeTimer.Interval = 15; // 约60FPS
            initialHeight = this.Height;
            initialWidth = this.Width;
            animationStart = DateTime.Now;

            closeTimer.Tick += (s, e) =>
            {
                // 计算动画进度（0到1之间）
                double progress = (DateTime.Now - animationStart).TotalMilliseconds / 1000.0;

                if (progress >= 1.0)
                {
                    // 动画完成
                    closeTimer.Stop();
                    isClosingByAnimation = true;
                    this.Close();
                    AnimationCompleted?.Invoke(this, EventArgs.Empty);
                    return;
                }

                // 更新窗口尺寸
                this.Height = (int)Math.Max(initialHeight * (1 - progress), 0);
                this.Width = (int)Math.Max(initialWidth * (1 - progress), 0);

                // （可选）保持窗口居中缩放
                this.Left += (int)(initialWidth * progress / 2);
                this.Top += (int)(initialHeight * progress / 2);
            };

            closeTimer.Start();
        }

        #endregion

        private void btnDelPrg_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMeasureName.Text.Trim())
                || string.IsNullOrEmpty(txtMeasureProgram.Text.Trim())
                || string.IsNullOrEmpty(txtTypeKey.Text.Trim()))
            {
                MessageBoxX.Show("量测程序、节点以及类型码不能为空！", "提示");
                return;
            }

            SQLiteParameter[] parameter = new SQLiteParameter[] { new SQLiteParameter("Type", txtTypeKey.Text.Trim()) };
            _sqLiteHelpers.Delete("MeaSurePrgCfg", "Type=@Type", parameter);
            ClearInfo();
            MessageBoxX.Show("删除成功！", "提示");
        }

        private void chkIsConnPLC_CheckedChanged(object sender, EventArgs e)
        {
            if (chkIsConnPLC.Checked)
            {
                btnEnd.Enabled = false;
                btnManual.Enabled = false;
            }
            else
            {
                btnEnd.Enabled = true;
                btnManual.Enabled = true;
            }
        }

        private void btnQueryPlc_Click(object sender, EventArgs e)
        {
            if (_frmQueryPlc == null || _frmQueryPlc.IsDisposed)
            {
                _frmQueryPlc = new FrmQueryPlc(_sqLiteHelpers, _modbusUitl);
                _frmQueryPlc.FormClosed += (s, args) => { _frmQueryPlc = null; };
            }
            _frmQueryPlc.Show();
            _frmQueryPlc.BringToFront();  // 激活并置顶窗体
        }

        private void btnDicConfig_Click(object sender, EventArgs e)
        {
            if (_frmDicConfig == null || _frmDicConfig.IsDisposed)
            {
                _frmDicConfig = new FrmDicConfig(_sqLiteHelpers);
                _frmDicConfig.FormClosed += (s, args) => { _frmDicConfig = null; };
            }
            _frmDicConfig.Show();
            _frmDicConfig.BringToFront();  // 激活并置顶窗体
        }

        private void PoolPostUrl()
        {
            var pollingService = new PollingService(
                pollingInterval: TimeSpan.FromSeconds(1),
                checkAction: async () =>
                {
                    if (chkIsSend.Checked)
                        await Task.Run(SendImgToServer);

                    return true; // 始终继续轮询
                }
            );

            // 订阅错误事件
            pollingService.OnError += ex =>
                Log.Error($"PoolPostUrl error: {ex.Message}");

            // 启动轮询
            pollingService.Start();
        }

        private async void SendImgToServer()
        {
            await Task.Run(async () =>
            {
                // 初始化客户端
                //var apiClient = new ApiClient("https://api.example.com/v1");
                // 设置认证令牌
                //_apiClient.SetBearerToken("your-access-token");

                try
                {
                    // POST 请求示例
                    var requestData = new Request
                    {
                        Status = _status,
                        Ip = _ip,
                        ImageData = _cmmVisionHelp.GetPicImageData(ScreenShotHelp.GetImage())
                    };

                    await _apiClient.PostAsync<Response>(Global.CfgInfos.Count(p => p.Key == "InterfaceName") != 0 ?
                            Global.CfgInfos.First(p => p.Key == "InterfaceName").Value : "/api/testpost", requestData);
                }
                catch (HttpRequestException ex)
                {
                    Log.Error($@"网络请求错误: {ex.Message}");
                }
                catch (InvalidOperationException ex)
                {
                    Log.Error($@"数据处理错误: {ex.Message}");
                }
            });
        }

        private void btnGoHome_Click(object sender, EventArgs e)
        {
            if (_isStart)
            {
                MessageBoxX.Show("运行过程中不能回安全位置！", "提示");
                return;
            }

            SQLiteParameter[] parameter = new SQLiteParameter[] { new SQLiteParameter("Type", "Home") };
            string sql = "SELECT PrgName,PrgPath,Type FROM MeaSurePrgCfg WHERE Type = @Type";
            DataSet dataSet = _sqLiteHelpers.ExecuteDataSet(sql, parameter);
            if (dataSet.Tables[0].Rows.Count == 0)
            {
                MessageBoxX.Show("回安全位置的程式没有配置，请去程式编辑配置！", "提示");
                return;
            }

            this.WindowState = FormWindowState.Minimized;
            bool goHomeStart = true;
            while (true)
            {
                GoHome(dataSet.Tables[0].Rows[0]["PrgPath"].ToString(), ref goHomeStart);
                if (!goHomeStart)
                    break;
                Thread.Sleep(3000);
            }
        }

        private void GoHome(string path, ref bool goHomeStart)
        {
            //具体操作
            if (txtExit.BackColor == System.Drawing.Color.LimeGreen)
            {
                //获取文件位置
                if (_cmmVisionHelp.GetCmmFilePos(ScreenShotHelp.GetImage(), out float x, out float y) == 0)
                {
                    //鼠标点击
                    Log.Info($"[Auto][Run] 弹出文件窗口 - Start Button: X: {x}, Y: {y}");
                    //NativeWindowHelp.Click(Convert.ToInt32(x), Convert.ToInt32(y));
                    _simulator.SimiuCrtlO();
                    Thread.Sleep(2000);
                    //var imageBitmap = ScreenShotHelp.GetImage();
                    //imageBitmap.Save(_fullFileName, ImageFormat.Png);

                    if (_cmmVisionHelp.GetCmmOpenFilePos(ScreenShotHelp.GetImage(), out float x0, out float y0, out float x1, out float y1) == 0)
                    {
                        Log.Info($"[Auto][Run] 打开量测程式 - Start Button: X: {x0}, Y: {y0}");
                        // 将文本放入剪贴板
                        Clipboard.SetText(path);
                        // 模拟Ctrl+V
                        //SendKeys.SendWait("^v");
                        _simulator.SimiuCrtlV();
                        Thread.Sleep(2000);
                        NativeWindowHelp.Click(Convert.ToInt32(x1), Convert.ToInt32(y1));
                        //判断打开但没有运行状态
                        Thread.Sleep(3000);
                        //imageBitmap = ScreenShotHelp.GetImage();
                        //imageBitmap.Save(_fullFileName, ImageFormat.Png);
                        if (_cmmVisionHelp.CheckCmmRunState(ScreenShotHelp.GetImage()) == 3)//check是否打开
                        {
                            _simulator.SimiuCrtlQ();

                            Thread.Sleep(2000);
                            //imageBitmap = ScreenShotHelp.GetImage();
                            //imageBitmap.Save(_fullFileName, ImageFormat.Png);
                            if (_cmmVisionHelp.CheckCmmRunState(ScreenShotHelp.GetImage()) == 1 || _cmmVisionHelp.CheckCmmRunState(ScreenShotHelp.GetImage()) == 2)//check是否打开
                            {
                                Log.Info($"【回家】开始运行1。。。");
                                _isTheSame = true;
                            }
                            else
                            {
                                Log.Error("【回家】测量程式运行失败1。");
                            }
                        }
                        else
                        { Log.Error("【回家】打开测量程式失败。"); }
                    }
                    else
                    {
                        Log.Error("【回家】获取【打开测量程式】位置失败。");
                    }
                }
                else
                {
                    Log.Error("【回家】获取【文件】位置失败。");
                }
            }
            else
            {
                if (txtRun.BackColor == System.Drawing.Color.LimeGreen || txtPause.BackColor == System.Drawing.Color.LimeGreen)
                {
                    Log.Error("【回家】运行中不能打开程式。");
                }
                else
                {
                    if (txtPreOrEnd.BackColor == System.Drawing.Color.LimeGreen)
                    {
                        if (_cmmVisionHelp.CheckCmmRunState(ScreenShotHelp.GetImage()) == 3)
                        {
                            Thread.Sleep(3000);

                            if (_isTheSame)
                            {
                                //-----判断结束，并退出；
                                //var imageBitmap = ScreenShotHelp.GetImage();
                                //imageBitmap.Save(_fullFileName, ImageFormat.Png);

                                if (_cmmVisionHelp.GetCmmFilePos(ScreenShotHelp.GetImage(), out float x, out float y) == 0)
                                {
                                    NativeWindowHelp.Click(Convert.ToInt32(x), Convert.ToInt32(y));
                                    Thread.Sleep(1000);
                                    //imageBitmap = ScreenShotHelp.GetImage();
                                    //imageBitmap.Save(_fullFileName, ImageFormat.Png);
                                    if (_cmmVisionHelp.GetCmmClosedPos(ScreenShotHelp.GetImage(), out float x1, out float y1) == 0)
                                    {
                                        NativeWindowHelp.Click(Convert.ToInt32(x1), Convert.ToInt32(y1));

                                        Thread.Sleep(2000);
                                        //imageBitmap = ScreenShotHelp.GetImage();
                                        //imageBitmap.Save(_fullFileName, ImageFormat.Png);

                                        if (_cmmVisionHelp.CheckCmmIsClosed(ScreenShotHelp.GetImage()) == 0)
                                        {
                                            //是同一个就关闭
                                            Log.Info($"【回家】退出成功。。。");
                                            _isTheSame = false;
                                            goHomeStart = false;
                                        }
                                        else
                                        { Log.Error("【回家】程式退出失败。"); }
                                    }
                                    else
                                    { Log.Error("【回家】退出获取【退出】位置失败。"); }
                                }
                                else
                                { Log.Error("【回家】退出获取【文件】位置失败。"); }
                            }
                            else
                            {
                                _simulator.SimiuCrtlQ();
                                //判断是否运行成功
                                Thread.Sleep(2000);
                                //var imageBitmap = ScreenShotHelp.GetImage();
                                //imageBitmap.Save(_fullFileName, ImageFormat.Png);
                                if (_cmmVisionHelp.CheckCmmRunState(ScreenShotHelp.GetImage()) == 1 || _cmmVisionHelp.CheckCmmRunState(ScreenShotHelp.GetImage()) == 2)
                                {
                                    Log.Info($"【回家】开始运行2。。。");
                                    _isTheSame = true;
                                }
                                else
                                { Log.Error("【回家】测量程式运行失败2。"); }
                            }
                        }
                    }
                    else
                    {
                        Log.Error("【回家】程序没有打开或者其他问题。");
                    }
                }
            }
        }

        private void Test()
        {
            Log.Error($@"不存图的值: {_cmmVisionHelp.CheckCmmRunState(ScreenShotHelp.GetImage())}");

            //Thread.Sleep(4000);
            var basePath = GetImageBasePath();
            if (!Directory.Exists(basePath))
                Directory.CreateDirectory(basePath);

            string fullFileName = Path.Combine(basePath, $"Screenflash.png");
            var imageBitmap = ScreenShotHelp.GetImage();
            imageBitmap.Save(fullFileName, ImageFormat.Png);

            Log.Error($@"存图的值: {_cmmVisionHelp.CheckCmmRunState(fullFileName)}");
        }
    }
}
