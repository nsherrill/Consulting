using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Tipshare
{
    public partial class ProgressBox : Form
    {
        public bool IsRunning = false;

        public ProgressBox()
        {
            InitializeComponent();
            this.Visible = false;            
            tTimer.Interval = 100;
        }

        public void StartProgress()
        {
            if (!IsRunning)
            {
                IsRunning = true;
                this.Visible = true;
                tTimer.Start();
            }

        }

        private void IncreaseBar()
        {
            pbBar.Value = (pbBar.Value + 1) % (pbBar.Maximum + 1);
        }

        public void StopProgress()
        {
            if (IsRunning)
            {
                IsRunning = false;
                this.Visible = false;
                tTimer.Stop();
            }
        }

        private void tTimer_Tick(object sender, EventArgs e)
        {
            IncreaseBar();
        }
    }
}
