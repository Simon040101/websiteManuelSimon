using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySql.Data.MySqlClient;
using XOVO.Models;
using XOVO.Models.db;

namespace XOVO.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home

        public ActionResult Index(FeedItem feedItem)
        {
            if(Session["isAdmin"] != null)
            {
                if((Convert.ToInt32(Session["isAdmin"]) == 0 ) || (Convert.ToInt32(Session["isAdmin"]) == 1))
                {
                    FeedRepository zr = new FeedRepository();
                    try
                    {
                        
                        zr.Open();

                        return View(zr.GetFeedItems());
                    }
                    catch (MySqlException)
                    {
                        return View("Message", new Message("Datenbankfehler", "", "Probleme mit der Datenbank.", "Versuchen Sie es später erneut."));
                    }
                    finally
                    {
                        zr.Close();
                    }

                }
            }

            return RedirectToAction("login", "user");

        }



       
        [HttpPost]
        public ActionResult PostFeed(IEnumerable<HttpPostedFileBase> files, string feedText)
        {


           // PostForm(feedItem);

            if (ModelState.IsValid)
            {

                FeedRepository fr = new FeedRepository();
                FeedItem feedItem = new FeedItem(/*???*/);   
                try
                {
                    fr.Open();

                    if (fr.InsertFeedItem(feedItem))
                    {
                        return View("Message", new Message("Posten", "", "Beitrag wurde erfolgreich gepostet", ""));
                    }
                    else
                    {
                        return View("Message", new Message("Posten", "", "Beitrag konnte nicht gepostet werden!", "Versuchen Sie es später erneut."));
                    }

                }
                catch (MySqlException)
                {
                    return View("Message", new Message("Datenbankfehler", "", "Probleme mit der Datenbankverbindung!", "Versuchen Sie es später erneut."));
                }
                finally
                {
                    fr.Close();
                }
            }

            return RedirectToAction("Index", "Home");
        }


        private void ValidatePostForm(FeedItem feedItemToValidate)
        {
            FeedRepository frd = new FeedRepository();

            frd.Open();


         
            if ((feedItemToValidate.FeedContent == null) || (feedItemToValidate.FeedContent.Trim().Length < 5))
            {
                ModelState.AddModelError("Textarea", "Ihr Text muss länger als 5 Buchstaben sein");
            }
            if ((feedItemToValidate.ImgPath == null))
            {
                ModelState.AddModelError("Datei", "Dateipfad existiert nicht");
            }
        }
        //private List<FeedItem> GetAllFeedItems()
        //{

        //    return new List<FeedItem>()
        //    {
        //        new FeedItem() {Name = "feed1"},
        //        new FeedItem() {Name = "feed2"},
        //        new FeedItem() {Name = "feed3"},
        //        new FeedItem() {Name = "feed4"},
        //    };
        //}
    }
}