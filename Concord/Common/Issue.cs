using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class Issue
    {
        public int Id;
        public string Name;
        public List<IssueComment> Comments;
        public DateTime SubmitDate;

        public Issue(int Id, string Name, DateTime SubmitDate, List<IssueComment> Comments)
            : this(Id, Name, SubmitDate)
        {
            this.Comments = Comments;
        }

        public Issue(int Id, string Name, DateTime SubmitDate)
        {
            this.Id = Id;
            this.Name = Name;
            this.SubmitDate = SubmitDate;
        }

        public bool Save()
        {
            return true;
        }

        public static Issue Find(int id)
        {
            List<IssueComment> comments = new List<IssueComment>();
            comments.Add(new IssueComment(id, "nick", "Introduce your company here. Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.", DateTime.Now));
            return new Issue(id, "tempname", DateTime.Now, comments);
        }

        public static Issue[] FindAllOpen()
        {
            List<Issue> pjs = new List<Issue>();
            List<IssueComment> comments = new List<IssueComment>();
            comments.Add(new IssueComment(0, "nick", "Introduce your company here. Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.", DateTime.Now));
            comments.Add(new IssueComment(0, "bfarr", "Introduce your company here. Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.", DateTime.Now));
            pjs.Add(new Issue(0, "tempname1", DateTime.Now, comments));

            comments = new List<IssueComment>();
            comments.Add(new IssueComment(1, "nick", "Introduce your company here. Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.", DateTime.Now));
            comments.Add(new IssueComment(1, "bfarr", "Introduce your company here. Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.", DateTime.Now));
            pjs.Add(new Issue(1, "tempname2", DateTime.Now, comments));

            return pjs.ToArray<Issue>();
        }
    }

    public class IssueComment
    {
        public int IssId;
        public int Id = -1;
        public string Comment;
        public string Author;
        public DateTime SaveDate;

        public IssueComment(int IssId, string Author, string Comment, DateTime SaveDate)
        {
            this.IssId = IssId;
            this.Author = Author;
            this.Comment = Comment;
            this.SaveDate = SaveDate;
        }

        public bool Save()
        {
            if (Id >= 0)
            {
                //update
            }

            //insert

            return true;
        }
    }
}
