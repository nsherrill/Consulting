using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Common
{
    public class TipshareResults
    {
        public List<TSR> Results;
        public DateTime StartDate, EndDate;
        public string ErrorString, Store;

        public TipshareResults(DateTime StartDate, DateTime EndDate, string Store, List<TSR> Results)
        {
            this.StartDate = StartDate;
            this.EndDate = EndDate;
            this.Store = Store;
            this.Results = Results;
        }

        public static DateTime GetLastSOWD()
        {
            DateTime temp = DateTime.Now.AddDays(-1);
            while (temp.DayOfWeek != ConfigHelper.StartOfWeekDay)
                temp = temp.AddDays(-1);
            return temp;
        }

        public static int GetIndexOf(string store)
        {
            List<SelectListItem> list = GetStoreList(0);
            for(int i=0; i<list.Count; i++)
                if (list[i].Text == store)
                    return i;
            return 0;
        }

        public static List<SelectListItem> GetStoreList(int index)
        {
            List<SelectListItem> result = new List<SelectListItem>();
            for (int i = 0; i < 44; i++)
            {
                if (i == 0)
                    result.Add(new SelectListItem() { Text = "All", Value = "all", Selected = i == index });
                else if (i < 10)
                    result.Add(new SelectListItem() { Text = "AB0" + i, Value = "AB0" + i, Selected = i == index });
                else if (i == 27)
                { /* do nothing */ }
                else
                    result.Add(new SelectListItem() { Text = "AB" + i, Value = "AB" + i, Selected = i == index });
            }

            return result;
        }        
    }

    public class TSR
    {
        public string Storename;
        public DateTime SaveDate;

        public TSR(string storename, DateTime savedate)
        {
            this.Storename = storename;
            this.SaveDate = savedate;
        }
    }
}
