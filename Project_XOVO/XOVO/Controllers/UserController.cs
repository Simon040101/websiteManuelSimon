using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XOVO.Models;
using MySql.Data.MySqlClient;
using XOVO.Models.db;

namespace XOVO.Controllers
{
    public class UserController : Controller
    {
        IUserRepositiory usersRepository;

        [HttpGet]
        public ActionResult ChangeLayout()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangeLayout(User user)
        {
            UserRepositiory ur = new UserRepositiory();
            ur.Open();

            bool ChangeLayout = ur.ChangeLayout(user);
            bool ChangeBackground = ur.ChangeBackground(user);

            Response.Cookies["layout_color"].Value = user.Layout_color;
            Response.Cookies["background_login"].Value = user.Background_login;

            return View(user);
        }

        public ActionResult Logout()
        {
            Session["isAdmin"] = null;

            return RedirectToAction("login", "user");
        }
        [HttpGet]
        public ActionResult ChangeData()
        {
            if ((Session["isAdmin"] != null) && (Convert.ToInt32(Session["isAdmin"]) != 3) && (Convert.ToInt32(Session["isAdmin"]) != 2))
            {
                return View();
            }
            else
            {
                return View("Message", new Message("Daten ändern", "Es tut uns leid :( Sie können Ihre Daten nicht ändern", "", "Melden Sie sich an"));
            }

        }
        [HttpPost]
        public ActionResult ChangeData(User user)
        {
            if ((Session["isAdmin"] != null) && (Convert.ToInt32(Session["isAdmin"]) != 3) && (Convert.ToInt32(Session["isAdmin"]) != 2))
            {
                ValidateRegistrationForm(user);

                if (ModelState.IsValid)
                {
                    UserRepositiory ur = new UserRepositiory();
                    ur.Open();

                    Session["UserID"] = user.ID;

                    bool ChangeSuccess = ur.ChangeData(user);
                    if(ChangeSuccess == true)
                    {
                        return Request.UrlReferrer == null ? (ActionResult)RedirectToAction("Index", "Home") : Redirect(Request.UrlReferrer.ToString());
                    }

                    else
                    {
                        return View("Message", new Message("Daten ändern", "Datenbankfehler", "Ihre Daten konnten nicht verändert werden", ""));
                    }
                }
                else
                {
                    return View("Message", new Message("Daten ändern", "Fehler", "Ihre Daten konnten nicht verändert werden", ""));
                }
            }
            else
            {
                return View("Message", new Message("Daten ändern", "Es tut uns leid :( Sie können Ihre Daten nicht ändern", "", "Melden Sie sich an"));
            }
        }

        public ActionResult UnlockUser(int id)
        {
            if ((Session["isAdmin"] != null) && (Convert.ToInt32(Session["isAdmin"]) == 0))
            {

                UserRepositiory ur = new UserRepositiory();
                ur.Open();
                ur.UnlockUser(id);
                return Request.UrlReferrer == null ? (ActionResult)RedirectToAction("Index", "Home") : Redirect(Request.UrlReferrer.ToString());
            }
            else
            {
                return View("Message", new Message("Sperren", "Sie sind nicht berechtigt einen Benutzer zu entsperren!!!", "", ""));
            }
        }

        public ActionResult LockUser(int id)
        {
            
            if ((Session["isAdmin"] != null) && (Convert.ToInt32(Session["isAdmin"]) == 0))
            {
                
                UserRepositiory ur = new UserRepositiory();
                ur.Open();
                ur.LockUser(id);
                return Request.UrlReferrer == null ? (ActionResult)RedirectToAction("Index", "Home") : Redirect(Request.UrlReferrer.ToString());
            }
            else
            {
                return View("Message", new Message("Sperren", "Sie sind nicht berechtigt einen Benutzer zu sperren!!!", "", ""));
            }
        }

        public ActionResult Delete(int id)
        {
            if ((Session["isAdmin"] != null) && (Convert.ToInt32(Session["isAdmin"]) == 0))
            {
                UserRepositiory ur = new UserRepositiory();
                ur.Open();
                ur.Delete(id);
                return Redirect(Request.UrlReferrer.ToString());
            }
            else
            {
                return View("Message", new Message("Löschen", "fehlende Berechtigung", "Sie sind nicht berehtigt eine Person zu löschen", ""));
            }
        }

        // GET: User
        public ActionResult Index()
        {

            return View();

        }

        /*
        [HttpGet]
        public ActionResult UserManagement()
        {
            if ((Session["isAdmin"] != null) && (Convert.ToInt32(Session["isAdmin"]) == 0))
            {
                List<User> users = LoadUsers();
                // List<User> foundUser = SearchUsers(firstname, lastname);

                return View(users);
               
            }
            else
            {
                return View("Message", new Message("Achtung", "fehlende Berechtigung", "Sie sind nicht berechtigt die Seite aufzurufen", ""));
            }
        }
        */

        [HttpGet]
        public ActionResult UserManagement(string firstname="", string lastname="")
        {
            if ((Session["isAdmin"] != null) && (Convert.ToInt32(Session["isAdmin"]) == 0))
            {
                List<User> usersToDisplay;

                if ( (firstname == "") && (lastname == "") ) {
                    usersToDisplay = LoadUsers();
                }
                else
                {
                    usersToDisplay = SearchUsers(firstname, lastname);
                }

                return View(usersToDisplay);

            }
            else
            {
                return View("Message", new Message("Achtung", "fehlende Berechtigung", "Sie sind nicht berechtigt die Seite aufzurufen", ""));
            }
        }

