using System;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Windows.Forms;
using CMMAuto.CommonHelp;
using CMMAuto.Extension;
using EasyModbus;
using KENDLL.Common;
using log4net;
using Panuon.UI.Silver;

namespace CMMAuto
{
    public partial class FrmQueryPlc : Form
    {
        // 配置项数据模型
        public class ConfigItem
        {
            [DisplayName("信号名称")]
            public string Name { get; set; }

            [DisplayName("起始地址")]
            public string Address { get; set; }

            [DisplayName("值内容")]
            public string Content { get; set; }

            [DisplayName("值长度")]
            public int Count { get; set; }

            [DisplayName("说明")]
            public string Remark { get; set; }
        }

        private readonly SQLiteHelper _sqLiteHelpers = null;
        private BindingList<ConfigItem> _configList = new BindingList<ConfigItem>();
        private static readonly ILog Log = LogManager.GetLogger(typeof(FrmQueryPlc));
        private ModbusUitl _modbusUitl;

        public FrmQueryPlc(SQLiteHelper sqLiteHelpers, ModbusUitl modbusUitl)
        {
            _sqLiteHelpers = sqLiteHelpers;
            _modbusUitl = modbusUitl;
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
        }

        private void LoadData()
        {
            grvConfig.DataSource = null;
            string sql = $@"SELECT * FROM PLCCfg ";
            DataSet dataSet = _sqLiteHelpers.ExecuteDataSet(sql, null);
            _configList = new BindingList<ConfigItem>();
            if (dataSet != null)
            {
                foreach (DataRow r in dataSet.Tables[0].Rows)
                {
                    _configList.Add(new ConfigItem
                    {
                        Name = r["Name"].ToString(),
                        Address = r["Address"].ToString(),
                        Count = r["Count"].ToString().StrToInt(),
                        Remark = r["Remark"].ToString(),
                        //Content = _modbusUitl.ReadHoldingRegisters(r["Address"].ToString().StrToInt(), r["Count"].ToString().StrToInt())
                        Content = r["Count"].ToString().StrToInt() != 1 ? _modbusUitl?.ReadHoldingRegistersConverString
                            (r["Address"].ToString().StrToInt(), r["Count"].ToString().StrToInt(), r["Count"].ToString().StrToInt()).Replace("\0", "")
                            : _modbusUitl?.ReadHoldingRegisters(r["Address"].ToString().StrToInt(), r["Count"].ToString().StrToInt())
                    });
                }
            }

            grvConfig.DataSource = _configList;
            grvConfig.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grvConfig.RowHeadersVisible = false;
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

        private void btnRW_Click(object sender, EventArgs e)
        {
            try
            {
                if (_modbusUitl != null)
                {
                    if (GetPlcAddressInfo("TEST").Rows.Count != 0)
                    {
                        _modbusUitl.WriteMultipleRegisters(GetPlcAddressInfo("TEST").Rows[0]["Address"].ToString().StrToInt(),
                            ModbusClient.ConvertStringToRegisters(txtWrite.Text.Trim()));

                        txtRead.Text = _modbusUitl.ReadHoldingRegistersConverString
                        (GetPlcAddressInfo("TEST").Rows[0]["Address"].ToString().StrToInt(),
                            GetPlcAddressInfo("TEST").Rows[0]["Count"].ToString().StrToInt(),
                            txtWrite.Text.Trim().Length + 1).Replace("\0", "");
                    }
                    else
                    {
                        MessageBoxX.Show("PLC信号地址【TEST】没有配置", "提示");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxX.Show($"读取PLC失败: {ex.Message}", "提示");
                Log.Error($"读取PLC失败: {ex.Message}");
            }
        }

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

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
