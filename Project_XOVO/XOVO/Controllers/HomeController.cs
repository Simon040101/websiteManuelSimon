using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
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
            if (Session["isAdmin"] != null)
            {
                if ((Convert.ToInt32(Session["isAdmin"]) == 0) || (Convert.ToInt32(Session["isAdmin"]) == 1))
                {
                    FeedRepository zr = new FeedRepository();
                    try
                    {
                        zr.Open();

                        return View(zr.GetFeedItems());
                    }
                    catch (MySqlException ex)
                    {
                        return View("Message",
                            new Message("Datenbankfehler", "", "Probleme mit der Datenbank.",
                                "Versuchen Sie es später erneut."));
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
        public ActionResult PostItem(HttpPostedFileBase imageFile, string feedContent)
        {

            ValidatePostForm(imageFile, feedContent);
            
            try
            {
                if (ModelState.IsValid)
                {
                    FeedItem fItem = new FeedItem();
                    FeedRepository fr = new FeedRepository();
                    fr.Open();

                    if (imageFile != null)
                    {
                        imageFile.SaveAs(Server.MapPath("~/Content/img/") + imageFile.FileName);
                        /// TODO: testen
                        fItem.Image = "/Content/img/" + imageFile.FileName;
                    }

                    fItem.UserForFeedID = Convert.ToInt32(Session["UserID"]);
                    fItem.FeedContent = feedContent;

                    bool posted = fr.InsertFeedItem(fItem);

                    if (posted == true)
                    {
                        return View("Message", new Message("Posten", "", "Beitrag wurde erfolgreich gepostet!", ""));
                    }

                    else
                    {
                        return View("Message", new Message("Posten", "", "Beitrag konnte nicht gepostet werden!", ""));
                    }
                }

                return View("Index");
                
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult CommentFeed(int id, string Comment)
        {
            int userID = Convert.ToInt32(Session["UserID"]);

            try
            {
                FeedRepository fr = new FeedRepository();
                fr.Open();

                bool success = fr.UserCommentFeed(userID, id, Comment);

                if (success == true)
                {
                    return Redirect(Request.UrlReferrer.ToString());
                }
                else
                {
                    return View("Message",
                        new Message("Beitrag", "Fehler", "Irgendetwas ist schief gelaufen",
                            "Versuche es später erneut!"));
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public ActionResult LikeFeed(int id)
        {
            int userID = Convert.ToInt32(Session["UserID"]);

            try
            {
                FeedRepository fr = new FeedRepository();
                fr.Open();

                bool success = fr.UserLikeFeed(userID, id);

                if (success == true)
                {
                    return Redirect(Request.UrlReferrer.ToString());
                }
                else 
                {
                    return View("Message",
                        new Message("Beitrag", "Fehler", "Irgendetwas ist schief gelaufen",
                            "Versuche es später erneut!"));
                }

                
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                FeedRepository fr = new FeedRepository();
                fr.Open();

                bool success = fr.DeleteFeed(id);

                if (success == true)
                {
                    return Redirect(Request.UrlReferrer.ToString());
                }
                else
                {
                    return View("Message",
                        new Message("Beitrag", "Fehler", "Irgendetwas ist schief gelaufen",
                            "Versuche es später erneut!"));
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        

        private void ValidatePostForm(HttpPostedFileBase imageFile, string feedContent)
        {
            if (((feedContent != null) && (imageFile != null)) ||
                ((feedContent != null) && (imageFile == null)))
            {
                if ((feedContent.Trim().Length < 5))
                {
                    ModelState.AddModelError("Textarea", "Ihr Text muss länger als 5 Buchstaben sein");
                }
            }
        }
    }
}