        [HttpGet]
         public ActionResult Login()
        {
            return View(new Login());
        }
        [HttpPost]
        public ActionResult Login(Login user)
         {
            try
            { 
                usersRepository = new UserRepositiory();
                usersRepository.Open();

                UserRole log = usersRepository.Authenticate(user.UsernameOrEmail, user.Password);
                if(log == UserRole.IsLocked)
                {
                    return View("Message_Registrierung", new Message("Login", "Gesperrt", "Sie sind leider vom Admin gesperrt worden", "Kontaktieren Sie den Admin"));
                }
                if(log == UserRole.Administrator)
                {
                    Session["isAdmin"] = true;
                    

                    User u = usersRepository.GetUser(user.UsernameOrEmail, user.Password);
                    Session["UserID"] = u.ID;

                    Response.Cookies["layout_color"].Value = u.Layout_color;
                    Response.Cookies["background_login"].Value = u.Background_login;

                    return RedirectToAction("index", "home");
                }
                else if(log == UserRole.RegisteredUser)
                {
                    User u = usersRepository.GetUser(user.UsernameOrEmail, user.Password);
                    Session["isAdmin"] = false;
                    Session["UserID"] = u.ID;
                    return RedirectToAction("index", "home");

                }
                else if(log == UserRole.NoUser)
                {
                    return View("Message_Registrierung", new Message("Login", "Sie sind noch kein Benutzer", "Es tut uns leid wir konnten dich in der Datenbank nicht finden :(", "Registrieren!!!")); 
                }
                else
                {
                    return View("Message_Registrierung", new Message("Login", "", "Es ist während der Anmeldung zu einem Fehler gekommen :(", "Versuchen Sie es später erneut"));
                }
                
            }
            catch (Exception)
            {
                return View("Message", new Message("Datenbankfehler", "", "Probleme mit der Datenbankverbindung!", "Versuchen Sie es später erneut."));
            }
            finally
            {
                usersRepository.Close();
           }

        }
        [HttpGet]
        public ActionResult Registration()
        {
            User u = new Models.User();
            return View(u);
        }
        [HttpPost]
        public ActionResult Registration(User user)
        {
            if (user == null)
            {
                return View(user);
            }

            ValidateRegistrationForm(user);

            if (ModelState.IsValid)
            {
                try
                {
                    usersRepository = new UserRepositiory();
                    usersRepository.Open();

                    if (usersRepository.Insert(user))
                    {
                        return View("Message_Registrierung", new Message("Registrierung", "", "Sie wurden erfolgreich registriert!", ""));
                    }
                    else
                    {
                        return View("Message_Registrierung", new Message("Registrierung", "", "Sie konnten nicht registriert werden!", "Versuchen Sie es später erneut."));
                    }

                }
                catch (MySqlException)
                {
                    return View("Message_Registrierung", new Message("Datenbankfehler", "", "Probleme mit der Datenbankverbindung!", "Versuchen Sie es später erneut."));
                }
                finally
                {
                    usersRepository.Close();
                }
            }

            return View(user);
        }

        private List<User> LoadUsers()
        {

                UserRepositiory ur = new UserRepositiory();
                ur.Open();

                List<User> aUser = new List<User>();

            return aUser = ur.GetAllUser();

        }

        private List<User> SearchUsers(string firstname, string lastname)
        {
            UserRepositiory ur = new UserRepositiory();
            ur.Open();

            return ur.SearchUser(firstname, lastname);

        }

        private void ValidateRegistrationForm(User userToValidate)
        {
            UserRepositiory ur = new UserRepositiory();

            ur.Open();
            

            if ((userToValidate.Firstname == null) || (userToValidate.Firstname.Trim().Length < 1))
            {
                ModelState.AddModelError("Firstname", "Bitte geben Sie einen sinnvollen Vornamen ein");
            }
            if ((userToValidate.Lastname == null) || (userToValidate.Lastname.Trim().Length < 1))
            {
                ModelState.AddModelError("Lastname", "Bitte geben Sie einen sinnvollen Nachnamen ein");
            }
            if ((userToValidate.Email == null) || (!userToValidate.Email.Contains("@")))
            {
                ModelState.AddModelError("Email", "Bitte geben Sie eine gültige Email an");
            }
            if (userToValidate.Birthdate >= (DateTime.Now))
            {
                ModelState.AddModelError("Birthdate", "Kommen Sie aus der Zukunft????");
            }
            if (ur.CheckDoubleUsername(userToValidate) == false)
            {
                ModelState.AddModelError("Username", "Der Benutzername ist leider schon vergeben");
            }
            if(ur.CheckDoubleEmail(userToValidate) == false)
            {
                ModelState.AddModelError("Email", "Es besteht bereits ein Konto mit dieser Email");
            }
            if (userToValidate.Username == null)
            {
                ModelState.AddModelError("Username", "Bitte geben Sie einen Benutzernamen ein.");
            }
            if ((userToValidate.Password == null) || (userToValidate.Password.Length < 8))
            {
                ModelState.AddModelError("Password", "Das Passwort muss mindestens 8 Zeichen beinhalten");
            }
            if(userToValidate.Password != userToValidate.PasswordWH)
            {
                ModelState.AddModelError("PasswordWH", "Die Passwörter stimmen nicht überein!");
            }
        }



    }
}