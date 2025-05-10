using System;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using CMMAuto.CommonHelp;
using log4net;
using Panuon.UI.Silver;

namespace CMMAuto
{
    public partial class FrmMeasure : Form
    {
        // 数据模型
        public class MeasureItem
        {
            [DisplayName("类型码")]
            public string Type { get; set; }

            [DisplayName("程式节点")]
            public string PrgName { get; set; }

            [DisplayName("程式路径")]
            public string PrgPath { get; set; }

            [DisplayName("创建时间")]
            public string CreateDate { get; set; }
        }

        private readonly SQLiteHelper _sqLiteHelpers = null;
        private BindingList<MeasureItem> _configList = new BindingList<MeasureItem>();
        private static readonly ILog Log = LogManager.GetLogger(typeof(FrmMeasure));

        public FrmMeasure(SQLiteHelper sqLiteHelpers)
        {
            _sqLiteHelpers = sqLiteHelpers;
            InitializeComponent();
            LoadData();
            StyleGridView();
        }

        private void LoadData()
        {
            grvConfig.DataSource = null;
            string sql = $@"SELECT Type,PrgName,PrgPath,strftime('%Y-%m-%d %H:%M:%S', CreateDate) as CreateDate FROM MeaSurePrgCfg ";
            DataSet dataSet = _sqLiteHelpers.ExecuteDataSet(sql, null);
            _configList = new BindingList<MeasureItem>();
            if (dataSet != null)
            {
                foreach (DataRow r in dataSet.Tables[0].Rows)
                {
                    _configList.Add(new MeasureItem
                    {
                        Type = r["Type"].ToString(),
                        PrgName = r["PrgName"].ToString(),
                        PrgPath = r["PrgPath"].ToString(),
                        CreateDate = r["CreateDate"].ToString()
                    });
                }
            }

            grvConfig.DataSource = _configList;
            grvConfig.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grvConfig.RowHeadersVisible = false;
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

        private void btnSelect_Click(object sender, EventArgs e)
        {
            LoadData();
            if (string.IsNullOrEmpty(txtMeasureName.Text.Trim())
                && string.IsNullOrEmpty(txtMeasureProgram.Text.Trim())
                && string.IsNullOrEmpty(txtTypeKey.Text.Trim())) return;

            var temp = _configList.Where(t => t.PrgName == txtMeasureName.Text ||
                                              t.Type == txtTypeKey.Text ||
                                              t.PrgPath == txtMeasureProgram.Text).ToList();
            _configList = new BindingList<MeasureItem>();
            foreach (var r in temp)
            {
                _configList.Add(new MeasureItem
                {
                    Type = r.Type,
                    PrgName = r.PrgName,
                    PrgPath = r.PrgPath,
                    CreateDate = r.CreateDate
                });
            }
            grvConfig.DataSource = _configList;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (FrmMeasureEdit editForm = new FrmMeasureEdit(_sqLiteHelpers))
            {
                if (editForm.ShowDialog() == DialogResult.OK)
                    LoadData();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (grvConfig.CurrentRow?.DataBoundItem is MeasureItem selected)
            {
                using (FrmMeasureEdit editForm = new FrmMeasureEdit(_sqLiteHelpers, selected.Type, selected.PrgName, selected.PrgPath))
                {
                    if (editForm.ShowDialog() == DialogResult.OK)
                        LoadData();
                }
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                if (grvConfig.CurrentRow?.DataBoundItem is MeasureItem selected)
                {
                    if (MessageBoxX.Show($"确认删除，类型码：{selected.Type}，程式节点：{selected.PrgName}，程式路径：{selected.PrgPath}？", "提示", null, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        SQLiteParameter[] parameter = new SQLiteParameter[] { new SQLiteParameter("Type", selected.Type) };
                        _sqLiteHelpers.Delete("MeaSurePrgCfg", "Type=@Type", parameter);

                        MessageBoxX.Show("删除成功！", "提示");
                        LoadData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxX.Show($"删除失败: {ex.Message}", "提示");
                Log.Error($"删除失败: {ex.Message}");
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtMeasureName.Text = "";
            txtMeasureProgram.Text = "";
            txtTypeKey.Text = "";
        }
    }
}
