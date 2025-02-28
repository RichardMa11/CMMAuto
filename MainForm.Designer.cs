
namespace CMMAuto
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.panelMiddle = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelCmmlog = new System.Windows.Forms.Panel();
            this.grpCmmlog = new System.Windows.Forms.GroupBox();
            this.drvCmmLog = new System.Windows.Forms.DataGridView();
            this.grpCmmInfo = new System.Windows.Forms.GroupBox();
            this.btnCmmTestLogQuery = new System.Windows.Forms.Button();
            this.btnClearInfo = new System.Windows.Forms.Button();
            this.btnInputTestPrg = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.txtMeasureName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtMeasureProgram = new System.Windows.Forms.TextBox();
            this.grpLog = new System.Windows.Forms.GroupBox();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.panelMidRight = new System.Windows.Forms.Panel();
            this.grpRight = new System.Windows.Forms.GroupBox();
            this.panelOperate = new System.Windows.Forms.Panel();
            this.panelAgvOperate = new System.Windows.Forms.Panel();
            this.grpAgvOperate = new System.Windows.Forms.GroupBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.grpCmmOperate = new System.Windows.Forms.GroupBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnEnd = new System.Windows.Forms.Button();
            this.btnManual = new System.Windows.Forms.Button();
            this.btnAutoRun = new System.Windows.Forms.Button();
            this.btnOpenFile = new System.Windows.Forms.Button();
            this.btnSaveImg = new System.Windows.Forms.Button();
            this.panelState = new System.Windows.Forms.Panel();
            this.panelAgvState = new System.Windows.Forms.Panel();
            this.grpAgvConnection = new System.Windows.Forms.GroupBox();
            this.txtHeartBeat = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.lblCommInfo = new System.Windows.Forms.Label();
            this.txtAgvDisconnect = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtAgvConnect = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.grpCmmState = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtExit = new System.Windows.Forms.TextBox();
            this.txtPause = new System.Windows.Forms.TextBox();
            this.txtRun = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtOther = new System.Windows.Forms.TextBox();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.timerlog = new System.Windows.Forms.Timer(this.components);
            this.trvTestPrgChoose = new System.Windows.Forms.TreeView();
            this.grpTestPrgChoose = new System.Windows.Forms.GroupBox();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.txtPreOrEnd = new System.Windows.Forms.TextBox();
            this.panelMiddle.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panelCmmlog.SuspendLayout();
            this.grpCmmlog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.drvCmmLog)).BeginInit();
            this.grpCmmInfo.SuspendLayout();
            this.grpLog.SuspendLayout();
            this.panelMidRight.SuspendLayout();
            this.grpRight.SuspendLayout();
            this.panelOperate.SuspendLayout();
            this.panelAgvOperate.SuspendLayout();
            this.grpAgvOperate.SuspendLayout();
            this.grpCmmOperate.SuspendLayout();
            this.panelState.SuspendLayout();
            this.panelAgvState.SuspendLayout();
            this.grpAgvConnection.SuspendLayout();
            this.grpCmmState.SuspendLayout();
            this.grpTestPrgChoose.SuspendLayout();
            this.panelLeft.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelMiddle
            // 
            this.panelMiddle.Controls.Add(this.panel1);
            this.panelMiddle.Controls.Add(this.panelMidRight);
            this.panelMiddle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMiddle.Location = new System.Drawing.Point(350, 0);
            this.panelMiddle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelMiddle.Name = "panelMiddle";
            this.panelMiddle.Size = new System.Drawing.Size(1037, 825);
            this.panelMiddle.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panelCmmlog);
            this.panel1.Controls.Add(this.grpLog);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(687, 825);
            this.panel1.TabIndex = 10;
            // 
            // panelCmmlog
            // 
            this.panelCmmlog.Controls.Add(this.grpCmmlog);
            this.panelCmmlog.Controls.Add(this.grpCmmInfo);
            this.panelCmmlog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCmmlog.Location = new System.Drawing.Point(0, 0);
            this.panelCmmlog.Name = "panelCmmlog";
            this.panelCmmlog.Size = new System.Drawing.Size(687, 529);
            this.panelCmmlog.TabIndex = 3;
            // 
            // grpCmmlog
            // 
            this.grpCmmlog.Controls.Add(this.drvCmmLog);
            this.grpCmmlog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpCmmlog.Location = new System.Drawing.Point(0, 162);
            this.grpCmmlog.Name = "grpCmmlog";
            this.grpCmmlog.Size = new System.Drawing.Size(687, 367);
            this.grpCmmlog.TabIndex = 2;
            this.grpCmmlog.TabStop = false;
            this.grpCmmlog.Text = "测量记录";
            // 
            // drvCmmLog
            // 
            this.drvCmmLog.AllowUserToAddRows = false;
            this.drvCmmLog.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.drvCmmLog.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.drvCmmLog.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.drvCmmLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.drvCmmLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.drvCmmLog.Location = new System.Drawing.Point(3, 19);
            this.drvCmmLog.Name = "drvCmmLog";
            this.drvCmmLog.ReadOnly = true;
            this.drvCmmLog.RowHeadersVisible = false;
            this.drvCmmLog.RowTemplate.Height = 23;
            this.drvCmmLog.Size = new System.Drawing.Size(681, 345);
            this.drvCmmLog.TabIndex = 0;
            // 
            // grpCmmInfo
            // 
            this.grpCmmInfo.Controls.Add(this.btnCmmTestLogQuery);
            this.grpCmmInfo.Controls.Add(this.btnClearInfo);
            this.grpCmmInfo.Controls.Add(this.btnInputTestPrg);
            this.grpCmmInfo.Controls.Add(this.label9);
            this.grpCmmInfo.Controls.Add(this.txtMeasureName);
            this.grpCmmInfo.Controls.Add(this.label1);
            this.grpCmmInfo.Controls.Add(this.txtMeasureProgram);
            this.grpCmmInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpCmmInfo.Location = new System.Drawing.Point(0, 0);
            this.grpCmmInfo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpCmmInfo.Name = "grpCmmInfo";
            this.grpCmmInfo.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpCmmInfo.Size = new System.Drawing.Size(687, 162);
            this.grpCmmInfo.TabIndex = 0;
            this.grpCmmInfo.TabStop = false;
            this.grpCmmInfo.Text = "程序信息";
            // 
            // btnCmmTestLogQuery
            // 
            this.btnCmmTestLogQuery.BackColor = System.Drawing.Color.LightBlue;
            this.btnCmmTestLogQuery.FlatAppearance.BorderSize = 0;
            this.btnCmmTestLogQuery.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightGray;
            this.btnCmmTestLogQuery.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gainsboro;
            this.btnCmmTestLogQuery.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCmmTestLogQuery.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCmmTestLogQuery.ForeColor = System.Drawing.Color.DarkBlue;
            this.btnCmmTestLogQuery.Location = new System.Drawing.Point(454, 107);
            this.btnCmmTestLogQuery.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCmmTestLogQuery.Name = "btnCmmTestLogQuery";
            this.btnCmmTestLogQuery.Size = new System.Drawing.Size(96, 42);
            this.btnCmmTestLogQuery.TabIndex = 8;
            this.btnCmmTestLogQuery.Text = "记录查询";
            this.btnCmmTestLogQuery.UseVisualStyleBackColor = true;
            this.btnCmmTestLogQuery.Click += new System.EventHandler(this.btnCmmTestLogQuery_Click);
            // 
            // btnClearInfo
            // 
            this.btnClearInfo.BackColor = System.Drawing.Color.LightBlue;
            this.btnClearInfo.FlatAppearance.BorderSize = 0;
            this.btnClearInfo.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightGray;
            this.btnClearInfo.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gainsboro;
            this.btnClearInfo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearInfo.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnClearInfo.ForeColor = System.Drawing.Color.DarkBlue;
            this.btnClearInfo.Location = new System.Drawing.Point(301, 107);
            this.btnClearInfo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnClearInfo.Name = "btnClearInfo";
            this.btnClearInfo.Size = new System.Drawing.Size(96, 42);
            this.btnClearInfo.TabIndex = 7;
            this.btnClearInfo.Text = "清   空";
            this.btnClearInfo.UseVisualStyleBackColor = true;
            this.btnClearInfo.Click += new System.EventHandler(this.btnClearInfo_Click);
            // 
            // btnInputTestPrg
            // 
            this.btnInputTestPrg.BackColor = System.Drawing.Color.LightBlue;
            this.btnInputTestPrg.FlatAppearance.BorderSize = 0;
            this.btnInputTestPrg.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightGray;
            this.btnInputTestPrg.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gainsboro;
            this.btnInputTestPrg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInputTestPrg.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnInputTestPrg.ForeColor = System.Drawing.Color.DarkBlue;
            this.btnInputTestPrg.Location = new System.Drawing.Point(148, 107);
            this.btnInputTestPrg.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnInputTestPrg.Name = "btnInputTestPrg";
            this.btnInputTestPrg.Size = new System.Drawing.Size(96, 42);
            this.btnInputTestPrg.TabIndex = 6;
            this.btnInputTestPrg.Text = "程序录入";
            this.btnInputTestPrg.UseVisualStyleBackColor = true;
            this.btnInputTestPrg.Click += new System.EventHandler(this.btnInputTestPrg_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.Gray;
            this.label9.Location = new System.Drawing.Point(14, 30);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(68, 17);
            this.label9.TabIndex = 5;
            this.label9.Text = "程序节点：";
            // 
            // txtMeasureName
            // 
            this.txtMeasureName.Location = new System.Drawing.Point(112, 27);
            this.txtMeasureName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtMeasureName.Name = "txtMeasureName";
            this.txtMeasureName.Size = new System.Drawing.Size(492, 23);
            this.txtMeasureName.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Gray;
            this.label1.Location = new System.Drawing.Point(14, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "程序路径：";
            // 
            // txtMeasureProgram
            // 
            this.txtMeasureProgram.Location = new System.Drawing.Point(112, 67);
            this.txtMeasureProgram.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtMeasureProgram.Name = "txtMeasureProgram";
            this.txtMeasureProgram.Size = new System.Drawing.Size(492, 23);
            this.txtMeasureProgram.TabIndex = 2;
            // 
            // grpLog
            // 
            this.grpLog.Controls.Add(this.txtLog);
            this.grpLog.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grpLog.Location = new System.Drawing.Point(0, 529);
            this.grpLog.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpLog.Name = "grpLog";
            this.grpLog.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpLog.Size = new System.Drawing.Size(687, 296);
            this.grpLog.TabIndex = 1;
            this.grpLog.TabStop = false;
            this.grpLog.Text = "系统日志";
            // 
            // txtLog
            // 
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog.Location = new System.Drawing.Point(3, 20);
            this.txtLog.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.Size = new System.Drawing.Size(681, 272);
            this.txtLog.TabIndex = 0;
            // 
            // panelMidRight
            // 
            this.panelMidRight.Controls.Add(this.grpRight);
            this.panelMidRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelMidRight.Location = new System.Drawing.Point(687, 0);
            this.panelMidRight.Name = "panelMidRight";
            this.panelMidRight.Size = new System.Drawing.Size(350, 825);
            this.panelMidRight.TabIndex = 9;
            // 
            // grpRight
            // 
            this.grpRight.Controls.Add(this.panelOperate);
            this.grpRight.Controls.Add(this.panelState);
            this.grpRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpRight.Location = new System.Drawing.Point(0, 0);
            this.grpRight.Name = "grpRight";
            this.grpRight.Size = new System.Drawing.Size(350, 825);
            this.grpRight.TabIndex = 8;
            this.grpRight.TabStop = false;
            // 
            // panelOperate
            // 
            this.panelOperate.Controls.Add(this.panelAgvOperate);
            this.panelOperate.Controls.Add(this.grpCmmOperate);
            this.panelOperate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelOperate.Location = new System.Drawing.Point(3, 349);
            this.panelOperate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelOperate.Name = "panelOperate";
            this.panelOperate.Size = new System.Drawing.Size(344, 473);
            this.panelOperate.TabIndex = 5;
            // 
            // panelAgvOperate
            // 
            this.panelAgvOperate.Controls.Add(this.grpAgvOperate);
            this.panelAgvOperate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelAgvOperate.Location = new System.Drawing.Point(0, 180);
            this.panelAgvOperate.Name = "panelAgvOperate";
            this.panelAgvOperate.Size = new System.Drawing.Size(344, 293);
            this.panelAgvOperate.TabIndex = 5;
            // 
            // grpAgvOperate
            // 
            this.grpAgvOperate.Controls.Add(this.btnSend);
            this.grpAgvOperate.Controls.Add(this.btnDisconnect);
            this.grpAgvOperate.Controls.Add(this.btnConnect);
            this.grpAgvOperate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpAgvOperate.Location = new System.Drawing.Point(0, 0);
            this.grpAgvOperate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpAgvOperate.Name = "grpAgvOperate";
            this.grpAgvOperate.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpAgvOperate.Size = new System.Drawing.Size(344, 293);
            this.grpAgvOperate.TabIndex = 4;
            this.grpAgvOperate.TabStop = false;
            this.grpAgvOperate.Text = "AGV操作";
            // 
            // btnSend
            // 
            this.btnSend.BackColor = System.Drawing.Color.LightBlue;
            this.btnSend.FlatAppearance.BorderSize = 0;
            this.btnSend.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightGray;
            this.btnSend.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gainsboro;
            this.btnSend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSend.ForeColor = System.Drawing.Color.DarkBlue;
            this.btnSend.Location = new System.Drawing.Point(52, 76);
            this.btnSend.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(96, 42);
            this.btnSend.TabIndex = 3;
            this.btnSend.Text = "发送信息";
            this.btnSend.UseVisualStyleBackColor = true;
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.BackColor = System.Drawing.Color.LightBlue;
            this.btnDisconnect.FlatAppearance.BorderSize = 0;
            this.btnDisconnect.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightGray;
            this.btnDisconnect.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gainsboro;
            this.btnDisconnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDisconnect.ForeColor = System.Drawing.Color.DarkBlue;
            this.btnDisconnect.Location = new System.Drawing.Point(194, 26);
            this.btnDisconnect.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(96, 42);
            this.btnDisconnect.TabIndex = 2;
            this.btnDisconnect.Text = "断开连接";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.BackColor = System.Drawing.Color.LightBlue;
            this.btnConnect.FlatAppearance.BorderSize = 0;
            this.btnConnect.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightGray;
            this.btnConnect.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gainsboro;
            this.btnConnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConnect.ForeColor = System.Drawing.Color.DarkBlue;
            this.btnConnect.Location = new System.Drawing.Point(52, 26);
            this.btnConnect.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(96, 42);
            this.btnConnect.TabIndex = 1;
            this.btnConnect.Text = "开始连接";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // grpCmmOperate
            // 
            this.grpCmmOperate.Controls.Add(this.btnClear);
            this.grpCmmOperate.Controls.Add(this.btnEnd);
            this.grpCmmOperate.Controls.Add(this.btnManual);
            this.grpCmmOperate.Controls.Add(this.btnAutoRun);
            this.grpCmmOperate.Controls.Add(this.btnOpenFile);
            this.grpCmmOperate.Controls.Add(this.btnSaveImg);
            this.grpCmmOperate.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpCmmOperate.Location = new System.Drawing.Point(0, 0);
            this.grpCmmOperate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpCmmOperate.Name = "grpCmmOperate";
            this.grpCmmOperate.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpCmmOperate.Size = new System.Drawing.Size(344, 180);
            this.grpCmmOperate.TabIndex = 1;
            this.grpCmmOperate.TabStop = false;
            this.grpCmmOperate.Text = "CMM操作";
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.LightBlue;
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightGray;
            this.btnClear.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gainsboro;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.ForeColor = System.Drawing.Color.DarkBlue;
            this.btnClear.Location = new System.Drawing.Point(194, 125);
            this.btnClear.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(96, 42);
            this.btnClear.TabIndex = 7;
            this.btnClear.Text = "测  试";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnEnd
            // 
            this.btnEnd.BackColor = System.Drawing.Color.LightBlue;
            this.btnEnd.FlatAppearance.BorderSize = 0;
            this.btnEnd.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightGray;
            this.btnEnd.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gainsboro;
            this.btnEnd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEnd.ForeColor = System.Drawing.Color.DarkBlue;
            this.btnEnd.Location = new System.Drawing.Point(52, 125);
            this.btnEnd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnEnd.Name = "btnEnd";
            this.btnEnd.Size = new System.Drawing.Size(96, 42);
            this.btnEnd.TabIndex = 6;
            this.btnEnd.Text = "退  出";
            this.btnEnd.UseVisualStyleBackColor = true;
            this.btnEnd.Click += new System.EventHandler(this.btnEnd_Click);
            // 
            // btnManual
            // 
            this.btnManual.BackColor = System.Drawing.Color.LightBlue;
            this.btnManual.FlatAppearance.BorderSize = 0;
            this.btnManual.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightGray;
            this.btnManual.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gainsboro;
            this.btnManual.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnManual.ForeColor = System.Drawing.Color.DarkBlue;
            this.btnManual.Location = new System.Drawing.Point(194, 74);
            this.btnManual.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnManual.Name = "btnManual";
            this.btnManual.Size = new System.Drawing.Size(96, 42);
            this.btnManual.TabIndex = 5;
            this.btnManual.Text = "手动测量";
            this.btnManual.UseVisualStyleBackColor = true;
            this.btnManual.Click += new System.EventHandler(this.btnManual_Click);
            // 
            // btnAutoRun
            // 
            this.btnAutoRun.BackColor = System.Drawing.Color.LightBlue;
            this.btnAutoRun.FlatAppearance.BorderSize = 0;
            this.btnAutoRun.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightGray;
            this.btnAutoRun.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gainsboro;
            this.btnAutoRun.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAutoRun.ForeColor = System.Drawing.Color.DarkBlue;
            this.btnAutoRun.Location = new System.Drawing.Point(52, 74);
            this.btnAutoRun.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnAutoRun.Name = "btnAutoRun";
            this.btnAutoRun.Size = new System.Drawing.Size(96, 42);
            this.btnAutoRun.TabIndex = 4;
            this.btnAutoRun.Text = "自动测量";
            this.btnAutoRun.UseVisualStyleBackColor = true;
            this.btnAutoRun.Click += new System.EventHandler(this.btnAutoRun_Click);
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.BackColor = System.Drawing.Color.LightBlue;
            this.btnOpenFile.FlatAppearance.BorderSize = 0;
            this.btnOpenFile.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightGray;
            this.btnOpenFile.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gainsboro;
            this.btnOpenFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenFile.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOpenFile.ForeColor = System.Drawing.Color.DarkBlue;
            this.btnOpenFile.Location = new System.Drawing.Point(52, 23);
            this.btnOpenFile.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size(96, 42);
            this.btnOpenFile.TabIndex = 0;
            this.btnOpenFile.Text = "测  试2";
            this.btnOpenFile.UseVisualStyleBackColor = true;
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // btnSaveImg
            // 
            this.btnSaveImg.BackColor = System.Drawing.Color.LightBlue;
            this.btnSaveImg.FlatAppearance.BorderSize = 0;
            this.btnSaveImg.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightGray;
            this.btnSaveImg.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gainsboro;
            this.btnSaveImg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveImg.ForeColor = System.Drawing.Color.DarkBlue;
            this.btnSaveImg.Location = new System.Drawing.Point(194, 23);
            this.btnSaveImg.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSaveImg.Name = "btnSaveImg";
            this.btnSaveImg.Size = new System.Drawing.Size(96, 42);
            this.btnSaveImg.TabIndex = 1;
            this.btnSaveImg.Text = "截屏并读取";
            this.btnSaveImg.UseVisualStyleBackColor = true;
            this.btnSaveImg.Click += new System.EventHandler(this.btnSaveImg_Click);
            // 
            // panelState
            // 
            this.panelState.Controls.Add(this.panelAgvState);
            this.panelState.Controls.Add(this.grpCmmState);
            this.panelState.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelState.Location = new System.Drawing.Point(3, 19);
            this.panelState.Name = "panelState";
            this.panelState.Size = new System.Drawing.Size(344, 330);
            this.panelState.TabIndex = 6;
            // 
            // panelAgvState
            // 
            this.panelAgvState.Controls.Add(this.grpAgvConnection);
            this.panelAgvState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelAgvState.Location = new System.Drawing.Point(0, 143);
            this.panelAgvState.Name = "panelAgvState";
            this.panelAgvState.Size = new System.Drawing.Size(344, 187);
            this.panelAgvState.TabIndex = 4;
            // 
            // grpAgvConnection
            // 
            this.grpAgvConnection.Controls.Add(this.txtHeartBeat);
            this.grpAgvConnection.Controls.Add(this.label8);
            this.grpAgvConnection.Controls.Add(this.lblCommInfo);
            this.grpAgvConnection.Controls.Add(this.txtAgvDisconnect);
            this.grpAgvConnection.Controls.Add(this.label7);
            this.grpAgvConnection.Controls.Add(this.txtAgvConnect);
            this.grpAgvConnection.Controls.Add(this.label6);
            this.grpAgvConnection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpAgvConnection.Location = new System.Drawing.Point(0, 0);
            this.grpAgvConnection.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpAgvConnection.Name = "grpAgvConnection";
            this.grpAgvConnection.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpAgvConnection.Size = new System.Drawing.Size(344, 187);
            this.grpAgvConnection.TabIndex = 3;
            this.grpAgvConnection.TabStop = false;
            this.grpAgvConnection.Text = "AGV通讯";
            // 
            // txtHeartBeat
            // 
            this.txtHeartBeat.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtHeartBeat.Location = new System.Drawing.Point(105, 96);
            this.txtHeartBeat.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtHeartBeat.Name = "txtHeartBeat";
            this.txtHeartBeat.Size = new System.Drawing.Size(21, 16);
            this.txtHeartBeat.TabIndex = 10;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(64, 96);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(32, 17);
            this.label8.TabIndex = 9;
            this.label8.Text = "心跳";
            // 
            // lblCommInfo
            // 
            this.lblCommInfo.AutoSize = true;
            this.lblCommInfo.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblCommInfo.ForeColor = System.Drawing.Color.Red;
            this.lblCommInfo.Location = new System.Drawing.Point(118, 148);
            this.lblCommInfo.Name = "lblCommInfo";
            this.lblCommInfo.Size = new System.Drawing.Size(103, 16);
            this.lblCommInfo.TabIndex = 8;
            this.lblCommInfo.Text = "AGV通讯中断";
            this.lblCommInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtAgvDisconnect
            // 
            this.txtAgvDisconnect.BackColor = System.Drawing.Color.Red;
            this.txtAgvDisconnect.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtAgvDisconnect.Location = new System.Drawing.Point(200, 45);
            this.txtAgvDisconnect.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtAgvDisconnect.Name = "txtAgvDisconnect";
            this.txtAgvDisconnect.Size = new System.Drawing.Size(21, 16);
            this.txtAgvDisconnect.TabIndex = 7;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(159, 45);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(32, 17);
            this.label7.TabIndex = 6;
            this.label7.Text = "断开";
            // 
            // txtAgvConnect
            // 
            this.txtAgvConnect.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtAgvConnect.Location = new System.Drawing.Point(105, 45);
            this.txtAgvConnect.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtAgvConnect.Name = "txtAgvConnect";
            this.txtAgvConnect.Size = new System.Drawing.Size(21, 16);
            this.txtAgvConnect.TabIndex = 5;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(64, 45);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(32, 17);
            this.label6.TabIndex = 4;
            this.label6.Text = "连接";
            // 
            // grpCmmState
            // 
            this.grpCmmState.Controls.Add(this.label10);
            this.grpCmmState.Controls.Add(this.txtPreOrEnd);
            this.grpCmmState.Controls.Add(this.label5);
            this.grpCmmState.Controls.Add(this.label4);
            this.grpCmmState.Controls.Add(this.txtExit);
            this.grpCmmState.Controls.Add(this.txtPause);
            this.grpCmmState.Controls.Add(this.txtRun);
            this.grpCmmState.Controls.Add(this.label3);
            this.grpCmmState.Controls.Add(this.label2);
            this.grpCmmState.Controls.Add(this.txtOther);
            this.grpCmmState.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpCmmState.Location = new System.Drawing.Point(0, 0);
            this.grpCmmState.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpCmmState.Name = "grpCmmState";
            this.grpCmmState.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpCmmState.Size = new System.Drawing.Size(344, 143);
            this.grpCmmState.TabIndex = 0;
            this.grpCmmState.TabStop = false;
            this.grpCmmState.Text = "CMM软件状态";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(64, 95);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(32, 17);
            this.label5.TabIndex = 7;
            this.label5.Text = "退出";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label4.Location = new System.Drawing.Point(159, 95);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 17);
            this.label4.TabIndex = 6;
            this.label4.Text = "其他";
            // 
            // txtExit
            // 
            this.txtExit.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtExit.Location = new System.Drawing.Point(105, 95);
            this.txtExit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtExit.Name = "txtExit";
            this.txtExit.Size = new System.Drawing.Size(21, 16);
            this.txtExit.TabIndex = 5;
            // 
            // txtPause
            // 
            this.txtPause.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPause.Location = new System.Drawing.Point(290, 44);
            this.txtPause.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtPause.Name = "txtPause";
            this.txtPause.Size = new System.Drawing.Size(21, 16);
            this.txtPause.TabIndex = 4;
            // 
            // txtRun
            // 
            this.txtRun.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtRun.Location = new System.Drawing.Point(200, 44);
            this.txtRun.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtRun.Name = "txtRun";
            this.txtRun.Size = new System.Drawing.Size(21, 16);
            this.txtRun.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(249, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "暂停";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(159, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "运行";
            // 
            // txtOther
            // 
            this.txtOther.BackColor = System.Drawing.Color.LimeGreen;
            this.txtOther.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtOther.Location = new System.Drawing.Point(200, 95);
            this.txtOther.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtOther.Name = "txtOther";
            this.txtOther.Size = new System.Drawing.Size(21, 16);
            this.txtOther.TabIndex = 0;
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            // 
            // timerlog
            // 
            this.timerlog.Enabled = true;
            this.timerlog.Interval = 1000;
            this.timerlog.Tick += new System.EventHandler(this.timerlog_Tick);
            // 
            // trvTestPrgChoose
            // 
            this.trvTestPrgChoose.CheckBoxes = true;
            this.trvTestPrgChoose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trvTestPrgChoose.Location = new System.Drawing.Point(3, 19);
            this.trvTestPrgChoose.Name = "trvTestPrgChoose";
            this.trvTestPrgChoose.Size = new System.Drawing.Size(344, 803);
            this.trvTestPrgChoose.TabIndex = 2;
            this.trvTestPrgChoose.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.trvTestPrgChoose_AfterCheck);
            // 
            // grpTestPrgChoose
            // 
            this.grpTestPrgChoose.Controls.Add(this.trvTestPrgChoose);
            this.grpTestPrgChoose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpTestPrgChoose.Location = new System.Drawing.Point(0, 0);
            this.grpTestPrgChoose.Name = "grpTestPrgChoose";
            this.grpTestPrgChoose.Size = new System.Drawing.Size(350, 825);
            this.grpTestPrgChoose.TabIndex = 4;
            this.grpTestPrgChoose.TabStop = false;
            this.grpTestPrgChoose.Text = "程序信息列表";
            // 
            // panelLeft
            // 
            this.panelLeft.Controls.Add(this.grpTestPrgChoose);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(0, 0);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(350, 825);
            this.panelLeft.TabIndex = 5;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label10.Location = new System.Drawing.Point(32, 44);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(64, 17);
            this.label10.TabIndex = 9;
            this.label10.Text = "准备(结束)";
            // 
            // txtPreOrEnd
            // 
            this.txtPreOrEnd.BackColor = System.Drawing.Color.White;
            this.txtPreOrEnd.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPreOrEnd.Location = new System.Drawing.Point(105, 44);
            this.txtPreOrEnd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtPreOrEnd.Name = "txtPreOrEnd";
            this.txtPreOrEnd.Size = new System.Drawing.Size(21, 16);
            this.txtPreOrEnd.TabIndex = 8;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1387, 825);
            this.Controls.Add(this.panelMiddle);
            this.Controls.Add(this.panelLeft);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AutoPCD";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.panelMiddle.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panelCmmlog.ResumeLayout(false);
            this.grpCmmlog.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.drvCmmLog)).EndInit();
            this.grpCmmInfo.ResumeLayout(false);
            this.grpCmmInfo.PerformLayout();
            this.grpLog.ResumeLayout(false);
            this.grpLog.PerformLayout();
            this.panelMidRight.ResumeLayout(false);
            this.grpRight.ResumeLayout(false);
            this.panelOperate.ResumeLayout(false);
            this.panelAgvOperate.ResumeLayout(false);
            this.grpAgvOperate.ResumeLayout(false);
            this.grpCmmOperate.ResumeLayout(false);
            this.panelState.ResumeLayout(false);
            this.panelAgvState.ResumeLayout(false);
            this.grpAgvConnection.ResumeLayout(false);
            this.grpAgvConnection.PerformLayout();
            this.grpCmmState.ResumeLayout(false);
            this.grpCmmState.PerformLayout();
            this.grpTestPrgChoose.ResumeLayout(false);
            this.panelLeft.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelMiddle;
        private System.Windows.Forms.GroupBox grpLog;
        private System.Windows.Forms.GroupBox grpCmmInfo;
        private System.Windows.Forms.GroupBox grpCmmState;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Timer timerlog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtMeasureProgram;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtExit;
        private System.Windows.Forms.TextBox txtPause;
        private System.Windows.Forms.TextBox txtRun;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtOther;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtMeasureName;
        private System.Windows.Forms.Button btnInputTestPrg;
        private System.Windows.Forms.Button btnCmmTestLogQuery;
        private System.Windows.Forms.Button btnClearInfo;
        private System.Windows.Forms.TreeView trvTestPrgChoose;
        private System.Windows.Forms.GroupBox grpTestPrgChoose;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.Panel panelOperate;
        private System.Windows.Forms.GroupBox grpAgvConnection;
        private System.Windows.Forms.TextBox txtHeartBeat;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblCommInfo;
        private System.Windows.Forms.TextBox txtAgvDisconnect;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtAgvConnect;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox grpCmmOperate;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnEnd;
        private System.Windows.Forms.Button btnManual;
        private System.Windows.Forms.Button btnAutoRun;
        private System.Windows.Forms.Button btnOpenFile;
        private System.Windows.Forms.Button btnSaveImg;
        private System.Windows.Forms.GroupBox grpAgvOperate;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Panel panelState;
        private System.Windows.Forms.Panel panelAgvState;
        private System.Windows.Forms.Panel panelAgvOperate;
        private System.Windows.Forms.GroupBox grpRight;
        private System.Windows.Forms.GroupBox grpCmmlog;
        private System.Windows.Forms.DataGridView drvCmmLog;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panelMidRight;
        private System.Windows.Forms.Panel panelCmmlog;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtPreOrEnd;
    }
}

