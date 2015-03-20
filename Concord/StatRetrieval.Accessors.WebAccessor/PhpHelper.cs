using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using StatRetrieval.Shared.Common;
using StatRetrieval.Shared.Common.Contracts;
using StatRetrieval.Shared.Common.DataContracts;

namespace StatRetrieval.Accessors.WebAccessor
{
    public static class PhpHelper
    {
        public static string HitService(string url)
        {
            LogHelper.Log(string.Format("Hitting url {0}", url), LogLocation.PhpHelper, LogType.ExtraVerbose);
            if (string.IsNullOrEmpty(url))
                return null;

            try
            {
                var request = WebRequest.Create(url);
                using (var response = request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        using (var sr = new StreamReader(stream))
                        {
                            var result = sr.ReadToEnd();

                            return result;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.Log("Exception caught while hitting php service: " + e.ToString(), LogLocation.PhpHelper, LogType.Error);
            }
            return null;
        }

        public static bool PostToService(string url, string postData)
        {
            LogHelper.Log(string.Format("Hitting url {0} with data {1}", url, postData), LogLocation.PhpHelper, LogType.ExtraVerbose);
            if (string.IsNullOrEmpty(url))
                return false;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";

                byte[] byteArray = Encoding.ASCII.GetBytes(postData);
                request.ContentLength = byteArray.Length;
                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }

                using (WebResponse response = request.GetResponse())
                {
                    var temp = (((HttpWebResponse)response).StatusDescription);
                    if (temp == "OK")
                    {
                        using (Stream dataStream = response.GetResponseStream())
                        {
                            using (StreamReader reader = new StreamReader(dataStream))
                            {
                                string responseFromServer = reader.ReadToEnd();
                                if (responseFromServer.Contains("SUCCESS"))
                                    return true;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.Log("Exception caught while posting php service: " + e.ToString(), LogLocation.PhpHelper, LogType.Error);
            }
            return false;
        }
    }
}
