using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StatRetrieval.Shared.Common;
using StatRetrieval.Shared.Common.Contracts;
using StatRetrieval.Shared.Common.DataContracts;

namespace StatRetrieval.Accessors.WebAccessor
{
    public class QueryRequestAccessor : IQueryRequestAccessor
    {
        public string TestMe(string param)
        {
            return param + ",QueryRequestAccessor";
        }

        public string GetRequestUrl(long timeStamp)
        {
            var signature = SecretKeyGenerator.GenerateKey(timeStamp);
            string url = string.Empty;

            if (ConfigHelper.PostWithSecretKey)
            {
                string paramStr = "{0}?apiKey={1}&secretKey={2}&storeId={3}&timeStamp={4}&signature={5}";
                url = string.Format(paramStr, ConfigHelper.QueryUrl, ConfigHelper.NitroApiKey, ConfigHelper.NitroSecretKey,
                    ConfigHelper.StoreId, timeStamp, signature);
            }
            else
            {
                string paramStr = "{0}?apiKey={1}&storeId={2}&timeStamp={3}&signature={4}";
                url = string.Format(paramStr, ConfigHelper.QueryUrl, ConfigHelper.NitroApiKey,
                    ConfigHelper.StoreId, timeStamp, signature);
            }
            return url;
        }

        public string GetRequestUrl(DateTime dateTime)
        {
            var timeStamp = GenericExtensions.DateTimeToUnixTimeStamp(dateTime);
            return GetRequestUrl(timeStamp);
        }

        public QueryRequestResult GetQueries()
        {
            string resultStr;
            try
            {
                var timeStamp = GenericExtensions.DateTimeToUnixTimeStamp(DateTime.Now);
                string url = GetRequestUrl(timeStamp);
               
                LogHelper.Log("Hitting url for requests: " + url, LogLocation.QueryRequestAccessor, LogType.Generic);
                resultStr = PhpHelper.HitService(url);

                var obj = GenericExtensions.DeserializeJson<Dictionary<string, object>>(resultStr);
                LogHelper.Log(obj.Keys.Count + " requests returned.", LogLocation.QueryRequestAccessor, LogType.Verbose);
                List<QueryRequest> reqs = new List<QueryRequest>();
                foreach (var reqKey in obj.Keys)
                {
                    RemoteRequestObject remoteReqObject = RequestConversionHelper.ParseRemoteRequest(obj[reqKey]);

                    if (remoteReqObject != null)
                        reqs.Add(RequestConversionHelper.ConvertToLocalRequest(remoteReqObject));
                }
                QueryRequestResult result = new QueryRequestResult()
                {
                    Requests = reqs.ToArray()
                };
                LogHelper.Log(result.Requests.Length + " requests deserialized correctly.", LogLocation.QueryRequestAccessor, LogType.Generic);

                return result;
            }
            catch (Exception e)
            {
                LogHelper.Log("Exception caught while getting queries: " + e.ToString(), LogLocation.QueryRequestAccessor, LogType.Error);
                return null;
            }
        }

        public class RemoteRequestObject
        {
            public string ID { get; set; }
            public string TYPE { get; set; }
            public string DESCRIPTION { get; set; }
            public string LASTQUERYTIME { get; set; }
            public string RESULTPATH { get; set; }
            public string QUERYINTERVAL { get; set; }
        }
    }
}
