using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using StatRetrieval.Managers.CycleManager;
using StatRetrieval.Shared.Common;
using StatRetrieval.Shared.Common.Contracts;
using StatRetrieval.Shared.Common.DataContracts;

namespace StatRetrieval.Clients.SRService
{
    public partial class SRService : ServiceBase
    {
        CycleManager cycleManager = new CycleManager();
        Timer timer = new Timer();

        public SRService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            ConsoleStart();
        }

        protected override void OnStop()
        {
            ConsoleStop();
        }

        internal void ConsoleStop()
        {
            timer.Stop();
        }

        internal void ConsoleStart()
        {
            cycleManager.InitFirstRun(); // sets ConfigHelper

            cycleManager.OnTimerChange += (o, e) =>
            {
                timer.Stop();
                timer.Interval = ConfigHelper.CycleFrequency_Milliseconds;
                LogHelper.Log("Cycle interval updated to " + ConfigHelper.CycleFrequency_Seconds + "s",
                    LogLocation.SRService, LogType.Generic);
                timer.Start();
            };

            timer.Interval = ConfigHelper.CycleFrequency_Milliseconds;
            timer.Elapsed += timer_Elapsed;
            timer.Start();
            System.Threading.ThreadPool.QueueUserWorkItem(
                new System.Threading.WaitCallback((o) =>
            {
                timer_Elapsed(null, null);
            }));
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            cycleManager.RunCycle();
        }

        internal void RunTestMode()
        {
            cycleManager.RunTests();
        }
    }
}
