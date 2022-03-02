using Common;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Tipshare
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Tipshare());
            }
            catch (Exception e)
            {
                Globals.Log(e.ToString());
                MessageBox.Show("Exception thrown:  \r\n" + e.ToString());
            }
        }
    }
}
