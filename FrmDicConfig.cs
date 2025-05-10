using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using CMMAuto.CommonHelp;
using log4net;
using Panuon.UI.Silver;

namespace CMMAuto
{
    public partial class FrmDicConfig : Form
    {
        // 配置项数据模型
        public class ConfigItem
        {
            [DisplayName("名称")]
            public string Key { get; set; }

            [DisplayName("值内容")]
            public string Value { get; set; }

            [DisplayName("说明")]
            public string Remark { get; set; }
        }

        private readonly SQLiteHelper _sqLiteHelpers = null;
        private readonly BindingList<ConfigItem> _configList = new BindingList<ConfigItem>();
        private static readonly ILog Log = LogManager.GetLogger(typeof(FrmDicConfig));
        public FrmDicConfig(SQLiteHelper sqLiteHelpers)
        {
            _sqLiteHelpers = sqLiteHelpers;
            InitializeComponent();
            LoadData();
            StyleGridView();
        }

        private void StyleGridView()
        {
            // 基础样式
            grvConfig.EnableHeadersVisualStyles = false;
            grvConfig.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.Azure,
                ForeColor = Color.FromArgb(80, 80, 80),
                Font = new Font("微软雅黑", 11, FontStyle.Bold),
                Padding = new Padding(0, 5, 0, 5)
            };

            // 行样式
            grvConfig.RowsDefaultCellStyle = new DataGridViewCellStyle
            {
                Font = new Font("微软雅黑", 9),
                BackColor = Color.Azure,
                SelectionBackColor = Color.DodgerBlue
            };
            grvConfig.AlternatingRowsDefaultCellStyle.BackColor = Color.LightBlue;

            // 自定义单元格边框
            grvConfig.CellPainting += (s, e) =>
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    e.PaintBackground(e.CellBounds, true);
                    ControlPaint.DrawBorder(e.Graphics, e.CellBounds,
                        Color.FromArgb(220, 220, 220), 1, ButtonBorderStyle.Solid,
                        Color.FromArgb(220, 220, 220), 1, ButtonBorderStyle.Solid,
                        Color.FromArgb(220, 220, 220), 1, ButtonBorderStyle.Solid,
                        Color.FromArgb(220, 220, 220), 1, ButtonBorderStyle.Solid);
                    e.PaintContent(e.CellBounds);
                    e.Handled = true;
                }
            };

            // 添加在 StyleGridView 方法中
            // 行悬浮高亮
            //grvConfig.CellMouseEnter += (s, e) =>
            //{
            //    grvConfig.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
            //};
            //grvConfig.CellMouseLeave += (s, e) =>
            //{
            //    grvConfig.Rows[e.RowIndex].DefaultCellStyle.BackColor = grvConfig.DefaultCellStyle.BackColor;
            //};

            // 添加列头排序指示符
            //grvConfig.ColumnHeaderMouseClick += (s, e) =>
            //{
            //    grvConfig.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection =
            //        grvConfig.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection == SortOrder.Ascending
            //            ? SortOrder.Descending
            //            : SortOrder.Ascending;
            //};

            // 添加按钮列
            DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn
            {
                HeaderText = @"操作",
                Text = "删除",
                UseColumnTextForButtonValue = true,
                FlatStyle = FlatStyle.Popup,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(220, 53, 69), // 标准红色（参考‌:ml-citation{ref="2,4" data="citationList"}）
                    ForeColor = Color.DarkBlue,
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            };
            grvConfig.Columns.Add(btnDelete);

            // 处理点击事件
            grvConfig.CellClick += (s, e) =>
            {
                if (e.ColumnIndex == btnDelete.Index && e.RowIndex >= 0)
                {
                    _configList.RemoveAt(e.RowIndex); // 数据绑定模式
                    // 或 dataGridView1.Rows.RemoveAt(e.RowIndex); // 非绑定模式
                }
            };

        }

        private void LoadData()
        {
            grvConfig.DataSource = null;
            string sql = $@"SELECT * FROM Cfg ";
            DataSet dataSet = _sqLiteHelpers.ExecuteDataSet(sql, null);
            if (dataSet != null)
            {
                foreach (DataRow r in dataSet.Tables[0].Rows)
                {
                    _configList.Add(new ConfigItem
                    {
                        Key = r["Key"].ToString(),
                        Value = r["Value"].ToString(),
                        Remark = r["Remark"].ToString()
                    });
                }
            }

            grvConfig.DataSource = _configList;
            grvConfig.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grvConfig.RowHeadersVisible = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            List<string> nameList = new List<string>();
            try
            {
                foreach (var c in _configList)
                {
                    if (string.IsNullOrEmpty(c.Key) || string.IsNullOrEmpty(c.Value))
                    {
                        MessageBoxX.Show("名称和值内容不能为空！", "提示");
                        return;
                    }

                    if (nameList.Contains(c.Key.Trim()))
                    {
                        MessageBoxX.Show($"名称:{c.Key}重复填写！", "提示");
                        return;
                    }
                    nameList.Add(c.Key.Trim());

                    data.Add(new Dictionary<string, object>
                    {
                        {"Key", c.Key},
                        {"Value", c.Value},
                        {"Remark", c.Remark},
                        {"CreateDate", DateTime.Now}
                    });
                }

                _sqLiteHelpers.Delete("Cfg", null, null);
                foreach (var dic in data)
                {
                    _sqLiteHelpers.InsertData("Cfg", dic);
                }

                var result = MessageBoxX.Show($@"已保存 {_configList.Count} 项配置", "提示");
                if (result == MessageBoxResult.OK)
                    this.Close();

            }
            catch (Exception exception)
            {
                Log.Error($"保存失败：{exception.Message + exception.StackTrace}");
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            _configList.Add(new ConfigItem { Key = "InterfaceRst", Value = "select_measuredresult", Remark = "查询测量结果" });
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            _configList.Add(new ConfigItem { Key = "InterfaceRun", Value = "select_inspecrunstatus", Remark = "查询InSpec运行状态" });
            _configList.Add(new ConfigItem { Key = "InterfaceRevIwp", Value = "receive_iwpfiles", Remark = "接收IWP文件名" });
            _configList.Add(new ConfigItem { Key = "InterfaceRst", Value = "select_measuredresult", Remark = "查询测量结果" });

            _configList.Add(new ConfigItem { Key = "InterfaceIsStart", Value = "select_ machinereadyresult", Remark = "查询机台是否就绪" });
            _configList.Add(new ConfigItem { Key = "InterfaceSignal", Value = "receive_measuring_signal", Remark = "上下料请求及完成信号" });
            _configList.Add(new ConfigItem { Key = "InterfaceIsLoading", Value = "receive_loading_unloading", Remark = "上下料接口" });

            _configList.Add(new ConfigItem { Key = "RefreshTime", Value = "2", Remark = "量测数据结果显示时效（注：单位小时/h）" });
            _configList.Add(new ConfigItem { Key = "Http", Value = "http://localhost:8200/autolink", Remark = "监控接口地址" });
            _configList.Add(new ConfigItem { Key = "InterfaceName", Value = "/api/testpost", Remark = "监控接口地址后缀" });
            _configList.Add(new ConfigItem { Key = "Password", Value = "1234", Remark = "程式编辑的权限密码" });
        }
    }
}
