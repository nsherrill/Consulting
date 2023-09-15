using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Common
{
    public static class DbHelper
    {
        public static T[] ExecuteReader<T>(string connectionString, string sqlString) where T : class, IReadable
        {
            List<T> result = new List<T>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    Globals.Log("hitting db", true);
                    try
                    {
                        SqlCommand cmd = conn.CreateCommand();
                        cmd.CommandText = sqlString;

                        cmd.CommandTimeout = 200;
                        SqlDataReader dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            result.Add(DataReaderExtensions.ParseReader<T>(dr));
                        }
                    }
                    catch (Exception e)
                    {
                        Globals.Log("error while getting tickets: \r\n" + e.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                Globals.Log("Exception occured while trying to hit DB: " + e.Message);
            }
            return result.ToArray();
        }

        public static long ExecuteScalar(string connectionString, string sqlString)
        {
            long result = -1;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    try
                    {
                        SqlCommand cmd = conn.CreateCommand();
                        cmd.CommandText = sqlString;

                        cmd.CommandTimeout = 200;
                        var scalar = cmd.ExecuteScalar();
                        if (!string.IsNullOrEmpty(scalar.ToString()))
                            result = long.Parse(scalar.ToString());
                        else result = 0;
                    }
                    catch (Exception e)
                    {
                        Globals.Log("error while getting tickets: \r\n" + e.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                Globals.Log("Exception occured while trying to hit DB: " + e.Message);
            }
            return result;
        }
    }
}
