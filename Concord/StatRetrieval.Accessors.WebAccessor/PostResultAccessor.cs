using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using StatRetrieval.Shared.Common;
using StatRetrieval.Shared.Common.Contracts;
using StatRetrieval.Shared.Common.DataContracts;

namespace StatRetrieval.Accessors.WebAccessor
{
    public class PostResultAccessor : IPostResultAccessor
    {
        public string TestMe(string param)
        {
            return param + ",QueryRequestAccessor";
        }

        public bool PostResult(string data, long timeStamp)
        {
            try
            {
                string url = string.Empty;
                var signature = SecretKeyGenerator.GenerateKey(timeStamp);

                if (ConfigHelper.PostWithSecretKey)
                {
                    string paramStr = "{0}?apiKey={1}&secretKey={2}&storeId={3}&timeStamp={4}&signature={5}";

                    url = string.Format(paramStr, ConfigHelper.ResultsUrl, ConfigHelper.NitroApiKey, ConfigHelper.NitroSecretKey,
                        ConfigHelper.StoreId, timeStamp, signature);
                }
                else
                {
                    string paramStr = "{0}?apiKey={1}&storeId={2}&timeStamp={3}&signature={4}";

                    url = string.Format(paramStr, ConfigHelper.ResultsUrl, ConfigHelper.NitroApiKey, 
                        ConfigHelper.StoreId, timeStamp, signature);
                }

                bool result = PhpHelper.PostToService(url, data);
                return result;
            }
            catch (Exception e)
            {
                LogHelper.Log("Error while posting result: " + e.ToString(), LogLocation.PostResultAccessor, LogType.Error);
                return false;
            }
        }
    }
}
