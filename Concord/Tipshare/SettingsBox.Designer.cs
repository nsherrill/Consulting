namespace Tipshare
{
    partial class SettingsBox
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
            this.gbDistribution = new System.Windows.Forms.GroupBox();
            this.lPercentHosts = new System.Windows.Forms.Label();
            this.nudHostPercent = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.nudBarPercent = new System.Windows.Forms.NumericUpDown();
            this.gbAdmin = new System.Windows.Forms.GroupBox();
            this.btnShowDebug = new System.Windows.Forms.Button();
            this.cbPast = new System.Windows.Forms.CheckBox();
            this.cbMin = new System.Windows.Forms.CheckBox();
            this.cbMax = new System.Windows.Forms.CheckBox();
            this.lPassword = new System.Windows.Forms.Label();
            this.lUsername = new System.Windows.Forms.Label();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.tbUsername = new System.Windows.Forms.TextBox();
            this.gbDistribution.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudHostPercent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBarPercent)).BeginInit();
            this.gbAdmin.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbDistribution
            // 
            this.gbDistribution.Controls.Add(this.lPercentHosts);
            this.gbDistribution.Controls.Add(this.nudHostPercent);
            this.gbDistribution.Controls.Add(this.label1);
            this.gbDistribution.Controls.Add(this.nudBarPercent);
            this.gbDistribution.Location = new System.Drawing.Point(12, 12);
            this.gbDistribution.Name = "gbDistribution";
            this.gbDistribution.Size = new System.Drawing.Size(183, 76);
            this.gbDistribution.TabIndex = 0;
            this.gbDistribution.TabStop = false;
            this.gbDistribution.Text = "Distribution";
            // 
            // lPercentHosts
            // 
            this.lPercentHosts.AutoSize = true;
            this.lPercentHosts.Location = new System.Drawing.Point(48, 47);
            this.lPercentHosts.Name = "lPercentHosts";
            this.lPercentHosts.Size = new System.Drawing.Size(109, 13);
            this.lPercentHosts.TabIndex = 3;
            this.lPercentHosts.Text = "% of tipshare to Hosts";
            // 
            // nudHostPercent
            // 
            this.nudHostPercent.Location = new System.Drawing.Point(4, 45);
            this.nudHostPercent.Name = "nudHostPercent";
            this.nudHostPercent.Size = new System.Drawing.Size(38, 20);
            this.nudHostPercent.TabIndex = 2;
            this.nudHostPercent.Value = new decimal(new int[] {
            80,
            0,
            0,
            0});
            this.nudHostPercent.ValueChanged += new System.EventHandler(this.nudHostPercent_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(48, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(133, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "% of tipshare to Bartenders";
            // 
            // nudBarPercent
            // 
            this.nudBarPercent.Location = new System.Drawing.Point(4, 19);
            this.nudBarPercent.Name = "nudBarPercent";
            this.nudBarPercent.Size = new System.Drawing.Size(38, 20);
            this.nudBarPercent.TabIndex = 0;
            this.nudBarPercent.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nudBarPercent.ValueChanged += new System.EventHandler(this.nudBarPercent_ValueChanged);
            // 
            // gbAdmin
            // 
            this.gbAdmin.Controls.Add(this.btnShowDebug);
            this.gbAdmin.Controls.Add(this.cbPast);
            this.gbAdmin.Controls.Add(this.cbMin);
            this.gbAdmin.Controls.Add(this.cbMax);
            this.gbAdmin.Controls.Add(this.lPassword);
            this.gbAdmin.Controls.Add(this.lUsername);
            this.gbAdmin.Controls.Add(this.tbPassword);
            this.gbAdmin.Controls.Add(this.tbUsername);
            this.gbAdmin.Location = new System.Drawing.Point(201, 12);
            this.gbAdmin.Name = "gbAdmin";
            this.gbAdmin.Size = new System.Drawing.Size(171, 149);
            this.gbAdmin.TabIndex = 1;
            this.gbAdmin.TabStop = false;
            this.gbAdmin.Text = "Admin";
            // 
            // btnShowDebug
            // 
            this.btnShowDebug.Location = new System.Drawing.Point(20, 116);
            this.btnShowDebug.Name = "btnShowDebug";
            this.btnShowDebug.Size = new System.Drawing.Size(129, 23);
            this.btnShowDebug.TabIndex = 7;
            this.btnShowDebug.Text = "Show Debug Info";
            this.btnShowDebug.UseVisualStyleBackColor = true;
            this.btnShowDebug.Visible = false;
            this.btnShowDebug.Click += new System.EventHandler(this.btnShowDebug_Click);
            // 
            // cbPast
            // 
            this.cbPast.AutoSize = true;
            this.cbPast.Location = new System.Drawing.Point(20, 93);
            this.cbPast.Name = "cbPast";
            this.cbPast.Size = new System.Drawing.Size(130, 17);
            this.cbPast.TabIndex = 6;
            this.cbPast.Text = "View Previous Weeks";
            this.cbPast.UseVisualStyleBackColor = true;
            this.cbPast.Visible = false;
            // 
            // cbMin
            // 
            this.cbMin.AutoSize = true;
            this.cbMin.Location = new System.Drawing.Point(89, 70);
            this.cbMin.Name = "cbMin";
            this.cbMin.Size = new System.Drawing.Size(60, 17);
            this.cbMin.TabIndex = 5;
            this.cbMin.Text = "No Min";
            this.cbMin.UseVisualStyleBackColor = true;
            this.cbMin.Visible = false;
            // 
            // cbMax
            // 
            this.cbMax.AutoSize = true;
            this.cbMax.Location = new System.Drawing.Point(20, 70);
            this.cbMax.Name = "cbMax";
            this.cbMax.Size = new System.Drawing.Size(63, 17);
            this.cbMax.TabIndex = 4;
            this.cbMax.Text = "No Max";
            this.cbMax.UseVisualStyleBackColor = true;
            this.cbMax.Visible = false;
            // 
            // lPassword
            // 
            this.lPassword.AutoSize = true;
            this.lPassword.Location = new System.Drawing.Point(8, 47);
            this.lPassword.Name = "lPassword";
            this.lPassword.Size = new System.Drawing.Size(53, 13);
            this.lPassword.TabIndex = 3;
            this.lPassword.Text = "Password";
            // 
            // lUsername
            // 
            this.lUsername.AutoSize = true;
            this.lUsername.Location = new System.Drawing.Point(6, 21);
            this.lUsername.Name = "lUsername";
            this.lUsername.Size = new System.Drawing.Size(55, 13);
            this.lUsername.TabIndex = 2;
            this.lUsername.Text = "Username";
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(67, 44);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.PasswordChar = '*';
            this.tbPassword.Size = new System.Drawing.Size(98, 20);
            this.tbPassword.TabIndex = 1;
            this.tbPassword.TextChanged += new System.EventHandler(this.tbPassword_TextChanged);
            // 
            // tbUsername
            // 
            this.tbUsername.Location = new System.Drawing.Point(67, 18);
            this.tbUsername.Name = "tbUsername";
            this.tbUsername.Size = new System.Drawing.Size(98, 20);
            this.tbUsername.TabIndex = 0;
            this.tbUsername.TextChanged += new System.EventHandler(this.tbUsername_TextChanged);
            // 
            // SettingsBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(385, 170);
            this.Controls.Add(this.gbAdmin);
            this.Controls.Add(this.gbDistribution);
            this.Name = "SettingsBox";
            this.Text = "SettingsBox";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SettingsBox_FormClosing);
            this.gbDistribution.ResumeLayout(false);
            this.gbDistribution.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudHostPercent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBarPercent)).EndInit();
            this.gbAdmin.ResumeLayout(false);
            this.gbAdmin.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbDistribution;
        private System.Windows.Forms.Label lPercentHosts;
        private System.Windows.Forms.NumericUpDown nudHostPercent;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudBarPercent;
        private System.Windows.Forms.GroupBox gbAdmin;
        private System.Windows.Forms.CheckBox cbMin;
        private System.Windows.Forms.CheckBox cbMax;
        private System.Windows.Forms.Label lPassword;
        private System.Windows.Forms.Label lUsername;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.TextBox tbUsername;
        private System.Windows.Forms.CheckBox cbPast;
    }
}