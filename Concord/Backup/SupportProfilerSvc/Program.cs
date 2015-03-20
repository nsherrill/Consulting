using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace SupportProfilerSvc
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            if (args != null && args.Length > 0 && args[0] == "console")
            {
                SupportProfilerSvc sps = new SupportProfilerSvc();
                sps.Start();

                Console.WriteLine("press enter to stop");
                Console.ReadLine();

                sps.Stop();
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] 
			    { 
				    new SupportProfilerSvc() 
			    };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
