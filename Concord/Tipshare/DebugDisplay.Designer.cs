namespace Tipshare
{
    partial class DebugDisplay
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
            this.lblDate = new System.Windows.Forms.Label();
            this.gbGeneralInfo = new System.Windows.Forms.GroupBox();
            this.tbInfo = new System.Windows.Forms.TextBox();
            this.gbSales = new System.Windows.Forms.GroupBox();
            this.lblSales = new System.Windows.Forms.Label();
            this.lblTickets = new System.Windows.Forms.Label();
            this.dgSales = new System.Windows.Forms.DataGridView();
            this.dgTickets = new System.Windows.Forms.DataGridView();
            this.gbEmployeeInfo = new System.Windows.Forms.GroupBox();
            this.dgEmployees = new System.Windows.Forms.DataGridView();
            this.gbGeneralInfo.SuspendLayout();
            this.gbSales.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgSales)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgTickets)).BeginInit();
            this.gbEmployeeInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgEmployees)).BeginInit();
            this.SuspendLayout();
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Location = new System.Drawing.Point(12, 9);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(35, 13);
            this.lblDate.TabIndex = 0;
            this.lblDate.Text = "label1";
            // 
            // gbGeneralInfo
            // 
            this.gbGeneralInfo.Controls.Add(this.tbInfo);
            this.gbGeneralInfo.Location = new System.Drawing.Point(12, 25);
            this.gbGeneralInfo.Name = "gbGeneralInfo";
            this.gbGeneralInfo.Size = new System.Drawing.Size(195, 183);
            this.gbGeneralInfo.TabIndex = 3;
            this.gbGeneralInfo.TabStop = false;
            this.gbGeneralInfo.Text = "General Info";
            // 
            // tbInfo
            // 
            this.tbInfo.Location = new System.Drawing.Point(6, 19);
            this.tbInfo.Multiline = true;
            this.tbInfo.Name = "tbInfo";
            this.tbInfo.ReadOnly = true;
            this.tbInfo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbInfo.Size = new System.Drawing.Size(183, 158);
            this.tbInfo.TabIndex = 2;
            this.tbInfo.WordWrap = false;
            // 
            // gbSales
            // 
            this.gbSales.Controls.Add(this.lblSales);
            this.gbSales.Controls.Add(this.lblTickets);
            this.gbSales.Controls.Add(this.dgSales);
            this.gbSales.Controls.Add(this.dgTickets);
            this.gbSales.Location = new System.Drawing.Point(12, 214);
            this.gbSales.Name = "gbSales";
            this.gbSales.Size = new System.Drawing.Size(591, 224);
            this.gbSales.TabIndex = 4;
            this.gbSales.TabStop = false;
            this.gbSales.Text = "Sales Info";
            // 
            // lblSales
            // 
            this.lblSales.AutoSize = true;
            this.lblSales.Location = new System.Drawing.Point(446, 16);
            this.lblSales.Name = "lblSales";
            this.lblSales.Size = new System.Drawing.Size(33, 13);
            this.lblSales.TabIndex = 8;
            this.lblSales.Text = "Sales";
            // 
            // lblTickets
            // 
            this.lblTickets.AutoSize = true;
            this.lblTickets.Location = new System.Drawing.Point(153, 16);
            this.lblTickets.Name = "lblTickets";
            this.lblTickets.Size = new System.Drawing.Size(42, 13);
            this.lblTickets.TabIndex = 7;
            this.lblTickets.Text = "Tickets";
            // 
            // dgSales
            // 
            this.dgSales.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgSales.Location = new System.Drawing.Point(343, 32);
            this.dgSales.Name = "dgSales";
            this.dgSales.ReadOnly = true;
            this.dgSales.Size = new System.Drawing.Size(233, 186);
            this.dgSales.TabIndex = 6;
            // 
            // dgTickets
            // 
            this.dgTickets.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgTickets.Location = new System.Drawing.Point(6, 32);
            this.dgTickets.Name = "dgTickets";
            this.dgTickets.ReadOnly = true;
            this.dgTickets.Size = new System.Drawing.Size(331, 186);
            this.dgTickets.TabIndex = 5;
            // 
            // gbEmployeeInfo
            // 
            this.gbEmployeeInfo.Controls.Add(this.dgEmployees);
            this.gbEmployeeInfo.Location = new System.Drawing.Point(213, 25);
            this.gbEmployeeInfo.Name = "gbEmployeeInfo";
            this.gbEmployeeInfo.Size = new System.Drawing.Size(390, 183);
            this.gbEmployeeInfo.TabIndex = 5;
            this.gbEmployeeInfo.TabStop = false;
            this.gbEmployeeInfo.Text = "Employee Info";
            // 
            // dgEmployees
            // 
            this.dgEmployees.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgEmployees.Location = new System.Drawing.Point(6, 19);
            this.dgEmployees.Name = "dgEmployees";
            this.dgEmployees.ReadOnly = true;
            this.dgEmployees.Size = new System.Drawing.Size(378, 158);
            this.dgEmployees.TabIndex = 7;
            // 
            // DebugDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(615, 450);
            this.Controls.Add(this.gbEmployeeInfo);
            this.Controls.Add(this.gbSales);
            this.Controls.Add(this.gbGeneralInfo);
            this.Controls.Add(this.lblDate);
            this.Name = "DebugDisplay";
            this.Text = "DebugDisplay";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DebugDisplay_FormClosing);
            this.Load += new System.EventHandler(this.DebugDisplay_Load);
            this.gbGeneralInfo.ResumeLayout(false);
            this.gbGeneralInfo.PerformLayout();
            this.gbSales.ResumeLayout(false);
            this.gbSales.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgSales)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgTickets)).EndInit();
            this.gbEmployeeInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgEmployees)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.GroupBox gbGeneralInfo;
        private System.Windows.Forms.TextBox tbInfo;
        private System.Windows.Forms.GroupBox gbSales;
        private System.Windows.Forms.GroupBox gbEmployeeInfo;
        private System.Windows.Forms.DataGridView dgSales;
        private System.Windows.Forms.DataGridView dgTickets;
        private System.Windows.Forms.Label lblTickets;
        private System.Windows.Forms.Label lblSales;
        private System.Windows.Forms.DataGridView dgEmployees;
    }
}