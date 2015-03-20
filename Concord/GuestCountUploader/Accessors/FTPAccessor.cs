using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Common;

namespace GuestCountUploader.Accessors
{
    public class FTPAccessor
    {
        public void UploadFile(string contents, string destinationFilename, string relativeDestination)
        {
            Globals.Log(string.Format("Uploading to {0}: {1}", destinationFilename, contents));

            try
            {
                var fileLoc = Path.Combine(ConfigHelper.FTPOutputLocation, Path.Combine(relativeDestination, destinationFilename));
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(fileLoc);
                request.Method = WebRequestMethods.Ftp.UploadFile;

                request.Credentials = new NetworkCredential(ConfigHelper.FTPOutputUsername, ConfigHelper.FTPOutputPassword);

                byte[] fileContents = Encoding.UTF8.GetBytes(contents);
                request.ContentLength = fileContents.Length;

                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(fileContents, 0, fileContents.Length);
                }

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    Console.WriteLine("Upload File Complete, status {0}", response.StatusDescription);
                }
            }
            catch (Exception e)
            {
                Globals.Log("exception caught while uploading to FTP loc: " + e.ToString(), "FTPAccessor.log");
            }
        }
    }
}
