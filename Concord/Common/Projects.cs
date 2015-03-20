using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace Common
{
    public class Projects
    {
        public int Id =-1;
        public string Name;
        public DateTime SubmitDate;
        public List<ProjectComment> Comments;
        public ProjectStatus Status;

        public Projects(int Id, string Name, DateTime SubmitDate, int Status, List<ProjectComment> Comments)
            : this(Id, Name, SubmitDate, Status)
        {
            this.Comments = Comments;
        }

        public Projects(int Id, string Name, DateTime SubmitDate, int Status)
        {
            this.Id = Id;
            this.Name = Name;
            this.SubmitDate = SubmitDate;
            this.Status = (ProjectStatus)Status;
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
                        Globals.Log("inserting project: " + Name + ", " + SubmitDate.ToString());
                        query =
                            "insert into Concord.dbo.projects (name, submitdate, status) values (@name, '"+
                            this.SubmitDate.ToString() + "', " + (int)this.Status + ")";
                    }
                    else
                    {
                        Globals.Log("updating project: " + Name + ", " + SubmitDate.ToString());
                        query = "update Concord.dbo.projects set name=@name, submitdate='" 
                            + this.SubmitDate.ToString() + "', status=" +
                            (int)this.Status + " where id = " + this.Id;
                    }

                    using (SqlCommand com = new SqlCommand(query, conn))
                    {
                        com.Parameters.AddWithValue("@name", this.Name);
                        com.ExecuteNonQuery();
                    }

                    if (Id < 0)
                    {
                        query = "select id from Concord.dbo.Projects where name=@name and submitdate ='" +
                            this.SubmitDate.ToString() + "'";
                        using (SqlCommand com = new SqlCommand(query, conn))
                        {
                            com.Parameters.AddWithValue("@name", this.Name);
                            using (SqlDataReader rdr = com.ExecuteReader())
                            {
                                if (rdr.Read())
                                {
                                    Id = (int)rdr[0];
                                }
                            }
                        }
                    }

                    if (this.Comments != null)
                    {
                        foreach (ProjectComment ic in this.Comments)
                        {
                            ic.ProjId = this.Id;
                            result = ic.Save() && result;
                        }
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

        public static Projects Find(int id)
        {
            Projects result = null;
            try
            {
                string connString = System.Configuration.ConfigurationManager.ConnectionStrings["CCWeb"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    string query =
                        "select * from Concord.dbo.Projects where id=" + id;

                    using (SqlCommand com = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader rdr = com.ExecuteReader())
                        {

                            if (rdr.Read())
                            {
                                string name = rdr["Name"].ToString();
                                string date = rdr["SubmitDate"].ToString();
                                string status = rdr["Status"].ToString();

                                DateTime dDate;
                                int iStatus;

                                if (date != null && DateTime.TryParse(date, out dDate) && int.TryParse(status, out iStatus))
                                {
                                    result = new Projects(id, name, dDate, iStatus);
                                }
                            }
                        }
                    }

                    if (result != null)
                    {
                        result.Comments = new List<ProjectComment>();
                        query =
                            "select userid, submitdate, comment, p.id from Concord.dbo.ProjectComments p, "+
                            "Concord.dbo.users u where u.id=p.userid and projectid=" + id;

                        using (SqlCommand com = new SqlCommand(query, conn))
                        {
                            using (SqlDataReader rdr = com.ExecuteReader())
                            {

                                while (rdr.Read())
                                {
                                    int author = (int)rdr["userid"];
                                    string date = rdr["SubmitDate"].ToString();
                                    string comment = rdr["Comment"].ToString();
                                    int iId = (int)rdr["Id"];

                                    DateTime dDate;

                                    if (date != null && DateTime.TryParse(date, out dDate))
                                    {
                                        result.Comments.Add(new ProjectComment(id, author, comment, dDate) { Id = iId });
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Globals.Log(e.ToString());
            }

            return result;
        }

        public static Projects[] FindAll()
        {
            List<Projects> result = new List<Projects>();
            try
            {
                string connString = System.Configuration.ConfigurationManager.ConnectionStrings["CCWeb"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    string query =
                        "select * from Concord.dbo.Projects order by submitdate desc";

                    using (SqlCommand com = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader rdr = com.ExecuteReader())
                        {

                            while (rdr.Read())
                            {
                                string name = rdr["Name"].ToString();
                                string date = rdr["SubmitDate"].ToString();
                                string id = rdr["Id"].ToString();
                                string status = rdr["Status"].ToString();

                                DateTime dDate;
                                int iId;
                                int iStatus;

                                if (date != null && DateTime.TryParse(date, out dDate)
                                    && int.TryParse(id, out iId) && int.TryParse(status, out iStatus))
                                {
                                    result.Add(new Projects(iId, name, dDate, iStatus));
                                }
                            }
                        }
                    }

                    foreach (Projects i in result)
                    {
                        i.Comments = new List<ProjectComment>();
                        query =
                            "select userid, submitdate, comment, p.id from Concord.dbo.ProjectComments p, "+
                                "Concord.dbo.users u where u.id=p.userid and projectid="+ i.Id;

                        using (SqlCommand com = new SqlCommand(query, conn))
                        {
                            using (SqlDataReader rdr = com.ExecuteReader())
                            {

                                while (rdr.Read())
                                {
                                    int author = (int)rdr["userid"];
                                    string date = rdr["SubmitDate"].ToString();
                                    string comment = rdr["Comment"].ToString();
                                    int iId = (int)rdr["Id"];

                                    DateTime dDate;

                                    if (date != null && DateTime.TryParse(date, out dDate))
                                    {
                                        i.Comments.Add(new ProjectComment(i.Id, author, comment, dDate) { Id = iId });
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Globals.Log(e.ToString());
            }

            return result.ToArray<Projects>();
        }

        public static Projects[] FindAll(ProjectStatus stat)
        {
            List<Projects> result = new List<Projects>();
            try
            {
                string connString = System.Configuration.ConfigurationManager.ConnectionStrings["CCWeb"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    string query =
                        "select * from Concord.dbo.Projects where status="+(int)stat;

                    using (SqlCommand com = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader rdr = com.ExecuteReader())
                        {

                            while (rdr.Read())
                            {
                                string name = rdr["Name"].ToString();
                                string date = rdr["SubmitDate"].ToString();
                                string id = rdr["Id"].ToString();
                                string status = rdr["Status"].ToString();

                                DateTime dDate;
                                int iId;
                                int iStatus;

                                if (date != null && DateTime.TryParse(date, out dDate)
                                    && int.TryParse(id, out iId) && int.TryParse(status, out iStatus))
                                {
                                    result.Add(new Projects(iId, name, dDate, iStatus));
                                }
                            }
                        }
                    }

                    foreach (Projects i in result)
                    {
                        i.Comments = new List<ProjectComment>();
                        query =
                            "select userid, Submitdate, comment, p.id from Concord.dbo.ProjectComments p, "+
                            "Concord.dbo.users u where u.id=p.userid and projectid=" + i.Id;

                        using (SqlCommand com = new SqlCommand(query, conn))
                        {
                            using (SqlDataReader rdr = com.ExecuteReader())
                            {

                                while (rdr.Read())
                                {
                                    int author = (int)rdr["userid"];
                                    string date = rdr["SubmitDate"].ToString();
                                    string comment = rdr["Comment"].ToString();
                                    int iId = (int)rdr["Id"];

                                    DateTime dDate;

                                    if (date != null && DateTime.TryParse(date, out dDate))
                                    {
                                        i.Comments.Add(new ProjectComment(i.Id, author, comment, dDate) { Id = iId });
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Globals.Log(e.ToString());
            }

            return result.ToArray<Projects>();
        }

        public static List<SelectListItem> GetStatusesItemList(int index)
        {
            List<SelectListItem> statuses = new List<SelectListItem>();
            ProjectStatus[] list = (ProjectStatus[])Enum.GetValues(typeof(Common.ProjectStatus));
            for (int i = 0; i < list.Length; i++)
                statuses.Add(new SelectListItem()
                    { Text = list[i].ToString(), Value = list[i].ToString(), Selected = i == index });
            
            return statuses;
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
                        "delete from Concord.dbo.Projects where id=" + id;

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

        public void SendEmails()
        {
            string emails = "";

            if (this.Comments != null && this.Comments.Count > 0)
            {
                foreach (ProjectComment comm in this.Comments)
                    if (comm.Author.UpdateByEmail && !emails.Contains("," + comm.Author.Email))
                        emails += "," + comm.Author.Email;

                Globals.SendEmail(emails.Substring(1), "CCWebUpdate@concordei.com",
                    "Project: "+this.Name + " (" + this.Status.ToString() + ") has been updated",
                    "View changes <a href=\"http://ccweb.concordei.com/Projects\">here</a>");
            }
        }
    }

    public class ProjectComment
    {
        public int ProjId;
        public int Id = -1;
        public string Comment;
        public Users Author;
        public DateTime SaveDate;

        public ProjectComment(int ProjId, string Author, string Comment, DateTime SaveDate)
            : this(ProjId, Comment, SaveDate)
        {
            this.Author = Users.Find(Author);
        }

        public ProjectComment(int ProjId, int Author, string Comment, DateTime SaveDate)
            : this(ProjId, Comment, SaveDate)
        {
            this.Author = Users.Find(Author);
        }

        public ProjectComment(int ProjId, Users Author, string Comment, DateTime SaveDate)
            : this(ProjId, Comment, SaveDate)
        {
            this.Author = Author;
        }

        private ProjectComment(int ProjId, string Comment, DateTime SaveDate)
        {
            this.ProjId = ProjId;
            this.Comment = Comment;
            this.SaveDate = SaveDate;
        }

        public bool Save()
        {
            try
            {
                string connString = System.Configuration.ConfigurationManager.ConnectionStrings["CCWeb"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    string query = "";
                    if (this.Id < 0)
                        query = "insert into Concord.dbo.projectComments (projectid, userid, comment, submitdate) values (" +
                            this.ProjId + ", " + (this.Author == null ? -1 : this.Author.Id) + ", @comment, '"+
                            DateTime.Now.ToString()+"')";
                    else
                        query = "update Concord.dbo.projectComments set projectid=" + this.ProjId +
                            ", userid=" + (this.Author == null ? -1 : this.Author.Id) + ", comment=@comment, submitdate='"+
                            DateTime.Now.ToString()+"' where id = " + this.Id;

                    using (SqlCommand com = new SqlCommand(query, conn))
                    {
                        com.Parameters.AddWithValue("@comment", this.Comment);
                        com.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Globals.Log(e.ToString());
            }
            return false;
        }
    }

}
