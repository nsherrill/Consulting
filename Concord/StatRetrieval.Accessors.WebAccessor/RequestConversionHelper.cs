using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StatRetrieval.Shared.Common;
using StatRetrieval.Shared.Common.DataContracts;

namespace StatRetrieval.Accessors.WebAccessor
{
    public static class RequestConversionHelper
    {
        public static QueryRequest ConvertToLocalRequest(QueryRequestAccessor.RemoteRequestObject sourceObj)
        {
            try
            {
                QueryType type = QueryType.SqlServer;
                switch (sourceObj.TYPE.ToUpper())
                {
                    case "DB":
                        type = QueryType.SqlServer;
                        break;
                    case "KDS":
                        type = QueryType.KDSBinary;
                        break;
                    case "FILE":
                        type = QueryType.FlatFile;
                        break;
                    case "NU":
                        type = QueryType.NUFlatFile;
                        break;
                    default:
                        throw new NotSupportedException("Request type not supported: " + sourceObj.TYPE);
                }
                DateTime dateToUse;

                if (sourceObj.LASTQUERYTIME.Contains("-"))
                    LogHelper.Log(sourceObj.LASTQUERYTIME + " contains negative!! [0]", LogLocation.Generic, LogType.Error);

                if (!DateTime.TryParse(sourceObj.LASTQUERYTIME, out dateToUse))
                {
                    long dateToUseLn;
                    if (!long.TryParse(sourceObj.LASTQUERYTIME, out dateToUseLn))
                        throw new NotSupportedException("Invalid date: " + sourceObj.LASTQUERYTIME);

                    if (dateToUseLn < 0)
                    {
                        LogHelper.Log("negative.. using ConfigHelper.DateTimeNow for " + sourceObj.ID, LogLocation.QueryRequestAccessor, LogType.Verbose);
                        dateToUse = ConfigHelper.DateTimeNow;
                    }
                    else
                        dateToUse = GenericExtensions.UnixTimeStampToDateTime(dateToUseLn);
                }

                if (ConfigHelper.TESTDATE != null
                    && ConfigHelper.TESTDATE.HasValue)
                {
                    long unixTestDateStamp = GenericExtensions.DateTimeToUnixTimeStamp(ConfigHelper.TESTDATE.Value);
                    var unixTestDateFromStamp = GenericExtensions.UnixTimeStampToDateTime(unixTestDateStamp);
                    long unixTestDateStamp2 = GenericExtensions.DateTimeToUnixTimeStamp(unixTestDateFromStamp);

                    LogHelper.Log(string.Format("USING TestDate: {0}... {1}... {2}", ConfigHelper.TESTDATE.Value, unixTestDateStamp, unixTestDateStamp2),
                        LogLocation.Generic, LogType.Verbose);
                    dateToUse = ConfigHelper.TESTDATE.Value;
                }

                var query = GenericExtensions.StringReplace(sourceObj.DESCRIPTION, "{{lastQueryTime}}", dateToUse.ToString());

                var interval = int.Parse(sourceObj.QUERYINTERVAL);
                var result = new QueryRequest()
                {
                    DateToUse = dateToUse,
                    Query = sourceObj.DESCRIPTION,
                    Type = type,
                    UniqueKey = sourceObj.ID,
                    ResultPath = sourceObj.RESULTPATH,
                    Interval = interval,
                };

                return result;
            }
            catch (Exception e)
            {
                LogHelper.Log("Exception caught while trying to convert request: " + e.ToString(), LogLocation.QueryRequestAccessor, LogType.Error);
                return null;
            }
        }

        internal static QueryRequestAccessor.RemoteRequestObject ParseRemoteRequest(object obj)
        {
            QueryRequestAccessor.RemoteRequestObject result = null;
            try
            {
                if (obj != null
                    && obj is Dictionary<string, object>)
                {
                    var typed = obj as Dictionary<string, object>;

                    var ID = typed["ID"].ToString();
                    var DESCRIPTION = typed["DESCRIPTION"].ToString();
                    var RESULTPATH = typed["RESULTPATH"].ToString();
                    var LASTQUERYTIME = typed["LASTQUERYTIME"].ToString();
                    var QUERYINTERVAL = typed["QUERYINTERVAL"].ToString();
                    var TYPE = typed["TYPE"].ToString();
                    result = new QueryRequestAccessor.RemoteRequestObject()
                    {
                        ID = ID,
                        DESCRIPTION = DESCRIPTION,
                        LASTQUERYTIME = LASTQUERYTIME,
                        RESULTPATH = RESULTPATH,
                        QUERYINTERVAL = QUERYINTERVAL,
                        TYPE = TYPE,
                    };
                }
            }
            catch { }
            return result;
        }
    }
}
