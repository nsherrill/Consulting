using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace StatRetrieval.Clients.SRService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            var shouldContinue = CheckForSameProcess();

            if (shouldContinue)
            {
                if (args != null
                    && args.Length > 0
                    && args[0] == "console")
                {
                    SRService srService = new SRService();
                    srService.ConsoleStart();

                    if (args.Length > 1
                        && args[1] == "test")
                        srService.RunTestMode();

                    Console.WriteLine("enter to exit");
                    Console.ReadLine();
                    srService.ConsoleStop();
                }
                else
                {
                    ServiceBase[] ServicesToRun = new ServiceBase[] 
                { 
                    new SRService() 
                };
                    ServiceBase.Run(ServicesToRun);
                }
            }
        }

        private static bool CheckForSameProcess()
        {
            var processes = System.Diagnostics.Process.GetProcesses();
            var statRetrievalProcs = processes.Where(p => p.ProcessName.ToLower().Contains("statretriev"));
            var currCount = statRetrievalProcs.Where(p => !p.ProcessName.ToLower().Contains("vshost")).Count();

            return currCount < 2;
        }
    }
}
