using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

using Common;

namespace CCWeb.Controllers
{
    public class IssuesController : Controller
    {
        //
        // GET: /Issues/

        public ActionResult Index()
        {
            Issues[] iss = Issues.FindAll();
            if(iss==null)
                return View(new List<Issues>());
            return View(iss.ToList<Issues>());
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index(string status)
        {
            IssueStatus stat = (IssueStatus)Enum.Parse(typeof(IssueStatus), status);
            Issues[] iss = Issues.FindAll(stat);
            if (iss == null)
                return View(new List<Issues>());
            return View(iss.ToList<Issues>());
        }

        public ActionResult Edit(int id)
        {
            Issues i = Issues.Find(id);
            if (i == null || id < 0)
            {
                return View(new Issues(-1, "New Issue", DateTime.Now, 1));
            }
            else
            {
                Globals.Log("issue #"+id+" found");
            }
            return View(i);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(string name, string status, string id, string author, string comment)
        {
            int iId = int.Parse(id);
            IssueStatus stat = (IssueStatus)Enum.Parse(typeof(IssueStatus), status);

            Issues i = Issues.Find(iId);

            if (i == null || iId < 0)
            {
                List<Common.IssueComment> comments = new List<IssueComment>();
                i = new Issues(iId, name, DateTime.Now, (int)stat);//, comments);
                if (!string.IsNullOrEmpty(comment) && comment != "Comment...")
                comments.Add(new IssueComment(-1, author, comment, DateTime.Now));
                i.Comments = comments;
            }
            else
            {
                i.Name = name;
                i.Status = stat;
                if (!string.IsNullOrEmpty(comment) && comment != "Comment...")
                i.Comments.Add(new IssueComment(iId, author, comment, DateTime.Now));
            }

            i.SendEmails();
            i.Save();

            Response.Redirect("/Issues");
            return null;
        }
        
        public ActionResult Remove(int? id)
        {
            if (id == null)
            {
                Response.Redirect("/Issues");
                return null;
            }
            Issues.Remove((int)id);

            Response.Redirect("/Issues");
            return null;
        }
    }
}
