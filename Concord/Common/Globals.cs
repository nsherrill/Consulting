using System;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace Common
{
    public class Globals
    {
        public static bool Testing
        {
            get
            {
                string temp = System.Configuration.ConfigurationManager.AppSettings["Mode"];
                string temp2 = System.Configuration.ConfigurationManager.AppSettings["setting"];
                if ((!string.IsNullOrEmpty(temp) && temp.ToLowerInvariant().Contains("test"))
                    || (!string.IsNullOrEmpty(temp2) && temp2.ToLowerInvariant().Contains("test")))
                {
                    return true;
                }
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
                    Log("Couldn't hit DB\r\n" + e.ToString());
                    return false;
                }
            }
        }

        public static void Log(string message, bool isTest = false, string fileName = "tipshare.log")
        {
            if (isTest && !Testing) return;

            try
            {

                System.IO.File.AppendAllText(Path.Combine(Directory.GetCurrentDirectory(), fileName),
                    Environment.NewLine + ConfigHelper.DateTimeNow.ToString() + ":  " + message);

            }
            catch (Exception)
            {
                try
                {
                    System.IO.File.AppendAllText(Path.Combine(Directory.GetCurrentDirectory(), fileName),
                        Environment.NewLine + ConfigHelper.DateTimeNow.ToString() + ":  " + message);
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
                    if (obj.Equals(searchItem as string, StringComparison.InvariantCultureIgnoreCase))
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
