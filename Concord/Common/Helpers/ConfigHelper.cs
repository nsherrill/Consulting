using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Common
{
    public static class ConfigHelper
    {
        #region props
        public static DateTime DateTimeNow { get { return DateTime.Now; } }
        public static bool IsTestMode { get { return GetConfigBool("IsTestMode", false); } }
        public static string StoreListPath { get { return GetConfigEntry("StoreListPath", "StoreList.txt"); } }
        public static string FTPOutputLocation { get { return GetConfigEntry("FTPOutputLocation"); } }
        public static string FTPOutputUsername { get { return GetConfigEntry("FTPOutputUsername"); } }
        public static string FTPOutputPassword { get { return GetConfigEntry("FTPOutputPassword"); } }

        private static DayOfWeek DefaultStartOfWeekDay = DayOfWeek.Monday;
        private static string strStartOfWeekDay { get { return GetConfigEntry("StartOfWeekDay", DefaultStartOfWeekDay.ToString()); } }
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
        #endregion

        #region helper methods
        private static Dictionary<string, NameValueCollection> cachedConfigSections = new Dictionary<string, NameValueCollection>();
        private static string GetStringFromSpecificSection(string section, string key, string dft = "")
        {
            string result = dft;
            if (!cachedConfigSections.ContainsKey(section))
                cachedConfigSections[section] = (NameValueCollection)ConfigurationManager.GetSection(section);

            if (cachedConfigSections[section] != null
                && cachedConfigSections[section].AllKeys != null
                && cachedConfigSections[section].AllKeys.Contains(key)
                && !string.IsNullOrEmpty(cachedConfigSections[section][key]))
            {
                result = cachedConfigSections[section][key];
            }

            return result;
        }

        private static double GetConfigDouble(string key, double defaultValue)
        {
            var result = GetConfigObject(key, defaultValue);
            if (result is double)
                return (double)result;
            else if (result is string)
                return double.Parse(result as string);
            return defaultValue;
        }

        private static bool GetConfigBool(string key, bool defaultValue)
        {
            var result = GetConfigObject(key, defaultValue);
            if (result is bool)
                return (bool)result;
            else if (result is string)
                return bool.Parse(result as string);
            return defaultValue;
        }

        private static long GetConfigLong(string key, long defaultValue)
        {
            var result = GetConfigObject(key, defaultValue);
            if (result is long)
                return (long)result;
            else if (result is int)
                return Convert.ToInt64(result);
            else if (result is string)
                return long.Parse(result as string);
            return defaultValue;
        }

        private static int GetConfigInt(string key, int defaultValue)
        {
            var result = GetConfigObject(key, defaultValue);
            if (result is long)
                return (int)result;
            else if (result is string)
                return int.Parse(result as string);
            return defaultValue;
        }

        private static string GetConfigEntry(string key)
        {
            return GetConfigObject(key, null) as string;
        }

        private static string[] GetConfigEntries(string key)
        {
            string[] result = new string[] { };
            string tempResult = ConfigurationManager.AppSettings[key];
            if (!string.IsNullOrEmpty(tempResult))
                result = tempResult.Split(',');
            return result;
        }

        private static string GetConfigEntry(string key, string defaultValue)
        {
            return GetConfigObject(key, defaultValue) as string;
        }

        private static object GetConfigObject(string key, object defaultValue)
        {
            object result = defaultValue;

            result = ConfigurationManager.AppSettings[key];

            if (result == null)
                result = defaultValue;

            return result;
        }

        public static string GetConnectionString(string key)
        {
            string result = ConfigurationManager.ConnectionStrings[key].ConnectionString;
            return result;
        }
        #endregion
    }
}
