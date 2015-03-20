using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class Project
    {
        public int Id;
        public string Name;
        public DateTime SubmitDate;
        public bool Active;
        public List<ProjectComment> Comments;

        public Project(int Id, string Name, DateTime SubmitDate, bool Active, List<ProjectComment> Comments)
            : this(Id, Name, SubmitDate, Active)
        {
            this.Comments = Comments;
        }

        public Project(int Id, string Name, DateTime SubmitDate, bool Active)
        {
            this.Id = Id;
            this.Name = Name;
            this.Active = Active;
            this.SubmitDate = SubmitDate;
        }

        public bool Save()
        {
            if (Id >= 0)
            {
                //update
            }

            //insert

            if (this.Comments != null)
                foreach (ProjectComment pc in this.Comments)
                    pc.Save();
            return true;
        }

        public static Project Find(int id)
        {
            List<ProjectComment> comments = new List<ProjectComment>();
            comments.Add(new ProjectComment(id, "nick", "comment", DateTime.Now));
            return new Project(id, "tempname", DateTime.Now, false, comments);
        }

        public static Project[] FindAllOpen()
        {
            List<Project> pjs = new List<Project>();
            List<ProjectComment> comments = new List<ProjectComment>();
            comments.Add(new ProjectComment(0,  "nsherrill", "Introduce your company here. Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.", DateTime.Now));
            comments.Add(new ProjectComment(0,  "bfarr", "Introduce your company here. Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.", DateTime.Now));
            pjs.Add(new Project(0, "tempname1", DateTime.Now, true, comments));

            comments = new List<ProjectComment>();
            comments.Add(new ProjectComment(1,  "nsherrill", "Introduce your company here. Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.", DateTime.Now));
            comments.Add(new ProjectComment(1,  "bfarr", "Introduce your company here. Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.", DateTime.Now));
            pjs.Add(new Project(1, "tempname2", DateTime.Now, true, comments));

            return pjs.ToArray<Project>();
        }

        public static Project[] FindAllOpenPotential()
        {
            List<Project> pjs = new List<Project>();
            List<ProjectComment> comments = new List<ProjectComment>();
            comments.Add(new ProjectComment(0, "nsherrill", "Introduce your company here. Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.", DateTime.Now));
            comments.Add(new ProjectComment(0, "bfarr", "Introduce your company here. Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.", DateTime.Now));
            pjs.Add(new Project(0, "tempname1", DateTime.Now, false, comments));

            comments = new List<ProjectComment>();
            comments.Add(new ProjectComment(1, "nsherrill", "Introduce your company here. Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.", DateTime.Now));
            comments.Add(new ProjectComment(1, "bfarr", "Introduce your company here. Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.", DateTime.Now));
            pjs.Add(new Project(1, "tempname2", DateTime.Now, false, comments));

            return pjs.ToArray<Project>();
        }
    }

    public class ProjectComment
    {
        public int ProjId;
        public int Id = -1;
        public string Comment;
        public string Author;
        public DateTime SaveDate;

        public ProjectComment(int ProjId, string Author, string Comment, DateTime SaveDate)
        {
            this.ProjId = ProjId;
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
