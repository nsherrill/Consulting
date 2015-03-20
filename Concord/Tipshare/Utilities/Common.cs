using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace Tipshare
{
    public static class Common
    {
        public static bool EntryListContains(string checkName, string checkEmployeeID, List<Entry> current)
        {
            foreach (Entry ent in current)
                if (ent.Name.Equals(checkName) && ent.EmployeeID.Equals(checkEmployeeID))
                    return true;
            return false;
        }

        public static DateTime GetFirstClockIn(Dictionary<int, Shift> shifts)
        {
            DateTime dtTemp = DateTime.Now;
            foreach (int key in shifts.Keys)
            {
                Shift shft = shifts[key];
                if (dtTemp > shft.ClockIn)
                    dtTemp = shft.ClockIn;
            }
            return dtTemp;
        }

        public static Shift IsTicketInShifts(Ticket ticket, Dictionary<int, Shift> shifts)
        {
            foreach (int i in shifts.Keys)
            {
                if (shifts[i].EmployeeID == ticket.EmployeeID)
                {
                    int temp = ShiftAndTicketIsAM(shifts[i], ticket);
                    if (temp != 0)
                    {
                        return shifts[i];
                        //return temp;
                    }
                }
            }
            return null;
        }

        public static int ShiftAndTicketIsAM(Shift shift, Ticket ticket)
        {
            if (shift.EmployeeID == ticket.EmployeeID
                && shift.ClockIn < ticket.ServeDate
                && shift.ClockOut > ticket.ServeDate)
            {
                if (shift.ClockIn.Hour < 12 && (shift.ClockOut.Hour > 18 || shift.ClockOut.Hour < 4))
                {
                    if (ticket.ServeDate.Hour < 16 || ticket.ServeDate.Hour > 4)
                        return -1;
                    else
                        return 1;
                }
                else if (shift.ClockIn.Hour < 15 && shift.ClockOut.Hour <= 17 && shift.ClockOut.Hour > 4)
                    return -1;
                else
                    return 1;
            }
            return 0;
        }

        public static Dictionary<int, Shift> GetServerShifts(DateTime currentDay, Dictionary<int, Shift> lsShifts)
        {
            DateTime start = new DateTime(
                currentDay.Year, currentDay.Month, currentDay.Day,
                2, 0, 0);
            DateTime end = new DateTime(
                currentDay.AddDays(1).Year, currentDay.AddDays(1).Month, currentDay.AddDays(1).Day,
                1, 59, 59);

            Dictionary<int, Shift> result = new Dictionary<int, Shift>();

            foreach (int key in lsShifts.Keys)
                if (Globals.ArrayContains(StaticProps.ServerJobCodes, lsShifts[key].JobCode)
                    //                    shft.JobCode.Equals("3")
                    && lsShifts[key].ClockIn > start
                    && lsShifts[key].ClockIn < end)
                    result.Add(result.Count, lsShifts[key]);

            return result;
        }

        public static DateTime GetLastClockOut(List<Shift> shifts)
        {
            DateTime dtTemp = DateTime.MinValue;
            foreach (Shift shft in shifts)
                if (dtTemp < shft.ClockOut)
                    dtTemp = shft.ClockOut;
            return dtTemp;
        }

        public static List<Entry> GetUniqueEntries(List<Entry> startList)
        {
            List<Entry> results = new List<Entry>();
            List<string> recordOfIds = new List<string>();

            startList.ForEach(e =>
            {
                if (!recordOfIds.Contains(e.EmployeeID))
                {
                    results.Add(e);
                    recordOfIds.Add(e.EmployeeID);
                }
            });

            return results;
        }

        public static string GetStoreName()
        {
            return System.Net.Dns.GetHostName();
        }

        public static string GetInternalFilename(DateTime dtEndDate)
        {
            string testStart = "";
            if (ConfigHelper.bTest)
                testStart = "test-";

            return testStart + "TS_" + dtEndDate.Date.ToShortDateString().Replace('/', '-') + ".csv";
        }

        public static int GetDateIndex(DateTime currDate, DateTime dtNow)
        {
            DateTime temp = dtNow;
            int i = 0;
            while (temp.Date > currDate.Date)
            {
                temp = temp.AddDays(-1);
                i++;
            }
            if (temp.Date < currDate.Date)
                return -1;
            return i;
            //for (int i = 0; i < 7; i++)
            //    if (dtEarliestViewable.AddDays(i).Date == currDate.Date)
            //        return i;
            //return -1;
        }

        public static int FindHowManyDaysTilSOWD(DateTime dtCurrentDay)
        {
            DateTime temp = dtCurrentDay.AddDays(-1);
            int i = 1;
            while (temp.DayOfWeek != ConfigHelper.StartOfWeekDay)
            {
                temp = temp.AddDays(-1);
                i++;
            }
            return i;
        }

        public static string GetDayName(int i)
        {
            int sDOW = (int)ConfigHelper.StartOfWeekDay;
            sDOW += i;
            if (sDOW >= 7)
                sDOW -= 7;

            DayOfWeek typedSDOW = (DayOfWeek)sDOW;
            return typedSDOW.ToString();

            //return i == 0 ? "Thursday" :
            //    i == 1 ? "Friday" :
            //    i == 2 ? "Saturday" :
            //    i == 3 ? "Sunday" :
            //    i == 4 ? "Monday" :
            //    i == 5 ? "Tuesday" :
            //    i == 6 ? "Wednesday" :
            //    "Unknown";
        }

        public static string GetID(string original)
        {
            return original.Substring(original.IndexOf(" - ") + 2).Trim();
        }

        public static string GetName(string original)
        {
            return original.Substring(0, original.IndexOf(" - ")).Trim();
        }
    }
}
