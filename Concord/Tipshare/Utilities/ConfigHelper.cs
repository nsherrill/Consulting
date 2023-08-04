using System;
using System.Configuration;

namespace Tipshare
{
    public static class ConfigHelper
    {
        public static string[] ServerJobCodes { get { return GetStringArray("ServerJobCodes", null); } }
        public static string[] HostJobCodes { get { return GetStringArray("HostJobCodes", null); } }
        public static string[] BarJobCodes { get { return GetStringArray("BarJobCodes", null); } }
        public static string[] OtherJobCodes { get { return GetStringArray("OtherJobCodes", null); } }

        private static bool _allowUndistributed = GetBool("AllowUndistributed", true);
        public static bool AllowUndistributed { get { return _allowUndistributed; } set { _allowUndistributed = value; } }

        public static bool bTest = ConfigurationManager.AppSettings["setting"].Contains("test");

        public static string[] AdditionalServerJobCodes { get { return GetStringArray("AdditionalServerJobCodes", new string[] { }); } }

        public static int MinimumShiftLength { get { return GetInt("MinimumShiftLength", 30); } }

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

        public static DayOfWeek StartOfWeekDay_MinusOne
        {
            get
            {
                var temp = (int)StartOfWeekDay;
                if (temp == 0)
                    temp += 7;
                temp -= 1;
                var dow = (DayOfWeek)temp;
                return dow;
            }
        }

        public static bool EnableBarHostSplit
        {
            get
            {
                bool enableBarHostSplit;
                var tempEnableBarHostSplit = GetString("EnableBarHostSplit", "true");
                if (!bool.TryParse(tempEnableBarHostSplit, out enableBarHostSplit))
                    enableBarHostSplit = true;
                return enableBarHostSplit;
            }
        }

        public static bool EnableRestrictedSaves
        {
            get
            {
                bool dftVal = true;
                bool result;
                var tempVal = GetString("EnableRestrictedSaves", dftVal.ToString());
                if (!bool.TryParse(tempVal, out result))
                    result = dftVal;
                return result;
            }
        }

        public static double LowerAmountAllowed
        {
            get
            {
                if (EnableRestrictedSaves)
                {
                    double result;
                    double dft = 1;
                    string temp = GetString("lowerAmountAllowed", dft.ToString());
                    if (!double.TryParse(temp, out result))
                        result = dft;
                    return result;
                }
                else return 1000000;
            }
        }

        public static double UpperAmountAllowed
        {
            get
            {
                if (EnableRestrictedSaves)
                {
                    double result;
                    double dft = 10;
                    string temp = GetString("upperAmountAllowed", dft.ToString());
                    if (!double.TryParse(temp, out result))
                        result = dft;
                    return result;
                }
                else return 1000000;
            }
        }

        public static DateTime DateTimeNow
        {
            get
            {
                DateTime dateResult;
                if (DateTime.TryParse(GetString("NowDateOverride", "null"), out dateResult)) return dateResult;
                return DateTime.Now;
            }
        }

        #region privates
        private static string[] GetStringArray(string key, string[] defaultVal)
        {
            string resultStr = GetString(key, "");
            if (!string.IsNullOrEmpty(resultStr))
            {
                string[] result = resultStr.Split(',');
                return result;
            }
            return defaultVal;
        }

        private static bool GetBool(string key, bool defaultVal = false)
        {
            bool result = defaultVal;
            string tempResult = ConfigurationManager.AppSettings[key];

            if (!string.IsNullOrEmpty(tempResult))
            {
                tempResult = tempResult.Trim().ToLower();

                if (bool.TryParse(tempResult, out result))
                {
                }
                else if (int.TryParse(tempResult, out var intResult))
                {
                    result = intResult > 0;
                }
                else if (tempResult == "yes")
                    result = true;
                else if (tempResult == "no")
                    result = false;
                else if (tempResult.StartsWith("t"))
                    result = true;
                else if (tempResult.StartsWith("f"))
                    result = false;
            }
            return result;
        }

        private static int GetInt(string key, int defaultVal = -1)
        {
            int result;
            string tempResult = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrEmpty(tempResult)
                || !int.TryParse(tempResult, out result))
            {
                result = defaultVal;
            }
            return result;
        }

        private static string GetString(string key, string defaultVal)
        {
            string result = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrEmpty(result))
                result = defaultVal;
            return result;
        }
        #endregion
    }
}
