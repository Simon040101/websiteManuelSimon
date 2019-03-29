using System;
using System.Collections.Generic;
using System.IO;
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
        public ActionResult PostFeed(string feedContent, HttpPostedFileBase imgPath1)
        {
            // Parameter überprüfen

            // falls Daten (Parameter) ok => DB-Teil
            //                      nicht ok => Fehlermeldung


            FeedRepository fr = new FeedRepository();
            fr.Open();
            

            ValidatePostForm(item);


            if (imgPath1 != null)
            {
                try
                {
                    string path = Path.Combine(Server.MapPath("/Content/img/"), Path.GetFileName(imgPath1.FileName));
                    imgPath1.SaveAs(path);
                    ViewBag.Message = "File uploaded successfully";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }

            }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }

            if (ModelState.IsValid)
            {
                try
                {
                    FeedItem fItem = new FeedItem();
                    fItem.UserForFeed = (User)Session["User"];
                    fItem.FeedContent = feedContent;
                    fItem.ImgPath = "???";
                    bool test = fr.InsertFeedItem(fItem);

                    if (test == true)
                    {
                        return View("Message", new Message("Hinzufügen", "", "Erfolgreich", ""));
                    }
                    else
                    {
                        return View("Message", new Message("Hinzufügen", "", "Nicht erfolgreich", ""));
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

                finally
                {
                    fr.Close();
                }

            }
            

            return View("Message", new Message("Posten", "", "Ungültige Eingabe", ""));

        }


        private void ValidatePostForm(FeedItem feedItemToValidate)
        {
            if (((feedItemToValidate.FeedContent != null) && (feedItemToValidate.ImgPath != null))||((feedItemToValidate.FeedContent != null) && (feedItemToValidate.ImgPath == null )))
            {
                if ((feedItemToValidate.FeedContent.Trim().Length < 5))
                {
                    ModelState.AddModelError("Textarea", "Ihr Text muss länger als 5 Buchstaben sein");
                }
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