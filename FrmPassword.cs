using System;
using System.Drawing;
using System.Windows.Forms;

namespace CMMAuto
{
    public partial class FrmPassword : Form
    {
        // 公开属性，用于获取输入的密码
        public string EnteredPassword { get; set; }
        public FrmPassword()
        {
            InitializeComponent();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            EnteredPassword = txtPassword.Text;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void FrmPassword_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(Pens.Gray, 0, 0, this.Width - 1, this.Height - 1);
        }
    }
}
