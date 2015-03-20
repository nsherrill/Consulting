using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;
using System.IO;

namespace Common
{
    public class Globals
    {
        public static bool Testing
        {
            get
            {
                string temp = System.Configuration.ConfigurationManager.AppSettings["Mode"];
                if (!string.IsNullOrEmpty(temp) && temp.ToLowerInvariant().Contains("test"))
                    return true;
                return false;
            }
        }

        public static bool CanHitDB
        {
            get
            {
                try
                {
                    string connString = System.Configuration.ConfigurationManager.ConnectionStrings["CCWeb"].ConnectionString;
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        conn.Open();
                    }
                    return true;
                }
                catch (Exception e)
                {
                    Log("Couldn't hit DB\r\n"+e.ToString());
                    return false;
                }
            }
        }

        public static void Log(string message, string filename = "tipshare.log")
        {
            try
            {
                //if (!EventLog.SourceExists("ConcordWeb"))
                //{
                //    EventLog.CreateEventSource("ConcordWeb", "ConcordWeb");
                //    while (!EventLog.SourceExists("ConcordWeb"))
                //    {
                //        System.Threading.Thread.Sleep(1000);
                //    }
                //}
                //if (EventLog.SourceExists("ConcordWeb"))
                //{
                //    // Create an EventLog instance and assign its source.
                //    EventLog myLog = new EventLog();
                //    myLog.Source = "ConcordWeb";

                //    // Write an informational entry to the event log.    
                //    myLog.WriteEntry(message);
                //}

                System.IO.File.AppendAllText(Path.Combine(Directory.GetCurrentDirectory(), filename),
                    Environment.NewLine + DateTime.Now.ToString() + ":  " + message);

            }
            catch (Exception)
            {
                try
                {
                    System.IO.File.AppendAllText(Path.Combine(Directory.GetCurrentDirectory(), filename),
                        Environment.NewLine + DateTime.Now.ToString() + ":  " + message);
                }
                catch (Exception e)
                {
                    Console.WriteLine("logging.. " + Environment.NewLine + e.ToString());
                }
            }
        }

        public static void SendEmail(string to, string from, string subject, string body)
        {
            MailMessage message = new MailMessage(from, to, subject, body);
            message.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient("smtpout.secureserver.net");
            smtp.Credentials = new NetworkCredential("nick@the-sherrills.com", "mackelvien");
            smtp.Send(message);
        }

        internal static bool ExecuteNonQuery(string query)
        {
            try
            {
                string connString =
                    @"Data Source=SC\SQLEXPRESS;initial catalog=Concord;Persist Security " +
                    @"Info=false;User ID=ccweb;password=thund3r!;";
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    using (SqlCommand com = new SqlCommand(query, conn))
                    {
                        com.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }

        public static bool ArrayContains<T>(T[] list, T searchItem)
        {
            if (typeof(T).ToString() == typeof(string).ToString())
            {
                var newList = list as string[];
                foreach (string obj in newList)
                {
                    if(obj.Equals(searchItem as string, StringComparison.InvariantCultureIgnoreCase))
                        return true;
                }
            }
            else
            {
                foreach (object obj in list)
                    if (obj.Equals(searchItem))
                        return true;
            }
            return false;        
        }
    }
}
