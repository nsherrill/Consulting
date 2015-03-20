using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GuestCountUploader.Managers;

namespace GuestCountUploader
{
    class Program
    {
        static void Main(string[] args)
        {
            ProcessManager pm = new ProcessManager();
            pm.PopulateAndUploadCustomerDefinitions();
        }
    }
}
