using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XOVO.Models;

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
                    return View(GetAllFeedItems());
                }
            }

            return RedirectToAction("login", "user");

        }

        private List<FeedItem> GetAllFeedItems()
        {

            return new List<FeedItem>()
            {
                new FeedItem() {Name = "feed1"},
                new FeedItem() {Name = "feed2"},
                new FeedItem() {Name = "feed3"},
                new FeedItem() {Name = "feed4"},
            };
        }
    }
}