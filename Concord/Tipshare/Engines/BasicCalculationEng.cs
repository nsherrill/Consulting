using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tipshare.Engines
{
    public class BasicCalculationEng
    {
        private ShiftDeterminationEng shiftDeterEng = new ShiftDeterminationEng();

        public double AddUpHours(Dictionary<int, Shift> shifts)
        {
            double temp = 0;
            foreach (int key in shifts.Keys)
            {
                Shift shft = shifts[key];
                temp += (shft.ClockOut - shft.ClockIn).TotalHours;
            }
            return temp;
        }

        public double GetHours(Dictionary<int, Shift> lsShifts, DateTime dtCurrentDay, string sEmpId, string[] jobCodes, double dSuggested, double dTotalSuggested, bool isAM)
        {
            if (dTotalSuggested != 0)
            {
                Dictionary<int, Shift> shifts = shiftDeterEng.GetShifts(dtCurrentDay, lsShifts, jobCodes, isAM);
                Dictionary<int, Shift> empShifts = new Dictionary<int, Shift>();
                foreach (int key in shifts.Keys)
                {
                    Shift sft = shifts[key];
                    if (sft.EmployeeID == sEmpId)
                        empShifts.Add(empShifts.Count, sft);
                }
                if (empShifts.Count == 0)
                    return 0;
                else if (empShifts.Count == 1)
                    return empShifts[0].Hours;
                else
                {
                    double totalHours = GetTotalHours(shifts, jobCodes);
                    if (totalHours > 0)
                        foreach (int key in empShifts.Keys)
                        {
                            Shift sft = empShifts[key];
                            if (sft.Hours / totalHours > dSuggested / dTotalSuggested - .05
                                && sft.Hours / totalHours < dSuggested / dTotalSuggested + .05)
                            {
                                return sft.Hours;
                            }
                        }

                    return 0;
                }
            }
            else
                return 0;
        }

        public double GetTotalHours(Dictionary<int, Shift> shifts, string[] jobCodes)
        {
            double result = 0;
            foreach (int key in shifts.Keys)
                if (Globals.ArrayContains(jobCodes, shifts[key].JobCode))
                    result += shifts[key].Hours;
            return result;
        }
    }
}
