using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace TipshareHistoryRetreiver
{
    public partial class Main : Form
    {
        private bool bLogging = false;
        private System.IO.FileInfo LogFile;

        public Main()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            pbBar.Minimum = 0;
            pbBar.Maximum = 4;

            dtpTo.Value = DateTime.Now;
            dtpFrom.Value = GetLastThursday();

            cbStore.Items.Clear();
            cbStore.Items.AddRange(
                GetStoreList(System.Configuration.ConfigurationSettings.AppSettings["Stores"]));
            cbStore.SelectedIndex = 0;
            LogFile = new FileInfo(
                System.Configuration.ConfigurationSettings.AppSettings["LogLocation"]);
            if (!LogFile.Exists)
                LogFile.Create();
            try
            {
                using (FileStream fs = LogFile.OpenWrite())
                {
                    bLogging = fs.CanWrite;
                }
            }
            catch (Exception)
            { }
        }

        private string[] GetStoreList(string listFile)
        {
            List<string> result = new List<string>();
            result.Add("All");
            if (!string.IsNullOrEmpty(listFile) && File.Exists(listFile))
            {
                string[] initialList = File.ReadAllLines(listFile);
                if (initialList != null && initialList.Length>0)
                {
                    result.AddRange(initialList);
                }
            }
            return result.ToArray<string>();
        }

        private DateTime GetLastThursday()
        {
            DateTime temp = DateTime.Now.AddDays(-1);
            while (temp.DayOfWeek != DayOfWeek.Thursday)            
                temp = temp.AddDays(-1);
            return temp;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            tbResult.Text = "";
            pbBar.Value = 0;

            pbBar.Value++;

            List<THRResult> results = GetData();

            tbResult.Text = GetDisplayText(results);

            pbBar.Value = pbBar.Maximum;
        }

        private string GetDisplayText(List<THRResult> results)
        {
            StringBuilder sb = new StringBuilder();
            if (results != null && results.Count > 0)
            {
                foreach (THRResult tr in results)
                    sb.AppendLine(tr.savedate.ToString() + ":  " + tr.storename);

                pbBar.Value++;
            }
            else
                sb.AppendLine("NONE");
            return sb.ToString();
        }

        private List<THRResult> GetData()
        {
            List<THRResult> result = new List<THRResult>();
            try
            {
                ConcordEI.TipshareWS.TipshareWS tws =
                    new TipshareHistoryRetreiver.ConcordEI.TipshareWS.TipshareWS();
                string[] TSresults = tws.GetProgramUsage(dtpFrom.Value, "");
                
                pbBar.Value++;

                if (TSresults != null && TSresults.Length > 0)
                {
                    result = FilterResults(TSresults, (string)cbStore.SelectedItem, dtpTo.Value);

                    pbBar.Value++;
                }
            }
            catch (Exception e)
            {
                Log("errored getting data from WS", e.ToString());
            }
            return result;
        }

        private void Log(string label, string message)
        {
            if (bLogging)
            {
                File.WriteAllLines(LogFile.FullName, new string[] { "","",
                    DateTime.Now.ToString() +":  "+ label, "", message });
            }
        }

        private List<THRResult> FilterResults(string[] TSresults, string store, DateTime endDateTime)
        {
            List<THRResult> rslt = new List<THRResult>();
            if (TSresults != null && TSresults.Length > 0)
            {
                foreach (string str in TSresults)
                {
                    string[] temp = str.Split(',');
                    DateTime dttemp;
                    if (temp != null && temp.Length > 1 && DateTime.TryParse(temp[0], out dttemp))
                    {
                        if(dttemp.Date<=endDateTime.Date
                                && (store == "All" || store == temp[1].ToUpperInvariant()))
                            rslt.Add(new THRResult()
                                { savedate = dttemp, storename = temp[1].ToUpperInvariant() });
                    }
                }
            }
            return rslt;
        }
    }

    public class THRResult
    {
        public string storename;
        public DateTime savedate;
    }
}
