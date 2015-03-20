using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Common
{
    public class SPRequest
    {
        private int _id, _storeId, _status, _source;
        private DateTime _submitDate;

        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }
        public int StoreId
        {
            get
            {
                return _storeId;
            }
            set
            {
                _storeId = value;
            }
        }
        public SPRequestStatus Status
        {
            get
            {
                return (SPRequestStatus)_status;
            }
            set
            {
                _status = (int)value;
            }
        }
        public DateTime SubmitDate
        {
            get
            {
                return _submitDate;
            }
            set
            {
                _submitDate = value;
            }
        }
        public SPSource Source
        {
            get
            {
                return (SPSource)_source;
            }
            set
            {
                _source = (int)value;
            }
        }

        public static SPRequest[] FindRequested(string shortStoreName)
        {
            List<SPRequest> result = new List<SPRequest>();
            try
            {
                string connString = System.Configuration.ConfigurationManager.ConnectionStrings["CCWeb"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    string query =
                        "select * from concord.dbo.sprequest r, concord.dbo.stores s where s.id=r.storeid and " +
                        "r.status=0 and s.shortname='" + shortStoreName + "'";

                    using (SqlCommand com = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader rdr = com.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                result.Add(new SPRequest()
                                {
                                    Id = (int)rdr["id"],
                                    Status = (SPRequestStatus)rdr["status"],
                                    StoreId = (int)rdr["storeid"],
                                    SubmitDate = (DateTime)rdr["submitdate"]
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
            }
            return result.ToArray<SPRequest>();
        }

        public static SPRequest FindRequested(int Id)
        {
            try
            {
                string connString = System.Configuration.ConfigurationManager.ConnectionStrings["CCWeb"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    string query =
                        "select * from concord.dbo.sprequest where id=" + Id;

                    using (SqlCommand com = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader rdr = com.ExecuteReader())
                        {
                            if (rdr.Read())
                            {
                                return (new SPRequest()
                                {
                                    Id = (int)rdr["id"],
                                    Status = (SPRequestStatus)rdr["status"],
                                    StoreId = (int)rdr["storeid"],
                                    SubmitDate = (DateTime)rdr["submitdate"],
                                    Source = (SPSource)rdr["source"]
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
            }
            return null;
        }

        public static bool InsertNew(SPRequest newRequest)
        {
            if (newRequest != null)
                try
                {
                    string connString = System.Configuration.ConfigurationManager.ConnectionStrings["CCWeb"].ConnectionString;
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        conn.Open();

                        string query =
                            "insert into concord.dbo.sprequest (storeid, submitdate, status, source) values " +
                            "(" + newRequest.StoreId + ", '" + newRequest.SubmitDate.ToString() + "', " + (int)newRequest.Status +
                            ", '" + newRequest.Source.ToString() + "')";

                        using (SqlCommand com = new SqlCommand(query, conn))
                        {
                            com.ExecuteNonQuery();
                        }
                    }
                    return true;
                }
                catch (Exception e)
                {
                }
            return false;
        }

        public bool Save()
        {
            try
            {
                string query = "";
                if (this.Id >= 0 && SPRequest.FindRequested(this.Id)!=null)
                {
                    query = "update sitestream.dbo.sprequests set id=" + this.Id + ", source='" + this.Source.ToString() + 
                        "', status=" + this.Status + ", storeid=" + this.StoreId + ", submitdate='" + this.SubmitDate + "' where id=" + this.Id;
                }
                else
                {
                    query = "insert into sitestream.dbo.sprequests (source, status, storeid, submitdate) values ('" + this.Source.ToString() +
                        "', " + this.Status + ", " + this.StoreId + ", '" + this.SubmitDate + "')";
                }

                return Common.Globals.ExecuteNonQuery(query);
            }
            catch (Exception e)
            {

            }
            return false;
        }
    }
}
