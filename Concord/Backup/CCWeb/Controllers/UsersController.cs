using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Common;

namespace CCWeb.Controllers
{
    public class UsersController : Controller
    {
        //
        // GET: /Users/

        public ActionResult Index()
        {
            List<Users> results = Users.FindAll().ToList<Users>();

            return View(results);
        }

        public ActionResult Edit(int id)
        {
            Users user= Users.Find(id);
            if (id < 0 || user == null)
                user = new Users(
                    "New User", "New Password", "New@email.com", 0, false) { Id = -1 };

            return View(user);
        }
        
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int id, string username, string password, string email, string usergroup, bool updatebyemail)
        {
            Users user= Users.Find(id);
            if (id < 0 || user == null)
                user = new Users(
                    username, password, email,
                    (int)((UserGroups)Enum.Parse(typeof(UserGroups), usergroup)), updatebyemail) { Id = -1 };
            else
            {
                user.UserName = username;
                user.Password = password;
                user.Email = email;
                user.UpdateByEmail = updatebyemail;
                user.UserGroup = (UserGroups)Enum.Parse(typeof(UserGroups), usergroup);
            }

            user.Save();

            Response.Redirect("/Users");

            return null;
        }

        public ActionResult Remove(int? id)
        {
            if (id == null)
            {
                Response.Redirect("/Users");
                return null;
            }
            Users.Remove((int)id);

            Response.Redirect("/Users");
            return null;
        }
    }
}
