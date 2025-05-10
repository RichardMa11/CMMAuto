using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Drawing;
using System.Windows.Forms;
using CMMAuto.CommonHelp;
using Panuon.UI.Silver;

namespace CMMAuto
{
    public partial class FrmMeasureEdit : Form
    {
        private readonly string _type;
        private readonly SQLiteHelper _sqLiteHelpers = null;

        public FrmMeasureEdit(SQLiteHelper sqLiteHelpers, string type = "", string node = "", string path = "")
        {
            InitializeComponent();
            _sqLiteHelpers = sqLiteHelpers;
            _type = type;
            txtMeasureName.Text = node;
            txtMeasureProgram.Text = path;
            txtTypeKey.Text = type;

            if (string.IsNullOrEmpty(_type)) return;
            txtMeasureName.Enabled = false;
            txtTypeKey.Enabled = false;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                if (string.IsNullOrEmpty(_type))
                {
                    //add
                    // check 是否已经存在
                    if (_sqLiteHelpers.QueryOne("MeaSurePrgCfg", "PrgName", txtMeasureName.Text.Trim()) != null)
                    {
                        MessageBoxX.Show("该量测节点已经存在！", "提示");
                        return;
                    }

                    // check 是否已经存在
                    if (_sqLiteHelpers.QueryOne("MeaSurePrgCfg", "Type", txtTypeKey.Text.Trim()) != null)
                    {
                        MessageBoxX.Show("该类型码已经存在！", "提示");
                        return;
                    }

                    Dictionary<string, object> dic = new Dictionary<string, object>
                    {
                        {"PrgName", txtMeasureName.Text.Trim()},
                        {"PrgPath", txtMeasureProgram.Text.Trim()},
                        {"Type", txtTypeKey.Text.Trim()},
                        {"CreateDate", DateTime.Now}
                    };

                    int result = _sqLiteHelpers.InsertData("MeaSurePrgCfg", dic);
                    MessageBoxX.Show("录入成功！", "提示");
                }
                else
                {
                    //edit
                    Dictionary<string, object> dic = new Dictionary<string, object>
                    {
                        {"PrgPath", txtMeasureProgram.Text.Trim()},
                        {"CreateDate", DateTime.Now}
                    };
                    SQLiteParameter[] parameter = new SQLiteParameter[] { new SQLiteParameter("Type", txtTypeKey.Text.Trim()) };
                    int result = _sqLiteHelpers.Update("MeaSurePrgCfg", dic, "Type=@Type", parameter);

                    MessageBoxX.Show("修改成功！", "提示");
                }

                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private bool ValidateInput()
        {
            bool isValid = !string.IsNullOrWhiteSpace(txtMeasureName.Text.Trim())
                           && !string.IsNullOrWhiteSpace(txtMeasureProgram.Text.Trim())
                           && !string.IsNullOrWhiteSpace(txtTypeKey.Text.Trim());

            if (!isValid) MessageBoxX.Show("量测程序、节点以及类型码不能为空！", "提示");
            return isValid;
        }

        private void FrmMeasureEdit_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(Pens.Gray, 0, 0, this.Width - 1, this.Height - 1);
        }
    }
}
