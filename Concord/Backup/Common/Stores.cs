using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Common
{
    public class Stores
    {
        private int _id, _regionId;
        private string _name, _shortName, _ipAddress;

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
        public int RegionId
        {
            get
            {
                return _regionId;
            }
            set
            {
                _regionId = value;
            }
        }
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
        public string ShortName
        {
            get
            {
                return _shortName;
            }
            set
            {
                _shortName = value;
            }
        }
        public string IpAddress
        {
            get
            {
                return _ipAddress;
            }
            set
            {
                _ipAddress = value;
            }
        }

        public static Stores Find(int Id)
        {
            Stores result = null;
            try
            {
                string connString = System.Configuration.ConfigurationManager.ConnectionStrings["CCWeb"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    string query =
                        "select * from Concord.dbo.stores where id=" + Id;

                    using (SqlCommand com = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader rdr = com.ExecuteReader())
                        {

                            if (rdr.Read())
                            {
                                string name = rdr["Name"].ToString();
                                string ipAddress = rdr["ipAddress"] == DBNull.Value ? "" : rdr["ipAddress"].ToString();
                                string sregionId = rdr["regionId"].ToString();
                                string shortName = rdr["shortName"].ToString();

                                int iregionId;

                                if (sregionId != null
                                    && int.TryParse(sregionId, out iregionId))
                                {
                                    result = new Stores() { Id = Id, Name = name, IpAddress = ipAddress, RegionId = iregionId, ShortName = shortName };
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
            }
            return result;
        }
    
        public static Stores Find(string Name)
        {
            Stores result = null;
            try
            {
                string connString = System.Configuration.ConfigurationManager.ConnectionStrings["CCWeb"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    string query =
                        "select * from Concord.dbo.stores where name=" + Name;

                    using (SqlCommand com = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader rdr = com.ExecuteReader())
                        {

                            if (rdr.Read())
                            {
                                string sid = rdr["id"].ToString();
                                string name = rdr["Name"].ToString();
                                string ipAddress = rdr["ipAddress"] == DBNull.Value ? "" : rdr["ipAddress"].ToString();
                                string sregionId = rdr["regionId"].ToString();
                                string shortName = rdr["shortName"].ToString();

                                int iregionId, iid;

                                if (sregionId != null && sid !=null
                                    && int.TryParse(sregionId, out iregionId) && int.TryParse(sid, out iid))
                                {
                                    result = new Stores() { Id = iid, Name = name, IpAddress = ipAddress, RegionId = iregionId, ShortName = shortName };
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
            }
            return result;
        }

        public static Stores FindByShortName(string ShortName)
        {
            Stores result = null;
            try
            {
                string connString = System.Configuration.ConfigurationManager.ConnectionStrings["CCWeb"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    string query =
                        "select * from Concord.dbo.stores where shortname='" + ShortName +"'";

                    using (SqlCommand com = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader rdr = com.ExecuteReader())
                        {

                            if (rdr.Read())
                            {
                                string sid = rdr["id"].ToString();
                                string name = rdr["Name"].ToString();
                                string ipAddress = rdr["ipAddress"] == DBNull.Value ? "" : rdr["ipAddress"].ToString();
                                string sregionId = rdr["regionId"].ToString();
                                string shortName = rdr["shortName"].ToString();

                                int iregionId, iid;

                                if (sregionId != null && sid != null
                                    && int.TryParse(sregionId, out iregionId) && int.TryParse(sid, out iid))
                                {
                                    result = new Stores() { Id = iid, Name = name, IpAddress = ipAddress, RegionId = iregionId, ShortName = shortName };
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
            }
            return result;
        }

        public Stores[] FindByRegion(int RegionId)
        {
            List<Stores> result = new List<Stores>();
            try
            {
                string connString = System.Configuration.ConfigurationManager.ConnectionStrings["CCWeb"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    string query =
                        "select * from Concord.dbo.stores where regionid=" + RegionId;

                    using (SqlCommand com = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader rdr = com.ExecuteReader())
                        {

                            while (rdr.Read())
                            {
                                string sid = rdr["id"].ToString();
                                string name = rdr["Name"].ToString();
                                string ipAddress = rdr["ipAddress"] == DBNull.Value ? "" : rdr["ipAddress"].ToString();
                                string sregionId = rdr["regionId"].ToString();
                                string shortName = rdr["shortName"].ToString();

                                int iregionId, iid;

                                if (sregionId != null && sid != null
                                    && int.TryParse(sregionId, out iregionId) && int.TryParse(sid, out iid))
                                {
                                    result.Add(new Stores() { Id = iid, Name = name, IpAddress = ipAddress, RegionId = iregionId, ShortName = shortName });
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
            }

            return result.ToArray<Stores>();
        }

        public bool Save()
        {
            try
            {
                string query = "";
                if (this.Id >= 0 && Stores.Find(this.Id) != null)
                {
                    query = "update sitestream.dbo.stores set id=" + this.Id + ", ipaddress='" + this.IpAddress.ToString() +
                        "', Name='" + this.Name + "', RegionId=" + this.RegionId + ", ShortName='" + this.ShortName + "' where id=" + this.Id;
                }
                else
                {
                    query = "insert into sitestream.dbo.stores (ipaddress, Name, RegionId, ShortName) values ('" + this.IpAddress.ToString() +
                        "', '" + this.Name + "', " + this.RegionId + ", '" + this.ShortName + "')";
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
