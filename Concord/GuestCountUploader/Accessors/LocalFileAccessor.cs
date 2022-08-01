using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Common;

namespace GuestCountUploader.Accessors
{
    public class LocalFileAccessor
    {
        public enum FileType
        {
            Elstar,
            ElstarHistory
        }

        public CustomerDefinition[] GetDefinitions()
        {
            List<CustomerDefinition> results = new List<CustomerDefinition>();

            try
            {
                string filePath = ConfigHelper.StoreListPath;
                if (!File.Exists(filePath))
                {
                    filePath = Path.Combine(Assembly.GetExecutingAssembly().Location, filePath);
                }
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    var splits = line.Split(',');
                    int cId = int.Parse(splits[0].Trim());
                    results.Add(new CustomerDefinition()
                    {
                        Id = cId,
                        IP = splits[1].Trim(),
                        LastFullDate = ConfigHelper.DateTimeNow.AddDays(-1).Date,
                    });
                }
            }
            catch (Exception e)
            {
                Globals.Log("exception caught while uploading to parsing custs from file: " + e.ToString(), "CustomerFileAccessor.log");
            }

            return results.ToArray();
        }

        public string GetSqlQuery(FileType fileType, DbParam[] paramList)
        {
            string result = string.Empty;

            string filename = fileType.ToString() + ".sql";
            if (File.Exists(filename))
            {
                result = File.ReadAllText(filename);
            }

            if (!string.IsNullOrEmpty(result)
                && paramList != null
                && paramList.Length > 0)
            {
                foreach (var parm in paramList)
                {
                    string key = "{" + parm.UniqueKey.Replace("{", "").Replace("}", "") + "}";
                    result = result.Replace(key, parm.Value);
                }
            }

            return result;
        }

    }
}
