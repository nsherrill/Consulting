using System;
using System.Collections.Generic;
using System.Text;

namespace Tipshare
{
    public class DaysData
    {
        public DateTime Date;
        public List<Entry> AMEntries, PMEntries, OtherEntries;
        public double TotalSuggested, TotalAMSuggested, TotalPMSuggested;
        public double TotalAdjusted, TotalAMAdjusted, TotalPMAdjusted;
        public bool DataSet;
        public double AMUnallocatedSugg, PMUnallocatedSugg;

        public double AMUnallocatedAdj
        {
            get
            {
                return Math.Round(TotalAMSuggested - TotalAMAdjusted, 2);
            }
        }
        public double PMUnallocatedAdj
        {
            get
            {
                return Math.Round(TotalPMSuggested - TotalPMAdjusted, 2);
            }
        }

        public DaysData(DateTime Date, List<Entry> AMEntries, List<Entry> PMEntries, List<Entry> OtherEntries)
        {
            this.AMEntries = AMEntries;
            this.PMEntries = PMEntries;
            this.OtherEntries = OtherEntries;
            this.Date = Date;
            DataSet = AMEntries != null && PMEntries != null && OtherEntries != null;
        }

        internal void SetAdjustedEqualToSuggested()
        {
            double temp = 0;
            foreach (Entry ent in AMEntries)
                temp += ent.AdjustedAmount;
            TotalAMAdjusted = temp;

            temp = 0;
            foreach (Entry ent in PMEntries)
                temp += ent.AdjustedAmount;
            TotalPMAdjusted = temp;

            TotalAdjusted = TotalPMAdjusted + TotalAMAdjusted;
        }
    }
}
