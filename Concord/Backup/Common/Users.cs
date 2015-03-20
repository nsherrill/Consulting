using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace Common
{
    public class Users
    {
        public int Id;
        public string UserName;
        public string Password;
        public string Email;
        public UserGroups UserGroup = UserGroups.Level1;
        public bool UpdateByEmail = false;

        public Users(string UserName, string Password, string Email, int UserGroup, bool UpdateByEmail)
        {
            this.UserName = UserName;
            this.Password = Password;
            this.Email = Email;
            this.UserGroup = (UserGroups)UserGroup;
            this.UpdateByEmail = UpdateByEmail;
        }

        public static Users Find(string username)
        {
            if (Globals.Testing)
                return new Users(username, "pass", "em", 1, false);
            else if (!Globals.CanHitDB)
            {
                return null;
            }
            else
            {
                try
                {
                    string connString = System.Configuration.ConfigurationManager.ConnectionStrings["CCWeb"].ConnectionString;
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        conn.Open();

                        string query = "select * from Concord.dbo.Users where username='" + username + "'";

                        using (SqlCommand com = new SqlCommand(query, conn))
                        {
                            SqlDataReader rdr = com.ExecuteReader();

                            if (rdr.Read())
                            {
                                string name = rdr["UserName"].ToString();
                                string pass = rdr["Password"].ToString();
                                string id = rdr["Id"].ToString();
                                string email = rdr["Email"].ToString();
                                string usergroup = rdr["UserGroup"].ToString();
                                bool update = (bool)rdr["UpdateByEmail"];

                                int iId;
                                int iUserGroup;

                                if (id != null && int.TryParse(id, out iId) && int.TryParse(usergroup, out iUserGroup))
                                {
                                    return (new Users(name, pass, email, iUserGroup, update) { Id = iId });
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                }

                return null;
            }
        }

        public static Users Find(int id)
        {
            if (Globals.Testing)
                return new Users("user", "pass", "em", 1, false) { Id = id };
            else if (!Globals.CanHitDB)
            {
                return null;
            }
            else
            {
                try
                {
                    string connString = System.Configuration.ConfigurationManager.ConnectionStrings["CCWeb"].ConnectionString;
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        conn.Open();

                        string query = "select * from Concord.dbo.Users where id=" + id;

                        using (SqlCommand com = new SqlCommand(query, conn))
                        {
                            SqlDataReader rdr = com.ExecuteReader();

                            if (rdr.Read())
                            {
                                string name = rdr["UserName"].ToString();
                                string pass = rdr["Password"].ToString();
                                string email = rdr["Email"].ToString();
                                int usergroup = (int)rdr["UserGroup"];
                                bool update = (bool)rdr["UpdateByEmail"];

                                return (new Users(name, pass, email, usergroup, update) { Id = id });

                            }
                        }
                    }
                }
                catch (Exception)
                {
                }

                return null;
            }
        }

        public static Users Find(string username, string password)
        {
            if (Globals.Testing)
                return new Users(username, password, "em", 1, false);
            else if (!Globals.CanHitDB)
            {
                return null;
            }
            else
            {
                try
                {
                    string connString = System.Configuration.ConfigurationManager.ConnectionStrings["CCWeb"].ConnectionString;
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        conn.Open();

                        string query =
                            "select * from Concord.dbo.Users where username='" + username +
                            "' and password = '"+password+"'";

                        using (SqlCommand com = new SqlCommand(query, conn))
                        {
                            SqlDataReader rdr = com.ExecuteReader();

                            if (rdr.Read())
                            {
                                string name = rdr["UserName"].ToString();
                                string pass = rdr["Password"].ToString();
                                string id = rdr["Id"].ToString();
                                string email = rdr["Email"].ToString();
                                int usergroup = (int)rdr["UserGroup"];
                                bool update = (bool)rdr["UpdateByEmail"];

                                int iId;

                                if (id != null && int.TryParse(id, out iId))
                                {
                                    return (new Users(name, pass, email, usergroup, update) { Id = iId });
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Globals.Log(e.ToString());
                }

                return null;
            }
        }

        public static Users[] FindAll()
        {
            List<Users> results = new List<Users>();
            if (Globals.Testing)
                return results.ToArray<Users>();
            else if (!Globals.CanHitDB)
            {
                return results.ToArray<Users>();
            }
            else
            {
                try
                {
                    string connString = System.Configuration.ConfigurationManager.ConnectionStrings["CCWeb"].ConnectionString;
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        conn.Open();

                        string query =
                            "select * from Concord.dbo.Users";

                        using (SqlCommand com = new SqlCommand(query, conn))
                        {
                            SqlDataReader rdr = com.ExecuteReader();

                            while (rdr.Read())
                            {
                                string name = rdr["UserName"].ToString();
                                string pass = rdr["Password"].ToString();
                                string id = rdr["Id"].ToString();
                                string email = rdr["Email"].ToString();
                                int usergroup = (int)rdr["UserGroup"];
                                bool update = (bool)rdr["UpdateByEmail"];

                                int iId;

                                if (id != null && int.TryParse(id, out iId))
                                {
                                    results.Add(new Users(name, pass, email, usergroup, update) { Id = iId });
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Globals.Log(e.ToString());
                }

                return results.ToArray<Users>();
            }
        }

        public bool Save()
        {
            try
            {
                bool result = true;

                string connString = System.Configuration.ConfigurationManager.ConnectionStrings["CCWeb"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    string query;

                    if (Id < 0)
                    {
                        Globals.Log("inserting user: " + this.UserName);
                        query =
                            "insert into Concord.dbo.users (username, password, email, usergroup, updatebyemail) values ('" +
                            this.UserName + "', '" + this.Password + "', '" + this.Email + "', " + (int)this.UserGroup +
                            ", " + (this.UpdateByEmail ? "1" : "0") + ")";
                    }
                    else
                    {
                        Globals.Log("updating user: " + this.UserName);
                        query = "update Concord.dbo.users set username='" + this.UserName +
                            "', password='" + this.Password + "', email='" +
                            this.Email + "', usergroup=" + (int)this.UserGroup + ", updatebyemail=" +
                            (this.UpdateByEmail ? "1" : "0") + " where id = " + this.Id;
                    }

                    using (SqlCommand com = new SqlCommand(query, conn))
                    {
                        com.ExecuteNonQuery();
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                Globals.Log(e.ToString());
            }
            return false;
        }

        public static bool Remove(int id)
        {
            bool result = false;
            try
            {
                string connString = System.Configuration.ConfigurationManager.ConnectionStrings["CCWeb"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    string query =
                        "delete from Concord.dbo.Users where id=" + id;

                    using (SqlCommand com = new SqlCommand(query, conn))
                    {
                        result = com.ExecuteNonQuery() == 1;
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                Globals.Log(e.ToString());
            }
            return false;
        }

        public static List<SelectListItem> GetUserGroupsList(int index)
        {
            List<SelectListItem> result = new List<SelectListItem>();

            foreach (UserGroups ug in (UserGroups[])Enum.GetValues(typeof(UserGroups)))
            {
                result.Add(new SelectListItem() { Text = ug.ToString(), Value = ug.ToString(), Selected = index == (int)ug });
            }

            return result;
        }
    }
}
