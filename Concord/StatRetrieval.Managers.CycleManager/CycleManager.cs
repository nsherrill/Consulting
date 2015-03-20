using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using StatRetrieval.Accessors.DataAccessor;
using StatRetrieval.Accessors.WebAccessor;
using StatRetrieval.Engines.DataProcessorEngine.cs;
using StatRetrieval.Shared.Common;
using StatRetrieval.Shared.Common.Contracts;
using StatRetrieval.Shared.Common.DataContracts;

namespace StatRetrieval.Managers.CycleManager
{
    public class CycleManager : ICycleManager
    {
        public event EventHandler OnTimerChange;

        IDbAccessor dbAccessor = new DbAccessor();
        IFileAccessor fileAccessor = new FileAccessor();
        IQueryRequestAccessor queryRequestAccessor = new QueryRequestAccessor();
        ILocalConfigAccessor localConfigAccessor = new LocalConfigAccessor();
        IResultProcessorEng resultProcessorEng = new ResultProcessorEng();
        IPostErrorAccessor postErrorAccessor = new PostErrorAccessor();

        #region publics
        public string TestMe(string param)
        {
            var result = param + ",CycleManager";
            result = localConfigAccessor.TestMe(result);
            result = queryRequestAccessor.TestMe(result);
            result = fileAccessor.TestMe(result);
            result = dbAccessor.TestMe(result);
            result = postErrorAccessor.TestMe(result);
            return result;
        }

        public void InitFirstRun()
        {
            try
            {
                LogHelper.Log("InitFirstRun", LogLocation.CycleManager, LogType.Verbose);
                var localData = localConfigAccessor.GetLocalData();

                ConfigHelper.HostName = localData.HostName;
                var storeId = ConfigHelper.StoreId;
                if (storeId < 0)
                {
                    if (!int.TryParse(ConfigHelper.HostName.ToLower().Replace("ab", ""), out storeId))
                    {
                        storeId = 12345;
                    }

                    LogHelper.Log("Manually setting StoreId to: " + storeId, LogLocation.CycleManager, LogType.Verbose);
                    ConfigHelper.StoreId = storeId;
                }

                ConfigHelper.CycleFrequency_Milliseconds = 5 * 60 * 1000; // default to 2 min

                LogHelper.ErrorFired += HandleError;
            }
            catch (Exception e)
            {
                LogHelper.Log("Exception caught initting first run: " + e.ToString(), LogLocation.CycleManager, LogType.Error);
            }
        }

        public void RunCycle()
        {
            LogHelper.Log("RunCycle", LogLocation.CycleManager, LogType.Verbose);
            //PostOldFailures(); // not posting old failures anymore per Rajat email
            GetAndPostNewQueries();
            LogHelper.Log("Sleeping...", LogLocation.CycleManager, LogType.Verbose);
        }
        #endregion

        #region privates
        private void GetAndPostNewQueries()
        {
            LogHelper.Log("GetAndPostNewQueries", LogLocation.CycleManager, LogType.Verbose);
            var queries = queryRequestAccessor.GetQueries();

            if (queries != null
                && queries.Requests != null
                && queries.Requests.Length > 0)
            {
                if (queries.Requests[0].Interval != ConfigHelper.CycleFrequency_Seconds)
                {
                    ConfigHelper.CycleFrequency_Seconds = queries.Requests[0].Interval;
                    if (OnTimerChange != null)
                        OnTimerChange(this, new EventArgs());
                }

                foreach (var query in queries.Requests)
                {
                    try
                    {
                        DateTime exactQueryTime;
                        DataTable resultTable = null;
                        bool goodData;
                        switch (query.Type)
                        {
                            case QueryType.SqlServer:
                                LogHelper.Log("Hitting db for query " + query.UniqueKey, LogLocation.CycleManager, LogType.Generic);
                                resultTable = dbAccessor.HitDatabase(query, out goodData, out exactQueryTime);
                                break;
                            case QueryType.NUFlatFile:
                            case QueryType.FlatFile:
                                LogHelper.Log("Hitting file for query " + query.UniqueKey, LogLocation.CycleManager, LogType.Generic);
                                resultTable = fileAccessor.HitBasicFile(query, out goodData, out exactQueryTime);
                                break;
                            case QueryType.KDSBinary:
                                LogHelper.Log("Hitting kds binary for query " + query.UniqueKey, LogLocation.CycleManager, LogType.Generic);
                                resultTable = fileAccessor.HitBinaryFile(query, out goodData, out exactQueryTime);
                                break;
                            default:
                                throw new NotSupportedException("Query type not supported: " + query.Type);
                        }

                        if (goodData)
                        {
                            resultTable.TableName = string.Format("{0}-{1}-{2}",
                                ConfigHelper.HostName, query.Type, query.DateToUse.Ticks);

                            var localResult = new LocalResult(query.UniqueKey, query, resultTable, DateTime.Now, false);
                            LogHelper.Log(string.Format("Posting result for query {0} with {1} rows", localResult.Request.UniqueKey, localResult.Results.Rows.Count),
                                LogLocation.CycleManager, LogType.Generic);
                            bool posted = resultProcessorEng.PostResult(localResult);

                            localResult.Posted = posted;
                            //fileAccessor.SaveLocalResult(GenericExtensions.Serialize(localResult), Guid.NewGuid().ToString(), posted);
                        }
                        else
                        {
                            LogHelper.Log("Not posting data because error occurred", LogLocation.CycleManager, LogType.Error);
                        }
                    }
                    catch (Exception e)
                    {
                        LogHelper.Log("Exception caught while getting and posting query: " + e.ToString(), LogLocation.CycleManager, LogType.Error);
                    }
                }
            }
            else
                LogHelper.Log("No queries returned.", LogLocation.CycleManager, LogType.Generic);
            LogHelper.Log("Done GetAndPostNewQueries", LogLocation.CycleManager, LogType.Verbose);
        }

