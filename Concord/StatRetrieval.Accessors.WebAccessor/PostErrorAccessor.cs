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
    public class PostErrorAccessor : IPostErrorAccessor
    {
        public string TestMe(string param)
        {
            return param + ",QueryRequestAccessor";
        }

        public bool PostError(string text, LogLocation logLoc)
        {
            if (text.Contains("PostErrorAccessor"))
                //recursive, dont do it..
                return false;

            try
            {
                var timeStamp = GenericExtensions.DateTimeToUnixTimeStamp(DateTime.Now);
                var signature = SecretKeyGenerator.GenerateKey(timeStamp);
                string url = string.Empty;

                if (ConfigHelper.PostWithSecretKey)
                {
                    string paramStr = "?apiKey={0}&secretKey={1}&storeId={2}&timeStamp={3}&signature{4}";
                    url = ConfigHelper.ErrorUrl + string.Format(paramStr,
                        ConfigHelper.NitroApiKey, ConfigHelper.NitroSecretKey, ConfigHelper.StoreId,
                        timeStamp, signature);
                }
                else
                {
                    string paramStr = "?apiKey={0}&storeId={1}&timeStamp={2}&signature{3}";
                    url = ConfigHelper.ErrorUrl + string.Format(paramStr,
                        ConfigHelper.NitroApiKey, ConfigHelper.StoreId,
                        timeStamp, signature);
                }

                string data = String.Format("{0}:\t{1}:\t{2}:\t{3}", DateTime.Now, ConfigHelper.StoreId, logLoc, text);

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
