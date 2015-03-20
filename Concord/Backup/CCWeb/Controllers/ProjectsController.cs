using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

using Common;

namespace CCWeb.Controllers
{
    public class ProjectsController : Controller
    {
        //
        // GET: /Projects/

        public ActionResult Index()
        {
            List<Projects> results = new List<Projects>();
            Projects[] temp = Projects.FindAll();
            if (temp != null)
                results = temp.ToList<Projects>();
            return View(results);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index(string status)
        {
            ProjectStatus stat = (ProjectStatus)Enum.Parse(typeof(ProjectStatus), status);

            List<Projects> results = new List<Projects>();
            Projects[] temp = Projects.FindAll(stat);
            if (temp != null)
                results = temp.ToList<Projects>();
            return View(results);
        }

        public ActionResult Edit(int id)
        {
            Projects p = Projects.Find(id);
            if (p == null || id < 0)
            {
                return View(new Projects(-1, "New Project", DateTime.Now, 0));
            }
            return View(p);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(string name, string status, string id, string author, string comment)
        {
            int iId = int.Parse(id);
            ProjectStatus stat = (ProjectStatus)Enum.Parse(typeof(ProjectStatus), status);

            Projects p = Projects.Find(iId);

            if (p == null || iId < 0)
            {
                List<Common.ProjectComment> comments = new List<ProjectComment>();
                p = new Projects(iId, name, DateTime.Now, (int)stat);
                if (!string.IsNullOrEmpty(comment) && comment != "Comment...")
                    comments.Add(new ProjectComment(-1, author, comment, DateTime.Now));
                p.Comments = comments;
            }
            else
            {
                p.Name = name;
                p.Status = stat;
                if(!string.IsNullOrEmpty(comment) && comment != "Comment...")
                    p.Comments.Add(new ProjectComment(-1, author, comment, DateTime.Now));
            }

            p.SendEmails();
            p.Save();
            
            Response.Redirect("/Projects");
            return null;
        }

        public ActionResult Remove(int? id)
        {
            if (id == null)
            {
                Response.Redirect("/Projects");
                return null;
            }
            Projects.Remove((int)id);

            Response.Redirect("/Projects");
            return null;
        }
    }
}
