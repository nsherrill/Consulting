using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Common;

namespace CCWeb.Controllers
{
    public class TipshareController : Controller
    {
        //
        // GET: /Tipshare/

        public ActionResult Index()
        {
            DateTime start = TipshareResults.GetLastThursday();
            DateTime end = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            return View(new TipshareResults(
                start,
                end,
                "All",
                GetData(start, end, "All")));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index(string startDate, string endDate, string store)
        {
            DateTime start, end;
            string error = "";
            if (!string.IsNullOrEmpty(startDate) && DateTime.TryParse(startDate, out start)
                && !string.IsNullOrEmpty(endDate) && DateTime.TryParse(endDate, out end))
            {
                // do nothing.
            }
            else
            {
                start = TipshareResults.GetLastThursday();
                end = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                error = "Correct your Dates";
            }

            return View(new TipshareResults(
                start,
                end,
                store,
                GetData(start, end, store)) { ErrorString = error });
        }

        private List<TSR> GetData(DateTime start, DateTime end, string store)
        {
            List<TSR> result = new List<TSR>();
            try
            {
                ConcordEI.TipshareWS.TipshareWS tws =
                    new ConcordEI.TipshareWS.TipshareWS();
                string[] TSresults = tws.GetProgramUsage(start, "");

                if (TSresults != null && TSresults.Length > 0)
                {
                    result = FilterResults(TSresults, store, end);
                }
            }
            catch (Exception e)
            {
                Globals.Log("errored getting data from WS"+Environment.NewLine+e.ToString());
            }
            return result;
        }

        private List<TSR> FilterResults(string[] TSresults, string store, DateTime endDateTime)
        {
            store = store.ToUpperInvariant();
            List<TSR> rslt = new List<TSR>();
            if (TSresults != null && TSresults.Length > 0)
            {
                foreach (string str in TSresults)
                {
                    string[] temp = str.Split(',');
                    DateTime dttemp;
                    if (temp != null && temp.Length > 1 && DateTime.TryParse(temp[0], out dttemp))
                    {
                        if (dttemp.Date <= endDateTime.Date
                                && (store == "ALL" || store == temp[1].ToUpperInvariant()))
                            rslt.Add(new TSR(temp[1].ToUpperInvariant(), dttemp));
                    }
                }
            }
            return rslt;
        }
    
    }
}
