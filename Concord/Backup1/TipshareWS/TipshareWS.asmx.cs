using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.IO;

using Common;

namespace TipshareWS
{
    /// <summary>
    /// Summary description for Service1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class TipshareWS : System.Web.Services.WebService
    {
        #region Tipshare
        [WebMethod]
        public Version GetTipshareVersion()
        {
            string filename = System.Configuration.ConfigurationManager.AppSettings["tipshare_version_file"];
            Version vers;
            if (File.Exists(filename))
                try
                {
                    vers = new Version(File.ReadAllText(filename).Trim());
                }
                catch (Exception) { }
            return new Version("1.0.0.0");
        }

        [WebMethod]
        public string[] GetProgramUsage(DateTime DateStart, string UserPass)
        {
            List<string> results = new List<string>();

            try
            {
                FileInfo fi = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["tipshare_usage_file"]);
                if (fi.Exists)
                {
                    string[] temp = File.ReadAllLines(fi.FullName);

                    foreach (string str in temp)
                    {
                        DateTime dtTemp;
                        if (!string.IsNullOrEmpty(str)
                            && str.IndexOf(',') >= 0
                            && DateTime.TryParse(str.Substring(0, str.IndexOf(',')), out dtTemp))
                        {
                            if (dtTemp > DateStart)
                                results.Add(str);

                        }
                    }
                }
                else
                    return new string[] { "FILE DNE" };
            }
            catch (Exception e)
            {
                return new string[] { "FILE ERROR:  "+e.ToString() };
            }
            return results.ToArray();
        }

        [WebMethod]
        public string RecordProgramUsage(string StoreName)
        {
            try
            {
                FileInfo fi = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["tipshare_usage_file"]);

                string startText = Environment.NewLine;

                if (!fi.Exists)
                {
                    fi.Create();
                    startText = "";
                }

                File.AppendAllText(fi.FullName,
                    startText + DateTime.Now.ToString() + "," + StoreName);

                return "Success";
            }
            catch (Exception e)
            {
                return "Exception:  " + e.ToString();
            }
            //return "WTF";
        }

        [WebMethod]
        public string GetUndistributedStores()
        {
            return "AB05,AB08,AB18,ab21,AB28,AB32";
        }

        [WebMethod]
        public string[] GetListOfCardsToSetAsUnallocated()
        {
            return new string[] {
                "PM BAR","AM BAR"
            };
        }
        #endregion

        #region SupportProfiler
        [WebMethod]
        public SPRequest[] GetRequests(string ShortStoreName)
        {
            return SPRequest.FindRequested(ShortStoreName);
        }

        [WebMethod]
        public bool UpdateRequest(SPRequest newRequest)
        {
            return newRequest.Save();
        }

        [WebMethod]
        public SPResults GetRecentCompleted(string ShortStoreName)
        {
            return SPResults.GetRecentCompleted(ShortStoreName);
        }

        [WebMethod]
        public bool InsertResults(SPResults newResults)
        {
            return SPResults.InsertNew(newResults);
        }

        [WebMethod]
        public string LastCompletedResultDate(string ShortStoreName)
        {
            return SPResults.GetRecentCompleted(ShortStoreName).CompletedDate.ToString();
        }
        #endregion
    }
}
