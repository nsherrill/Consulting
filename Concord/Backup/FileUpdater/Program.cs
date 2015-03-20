using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Threading;
using System.IO;

namespace FileUpdater
{
    class Program
    {
        static void Main(string[] args)
        {
  //          FileUpdater fu = new FileUpdater(System.Configuration.ConfigurationSettings.AppSettings["setting"]);
  //          fu.Go();
        }
    }

//    class FileUpdater
//    {
//        bool bTest;
//        List<FileUpdaterWS.Files> fFiles = new List<ConcordEI.TipshareWS.Files>();
//        string sServerType;
//        string sDataCenter;
//        string sHostName;
//        List<Thread> threads;

//        public FileUpdater(string setting)
//        {
//            this.bTest = setting.ToLower().Contains("test");
//        }

//        public void Go()
//        {
//            try
//            {
//                GetServerData();

//                FileUpdaterWS.FileUpdaterWS fuws = new global::FileUpdater.FileUpdaterWS.FileUpdaterWS();
//                fFiles = fuws.GetListOfFiles(sServerType, sDataCenter).ToList<FileUpdaterWS.Files>();
//                RemoveOldBackups(fFiles);
//                BackupFiles(fFiles);
//                if (!DownloadFiles(fFiles))
//                {
//                    string message = GenerateEmailMessage(fFiles);
//                    SendQuickEmail("nsherrill@internap.com", "FileUpdater@internapcdn.com", "FileUpdater error for " + sHostName, message, false);
//                }
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine(e.ToString());
//                Console.ReadLine();
//            }
//        }

//        private string GenerateEmailMessage(List<FileUpdaterWS.Files> files)
//        {
//            string temp = "Never finished downloading files:\r\n";
//            foreach (FileUpdaterWS.Files f in files)
//                temp += "\r\n" + f.RemotePath;
//            return temp;
//        }

//        private void SendQuickEmail(string sTo, string sFrom, string sSubject, string sMessage, bool bError)
//        {
//            MailMessage mail;
//            SmtpClient sCli;
//            string error = "";
//            try
//            {
//                mail = new MailMessage(sFrom, sTo);
//                mail.Subject = sSubject;
//                mail.Body = sMessage;

//                sCli = new SmtpClient("sbs2.littlebigendian.com");
//                if (!bTest)
//                    sCli.Send(mail);
//            }
//            catch (Exception e)
//            {
//                bError = true;
//                error = e.ToString();
//            }
//            if (bError)
//            {
//                mail = new MailMessage(sFrom, "nsherrill@internap.com");
//                mail.Subject = "ERROR: " + sSubject;
//                mail.Body = error + "\r\n\r\n\r\n" + sMessage;

//                sCli = new SmtpClient("sbs2.littlebigendian.com");
//                sCli.Send(mail);
//            }
//        }

//        private bool DownloadFiles(List<FileUpdaterWS.Files> fFiles)
//        {
//            threads = new List<Thread>();
//            foreach (FileUpdaterWS.Files f in fFiles)
//            {
//                threads.Add(new Thread(new ParameterizedThreadStart(ThreadMethod)));
//                threads[threads.Count - 1].Start(f);
//            }

//            int i = 0;
//            while (!DetectThreadsFinished() && i < 1000 * 60)
//            {
//                i++;
//                Thread.Sleep(15);
//            }
//            return DetectThreadsFinished();
//        }

//        private void ThreadMethod(object o)
//        {
//            FileUpdaterWS.Files f = (FileUpdaterWS.Files)o;
//            Thread.Sleep(f.SecondsToStart * 1000);
//            try
//            {
//                if (!bTest)
//                {
//                    System.Net.WebClient wc = new System.Net.WebClient();
//                    wc.DownloadFile(f.RemotePath, f.LocalPath);
//                }
//            }
//            catch (Exception)
//            { }
//            return;
//        }

//        private bool DetectThreadsFinished()
//        {
//            bool bDone = true;
//            for (int i = 0; i < threads.Count && bDone; i++)
//                bDone = threads[i].IsAlive;
//            return bDone;
//        }

//        private void BackupFiles(List<FileUpdaterWS.Files> fFiles)
//        {
//            foreach (FileUpdaterWS.Files f in fFiles)
//            {
//                if (File.Exists(f.LocalPath))
//                {
//                    if (f.LocalPath.LastIndexOf('.') > 0)
//                        File.Copy(f.LocalPath, f.LocalPath.Insert(f.LocalPath.LastIndexOf('.'), "2"));
//                    else
//                        File.Copy(f.LocalPath, f.LocalPath + "2");
//                }
//            }
//        }

//        private void RemoveOldBackups(List<FileUpdaterWS.Files> fFiles)
//        {
//            foreach (FileUpdaterWS.Files f in fFiles)
//            {
//                int iIndex = f.LocalPath.LastIndexOf('.') > 0 ? f.LocalPath.LastIndexOf('.') : f.LocalPath.Length - 1;
//                string path = f.LocalPath.Insert(iIndex, "2");
//                for (int i = 3; File.Exists(path); i++)
//                {
//                    File.Delete(path);
//                    iIndex = f.LocalPath.LastIndexOf('.') > 0 ? f.LocalPath.LastIndexOf('.') : f.LocalPath.Length - 1;
//                    path = f.LocalPath.Insert(iIndex, i.ToString());
//                }
//            }
//        }

//        private void GetServerData()
//        {
//            sHostName = System.Net.Dns.GetHostName();
//            if (sHostName.IndexOf('-') >= 0)
//            {
//                sServerType = sHostName.Substring(0, sHostName.IndexOf('-'));
//                sDataCenter = sHostName.Substring(sHostName.IndexOf('-') + 1, 3);
//            }
//            else
//            {
//                sServerType = "UNKNOWN";
//                sDataCenter = "UNKNOWN";
//            }
//        }

      
    //}
}
