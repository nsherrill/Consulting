using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace TipshareHistoryRetreiver
{
    public static class ConfigHelper
    {
        private static DayOfWeek DefaultStartOfWeekDay = DayOfWeek.Monday;
        private static string strStartOfWeekDay { get { return GetString("StartOfWeekDay", DefaultStartOfWeekDay.ToString()); } }
        private static DayOfWeek? typedStartOfWeekDay = null;
        public static DayOfWeek StartOfWeekDay
        {
            get
            {
                if (typedStartOfWeekDay == null)
                {
                    try
                    {
                        typedStartOfWeekDay = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), strStartOfWeekDay, true);
                    }
                    catch (Exception) { typedStartOfWeekDay = DefaultStartOfWeekDay; }
                }
                return typedStartOfWeekDay.Value;
            }
        }

        private static string GetString(string key, string defaultVal)
        {
            string result = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrEmpty(result))
                result = defaultVal;
            return result;
        }

        public static DateTime DateTimeNow { get { return DateTime.Now; } }
    }
}
