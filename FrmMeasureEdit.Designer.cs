
namespace CMMAuto
{
    partial class FrmMeasureEdit
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMeasureEdit));
            this.btnConfirm = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.txtTypeKey = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtMeasureName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtMeasureProgram = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnConfirm
            // 
            this.btnConfirm.BackColor = System.Drawing.Color.LightBlue;
            this.btnConfirm.FlatAppearance.BorderSize = 0;
            this.btnConfirm.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightGray;
            this.btnConfirm.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gainsboro;
            this.btnConfirm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfirm.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnConfirm.ForeColor = System.Drawing.Color.DarkBlue;
            this.btnConfirm.Location = new System.Drawing.Point(178, 221);
            this.btnConfirm.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(96, 42);
            this.btnConfirm.TabIndex = 12;
            this.btnConfirm.Text = "确   认";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.LightBlue;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightGray;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gainsboro;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCancel.ForeColor = System.Drawing.Color.DarkBlue;
            this.btnCancel.Location = new System.Drawing.Point(325, 221);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(96, 42);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "取   消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.ForeColor = System.Drawing.Color.Gray;
            this.label14.Location = new System.Drawing.Point(34, 51);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(56, 17);
            this.label14.TabIndex = 35;
            this.label14.Text = "类型码：";
            // 
            // txtTypeKey
            // 
            this.txtTypeKey.Location = new System.Drawing.Point(132, 48);
            this.txtTypeKey.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtTypeKey.Name = "txtTypeKey";
            this.txtTypeKey.Size = new System.Drawing.Size(425, 23);
            this.txtTypeKey.TabIndex = 34;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.Gray;
            this.label9.Location = new System.Drawing.Point(34, 95);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(68, 17);
            this.label9.TabIndex = 33;
            this.label9.Text = "程式节点：";
            // 
            // txtMeasureName
            // 
            this.txtMeasureName.Location = new System.Drawing.Point(132, 92);
            this.txtMeasureName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtMeasureName.Name = "txtMeasureName";
            this.txtMeasureName.Size = new System.Drawing.Size(425, 23);
            this.txtMeasureName.TabIndex = 32;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Gray;
            this.label1.Location = new System.Drawing.Point(34, 139);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 17);
            this.label1.TabIndex = 31;
            this.label1.Text = "程式路径：";
            // 
            // txtMeasureProgram
            // 
            this.txtMeasureProgram.Location = new System.Drawing.Point(132, 136);
            this.txtMeasureProgram.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtMeasureProgram.Name = "txtMeasureProgram";
            this.txtMeasureProgram.Size = new System.Drawing.Size(425, 23);
            this.txtMeasureProgram.TabIndex = 30;
            // 
            // FrmMeasureEdit
            // 
            this.AcceptButton = this.btnConfirm;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Azure;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(600, 300);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.txtTypeKey);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtMeasureName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtMeasureProgram);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnConfirm);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FrmMeasureEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FrmPassword";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FrmMeasureEdit_Paint);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtTypeKey;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtMeasureName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtMeasureProgram;
    }
}