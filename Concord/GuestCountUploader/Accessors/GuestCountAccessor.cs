using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Common;

namespace GuestCountUploader.Accessors
{
    public class GuestCountAccessor
    {
        public long GetGuestCount(string dbAddy, DateTime date)
        {
            long result = 0;
            try
            {
                LocalFileAccessor lfa = new LocalFileAccessor();

                SqlConnectionStringBuilder sb = new SqlConnectionStringBuilder();
                sb.InitialCatalog = "elstar";
                sb.DataSource = dbAddy;
                sb.UserID = "sa";
                sb.Password = "ratsle";
                sb.MultipleActiveResultSets = true;
                sb.ConnectTimeout = 500;

                var yesterday = date;
                var tomorrow = date.AddDays(1);
                var dateStart = new DateTime(yesterday.Year, yesterday.Month, yesterday.Day, 5, 0, 0).ToString();
                var dateEnd = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 5, 0, 0).ToString();
                var dbParamList = new DbParam[]
            {
                new DbParam("DESIREDDATESTART", dateStart),
                new DbParam("DESIREDDATEEND", dateEnd)
            };

                string sqlString = lfa.GetSqlQuery(LocalFileAccessor.FileType.Elstar, dbParamList);

                Console.WriteLine("running on {0}.{1}: {2}", dbAddy, sb.InitialCatalog, sqlString);
                result = DbHelper.ExecuteScalar(sb.ToString(), sqlString);

                sb.InitialCatalog = "elstarhistory";
                sqlString = lfa.GetSqlQuery(LocalFileAccessor.FileType.ElstarHistory, dbParamList);
                Console.WriteLine("running on {0}.{1}: {2}", dbAddy, sb.InitialCatalog, sqlString);
                var tempResult = DbHelper.ExecuteScalar(sb.ToString(), sqlString);
                result += tempResult;
            }
            catch (Exception e)
            {
                Console.WriteLine("EXCEPTION: {0}: {1}", "GuestCountAccessor.GetGuestCount", e.ToString());
            }
            Globals.Log(string.Format("Returning GuestCount[{0}] for {1}", result, dbAddy));
            return result;
        }
    }
}
