using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using StatRetrieval.Shared.Common.DataContracts;

namespace StatRetrieval.Shared.Common
{
    public static class GenericExtensions
    {
        public static T GetValue<T>(this ValuePair[] source, string key, T defaultVal)
        {
            if (source != null
                && source.Length > 0)
            {
                var pair = source.Where(k => k.Key == key).FirstOrDefault();
                if (pair != null)
                {
                    T tempResult;
                    if (ParseValue<T>(pair.Value, out tempResult))
                        return tempResult;
                }
            }

            return defaultVal;
        }

        private static bool ParseValue<T>(string sourceVal, out T tempResult)
        {
            if (string.IsNullOrEmpty(sourceVal))
            {
                tempResult = default(T);
                return false;
            }

            var typeStr = typeof(T).ToString();

            switch (typeStr)
            {
                case "System.String":
                    tempResult = (T)(object)sourceVal;
                    return true;
                    break;
                case "System.Integer":
                    int tempInt;
                    if (int.TryParse(sourceVal, out tempInt))
                    {
                        tempResult = (T)(object)tempInt;
                        return true;
                    }
                    break;
                default:
                    break;
            }

            tempResult = default(T);
            return false;
        }

        public static string Serialize(object obj)
        {
            if (obj == null)
                return string.Empty;

            try
            {
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                {
                    StringBuilder sb = new StringBuilder();
                    using (var sw = new StringWriter(sb))
                    {
                        serializer.Serialize(sw, obj);
                        return sb.ToString();
                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.Log("Exception caught while serializing: " + obj.ToString() + "\r\n\r\n" + e.ToString(), LogLocation.Generic, LogType.Error);
                return string.Empty;
            }
        }

        public static T Deserialize<T>(string source)
        {
            if (string.IsNullOrEmpty(source))
                return default(T);

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                {
                    using (var sr = new StringReader(source))
                    {
                        return (T)serializer.Deserialize(sr);
                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.Log("Exception caught while deserializing: " + source + "\r\n\r\n" + e.ToString(), LogLocation.Generic, LogType.Error);
                return default(T);
            }
        }

        public static string StringReplace(string source, string sourceReplaceString, string replaceString)
        {
            string result = source;
            while (source.ToLower().Contains(sourceReplaceString.ToLower()))
            {
                int index = source.ToLower().IndexOf(sourceReplaceString.ToLower());
                result = source.Substring(0, index);
                result += replaceString;
                result += source.Substring(index + sourceReplaceString.Length);
                source = result;
            }
            return result;
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            var result = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            result = result.AddSeconds(unixTimeStamp).ToLocalTime();
            LogHelper.Log(string.Format("Converted {0} to {1}", unixTimeStamp, result), LogLocation.Generic, LogType.Verbose);
            return result;
        }
        public static long DateTimeToUnixTimeStamp(DateTime dateTime)
        {
            TimeSpan diff;
            if (TimeZoneInfo.Local.IsDaylightSavingTime(dateTime))
            {
                // the hour diff is from daylight savings :-/
                diff = dateTime - (new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).ToLocalTime().AddHours(1));
            }
            else
                diff = dateTime - (new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).ToLocalTime());

            var result = diff.TotalSeconds;

            LogHelper.Log(string.Format("Converted {0} to {1}", dateTime, result), LogLocation.Generic, LogType.Verbose);
            return (long)result;
        }

        public static T DeserializeJson<T>(string resultStr)
        {
            var serializer = new JavaScriptSerializer();
            T values = serializer.Deserialize<T>(resultStr);

            return values;
        }

        public static string SerializeJson(object obj)
        {
            var serializer = new JavaScriptSerializer();
            StringBuilder sb = new StringBuilder();
            serializer.Serialize(obj, sb);

            return sb.ToString();
        }
    }
}
