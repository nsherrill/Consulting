using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using StatRetrieval.Shared.Common.DataContracts;

namespace StatRetrieval.Shared.Common
{
    public static class SecretKeyGenerator
    {
        public static string GenerateKey(long dateStamp)
        {
            return GenerateKey(
                ConfigHelper.NitroApiKey,
                ConfigHelper.NitroSecretKey,
                dateStamp,
                ConfigHelper.StoreId.ToString());
        }
        public static string GenerateKey(string apiKey, string secretKey, long dateStamp, string storeId)
        {
            string tempResult =
               apiKey
                + secretKey
                + dateStamp.ToString()
                + "STORE_" +storeId.ToString();
            var tempLength = tempResult.Length;
            tempResult += tempLength.ToString();

            var result = CalculateMD5Hash(tempResult);

            LogHelper.Log(
                string.Format("Generating key: apiKey={0}&secretKey={1}&timeStamp={2}&storeId={3}&length={4}&result={5}",
                apiKey, secretKey, dateStamp, storeId, tempLength, result),
                LogLocation.Generic, LogType.Verbose);

            return result;
        }

        public static string GenerateKey(DateTime dateTime)
        {
            var dateStamp = GenericExtensions.DateTimeToUnixTimeStamp(dateTime);
            return GenerateKey(dateStamp);
        }

        private static string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
