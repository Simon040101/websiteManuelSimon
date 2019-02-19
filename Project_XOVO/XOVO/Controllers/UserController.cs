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

        // GET: User
        public ActionResult Index()
        {
            return View();
        }
         [HttpGet]
         public ActionResult Login()
        {
            return View(new Login());
        }
        [HttpPost]
        public ActionResult Login(Login user)
        {
            usersRepository = new UserRepositiory();
            usersRepository.Open();
            bool log = usersRepository.Authenticate(user.UsernameOrEmail, user.Password);
            if(log == true)
            {
                return View("Message_Registrierung", new Message("Login", "", "Sie wurden erfolgreich angemeldet", ""));
            }
            else if(log == false)
            {
                return View("Message_Registrierung", new Message("Login", "", "Es ist während der Anmeldung zu einem Fehler gekommen :(", "Versuchen Sie es später erneut"));
            }

            return View("Message_Regisrierung", new Message("Allgemeiner Fehler", "", "Es ist zu einem Fehler gekommen", "Probieren Sie es später erneut!"));
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