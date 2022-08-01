using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace Tipshare
{
    public static class Logger
    {
        public static void Log(string logmessage, bool isStart)
        {
            if (isStart)
                Log(Environment.NewLine + Environment.NewLine + logmessage);
            else
                Log(logmessage);
        }

        public static void Log(string logmessage)
        {
            string logFile = "tipshare.log";
            try
            {
                CheckForHugeLogFile(logFile);

                using (StreamWriter sw = File.AppendText(Path.Combine(Directory.GetCurrentDirectory(), logFile)))
                {
                    sw.WriteLine(ConfigHelper.DateTimeNow + ": " + logmessage);
                }
            }
            catch (Exception)
            {
                //dcDebugContainer[iCurrDay].ErrorStrings.Add("Exception saving to file: " + e.ToString());
                Thread.Sleep(200);
                try
                {
                    using (StreamWriter sw = File.AppendText(Path.Combine(Directory.GetCurrentDirectory(), logFile)))
                    {
                        sw.WriteLine(ConfigHelper.DateTimeNow + ": " + logmessage);
                    }
                }
                catch (Exception)
                {
                    //dcDebugContainer[iCurrDay].ErrorStrings.Add("Exception saving to file AGAIN: " + e2.ToString());
                }
            }
        }

        private static void CheckForHugeLogFile(string logFile)
        {
            FileInfo fi = new FileInfo(logFile);
            if (fi.Length > 2000000)
            {
                try
                {
                    fi.Delete();
                }
                catch (Exception)
                {
                    try
                    {
                        fi.Delete();
                    }
                    catch (Exception)
                    { }
                }
            }
        }
    }
}
