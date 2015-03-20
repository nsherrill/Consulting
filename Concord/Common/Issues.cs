using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace Common
{
    public class Issues
    {
        public int Id=-1;
        public string Name;
        public List<IssueComment> Comments;
        public DateTime SubmitDate;
        public IssueStatus Status;

        public Issues(int Id, string Name, DateTime SubmitDate, int Status, List<IssueComment> Comments)
            : this(Id, Name, SubmitDate, Status)
        {
            this.Comments = Comments;
        }

        public Issues(int Id, string Name, DateTime SubmitDate, int Status)
        {
            this.Id = Id;
            this.Name = Name;
            this.SubmitDate = SubmitDate;
            this.Status = (IssueStatus)Status;
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
                        Globals.Log("inserting issue: " + Name + ", " + SubmitDate.ToString());
                        query =
                            "insert into Concord.dbo.Issues (name, submitdate, status) values (@name, '" + 
                            this.SubmitDate.ToString() + "', " + (int)this.Status + ")";
                    }
                    else
                    {
                        Globals.Log("updating issue: " + Name + ", " + SubmitDate.ToString());
                        query = "update Concord.dbo.Issues set name=@name, submitdate='" + 
                            this.SubmitDate.ToString() + "', status=" +
                            (int)this.Status + " where id = " + this.Id;
                    }

                    using (SqlCommand com = new SqlCommand(query, conn))
                    {
                        com.Parameters.AddWithValue("@name", this.Name);
                        com.ExecuteNonQuery();
                    }

                    if (Id < 0)
                    {
                        query = "select id from Concord.dbo.Issues where name=@name and submitdate ='" + 
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
                        foreach (IssueComment ic in this.Comments)
                        {
                            ic.IssId = Id;
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
    
        public static Issues Find(int id)
        {
            Issues result = null;
            try
            {
                string connString = System.Configuration.ConfigurationManager.ConnectionStrings["CCWeb"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    string query =
                        "select * from Concord.dbo.Issues where id=" + id;

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

                                if (date != null && status != null 
                                    && DateTime.TryParse(date, out dDate) && int.TryParse(status, out iStatus))
                                {
                                    result = new Issues(id, name, dDate, iStatus);
                                }
                            }
                        }
                    }
                    
                    if (result != null)
                    {
                        result.Comments = new List<IssueComment>();

                        query =
                            "select userid, submitdate, comment, i.Id from Concord.dbo.IssueComments i, "+
                                "Concord.dbo.Users u where u.id = i.UserId and issueid=" + id;

                        using (SqlCommand com = new SqlCommand(query, conn))
                        {
                            using (SqlDataReader rdr = com.ExecuteReader())
                            {

                                while (rdr.Read())
                                {
                                    string author = rdr["UserId"].ToString();
                                    string date = rdr["SubmitDate"].ToString();
                                    string comment = rdr["Comment"].ToString();
                                    int iICid = (int)rdr["Id"];

                                    DateTime dDate;
                                    int authorid;

                                    if (date != null && DateTime.TryParse(date, out dDate)
                                        && int.TryParse(author, out authorid))
                                    {
                                        result.Comments.Add(new IssueComment(id, authorid, comment, dDate) { Id = iICid });
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

        public static Issues[] FindAll()
        {
            List<Issues> result = new List<Issues>();
            try
            {
                string connString = System.Configuration.ConfigurationManager.ConnectionStrings["CCWeb"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    string query =
                        "select * from Concord.dbo.Issues order by submitdate desc";

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
                                    result.Add(new Issues(iId, name, dDate, iStatus));
                                }
                            }
                        }
                    }

                    foreach (Issues i in result)
                    {
                        i.Comments = new List<IssueComment>();
                        query =
                            "select userid, submitdate, comment, i.id from Concord.dbo.IssueComments i, "+
                            "Concord.dbo.Users u where u.id = i.UserId and issueid=" + i.Id;

                        using (SqlCommand com = new SqlCommand(query, conn))
                        {
                            using (SqlDataReader rdr = com.ExecuteReader())
                            {

                                while (rdr.Read())
                                {

                                    string author = rdr["UserId"].ToString();
                                    string date = rdr["SubmitDate"].ToString();
                                    string comment = rdr["Comment"].ToString();
                                    int iICid = (int)rdr["Id"];

                                    DateTime dDate;
                                    int iAuthor;


                                    if (date != null && DateTime.TryParse(date, out dDate)
                                        && int.TryParse(author, out iAuthor))
                                    {
                                        i.Comments.Add(new IssueComment(i.Id, iAuthor, comment, dDate) { Id = iICid });
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

            return result.ToArray<Issues>();
        }

        public static Issues[] FindAll(IssueStatus stat)
        {
            List<Issues> result = new List<Issues>();
            try
            {
                string connString = System.Configuration.ConfigurationManager.ConnectionStrings["CCWeb"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    string query =
                        "select * from Concord.dbo.Issues where status=" + (int)stat;

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
                                    result.Add(new Issues(iId, name, dDate, iStatus));
                                }
                            }
                        }
                    }

                    foreach (Issues i in result)
                    {
                        i.Comments = new List<IssueComment>();
                        query =
                            "select userid, submitdate, comment, i.id from Concord.dbo.IssueComments i, "+
                                "Concord.dbo.Users u where u.id = i.UserId and issueid=" + i.Id;

                        using (SqlCommand com = new SqlCommand(query, conn))
                        {
                            using (SqlDataReader rdr = com.ExecuteReader())
                            {

                                while (rdr.Read())
                                {

                                    string author = rdr["UserId"].ToString();
                                    string date = rdr["SubmitDate"].ToString();
                                    string comment = rdr["Comment"].ToString();
                                    int iICid = (int)rdr["Id"];

                                    DateTime dDate;

                                    if (date != null && DateTime.TryParse(date, out dDate))
                                    {
                                        i.Comments.Add(new IssueComment(i.Id, author, comment, dDate) { Id = iICid });
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

            return result.ToArray<Issues>();
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
                        "delete from Concord.dbo.Issues where id=" + id;

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

        public static List<SelectListItem> GetStatusesItemList(int index)
        {
            List<SelectListItem> statuses = new List<SelectListItem>();
            IssueStatus[] list = (IssueStatus[])Enum.GetValues(typeof(Common.IssueStatus));
            for (int i = 0; i < list.Length; i++)
                statuses.Add(new SelectListItem()
                    { Text = list[i].ToString(), Value = list[i].ToString(), Selected = i == index });

            return statuses;
        }

        public void SendEmails()
        {
            string emails = "";

            if (this.Comments != null && this.Comments.Count > 0)
            {
                foreach (IssueComment comm in this.Comments)
                    if (comm.Author.UpdateByEmail && !emails.Contains("," + comm.Author.Email))
                        emails += "," + comm.Author.Email;

                Globals.SendEmail(emails.Substring(1), "CCWebUpdate@concordei.com",
                    "Issue: "+this.Name + " (" + this.Status.ToString() + ") has been updated",
                    "View changes <a href=\"http://ccweb.concordei.com/Issues\">here</a>");
            }
        }
    }

    public class IssueComment
    {
        public int IssId;
        public int Id = -1;
        public string Comment;
        public Users Author;
        public DateTime SaveDate;

        public IssueComment(int IssId, string Author, string Comment, DateTime SaveDate)
            : this(IssId, Comment, SaveDate)
        {
            this.Author = Users.Find(Author);
        }

        public IssueComment(int IssId, int Author, string Comment, DateTime SaveDate)
            : this(IssId, Comment, SaveDate)
        {
            this.Author = Users.Find(Author);
        }

        public IssueComment(int IssId, Users Author, string Comment, DateTime SaveDate)
            :this(IssId, Comment, SaveDate)
        {
            this.Author = Author;
        }

        private IssueComment(int IssId, string Comment, DateTime SaveDate)
        {
            this.IssId = IssId;
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
                    {
                        Globals.Log("inserting new issue: " + this.Author + " for " + this.IssId);
                        query = "insert into Concord.dbo.IssueComments (issueid, userid, comment, submitdate) values (" +
                            this.IssId + ", " + (this.Author == null ? -1 : this.Author.Id) + ", @comment, '" +
                            this.SaveDate.ToString()+"')";
                    }
                    else
                    {
                        Globals.Log("updating new issue: " + this.Author + " for " + this.IssId);
                        query = "update Concord.dbo.IssueComments set issueid=" + this.IssId +
                            ",userid=" + (this.Author == null ? -1 : this.Author.Id) + ",comment=@comment, submitdate='"+
                            this.SaveDate.ToString()+"' where id = " + this.Id;
                    }

                    using (SqlCommand com = new SqlCommand(query, conn))
                    {
                        com.Parameters.AddWithValue("@comment", this.Comment);
                        Globals.Log("rows affected: "+com.ExecuteNonQuery());
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
