using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using Common;
using System.Threading;

namespace SupportProfilerSvc
{
    public partial class SupportProfilerSvc : ServiceBase
    {
        #region Vars
        bool bContinueChecks = false;
        Thread tCheckerThread;
        List<Thread> ltProfilingThreads;
        string sStoreName;
        #endregion

        #region ServiceMeths
        public SupportProfilerSvc()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Start();
        }

        protected override void OnStop()
        {
            Stop();
        }

        public void Start()
        {
            this.sStoreName = GetStoreName();
            ltProfilingThreads = new List<Thread>();
            StartCheckerSchedule();
        }

        public void Stop()
        {
        }
        #endregion

        #region Methods
        #region GeneralMethods
        private string GetStoreName()
        {
            return System.Net.Dns.GetHostName();
        }
        #endregion

        #region CheckerMethods
        private void StartCheckerSchedule()
        {
            bContinueChecks = true;
            tCheckerThread = new Thread(ThreadChecks);
            tCheckerThread.Start(new ThreadArgs() { StoreName = this.sStoreName });
        }

        private void ThreadChecks(object o)
        {
            ThreadArgs ta = (ThreadArgs)o;
            ConcordEI.TipshareWS.TipshareWS tws = new global::SupportProfilerSvc.ConcordEI.TipshareWS.TipshareWS();

            while (this.bContinueChecks)
            {
                SPRequest req = Check(ta.StoreName, tws);
                if (req != null)
                {
                    int index = ltProfilingThreads.Count;
                    ltProfilingThreads.Add(new Thread(SubmitNewProfile));
                    ltProfilingThreads[index].Start();
                }
                Thread.Sleep(1000 * 60 * 5);
            }
        }

        private SPRequest Check(string storename, global::SupportProfilerSvc.ConcordEI.TipshareWS.TipshareWS tws)
        {
            global::SupportProfilerSvc.ConcordEI.TipshareWS.SPRequest[] tempRequests = tws.GetRequests(storename);

            if (tempRequests != null && tempRequests.Length > 0)
            {
                SPRequest result = new SPRequest()
                {
                    Id = tempRequests[0].Id,
                    Source = (SPSource)(int)tempRequests[0].Source,
                    Status = SPRequestStatus.Pending,
                    StoreId = tempRequests[0].StoreId,
                    SubmitDate = tempRequests[0].SubmitDate
                };

                tempRequests[0].Status = global::SupportProfilerSvc.ConcordEI.TipshareWS.SPRequestStatus.Pending;
                tws.UpdateRequest(tempRequests[0]);
                return result;
            }
            return null;
        }
        #endregion

        #region ProfileMethods
        private void SubmitNewProfile()
        {
            //todo:  get stats!!
            Globals.Log("would be getting data!!");
        }
        #endregion

        #endregion
    }

    class ThreadArgs
    {
        public string StoreName;
    }
}
