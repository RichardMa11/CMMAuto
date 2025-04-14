using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using CMMAuto.CommonHelp;
using CMMAuto.Extension;
using log4net;
using Panuon.UI.Silver;

namespace CMMAuto
{
    public partial class FrmConfig : Form
    {
        // 配置项数据模型
        public class ConfigItem
        {
            [DisplayName("信号名称")]
            public string Name { get; set; }

            [DisplayName("起始地址")]
            public string Address { get; set; }

            [DisplayName("值长度")]
            public int Count { get; set; }

            [DisplayName("说明")]
            public string Remark { get; set; }
        }

        private readonly SQLiteHelper _sqLiteHelpers = null;
        private readonly BindingList<ConfigItem> _configList = new BindingList<ConfigItem>();
        private static readonly ILog Log = LogManager.GetLogger(typeof(FrmConfig));
        public FrmConfig(SQLiteHelper sqLiteHelpers)
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
            string sql = $@"SELECT * FROM PLCCfg ";
            DataSet dataSet = _sqLiteHelpers.ExecuteDataSet(sql, null);
            if (dataSet != null)
            {
                foreach (DataRow r in dataSet.Tables[0].Rows)
                {
                    _configList.Add(new ConfigItem
                    {
                        Name = r["Name"].ToString(),
                        Address = r["Address"].ToString(),
                        Count = r["Count"].ToString().StrToInt(),
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
                    if (string.IsNullOrEmpty(c.Name) || string.IsNullOrEmpty(c.Address) || string.IsNullOrEmpty(c.Count.ToString()))
                    {
                        MessageBoxX.Show("信号名称、起始地址和值长度不能为空！", "提示");
                        return;
                    }

                    // check 是否已经存在
                    //if (_sqLiteHelpers.QueryOne("PLCCfg", "Name", c.Name) != null)
                    //{
                    //    MessageBoxX.Show($"信号名称:{c.Name}数据库已经存在！", "提示");
                    //    return;
                    //}

                    if (nameList.Contains(c.Name.Trim()))
                    {
                        MessageBoxX.Show($"信号名称:{c.Name}重复填写！", "提示");
                        return;
                    }
                    nameList.Add(c.Name.Trim());

                    data.Add(new Dictionary<string, object>
                    {
                        {"Name", c.Name},
                        {"Address", c.Address},
                        {"Count", c.Count},
                        {"Remark", c.Remark},
                        {"CreateDate", DateTime.Now}
                    });
                }

                _sqLiteHelpers.Delete("PLCCfg", null, null);
                foreach (var dic in data)
                {
                    _sqLiteHelpers.InsertData("PLCCfg", dic);
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
            _configList.Add(new ConfigItem { Name = "CMM_Ready", Address = "OX11", Count = 4, Remark = "CMM 空闲" });
        }

        private void grvConfig_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            // 排除新行或非目标列
            if (grvConfig.Rows[e.RowIndex].IsNewRow || e.ColumnIndex != 3)
                return;

            string input = e.FormattedValue?.ToString();
            if (!int.TryParse(input, out int value) || value < 0)
            {
                grvConfig.Rows[e.RowIndex].ErrorText = "请输入0或正整数";
                MessageBoxX.Show(@"值长度请输入0或正整数", "提示");
                e.Cancel = true; // 阻止离开单元格
            }
            else
            {
                grvConfig.Rows[e.RowIndex].ErrorText = ""; // 清除错误提示
            }
        }
    }
}
