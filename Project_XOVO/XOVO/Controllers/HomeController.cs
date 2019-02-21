using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace XOVO.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            if(Session["isAdmin"] != null)
            {
                if((Convert.ToInt32(Session["isAdmin"]) == 0 ) || (Convert.ToInt32(Session["isAdmin"]) == 1))
                {
                    return View();
                }
            }

            return RedirectToAction("login", "user");

        }
    }
}