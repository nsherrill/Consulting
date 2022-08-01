using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using StatRetrieval.Shared.Common;
using StatRetrieval.Shared.Common.Contracts;
using StatRetrieval.Shared.Common.DataContracts;

namespace StatRetrieval.Accessors.DataAccessor
{
    public class DbAccessor : IDbAccessor
    {
        private const string employeeQuery = @"
select b.iconcept, e.iemployeeid, e.sfirstname, e.slastname, sssn
from elstar.dbo.businessinfo b, 
    elstar.dbo.employee e";
        public string TestMe(string param)
        {
            return param + ",DbAccessor";
        }

        public List<EmployeeData> GetEmployees()
        {
            LogHelper.Log("Getting employees", LogLocation.DbAccessor, LogType.Verbose);
            SqlConnectionStringBuilder sb = new SqlConnectionStringBuilder();
            sb.InitialCatalog = "elstar";
            sb.DataSource = ConfigHelper.DatabaseAddress;
            sb.UserID = ConfigHelper.DBUser;
            sb.Password = ConfigHelper.DBPassword;
            sb.MultipleActiveResultSets = true;
            sb.ConnectTimeout = 500;

            DataTable result = new DataTable();
            DateTime temp;
            try
            {
                result = HitDb(sb, employeeQuery, out temp);
            }
            catch (Exception e)
            {
                try
                {
                    sb.DataSource = ConfigHelper.BackupDatabaseAddress;
                    result = HitDb(sb, employeeQuery, out temp);
                }
                catch (Exception e2)
                {
                    LogHelper.Log(string.Format("Exception occured while trying to hit [{0}]: [[{1}]] {2}", sb.DataSource, employeeQuery, e2.ToString())
                        , LogLocation.DbAccessor, LogType.Error);
                }
            }

            List<EmployeeData> resultEmps = GetEmployeesFromTable(result);
            LogHelper.Log(resultEmps.Count + " employees found", LogLocation.DbAccessor, LogType.Verbose);

            return resultEmps;
        }

        public DataTable HitDatabase(QueryRequest request, out bool goodData, out DateTime exactQueryTime)
        {
            exactQueryTime = ConfigHelper.DateTimeNow;
            request.Query = GenericExtensions.StringReplace(request.Query, "{{lastQueryTime}}", request.DateToUse.ToString());

            DataTable result = new DataTable();

            SqlConnectionStringBuilder sb = new SqlConnectionStringBuilder();
            sb.InitialCatalog = "elstar";
            sb.DataSource = ConfigHelper.DatabaseAddress;
            sb.UserID = ConfigHelper.DBUser;
            sb.Password = ConfigHelper.DBPassword;
            sb.MultipleActiveResultSets = true;
            sb.ConnectTimeout = 500;

            try
            {
                result = HitDb(sb, request.Query, out exactQueryTime);
                goodData = true;
            }
            catch (Exception e)
            {
                LogHelper.Log("Exception occured while trying to hit primary db.  Resorting to backup.", LogLocation.DbAccessor, LogType.Error);

                try
                {
                    sb.DataSource = ConfigHelper.BackupDatabaseAddress;
                    result = HitDb(sb, request.Query, out exactQueryTime);
                    goodData = true;
                }
                catch (Exception e2)
                {
                    LogHelper.Log(string.Format("Exception occured while trying to hit [{0}]: [[{1}]]: {2}", sb.DataSource, request.Query, e2.ToString())
                        , LogLocation.DbAccessor, LogType.Error);
                    goodData = false;
                }
            }

            return result;
        }

        #region privates
        private DataTable HitDb(SqlConnectionStringBuilder sb, string query, out DateTime resultDateTime, string uniqueKey = null)
        {
            resultDateTime = ConfigHelper.DateTimeNow;
            bool hadError = false;
            DataTable result = new DataTable();
            using (SqlConnection conn = new SqlConnection(sb.ConnectionString))
            {
                conn.Open();

                try
                {
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = query;

                    cmd.CommandTimeout = 200;
                    SqlDataReader dr = cmd.ExecuteReader();
                    resultDateTime = ConfigHelper.DateTimeNow;
                    result.Load(dr);
                }
                catch (Exception e)
                {
                    hadError = true;
                    LogHelper.Log("Error while getting data: \r\n" + e.ToString(), LogLocation.DbAccessor, LogType.Error);
                }

                if (ConfigHelper.LogVerbose
                    && !hadError)
                {
                    try
                    {
                        SqlCommand cmd = conn.CreateCommand();
                        cmd.CommandText = query;

                        cmd.CommandTimeout = 200;
                        var rdr = cmd.ExecuteReader();
                        resultDateTime = ConfigHelper.DateTimeNow;

                        long count = 0;
                        while (rdr.Read())
                            count++;

                        LogHelper.Log(string.Format("Query {0} returned {1} rows: {2}", uniqueKey, count, query), LogLocation.DbAccessor, LogType.Verbose);
                    }
                    catch (Exception e)
                    {
                        LogHelper.Log("Error while getting data again..: \r\n" + e.ToString(), LogLocation.DbAccessor, LogType.Error);
                    }
                }
            }
            return result;
        }

        private List<EmployeeData> GetEmployeesFromTable(DataTable sourceTable)
        {
            List<EmployeeData> result = new List<EmployeeData>();

            for (int i = 0; i < sourceTable.Rows.Count; i++)
            {
                var ssn = sourceTable.Rows[i]["sssn"].ToString();
                var name = sourceTable.Rows[i]["sfirstname"].ToString() + " " + sourceTable.Rows[i]["slastname"].ToString();
                var empId = sourceTable.Rows[i]["iemployeeid"].ToString();
                result.Add(new EmployeeData()
                {
                    Name = name,
                    EmployeeId = empId,
                    SSN = ssn
                });
            }
            return result;
        }
        #endregion
    }
}