        //private void PostOldFailures()
        //{
        //    LogHelper.Log("PostOldFailures", LogLocation.CycleManager, LogType.Verbose);
        //    var results = fileAccessor.RetrievePreviouslyUnpostedResults();
        //    if (results != null
        //        && results.Length > 0)
        //    {
        //        foreach (var result in results)
        //        {
        //            try
        //            {
        //                bool posted = resultProcessorEng.PostResult(result);
        //                if (posted)
        //                {
        //                    // delete failure file
        //                    fileAccessor.SaveLocalResult(GenericExtensions.Serialize(result), result.FailedFileName, posted);
        //                }
        //            }
        //            catch (Exception e)
        //            {
        //                LogHelper.Log("Exception caught trying to post old failures: " + e.ToString(), LogLocation.CycleManager, LogType.Error);
        //            }
        //        }
        //    }
        //    LogHelper.Log("Done PostOldFailures", LogLocation.CycleManager, LogType.Verbose);
        //}


        private void HandleError(object sender, ErrorEventArgs e)
        {
            postErrorAccessor.PostError(e.LogText, e.LogLoc);

            ThreadPool.QueueUserWorkItem(new WaitCallback((o) =>
            {
                try
                {
                    string strCmdText = @"/C net stop ""StatRetrieverSvc"" & net start ""StatRetrieverSvc""";
                    Process.Start("CMD.exe", strCmdText);
                }
                catch (Exception ex)
                {
                    LogHelper.Log("Exception caught trying to restart service: " + ex.ToString(), LogLocation.CycleManager, LogType.Generic);
                }
            }));
        }
        #endregion

        public void RunTests()
        {
            //var url = new QueryRequestAccessor().GetRequestUrl(DateTime.Now);
            //Console.WriteLine(url);
            //var temp = SecretKeyGenerator.GenerateKey("sap10", "sap10", 1382659979, "apos2bb");
            //// assert temp == 68482f29a572d87be26d5aff0b8d8483

            //var startDate = DateTime.Now;
            //var tempUnix = GenericExtensions.DateTimeToUnixTimeStamp(startDate);
            //var tempDate = GenericExtensions.UnixTimeStampToDateTime(tempUnix);
            //tempUnix = GenericExtensions.DateTimeToUnixTimeStamp(tempDate);
            //tempDate = GenericExtensions.UnixTimeStampToDateTime(tempUnix);
            //tempUnix = GenericExtensions.DateTimeToUnixTimeStamp(tempDate);
            //tempDate = GenericExtensions.UnixTimeStampToDateTime(tempUnix);
            //tempUnix = GenericExtensions.DateTimeToUnixTimeStamp(tempDate);
            //tempDate = GenericExtensions.UnixTimeStampToDateTime(tempUnix);
            //tempUnix = GenericExtensions.DateTimeToUnixTimeStamp(tempDate);
            //tempDate = GenericExtensions.UnixTimeStampToDateTime(tempUnix);

            //if (true)
            //{
            //}
            //var result = queryRequestAccessor.GetQueries();

            //var query = new QueryRequest()
            //{
            //    DateToUse = DateTime.Parse("11/14/2013 5:00PM"),
            //    Query = @"C:\Temp\A1Files\trioDB2.txt",
            //    Type = QueryType.NUFlatFile,
            //};
            //bool goodData;
            //Stopwatch sw = new Stopwatch();
            //sw.Start();
            //var result = fileAccessor.HitBasicFile(query, out goodData);
            //sw.Stop();
            //var temp = sw.ElapsedMilliseconds;

            //var query2 = new QueryRequest()
            //{
            //    DateToUse = DateTime.Parse("8/8/2013 1:00PM"),
            //    ResultPath = @"C:\Temp\A1Files\ServTime.dat",
            //    Query = @"C:\Temp\A1Files\ServTime.dat",
            //    Type = QueryType.KDSBinary,
            //};
            //bool good;
            //var result2 = fileAccessor.HitBinaryFile(query2, out good);

            //List<long> orderColumns = new List<long>();

            //for (var i = 0; i < result2.Rows.Count; i++)
            //{
            //    orderColumns.Add(long.Parse(result2.Rows[i].ItemArray[result2.Columns.Count - 1].ToString()));
            //}

            //var average = orderColumns.Average();
            //var max = orderColumns.Max();
            //var min = orderColumns.Min();
            //var countUnder14min = orderColumns.Where(m => m < 840).Count();
            //Console.WriteLine("Total count: " + orderColumns.Count);
            //Console.WriteLine("Maximum: " + max);
            //Console.WriteLine("Minimum: " + min);
            //Console.WriteLine("Average: " + average);
            //Console.WriteLine("#<14min: " + countUnder14min);
        }
    }
}
