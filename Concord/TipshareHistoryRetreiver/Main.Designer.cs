namespace TipshareHistoryRetreiver
{
    partial class Main
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
            this.gbSettings = new System.Windows.Forms.GroupBox();
            this.lblTo = new System.Windows.Forms.Label();
            this.lblFrom = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.cbStore = new System.Windows.Forms.ComboBox();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.btnSearch = new System.Windows.Forms.Button();
            this.gbResult = new System.Windows.Forms.GroupBox();
            this.tbResult = new System.Windows.Forms.TextBox();
            this.ssStrip = new System.Windows.Forms.StatusStrip();
            this.pbBar = new System.Windows.Forms.ToolStripProgressBar();
            this.gbSettings.SuspendLayout();
            this.gbResult.SuspendLayout();
            this.ssStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbSettings
            // 
            this.gbSettings.Controls.Add(this.lblTo);
            this.gbSettings.Controls.Add(this.lblFrom);
            this.gbSettings.Controls.Add(this.dtpTo);
            this.gbSettings.Controls.Add(this.cbStore);
            this.gbSettings.Controls.Add(this.dtpFrom);
            this.gbSettings.Controls.Add(this.btnSearch);
            this.gbSettings.Location = new System.Drawing.Point(12, 12);
            this.gbSettings.Name = "gbSettings";
            this.gbSettings.Size = new System.Drawing.Size(313, 105);
            this.gbSettings.TabIndex = 5;
            this.gbSettings.TabStop = false;
            this.gbSettings.Text = "Settings";
            // 
            // lblTo
            // 
            this.lblTo.AutoSize = true;
            this.lblTo.Location = new System.Drawing.Point(32, 46);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(20, 13);
            this.lblTo.TabIndex = 10;
            this.lblTo.Text = "To";
            // 
            // lblFrom
            // 
            this.lblFrom.AutoSize = true;
            this.lblFrom.Location = new System.Drawing.Point(30, 22);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(30, 13);
            this.lblFrom.TabIndex = 9;
            this.lblFrom.Text = "From";
            // 
            // dtpTo
            // 
            this.dtpTo.Location = new System.Drawing.Point(87, 45);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(200, 20);
            this.dtpTo.TabIndex = 8;
            // 
            // cbStore
            // 
            this.cbStore.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbStore.FormattingEnabled = true;
            this.cbStore.Location = new System.Drawing.Point(33, 73);
            this.cbStore.Name = "cbStore";
            this.cbStore.Size = new System.Drawing.Size(121, 21);
            this.cbStore.TabIndex = 7;
            // 
            // dtpFrom
            // 
            this.dtpFrom.Location = new System.Drawing.Point(87, 19);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(200, 20);
            this.dtpFrom.TabIndex = 6;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(212, 71);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 5;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // gbResult
            // 
            this.gbResult.Controls.Add(this.tbResult);
            this.gbResult.Location = new System.Drawing.Point(12, 123);
            this.gbResult.Name = "gbResult";
            this.gbResult.Size = new System.Drawing.Size(313, 281);
            this.gbResult.TabIndex = 6;
            this.gbResult.TabStop = false;
            this.gbResult.Text = "Results";
            // 
            // tbResult
            // 
            this.tbResult.Location = new System.Drawing.Point(6, 19);
            this.tbResult.Multiline = true;
            this.tbResult.Name = "tbResult";
            this.tbResult.ReadOnly = true;
            this.tbResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbResult.Size = new System.Drawing.Size(301, 256);
            this.tbResult.TabIndex = 2;
            this.tbResult.WordWrap = false;
            // 
            // ssStrip
            // 
            this.ssStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pbBar});
            this.ssStrip.Location = new System.Drawing.Point(0, 394);
            this.ssStrip.Name = "ssStrip";
            this.ssStrip.Size = new System.Drawing.Size(337, 22);
            this.ssStrip.TabIndex = 7;
            this.ssStrip.Text = "statusStrip1";
            // 
            // pbBar
            // 
            this.pbBar.Name = "pbBar";
            this.pbBar.Size = new System.Drawing.Size(150, 16);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(337, 416);
            this.Controls.Add(this.ssStrip);
            this.Controls.Add(this.gbResult);
            this.Controls.Add(this.gbSettings);
            this.Name = "Main";
            this.Text = "Tipshare History Retriever";
            this.gbSettings.ResumeLayout(false);
            this.gbSettings.PerformLayout();
            this.gbResult.ResumeLayout(false);
            this.gbResult.PerformLayout();
            this.ssStrip.ResumeLayout(false);
            this.ssStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbSettings;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.ComboBox cbStore;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.GroupBox gbResult;
        private System.Windows.Forms.TextBox tbResult;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.StatusStrip ssStrip;
        private System.Windows.Forms.ToolStripProgressBar pbBar;
    }
}

