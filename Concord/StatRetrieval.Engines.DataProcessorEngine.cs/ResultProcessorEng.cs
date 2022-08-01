using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using StatRetrieval.Accessors.DataAccessor;
using StatRetrieval.Accessors.WebAccessor;
using StatRetrieval.Shared.Common;
using StatRetrieval.Shared.Common.Contracts;
using StatRetrieval.Shared.Common.DataContracts;

namespace StatRetrieval.Engines.DataProcessorEngine.cs
{
    public class ResultProcessorEng : IResultProcessorEng
    {
        List<EmployeeData> cachedEmployees = null;
        DateTime lastEmployeeSnag = DateTime.MinValue;
        IDbAccessor dbAccessor = new DbAccessor();
        IPostResultAccessor postResultAccessor = new PostResultAccessor();

        private List<EmployeeData> GetEmployees()
        {
            if (cachedEmployees == null
                || lastEmployeeSnag < ConfigHelper.DateTimeNow.AddMinutes(-30))
            {
                cachedEmployees = dbAccessor.GetEmployees();
                lastEmployeeSnag = ConfigHelper.DateTimeNow;
            }
            return cachedEmployees;
        }

        public string TestMe(string param)
        {
            return param + ",ResultProcessorEng";
        }

        public class RemoteResultObject
        {
            public string ID { get; set; }
            public string TYPE { get; set; }
            public long QUERYTIME { get; set; }
            public string RESULTS { get; set; }
        }

        public bool PostResult(LocalResult localResult)
        {
            try
            {
                string type = string.Empty;
                switch (localResult.Request.Type)
                {
                    case QueryType.FlatFile:
                        type = "FILE";
                        break;
                    case QueryType.KDSBinary:
                        type = "KDS";
                        break;
                    case QueryType.NUFlatFile:
                        type = "NU";
                        break;
                    case QueryType.SqlServer:
                        type = "DB";
                        break;
                    default:
                        throw new ArgumentException();
                }

                var queryTime = GenericExtensions.DateTimeToUnixTimeStamp(localResult.Time);
                if (queryTime < 0)
                {
                    LogHelper.Log("Using ConfigHelper.DateTimeNow for " + localResult.Request.UniqueKey, LogLocation.ResultProcessorEng, LogType.Verbose);
                    queryTime = GenericExtensions.DateTimeToUnixTimeStamp(ConfigHelper.DateTimeNow);
                }

                RemoteResultObject obj = new RemoteResultObject()
                {
                    ID = localResult.Request.UniqueKey,
                    TYPE = type,
                    QUERYTIME = queryTime,
                    RESULTS = "{REPLACEMENT RESULTS}",
                };

                var temp = GenericExtensions.SerializeJson(obj);

                temp = "{\"" + localResult.UniqueKey + "\": " + temp + "}";
                bool addStoreId = localResult.Request.Type == QueryType.NUFlatFile;
                string resultString = BuildResultString(localResult.Results, GetEmployees(), addStoreId);
                temp = temp.Replace("\"{REPLACEMENT RESULTS}\"", "[" + resultString + "]");
                var result = postResultAccessor.PostResult(temp, queryTime);

                LogHelper.Log((result ? "S" : "Uns") + "uccessfully posted " + localResult.Results.Rows.Count + " rows for query " + localResult.Request.UniqueKey,
                    LogLocation.ResultProcessorEng, LogType.Verbose);

                return result;
            }
            catch (Exception e)
            {
                LogHelper.Log("Exception caught trying to post result for "+localResult.Request.UniqueKey + ": " + e.ToString(), LogLocation.ResultProcessorEng, LogType.Error);
                return false;
            }
        }

        private string BuildResultString(DataTable data, List<EmployeeData> emps, bool addStoreId)
        {
            StringBuilder overallSb = new StringBuilder();
            bool includeEmpId = true;

            for (int row = 0; row < data.Rows.Count; row++)
            {
                if (row > 0)
                    overallSb.Append(",");

                overallSb.Append("{");

                string employeeId = null;
                StringBuilder rowSb = new StringBuilder();
                for (int col = 0; col < data.Columns.Count; col++)
                {
                    if (data.Columns[col].ColumnName.ToLower().Contains("employeeid"))
                    {
                        includeEmpId = false;
                    }
                    else if (data.Columns[col].ColumnName.ToLower().Contains("ssn"))
                    {
                        var ssn = data.Rows[row].ItemArray[col].ToString();
                        var emp = emps.Where(e => e.SSN == ssn).FirstOrDefault();
                        if (emp != null)
                            employeeId = emp.EmployeeId;
                    }

                    var curVal = string.Format("\"{0}\":\"{1}\"",
                        data.Columns[col].ColumnName,
                        data.Rows[row].ItemArray[col].ToString());

                    if(rowSb.Length > 0)
                        rowSb.Append(",");
                    rowSb.Append(curVal);
                }

                ValuePair vpStoreId = new ValuePair("StoreId", ConfigHelper.StoreId.ToString());
                ValuePair vpEmployeeId = new ValuePair("EmployeeId", employeeId);

                if (addStoreId)
                {
                    overallSb.Append(vpStoreId.ToString());
                    overallSb.Append(',');
                }
                if (includeEmpId)
                {
                    overallSb.Append(vpEmployeeId.ToString());
                    overallSb.Append(',');
                }
                overallSb.Append(rowSb.ToString());
                overallSb.Append('}');
            }

            return overallSb.ToString();
        }
    }
}
