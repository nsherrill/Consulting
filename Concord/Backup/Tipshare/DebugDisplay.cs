using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Tipshare
{
    public partial class DebugDisplay : Form
    {
        public DebugDisplay()
        {
            InitializeComponent();
        }

        private void DebugDisplay_Load(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        public void DisplayData(DebugContainer dc)
        {
            if (dc != null)
            {
                lblDate.Text = dc.DateOfInfo.ToShortDateString();
                tbInfo.Text = dc.GetInfoData();

                Point oldLocation = dgEmployees.Location;
                Size oldSize = dgEmployees.Size;
                int[] colWidths;
                dgEmployees.DataSource = dc.GetEmployeeDataSet(out colWidths);
                if (colWidths != null && colWidths.Length > 0)
                    for (int i = 0; i < colWidths.Length; i++)
                        dgEmployees.Columns[i].Width = colWidths[i];

                oldLocation = dgSales.Location;
                oldSize = dgSales.Size;
                dgSales.DataSource = dc.GetSalesDataSet(out colWidths);
                if (colWidths != null && colWidths.Length > 0)
                    for (int i = 0; i < colWidths.Length; i++)
                        dgSales.Columns[i].Width = colWidths[i];

                oldLocation = dgTickets.Location;
                oldSize = dgTickets.Size;
                dgTickets.DataSource = dc.GetTicketsDataSet(out colWidths);
                if (colWidths != null && colWidths.Length > 0)
                    for (int i = 0; i < colWidths.Length; i++)
                        dgTickets.Columns[i].Width = colWidths[i];
            }
        }

        public void HideDisplay()
        {
            this.Visible = false;
        }
    }
}
