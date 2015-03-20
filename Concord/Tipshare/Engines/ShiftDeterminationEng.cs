using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace Tipshare.Engines
{
    public class ShiftDeterminationEng
    {
        public Dictionary<int, Shift> GetShifts(DateTime dtCurrentDay, Dictionary<int, Shift> shifts, string[] jobCodes, bool isAM)
        {
            Dictionary<int, Shift> results = new Dictionary<int, Shift>();

            DateTime start = new DateTime(dtCurrentDay.Year, dtCurrentDay.Month, dtCurrentDay.Day,
                2, 0, 0);
            DateTime end = new DateTime(dtCurrentDay.AddDays(1).Year, dtCurrentDay.AddDays(1).Month, dtCurrentDay.AddDays(1).Day,
                1, 59, 59);

            //bool bContinue = true;
            //for (int i = 0; i < shifts.Count && bContinue; i++)
            bool bLog = false;
            for (int i = 0; i < shifts.Count; i++)
            {
                if (!shifts.ContainsKey(i))
                {
                    Logger.Log("shifts doesnt contain index: " + i + "... going to log'em all!");
                    bLog = true;
                }
            }
            if (bLog)
                foreach (var i in shifts.Keys)
                {
                    Logger.Log("shift exists: " + shifts[i].Name + ", " + shifts[i].ClockIn);
                }

            foreach (var i in shifts.Keys)
            {
                if (shifts[i].ClockIn > start && shifts[i].ClockIn < end
                    && Globals.ArrayContains(jobCodes, shifts[i].JobCode))
                {
                    if (shifts[i].ClockIn.Hour < 12
                        && (shifts[i].ClockOut.Hour > 18
                        || shifts[i].ClockOut.Hour < 4))
                    {
                        if (isAM)
                            results.Add(results.Count, new Shift(shifts[i].Name, shifts[i].EmployeeID, shifts[i].JobCode, shifts[i].ClockIn,
                                new DateTime(shifts[i].ClockOut.Year, shifts[i].ClockOut.Month, shifts[i].ClockOut.Day,
                                    StaticProps.AMPMBreakHour, 0, 0)));
                        else
                            results.Add(results.Count, new Shift(shifts[i].Name, shifts[i].EmployeeID, shifts[i].JobCode,
                                new DateTime(shifts[i].ClockIn.Year, shifts[i].ClockIn.Month, shifts[i].ClockIn.Day,
                                    StaticProps.AMPMBreakHour, 0, 0), shifts[i].ClockOut));
                    }
                    else if (shifts[i].ClockOut.Hour <= 17 && shifts[i].ClockOut.Hour > 4)
                    {
                        if (isAM)
                            results.Add(results.Count, shifts[i]);
                    }
                    else
                    {
                        if (!isAM)
                            results.Add(results.Count, shifts[i]);
                    }
                }
                else if (shifts[i].ClockIn < start)
                    break;
            }
            return results;
        }

        public void GetShifts(DateTime currentDay, Dictionary<int, Shift> shifts, string[] jobCodes,
            out Dictionary<int, Shift> AMShifts, out Dictionary<int, Shift> PMShifts)
        {
            PMShifts = new Dictionary<int, Shift>();
            AMShifts = new Dictionary<int, Shift>();

            DateTime start = new DateTime(currentDay.Year, currentDay.Month, currentDay.Day,
                2, 0, 0);
            DateTime end = new DateTime(currentDay.AddDays(1).Year, currentDay.AddDays(1).Month, currentDay.AddDays(1).Day,
                1, 59, 59);

            foreach (int i in shifts.Keys)
            {
                if ((shifts[i].ClockOut - shifts[i].ClockIn)
                    < new TimeSpan(0, ConfigHelper.MinimumShiftLength, 0))
                {
                    // if the shift is less than N minutes long, ignore it.
                    continue;
                }


                if (shifts[i].ClockIn > start && shifts[i].ClockIn < end
                    && Globals.ArrayContains(jobCodes, shifts[i].JobCode))
                {
                    if (shifts[i].ClockIn.Hour < 12
                        && (shifts[i].ClockOut.Hour > 18
                        || shifts[i].ClockOut.Hour < 4))
                    {
                        AMShifts.Add(AMShifts.Count, new Shift(shifts[i].Name, shifts[i].EmployeeID, shifts[i].JobCode, shifts[i].ClockIn,
                            new DateTime(shifts[i].ClockOut.Year, shifts[i].ClockOut.Month, shifts[i].ClockOut.Day,
                                StaticProps.AMPMBreakHour, 0, 0)));
                        PMShifts.Add(PMShifts.Count, new Shift(shifts[i].Name, shifts[i].EmployeeID, shifts[i].JobCode,
                            new DateTime(shifts[i].ClockIn.Year, shifts[i].ClockIn.Month, shifts[i].ClockIn.Day,
                                StaticProps.AMPMBreakHour, 0, 0), shifts[i].ClockOut));
                    }
                    else if (shifts[i].ClockOut.Hour <= 17 && shifts[i].ClockOut.Hour > 4)
                        AMShifts.Add(AMShifts.Count, shifts[i]);
                    else
                        PMShifts.Add(PMShifts.Count, shifts[i]);
                }
                else if (shifts[i].ClockIn < start && StaticProps.PosType != POSType.Aloha)
                    break;
            }
        }
    }
}
