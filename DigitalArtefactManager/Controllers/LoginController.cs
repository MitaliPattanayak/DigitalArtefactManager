using DigitalArtefactManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DigitalArtefactManager.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public  ActionResult Login(User users)
        {
            if (ModelState.IsValid)
            {
                using (DigitalArtefactEntities db = new DigitalArtefactEntities())
                {
                    var obj = db.Users.Where(a => a.UserName.Equals(users.UserName) && a.Password.Equals(users.Password)).FirstOrDefault();
                    if (obj != null)
                    {
                        Session["UserID"] = obj.UserId.ToString();
                        Session["UserName"] = obj.UserName.ToString();
                       return RedirectToAction("UserRedirection");
                    }
                }
            }
            
            return View(users);
        }

        public ActionResult UserRedirection()
        {
            if (Session["UserName"] != null)
            {
                if (Session["UserName"].ToString() == "admin")
                {
                        return RedirectToAction("Index", "Articles");
                }
                else
                {
                    return RedirectToAction("IndexEmployee", "Articles");
                }
            }
            else
            {
                //@ViewBag.Message = "Login Successful!";
                return View("Login");
            }
        }
    }
}