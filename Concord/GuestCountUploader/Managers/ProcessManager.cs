using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using GuestCountUploader.Accessors;
using GuestCountUploader.Engines;

namespace GuestCountUploader.Managers
{
    public class ProcessManager
    {
        public void PopulateAndUploadCustomerDefinitions()
        {
            var custFileAcc = new LocalFileAccessor();
            var sourceDefs = custFileAcc.GetDefinitions();

            var guestCountAcc = new GuestCountAccessor();
            List<CustomerDefinition> results = new List<CustomerDefinition>();
            foreach (var def in sourceDefs)
            {
                def.LastGuestCount = guestCountAcc.GetGuestCount(def.IP, DateTime.Now.AddDays(-1).Date);
            }

            var dataConversionEng = new DataConversionEngine();
            var fileContents = dataConversionEng.FormatDefinitions(sourceDefs, DateTime.Now.AddDays(-1).Date);

            var ftpAcc = new FTPAccessor();
            ftpAcc.UploadFile(fileContents, "file.txt", "");
        }
    }
}
