using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tipshare.Engines
{
    public class EntryCalculationEng
    {
        #region publics
        public List<Entry> GetOtherEntries(DateTime currentDay, Dictionary<int, Shift> shifts, string[] jobCodes,
            List<Entry> eAM, List<Entry> ePM, string[] saEmployeeNamesToAddToUnallocated)
        {
            List<Entry> lTemp = new List<Entry>();

            DateTime start = new DateTime(currentDay.Year, currentDay.Month, currentDay.Day,
                2, 0, 0);
            DateTime end = new DateTime(currentDay.AddDays(1).Year, currentDay.AddDays(1).Month, currentDay.AddDays(1).Day,
                1, 59, 59);

            Dictionary<int, Shift> newshifts = new Dictionary<int, Shift>();
            foreach (int key in shifts.Keys)
                if (Globals.ArrayContains(jobCodes, shifts[key].JobCode)
                    && shifts[key].ClockIn > start
                    && shifts[key].ClockIn < end)
                    newshifts.Add(newshifts.Count, shifts[key]);

            lTemp.AddRange(GetUniqueEntries(newshifts, lTemp, saEmployeeNamesToAddToUnallocated));
            lTemp.AddRange(GetUniqueEntries(eAM, lTemp, saEmployeeNamesToAddToUnallocated));
            lTemp.AddRange(GetUniqueEntries(ePM, lTemp, saEmployeeNamesToAddToUnallocated));

            lTemp.Sort();

            return lTemp;
        }

        public List<Entry> GetEntries(double BarAmount, Dictionary<int, Shift> lsBarShifts, double dBarHours,
            double HostAmount, Dictionary<int, Shift> lsHostShifts, double dHostHours, string[] empsToUnallocate,
            out double unallocated)
        {
            List<Entry> leResult = new List<Entry>();
            unallocated = 0;

            if (dBarHours == 0 && dHostHours == 0)
                return leResult;
            else if (dBarHours == 0)
                HostAmount += BarAmount;
            else if (dHostHours == 0)
                BarAmount += HostAmount;

            if (lsBarShifts != null)
            {
                foreach (int key in lsBarShifts.Keys)
                {
                    Shift shft = lsBarShifts[key];
                    if (dBarHours == 0)
                    {
                        if (Globals.ArrayContains(empsToUnallocate, shft.Name))
                            unallocated += 0;
                        else
                            leResult.Add(new Entry(shft.Name, shft.EmployeeID, 0, 0, shft.Hours));
                    }
                    else
                    {
                        double temp = Math.Round(BarAmount * ((shft.ClockOut - shft.ClockIn).TotalHours / dBarHours), 2);
                        if (Globals.ArrayContains(empsToUnallocate, shft.Name))
                        {
                            unallocated += temp;
                        }
                        else
                            leResult.Add(new Entry(shft.Name, shft.EmployeeID, temp, temp, shft.Hours));
                    }
                }
            }
            if (lsHostShifts != null)
            {
                foreach (int key in lsHostShifts.Keys)
                {
                    Shift shft = lsHostShifts[key];
                    if (dHostHours == 0)
                    {
                        if (Globals.ArrayContains(empsToUnallocate, shft.Name))
                            unallocated += 0;
                        else
                            leResult.Add(new Entry(shft.Name, shft.EmployeeID, 0, 0, shft.Hours));
                    }
                    else
                    {
                        double temp = Math.Round(HostAmount * ((shft.ClockOut - shft.ClockIn).TotalHours / dHostHours), 2);
                        if (Globals.ArrayContains(empsToUnallocate, shft.Name))
                            unallocated += temp;
                        else
                            leResult.Add(new Entry(shft.Name, shft.EmployeeID, temp, temp, shft.Hours));
                    }
                }
            }
            return leResult;
        }

        public List<Entry> GetOtherEntries(DateTime currentDay, Dictionary<int, Shift> shifts, string[] jobCodes,
            Dictionary<int, Shift> AMBarShifts, Dictionary<int, Shift> AMHostShifts,
            Dictionary<int, Shift> PMBarShifts, Dictionary<int, Shift> PMHostShifts,
            string[] empsToUnallocate)
        {
            List<Entry> lTemp = new List<Entry>();

            DateTime start = new DateTime(currentDay.Year, currentDay.Month, currentDay.Day,
                3, 0, 0);
            DateTime end = new DateTime(currentDay.AddDays(1).Year, currentDay.AddDays(1).Month, currentDay.AddDays(1).Day,
                2, 59, 59);

            Dictionary<int, Shift> newshifts = new Dictionary<int, Shift>();
            foreach (int key in shifts.Keys)
                if (!Globals.ArrayContains(empsToUnallocate, shifts[key].Name)
                    && Globals.ArrayContains(jobCodes, shifts[key].JobCode)
                    && shifts[key].ClockIn > start
                    && shifts[key].ClockIn < end)
                    newshifts.Add(newshifts.Count, shifts[key]);

            lTemp.AddRange(GetUniqueEntries(newshifts, lTemp, empsToUnallocate));
            lTemp.AddRange(GetUniqueEntries(AMBarShifts, lTemp, empsToUnallocate));
            lTemp.AddRange(GetUniqueEntries(AMHostShifts, lTemp, empsToUnallocate));
            lTemp.AddRange(GetUniqueEntries(PMBarShifts, lTemp, empsToUnallocate));
            lTemp.AddRange(GetUniqueEntries(PMHostShifts, lTemp, empsToUnallocate));

            lTemp.Sort();
            lTemp = Common.GetUniqueEntries(lTemp);

            if (ConfigHelper.AllowUndistributed)
                lTemp.Add(new Entry("Undistributed", "UNDIST", 0, 0, 0));

            return lTemp;
        }
        #endregion

        private List<Entry> GetUniqueEntries(List<Entry> list, List<Entry> original, string[] empsToUnallocate)
        {
            Dictionary<int, Shift> lTemp = new Dictionary<int, Shift>();
            foreach (Entry ent in list)
                lTemp.Add(lTemp.Count, new Shift(ent.Name, ent.EmployeeID, "-1", ConfigHelper.DateTimeNow, ConfigHelper.DateTimeNow));
            return GetUniqueEntries(lTemp, original, empsToUnallocate);
        }

        private List<Entry> GetUniqueEntries(Dictionary<int, Shift> newshifts, List<Entry> current, string[] empsToUnallocate)
        {
            List<Entry> result = new List<Entry>();

            foreach (int key in newshifts.Keys)
            {
                if (!Globals.ArrayContains<string>(empsToUnallocate, newshifts[key].Name)
                    && !Common.EntryListContains(newshifts[key].Name, newshifts[key].EmployeeID, current)
                    && !Globals.ArrayContains<string>(StaticProps.ServerOverrideNames, newshifts[key].Name))
                {
                    current.Add(new Entry(newshifts[key].Name, newshifts[key].EmployeeID, 0, 0, newshifts[key].Hours));
                    result.Add(new Entry(newshifts[key].Name, newshifts[key].EmployeeID, 0, 0, newshifts[key].Hours));
                }
            }

            return result;
        }
    }
}
