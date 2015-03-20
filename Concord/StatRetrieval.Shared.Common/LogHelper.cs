using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using StatRetrieval.Shared.Common.DataContracts;

namespace StatRetrieval.Shared.Common
{
    public static class LogHelper
    {
        public static EventHandler<ErrorEventArgs> ErrorFired { get; set; }

        static object lockObj = new object();
        public static void Log(string text, LogLocation logLoc, LogType logType)
        {
            try
            {
                //if (logType == LogType.Error
                //    && ErrorFired != null)
                //{
                //    ErrorFired(new object(), new ErrorEventArgs(text, logLoc));
                //}

                text = string.Format("{0}{1}:\t{2}_{3}: {4}", Environment.NewLine, DateTime.Now, logLoc.ToString(), logType.ToString(), text);
                Console.Write(text);
                string filePath = Path.Combine(ConfigHelper.LogPath, "StatRetrieval_log.txt");

                lock (lockObj)
                {
                    if (!Directory.Exists(ConfigHelper.LogPath))
                        Directory.CreateDirectory(ConfigHelper.LogPath);
                    if ((logType != LogType.Verbose || ConfigHelper.LogVerbose)
                        && (logType != LogType.ExtraVerbose || ConfigHelper.LogExtraVerbose))
                    {
                        File.AppendAllText(filePath, text);
                    }
                }
            }
            catch
            {
            }
        }
    }

    public class ErrorEventArgs : EventArgs
    {
        public LogLocation LogLoc { get; set; }
        public string LogText { get; set; }

        public ErrorEventArgs(string logText, LogLocation logLoc)
        {
            this.LogLoc = logLoc;
            this.LogText = logText;
        }
    }
}
