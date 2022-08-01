using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace StatRetrieval.Shared.Common
{
    public static class ConfigHelper
    {
        public static int? _storeId = null;
        public static int StoreId
        {
            get
            {
                if (_storeId == null || !_storeId.HasValue) 
                    return GetInt("StoreId", -1);
                return _storeId.Value;
            }
            set { _storeId = value; }
        }
        public static string HostName { get; set; }
        public static string DatabaseAddress { get { return GetString("DBAddress", "localhost"); } }
        public static string BackupDatabaseAddress { get { return GetString("BackupDBAddress", "192.168.101.4"); } }
        public static string DBUser { get { return GetString("DBUser", "franuser"); } }
        //public static string DBUser { get { return GetString("DBUser", "sa"); } }
        public static string DBPassword { get { return GetString("DBPassword", "tNCn31cVeP"); } }
        //public static string DBPassword { get { return GetString("DBPassword", "ratsle"); } }
        public static string LineTrackerFilePath { get { return GetString("LineTrackerFilePath", @"c:\temp\LineTrackerFilePath.txt"); } }

        private static int _cycleFrequency_Milliseconds = 15 * 60 * 1000;
        public static int CycleFrequency_Milliseconds { get { return _cycleFrequency_Milliseconds; } set { _cycleFrequency_Milliseconds = value; } }
        public static int CycleFrequency_Seconds { get { return _cycleFrequency_Milliseconds / 1000; } set { _cycleFrequency_Milliseconds = value * 1000; } }

        public static string LocalResultsPath { get { return GetString("LocalResultsPath", @"c:\temp\CycleResults"); } }
        public static string LogPath { get { return GetString("LogPath", @"c:\logs\CycleResults\"); } }

        private static string _resultsUrl = "https://assets.bunchball.net/customers/applebees/aposResults.php";
        public static string ResultsUrl { get { return GetString("ResultsUrl", _resultsUrl); } }

        private static string _queryUrl = "https://assets.bunchball.net/customers/applebees/aposQuery.php";
        public static string QueryUrl { get { return GetString("QueryUrl", _queryUrl); } }

        private static string _errorUrl = "https://assets.bunchball.net/customers/applebees/aposError.php";
        public static string ErrorUrl { get { return GetString("ErrorUrl", _errorUrl); } }

        private static string _nitroApiKey = "531db98c733347b5be6e5be0299f2a22";
        public static string NitroApiKey { get { return GetString("NitroApiKey", _nitroApiKey); } }

        private static string _nitroSecretKey = "311dddde99854574a391b7c6e3c971b2";
        public static string NitroSecretKey { get { return GetString("NitroSecretKey", _nitroSecretKey); } }

        public static DateTime? TESTDATE { get { return GetDateTime("TESTDATE"); } }

        public static bool PostWithSecretKey { get { return GetBool("PostWithSecretKey", false); } } // change to false on Rajat's command.

        public static bool LogVerbose { get { return GetBool("LogVerbose", false); } }
        public static bool LogExtraVerbose { get { return GetBool("LogExtraVerbose", false); } }

        public static DateTime DateTimeNow { get { return DateTime.Now; } }

        #region privates
        private static string GetString(string key, string dft)
        {
            if (ConfigurationManager.AppSettings.AllKeys.Contains(key)
                && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key]))
                return ConfigurationManager.AppSettings[key];
            return dft;
        }

        private static bool GetBool(string key, bool dft)
        {
            bool temp;
            if (ConfigurationManager.AppSettings.AllKeys.Contains(key)
                && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key])
                && bool.TryParse(ConfigurationManager.AppSettings[key], out temp))
                return temp;
            return dft;
        }

        private static int GetInt(string key, int dft)
        {
            int temp;
            if (ConfigurationManager.AppSettings.AllKeys.Contains(key)
                && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key])
                && int.TryParse(ConfigurationManager.AppSettings[key], out temp))
                return temp;
            return dft;
        }

        private static DateTime? GetDateTime(string key)
        {
            DateTime temp;
            if (ConfigurationManager.AppSettings.AllKeys.Contains(key)
                && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key])
                && DateTime.TryParse(ConfigurationManager.AppSettings[key], out temp))
                return temp;
            return null;
        }
        #endregion
    }
}
