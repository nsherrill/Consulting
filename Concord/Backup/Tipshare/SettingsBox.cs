using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Tipshare
{
    public partial class SettingsBox : Form
    {
        public bool bRefresh = false;
        public DateTime dtDateChanged = DateTime.Now;

        public Button btnSave;
        public Button btnShowDebug;

        public SettingsBox(int myX, int myY)
        {
            InitializeComponent();
            btnSave = new Button();
            btnSave.Text = "Save Settings";
            btnSave.Size = new Size(183, 36);
            btnSave.Location = new Point(12, 95);
            btnSave.Click += new EventHandler(btnSave_Click);
            this.Controls.Add(btnSave);

            this.Visible = false;
            this.Location = new Point(myX - this.Size.Width / 2, myY - this.Size.Height / 2);
        }

        public void Open(int BarPercent, int HostPercent)
        {
            nudBarPercent.Value = BarPercent;
            nudHostPercent.Value = HostPercent;
            this.Visible = true;
        }

        public bool PastAvailable()
        {
            return cbPast.Checked;
        }

        private void nudHostPercent_ValueChanged(object sender, EventArgs e)
        {
            if (nudBarPercent.Value != 100 - nudHostPercent.Value)
                nudBarPercent.Value = 100 - nudHostPercent.Value;
        }

        private void nudBarPercent_ValueChanged(object sender, EventArgs e)
        {
            if (nudHostPercent.Value != 100 - nudBarPercent.Value)
                nudHostPercent.Value = 100 - nudBarPercent.Value;
            if (dtDateChanged < DateTime.Now.AddSeconds(-10))
                bRefresh = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void tbUsername_TextChanged(object sender, EventArgs e)
        {
            CheckCreds();
        }

        private void tbPassword_TextChanged(object sender, EventArgs e)
        {
            CheckCreds();
        }

        private void CheckCreds()
        {
            bool temp = tbUsername.Text.Equals("admin") && tbPassword.Text.Equals("$upp0rt");

            cbMax.Visible = temp;
            cbMin.Visible = temp;
            cbPast.Visible = temp;
            btnShowDebug.Visible = temp;
        }

        private void btnShowDebug_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }
    }
}
