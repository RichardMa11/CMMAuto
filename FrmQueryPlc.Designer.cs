
namespace CMMAuto
{
    partial class FrmQueryPlc
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmQueryPlc));
            this.mainPanel = new System.Windows.Forms.TableLayoutPanel();
            this.bottomPanel = new System.Windows.Forms.Panel();
            this.btnRW = new System.Windows.Forms.Button();
            this.grvConfig = new System.Windows.Forms.DataGridView();
            this.txtWrite = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtRead = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.mainPanel.SuspendLayout();
            this.bottomPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grvConfig)).BeginInit();
            this.SuspendLayout();
            // 
            // mainPanel
            // 
            this.mainPanel.ColumnCount = 1;
            this.mainPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainPanel.Controls.Add(this.bottomPanel, 0, 1);
            this.mainPanel.Controls.Add(this.grvConfig, 0, 0);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(0, 0);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.RowCount = 2;
            this.mainPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.mainPanel.Size = new System.Drawing.Size(780, 457);
            this.mainPanel.TabIndex = 0;
            // 
            // bottomPanel
            // 
            this.bottomPanel.BackColor = System.Drawing.Color.White;
            this.bottomPanel.Controls.Add(this.btnRefresh);
            this.bottomPanel.Controls.Add(this.txtRead);
            this.bottomPanel.Controls.Add(this.label1);
            this.bottomPanel.Controls.Add(this.txtWrite);
            this.bottomPanel.Controls.Add(this.label13);
            this.bottomPanel.Controls.Add(this.btnRW);
            this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bottomPanel.Location = new System.Drawing.Point(3, 400);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Size = new System.Drawing.Size(774, 54);
            this.bottomPanel.TabIndex = 3;
            // 
            // btnRW
            // 
            this.btnRW.BackColor = System.Drawing.Color.LightBlue;
            this.btnRW.FlatAppearance.BorderSize = 0;
            this.btnRW.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightGray;
            this.btnRW.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gainsboro;
            this.btnRW.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRW.ForeColor = System.Drawing.Color.DarkBlue;
            this.btnRW.Location = new System.Drawing.Point(20, 7);
            this.btnRW.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnRW.Name = "btnRW";
            this.btnRW.Size = new System.Drawing.Size(96, 42);
            this.btnRW.TabIndex = 2;
            this.btnRW.Text = "读  取";
            this.btnRW.UseVisualStyleBackColor = true;
            this.btnRW.Click += new System.EventHandler(this.btnRW_Click);
            // 
            // grvConfig
            // 
            this.grvConfig.AllowUserToAddRows = false;
            this.grvConfig.BackgroundColor = System.Drawing.Color.Azure;
            this.grvConfig.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.grvConfig.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Azure;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grvConfig.DefaultCellStyle = dataGridViewCellStyle1;
            this.grvConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grvConfig.Location = new System.Drawing.Point(3, 3);
            this.grvConfig.Name = "grvConfig";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Azure;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(2);
            this.grvConfig.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.grvConfig.Size = new System.Drawing.Size(774, 391);
            this.grvConfig.TabIndex = 4;
            this.grvConfig.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.grvConfig_CellValidating);
            // 
            // txtWrite
            // 
            this.txtWrite.Location = new System.Drawing.Point(210, 16);
            this.txtWrite.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtWrite.Name = "txtWrite";
            this.txtWrite.Size = new System.Drawing.Size(116, 23);
            this.txtWrite.TabIndex = 18;
            this.txtWrite.Text = "ABCD00001";
            this.txtWrite.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ForeColor = System.Drawing.Color.Gray;
            this.label13.Location = new System.Drawing.Point(137, 19);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(56, 17);
            this.label13.TabIndex = 17;
            this.label13.Text = "写入值：";
            // 
            // txtRead
            // 
            this.txtRead.Location = new System.Drawing.Point(427, 16);
            this.txtRead.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtRead.Name = "txtRead";
            this.txtRead.Size = new System.Drawing.Size(116, 23);
            this.txtRead.TabIndex = 20;
            this.txtRead.Text = "ABCD00001";
            this.txtRead.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Gray;
            this.label1.Location = new System.Drawing.Point(354, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 17);
            this.label1.TabIndex = 19;
            this.label1.Text = "读取值：";
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.LightBlue;
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightGray;
            this.btnRefresh.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gainsboro;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.ForeColor = System.Drawing.Color.DarkBlue;
            this.btnRefresh.Location = new System.Drawing.Point(669, 6);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(96, 42);
            this.btnRefresh.TabIndex = 21;
            this.btnRefresh.Text = "刷  新";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // FrmQueryPlc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Azure;
            this.ClientSize = new System.Drawing.Size(780, 457);
            this.Controls.Add(this.mainPanel);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FrmQueryPlc";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PLC值查询";
            this.mainPanel.ResumeLayout(false);
            this.bottomPanel.ResumeLayout(false);
            this.bottomPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grvConfig)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel mainPanel;
        private System.Windows.Forms.Button btnRW;
        private System.Windows.Forms.Panel bottomPanel;
        private System.Windows.Forms.DataGridView grvConfig;
        private System.Windows.Forms.TextBox txtRead;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtWrite;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button btnRefresh;
    }
}