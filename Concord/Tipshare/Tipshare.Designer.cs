namespace Tipshare
{
    partial class Tipshare
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
            this.components = new System.ComponentModel.Container();
            this.gbAM = new System.Windows.Forms.GroupBox();
            this.tbAMUnallocatedAdj = new System.Windows.Forms.TextBox();
            this.lblAMCurrentUnallocated = new System.Windows.Forms.Label();
            this.tbAMUnallocatedSugg = new System.Windows.Forms.TextBox();
            this.lblAMUnallocated = new System.Windows.Forms.Label();
            this.lAMAdj = new System.Windows.Forms.Label();
            this.lAMSugg = new System.Windows.Forms.Label();
            this.lAMID = new System.Windows.Forms.Label();
            this.lAMName = new System.Windows.Forms.Label();
            this.gbControls = new System.Windows.Forms.GroupBox();
            this.btnDistributeUnallocated = new System.Windows.Forms.Button();
            this.btnSettings = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.lDate = new System.Windows.Forms.Label();
            this.lDay = new System.Windows.Forms.Label();
            this.bDayBack = new System.Windows.Forms.Button();
            this.bDayForward = new System.Windows.Forms.Button();
            this.gbPM = new System.Windows.Forms.GroupBox();
            this.tbPMUnallocatedAdj = new System.Windows.Forms.TextBox();
            this.lblPMCurrentUnallocated = new System.Windows.Forms.Label();
            this.tbPMUnallocatedSugg = new System.Windows.Forms.TextBox();
            this.lblPMUnallocated = new System.Windows.Forms.Label();
            this.lPMAdj = new System.Windows.Forms.Label();
            this.lPMSugg = new System.Windows.Forms.Label();
            this.lPMID = new System.Windows.Forms.Label();
            this.lPMName = new System.Windows.Forms.Label();
            this.msMenu = new System.Windows.Forms.MenuStrip();
            this.mFile = new System.Windows.Forms.ToolStripMenuItem();
            this.miJump = new System.Windows.Forms.ToolStripMenuItem();
            this.miPrint = new System.Windows.Forms.ToolStripMenuItem();
            this.miExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.miHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.miAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.tTimer = new System.Windows.Forms.Timer(this.components);
            this.gbTotals = new System.Windows.Forms.GroupBox();
            this.lTotalSugg = new System.Windows.Forms.Label();
            this.lTotalAdj = new System.Windows.Forms.Label();
            this.lLabTotSug = new System.Windows.Forms.Label();
            this.lTotalAMSugg = new System.Windows.Forms.Label();
            this.lTotalAMAdj = new System.Windows.Forms.Label();
            this.lLabTotAMSugg = new System.Windows.Forms.Label();
            this.lTotalPMSugg = new System.Windows.Forms.Label();
            this.lTotalPMAdj = new System.Windows.Forms.Label();
            this.lLabTotPMSugg = new System.Windows.Forms.Label();
            this.gbAM.SuspendLayout();
            this.gbControls.SuspendLayout();
            this.gbPM.SuspendLayout();
            this.msMenu.SuspendLayout();
            this.gbTotals.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbAM
            // 
            this.gbAM.Controls.Add(this.tbAMUnallocatedAdj);
            this.gbAM.Controls.Add(this.lblAMCurrentUnallocated);
            this.gbAM.Controls.Add(this.tbAMUnallocatedSugg);
            this.gbAM.Controls.Add(this.lblAMUnallocated);
            this.gbAM.Controls.Add(this.lAMAdj);
            this.gbAM.Controls.Add(this.lAMSugg);
            this.gbAM.Controls.Add(this.lAMID);
            this.gbAM.Controls.Add(this.lAMName);
            this.gbAM.Location = new System.Drawing.Point(12, 27);
            this.gbAM.Name = "gbAM";
            this.gbAM.Size = new System.Drawing.Size(318, 430);
            this.gbAM.TabIndex = 1;
            this.gbAM.TabStop = false;
            this.gbAM.Text = "AM Declarations";
            // 
            // tbAMUnallocatedAdj
            // 
            this.tbAMUnallocatedAdj.Location = new System.Drawing.Point(277, 404);
            this.tbAMUnallocatedAdj.Name = "tbAMUnallocatedAdj";
            this.tbAMUnallocatedAdj.ReadOnly = true;
            this.tbAMUnallocatedAdj.Size = new System.Drawing.Size(35, 20);
            this.tbAMUnallocatedAdj.TabIndex = 14;
            // 
            // lblAMCurrentUnallocated
            // 
            this.lblAMCurrentUnallocated.AutoSize = true;
            this.lblAMCurrentUnallocated.Location = new System.Drawing.Point(167, 407);
            this.lblAMCurrentUnallocated.Name = "lblAMCurrentUnallocated";
            this.lblAMCurrentUnallocated.Size = new System.Drawing.Size(104, 13);
            this.lblAMCurrentUnallocated.TabIndex = 13;
            this.lblAMCurrentUnallocated.Text = "Current Unallocated:";
            // 
            // tbAMUnallocatedSugg
            // 
            this.tbAMUnallocatedSugg.Location = new System.Drawing.Point(118, 404);
            this.tbAMUnallocatedSugg.Name = "tbAMUnallocatedSugg";
            this.tbAMUnallocatedSugg.ReadOnly = true;
            this.tbAMUnallocatedSugg.Size = new System.Drawing.Size(35, 20);
            this.tbAMUnallocatedSugg.TabIndex = 12;
            // 
            // lblAMUnallocated
            // 
            this.lblAMUnallocated.AutoSize = true;
            this.lblAMUnallocated.Location = new System.Drawing.Point(6, 407);
            this.lblAMUnallocated.Name = "lblAMUnallocated";
            this.lblAMUnallocated.Size = new System.Drawing.Size(106, 13);
            this.lblAMUnallocated.TabIndex = 10;
            this.lblAMUnallocated.Text = "Starting Unallocated:";
            // 
            // lAMAdj
            // 
            this.lAMAdj.AutoSize = true;
            this.lAMAdj.Location = new System.Drawing.Point(276, 16);
            this.lAMAdj.Name = "lAMAdj";
            this.lAMAdj.Size = new System.Drawing.Size(22, 13);
            this.lAMAdj.TabIndex = 9;
            this.lAMAdj.Text = "Adj";
            // 
            // lAMSugg
            // 
            this.lAMSugg.AutoSize = true;
            this.lAMSugg.Location = new System.Drawing.Point(216, 16);
            this.lAMSugg.Name = "lAMSugg";
            this.lAMSugg.Size = new System.Drawing.Size(32, 13);
            this.lAMSugg.TabIndex = 8;
            this.lAMSugg.Text = "Sugg";
            // 
            // lAMID
            // 
            this.lAMID.AutoSize = true;
            this.lAMID.Location = new System.Drawing.Point(169, 16);
            this.lAMID.Name = "lAMID";
            this.lAMID.Size = new System.Drawing.Size(18, 13);
            this.lAMID.TabIndex = 7;
            this.lAMID.Text = "ID";
            // 
            // lAMName
            // 
            this.lAMName.AutoSize = true;
            this.lAMName.Location = new System.Drawing.Point(69, 16);
            this.lAMName.Name = "lAMName";
            this.lAMName.Size = new System.Drawing.Size(35, 13);
            this.lAMName.TabIndex = 6;
            this.lAMName.Text = "Name";
            // 
            // gbControls
            // 
            this.gbControls.Controls.Add(this.btnDistributeUnallocated);
            this.gbControls.Controls.Add(this.btnSettings);
            this.gbControls.Controls.Add(this.btnReset);
            this.gbControls.Controls.Add(this.btnSave);
            this.gbControls.Controls.Add(this.lDate);
            this.gbControls.Controls.Add(this.lDay);
            this.gbControls.Controls.Add(this.bDayBack);
            this.gbControls.Controls.Add(this.bDayForward);
            this.gbControls.Location = new System.Drawing.Point(12, 518);
            this.gbControls.Name = "gbControls";
            this.gbControls.Size = new System.Drawing.Size(648, 62);
            this.gbControls.TabIndex = 2;
            this.gbControls.TabStop = false;
            this.gbControls.Text = "Controls";
            // 
            // btnDistributeUnallocated
            // 
            this.btnDistributeUnallocated.Enabled = false;
            this.btnDistributeUnallocated.Location = new System.Drawing.Point(123, 19);
            this.btnDistributeUnallocated.Name = "btnDistributeUnallocated";
            this.btnDistributeUnallocated.Size = new System.Drawing.Size(111, 35);
            this.btnDistributeUnallocated.TabIndex = 13;
            this.btnDistributeUnallocated.Text = "Distribute Unallocated";
            this.btnDistributeUnallocated.UseVisualStyleBackColor = true;
            this.btnDistributeUnallocated.Click += new System.EventHandler(this.btnDistributeUnallocated_Click);
            // 
            // btnSettings
            // 
            this.btnSettings.Location = new System.Drawing.Point(6, 19);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(111, 35);
            this.btnSettings.TabIndex = 12;
            this.btnSettings.Text = "Advanced Settings";
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(414, 19);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(111, 35);
            this.btnReset.TabIndex = 11;
            this.btnReset.Text = "Reset Day";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(531, 19);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(111, 35);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "SAVE";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lDate
            // 
            this.lDate.AutoSize = true;
            this.lDate.Location = new System.Drawing.Point(297, 19);
            this.lDate.Name = "lDate";
            this.lDate.Size = new System.Drawing.Size(64, 13);
            this.lDate.TabIndex = 9;
            this.lDate.Text = "##DATE##";
            // 
            // lDay
            // 
            this.lDay.AutoSize = true;
            this.lDay.Location = new System.Drawing.Point(308, 41);
            this.lDay.Name = "lDay";
            this.lDay.Size = new System.Drawing.Size(26, 13);
            this.lDay.TabIndex = 8;
            this.lDay.Text = "Day";
            // 
            // bDayBack
            // 
            this.bDayBack.Location = new System.Drawing.Point(263, 35);
            this.bDayBack.Name = "bDayBack";
            this.bDayBack.Size = new System.Drawing.Size(39, 23);
            this.bDayBack.TabIndex = 1;
            this.bDayBack.Text = "<<";
            this.bDayBack.UseVisualStyleBackColor = true;
            this.bDayBack.Click += new System.EventHandler(this.bDayBack_Click);
            // 
            // bDayForward
            // 
            this.bDayForward.Location = new System.Drawing.Point(340, 36);
            this.bDayForward.Name = "bDayForward";
            this.bDayForward.Size = new System.Drawing.Size(39, 23);
            this.bDayForward.TabIndex = 0;
            this.bDayForward.Text = ">>";
            this.bDayForward.UseVisualStyleBackColor = true;
            this.bDayForward.Click += new System.EventHandler(this.bDayForward_Click);
            // 
            // gbPM
            // 
            this.gbPM.Controls.Add(this.tbPMUnallocatedAdj);
            this.gbPM.Controls.Add(this.lblPMCurrentUnallocated);
            this.gbPM.Controls.Add(this.tbPMUnallocatedSugg);
            this.gbPM.Controls.Add(this.lblPMUnallocated);
            this.gbPM.Controls.Add(this.lPMAdj);
            this.gbPM.Controls.Add(this.lPMSugg);
            this.gbPM.Controls.Add(this.lPMID);
            this.gbPM.Controls.Add(this.lPMName);
            this.gbPM.Location = new System.Drawing.Point(342, 27);
            this.gbPM.Name = "gbPM";
            this.gbPM.Size = new System.Drawing.Size(318, 430);
            this.gbPM.TabIndex = 4;
            this.gbPM.TabStop = false;
            this.gbPM.Text = "PM Declarations";
            // 
            // tbPMUnallocatedAdj
            // 
            this.tbPMUnallocatedAdj.Location = new System.Drawing.Point(277, 404);
            this.tbPMUnallocatedAdj.Name = "tbPMUnallocatedAdj";
            this.tbPMUnallocatedAdj.ReadOnly = true;
            this.tbPMUnallocatedAdj.Size = new System.Drawing.Size(35, 20);
            this.tbPMUnallocatedAdj.TabIndex = 18;
            // 
            // lblPMCurrentUnallocated
            // 
            this.lblPMCurrentUnallocated.AutoSize = true;
            this.lblPMCurrentUnallocated.Location = new System.Drawing.Point(167, 407);
            this.lblPMCurrentUnallocated.Name = "lblPMCurrentUnallocated";
            this.lblPMCurrentUnallocated.Size = new System.Drawing.Size(104, 13);
            this.lblPMCurrentUnallocated.TabIndex = 17;
            this.lblPMCurrentUnallocated.Text = "Current Unallocated:";
            // 
            // tbPMUnallocatedSugg
            // 
            this.tbPMUnallocatedSugg.Location = new System.Drawing.Point(115, 404);
            this.tbPMUnallocatedSugg.Name = "tbPMUnallocatedSugg";
            this.tbPMUnallocatedSugg.ReadOnly = true;
            this.tbPMUnallocatedSugg.Size = new System.Drawing.Size(35, 20);
            this.tbPMUnallocatedSugg.TabIndex = 15;
            // 
            // lblPMUnallocated
            // 
            this.lblPMUnallocated.AutoSize = true;
            this.lblPMUnallocated.Location = new System.Drawing.Point(6, 407);
            this.lblPMUnallocated.Name = "lblPMUnallocated";
            this.lblPMUnallocated.Size = new System.Drawing.Size(106, 13);
            this.lblPMUnallocated.TabIndex = 14;
            this.lblPMUnallocated.Text = "Starting Unallocated:";
            // 
            // lPMAdj
            // 
            this.lPMAdj.AutoSize = true;
            this.lPMAdj.Location = new System.Drawing.Point(276, 16);
            this.lPMAdj.Name = "lPMAdj";
            this.lPMAdj.Size = new System.Drawing.Size(22, 13);
            this.lPMAdj.TabIndex = 13;
            this.lPMAdj.Text = "Adj";
            // 
            // lPMSugg
            // 
            this.lPMSugg.AutoSize = true;
            this.lPMSugg.Location = new System.Drawing.Point(216, 16);
            this.lPMSugg.Name = "lPMSugg";
            this.lPMSugg.Size = new System.Drawing.Size(32, 13);
            this.lPMSugg.TabIndex = 12;
            this.lPMSugg.Text = "Sugg";
            // 
            // lPMID
            // 
            this.lPMID.AutoSize = true;
            this.lPMID.Location = new System.Drawing.Point(169, 16);
            this.lPMID.Name = "lPMID";
            this.lPMID.Size = new System.Drawing.Size(18, 13);
            this.lPMID.TabIndex = 11;
            this.lPMID.Text = "ID";
            // 
            // lPMName
            // 
            this.lPMName.AutoSize = true;
            this.lPMName.Location = new System.Drawing.Point(69, 16);
            this.lPMName.Name = "lPMName";
            this.lPMName.Size = new System.Drawing.Size(35, 13);
            this.lPMName.TabIndex = 10;
            this.lPMName.Text = "Name";
            // 
            // msMenu
            // 
            this.msMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mFile,
            this.mAbout});
            this.msMenu.Location = new System.Drawing.Point(0, 0);
            this.msMenu.Name = "msMenu";
            this.msMenu.Size = new System.Drawing.Size(672, 24);
            this.msMenu.TabIndex = 5;
            this.msMenu.Text = "MenuStrip";
            // 
            // mFile
            // 
            this.mFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miJump,
            this.miPrint,
            this.miExit});
            this.mFile.Name = "mFile";
            this.mFile.Size = new System.Drawing.Size(37, 20);
            this.mFile.Text = "File";
            // 
            // miJump
            // 
            this.miJump.Name = "miJump";
            this.miJump.Size = new System.Drawing.Size(148, 22);
            this.miJump.Text = "Jump to day...";
            // 
            // miPrint
            // 
            this.miPrint.Name = "miPrint";
            this.miPrint.Size = new System.Drawing.Size(148, 22);
            this.miPrint.Text = "Print Today";
            this.miPrint.Click += new System.EventHandler(this.miPrint_Click);
            // 
            // miExit
            // 
            this.miExit.Name = "miExit";
            this.miExit.Size = new System.Drawing.Size(148, 22);
            this.miExit.Text = "Exit";
            this.miExit.Click += new System.EventHandler(this.miExit_Click);
            // 
            // mAbout
            // 
            this.mAbout.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miHelp,
            this.miAbout});
            this.mAbout.Name = "mAbout";
            this.mAbout.Size = new System.Drawing.Size(52, 20);
            this.mAbout.Text = "About";
            // 
            // miHelp
            // 
            this.miHelp.Name = "miHelp";
            this.miHelp.Size = new System.Drawing.Size(107, 22);
            this.miHelp.Text = "Help";
            this.miHelp.Click += new System.EventHandler(this.miHelp_Click);
            // 
            // miAbout
            // 
            this.miAbout.Name = "miAbout";
            this.miAbout.Size = new System.Drawing.Size(107, 22);
            this.miAbout.Text = "About";
            this.miAbout.Click += new System.EventHandler(this.miAbout_Click);
            // 
            // tTimer
            // 
            this.tTimer.Tick += new System.EventHandler(this.tTimer_Tick);
            // 
            // gbTotals
            // 
            this.gbTotals.Controls.Add(this.lTotalSugg);
            this.gbTotals.Controls.Add(this.lTotalAdj);
            this.gbTotals.Controls.Add(this.lLabTotSug);
            this.gbTotals.Controls.Add(this.lTotalAMSugg);
            this.gbTotals.Controls.Add(this.lTotalAMAdj);
            this.gbTotals.Controls.Add(this.lLabTotAMSugg);
            this.gbTotals.Controls.Add(this.lTotalPMSugg);
            this.gbTotals.Controls.Add(this.lTotalPMAdj);
            this.gbTotals.Controls.Add(this.lLabTotPMSugg);
            this.gbTotals.Location = new System.Drawing.Point(12, 463);
            this.gbTotals.Name = "gbTotals";
            this.gbTotals.Size = new System.Drawing.Size(648, 49);
            this.gbTotals.TabIndex = 6;
            this.gbTotals.TabStop = false;
            this.gbTotals.Text = "Totals";
            // 
            // lTotalSugg
            // 
            this.lTotalSugg.AutoSize = true;
            this.lTotalSugg.Location = new System.Drawing.Point(285, 29);
            this.lTotalSugg.Name = "lTotalSugg";
            this.lTotalSugg.Size = new System.Drawing.Size(21, 13);
            this.lTotalSugg.TabIndex = 8;
            this.lTotalSugg.Text = "##";
            // 
            // lTotalAdj
            // 
            this.lTotalAdj.AutoSize = true;
            this.lTotalAdj.Location = new System.Drawing.Point(348, 29);
            this.lTotalAdj.Name = "lTotalAdj";
            this.lTotalAdj.Size = new System.Drawing.Size(21, 13);
            this.lTotalAdj.TabIndex = 7;
            this.lTotalAdj.Text = "##";
            // 
            // lLabTotSug
            // 
            this.lLabTotSug.AutoSize = true;
            this.lLabTotSug.Location = new System.Drawing.Point(285, 16);
            this.lLabTotSug.Name = "lLabTotSug";
            this.lLabTotSug.Size = new System.Drawing.Size(79, 13);
            this.lLabTotSug.TabIndex = 6;
            this.lLabTotSug.Text = "Total Sugg/Adj";
            // 
            // lTotalAMSugg
            // 
            this.lTotalAMSugg.AutoSize = true;
            this.lTotalAMSugg.Location = new System.Drawing.Point(32, 29);
            this.lTotalAMSugg.Name = "lTotalAMSugg";
            this.lTotalAMSugg.Size = new System.Drawing.Size(21, 13);
            this.lTotalAMSugg.TabIndex = 5;
            this.lTotalAMSugg.Text = "##";
            // 
            // lTotalAMAdj
            // 
            this.lTotalAMAdj.AutoSize = true;
            this.lTotalAMAdj.Location = new System.Drawing.Point(83, 29);
            this.lTotalAMAdj.Name = "lTotalAMAdj";
            this.lTotalAMAdj.Size = new System.Drawing.Size(21, 13);
            this.lTotalAMAdj.TabIndex = 4;
            this.lTotalAMAdj.Text = "##";
            // 
            // lLabTotAMSugg
            // 
            this.lLabTotAMSugg.AutoSize = true;
            this.lLabTotAMSugg.Location = new System.Drawing.Point(19, 16);
            this.lLabTotAMSugg.Name = "lLabTotAMSugg";
            this.lLabTotAMSugg.Size = new System.Drawing.Size(98, 13);
            this.lLabTotAMSugg.TabIndex = 3;
            this.lLabTotAMSugg.Text = "Total AM Sugg/Adj";
            // 
            // lTotalPMSugg
            // 
            this.lTotalPMSugg.AutoSize = true;
            this.lTotalPMSugg.Location = new System.Drawing.Point(545, 29);
            this.lTotalPMSugg.Name = "lTotalPMSugg";
            this.lTotalPMSugg.Size = new System.Drawing.Size(21, 13);
            this.lTotalPMSugg.TabIndex = 2;
            this.lTotalPMSugg.Text = "##";
            // 
            // lTotalPMAdj
            // 
            this.lTotalPMAdj.AutoSize = true;
            this.lTotalPMAdj.Location = new System.Drawing.Point(596, 29);
            this.lTotalPMAdj.Name = "lTotalPMAdj";
            this.lTotalPMAdj.Size = new System.Drawing.Size(21, 13);
            this.lTotalPMAdj.TabIndex = 1;
            this.lTotalPMAdj.Text = "##";
            // 
            // lLabTotPMSugg
            // 
            this.lLabTotPMSugg.AutoSize = true;
            this.lLabTotPMSugg.Location = new System.Drawing.Point(530, 16);
            this.lLabTotPMSugg.Name = "lLabTotPMSugg";
            this.lLabTotPMSugg.Size = new System.Drawing.Size(98, 13);
            this.lLabTotPMSugg.TabIndex = 0;
            this.lLabTotPMSugg.Text = "Total PM Sugg/Adj";
            // 
            // Tipshare
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(672, 592);
            this.Controls.Add(this.gbTotals);
            this.Controls.Add(this.gbPM);
            this.Controls.Add(this.gbControls);
            this.Controls.Add(this.gbAM);
            this.Controls.Add(this.msMenu);
            this.MainMenuStrip = this.msMenu;
            this.Name = "Tipshare";
            this.Text = "Tipshare##VERSION## - ##STORENAME##";
            this.Load += new System.EventHandler(this.Tipshare_Load);
            this.gbAM.ResumeLayout(false);
            this.gbAM.PerformLayout();
            this.gbControls.ResumeLayout(false);
            this.gbControls.PerformLayout();
            this.gbPM.ResumeLayout(false);
            this.gbPM.PerformLayout();
            this.msMenu.ResumeLayout(false);
            this.msMenu.PerformLayout();
            this.gbTotals.ResumeLayout(false);
            this.gbTotals.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbAM;
        private System.Windows.Forms.GroupBox gbControls;
        private System.Windows.Forms.GroupBox gbPM;
        private System.Windows.Forms.Button bDayBack;
        private System.Windows.Forms.Button bDayForward;
        private System.Windows.Forms.Label lAMName;
        private System.Windows.Forms.Label lAMAdj;
        private System.Windows.Forms.Label lAMSugg;
        private System.Windows.Forms.Label lAMID;
        private System.Windows.Forms.Label lDate;
        private System.Windows.Forms.Label lDay;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDistributeUnallocated;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Label lPMAdj;
        private System.Windows.Forms.Label lPMSugg;
        private System.Windows.Forms.Label lPMID;
        private System.Windows.Forms.Label lPMName;
        private System.Windows.Forms.MenuStrip msMenu;
        private System.Windows.Forms.ToolStripMenuItem mFile;
        private System.Windows.Forms.ToolStripMenuItem miJump;
        private System.Windows.Forms.ToolStripMenuItem miPrint;
        private System.Windows.Forms.ToolStripMenuItem miExit;
        private System.Windows.Forms.ToolStripMenuItem mAbout;
        private System.Windows.Forms.ToolStripMenuItem miHelp;
        private System.Windows.Forms.ToolStripMenuItem miAbout;
        private System.Windows.Forms.Timer tTimer;
        private System.Windows.Forms.GroupBox gbTotals;
        private System.Windows.Forms.Label lTotalPMAdj;
        private System.Windows.Forms.Label lLabTotPMSugg;
        private System.Windows.Forms.Label lTotalAMSugg;
        private System.Windows.Forms.Label lTotalAMAdj;
        private System.Windows.Forms.Label lLabTotAMSugg;
        private System.Windows.Forms.Label lTotalPMSugg;
        private System.Windows.Forms.Label lTotalSugg;
        private System.Windows.Forms.Label lTotalAdj;
        private System.Windows.Forms.Label lLabTotSug;
        private System.Windows.Forms.TextBox tbAMUnallocatedSugg;
        private System.Windows.Forms.Label lblAMUnallocated;
        private System.Windows.Forms.TextBox tbPMUnallocatedSugg;
        private System.Windows.Forms.Label lblPMUnallocated;
        private System.Windows.Forms.TextBox tbAMUnallocatedAdj;
        private System.Windows.Forms.Label lblAMCurrentUnallocated;
        private System.Windows.Forms.TextBox tbPMUnallocatedAdj;
        private System.Windows.Forms.Label lblPMCurrentUnallocated;
    }
}

