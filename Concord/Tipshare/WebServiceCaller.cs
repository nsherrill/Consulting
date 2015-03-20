using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Tipshare
{
    public class WebServiceCaller
    {
        private bool hasAccessToRealWS = false;

        public WebServiceCaller()
        {
            string temp = ConfigurationManager.AppSettings["HasAccessToRealWS"];
            if (!string.IsNullOrEmpty(temp) &&
                (temp.Equals("true", StringComparison.InvariantCultureIgnoreCase)
                || temp.Equals("yes", StringComparison.InvariantCultureIgnoreCase)))
            {
                hasAccessToRealWS = true;
            }
            else
                hasAccessToRealWS = false;
        }

        public string GetUndistributedStores()
        {
            string result = null;
            if (hasAccessToRealWS)
            {
                try
                {
                    TipshareWS.TipshareWS tws = new TipshareWS.TipshareWS();
                    result = tws.GetUndistributedStores();
                }
                catch (Exception)
                {
                    result = "AB05,AB08,AB18,ab21,AB28,AB32";
                }
            }
            else
                result = "AB05,AB08,AB18,ab21,AB28,AB32";

            return result;
        }

        public string[] GetListOfCardsToSetAsUnallocated()
        {
            string[] result = null;
            if (hasAccessToRealWS)
            {
                try
                {
                    TipshareWS.TipshareWS tws = new TipshareWS.TipshareWS();
                    result = tws.GetListOfCardsToSetAsUnallocated();
                }
                catch (Exception)
                {
                    result = new string[]
                        {
                            "PM BAR",
                            "AM BAR",
                            "TOGO CARD",
                        };
                }
            }
            else
                result = new string[] 
                    {
                        "PM BAR",
                        "AM BAR",
                        "TOGO CARD",
                    };

            return result;
        }

        public string RecordProgramUsage(string storeName)
        {
            string result = null;
            if (hasAccessToRealWS)
            {
                try
                {
                    TipshareWS.TipshareWS tws = new TipshareWS.TipshareWS();
                    result = tws.RecordProgramUsage(storeName);
                }
                catch (Exception)
                {
                    result = "Fail";
                }
            }
            else
                result = "Success";

            return result;
        }
    }
}
