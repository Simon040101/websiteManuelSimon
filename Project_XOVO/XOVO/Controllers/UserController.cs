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
            return View();
        }
        [HttpPost]
        public ActionResult Login(User user)
        {
            return View();
        }
        [HttpGet]
        public ActionResult Registration()
        {
            return View();
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
                        return View("Message", new Message("Registrierung", "", "Sie wurden erfolgreich registriert!", ""));
                    }
                    else
                    {
                        return View("Message", new Message("Registrierung", "", "Sie konnten nicht registriert werden!", "Versuchen Sie es später erneut."));
                    }

                }
                catch (MySqlException)
                {
                    return View("Message", new Message("Datenbankfehler", "", "Probleme mit der Datenbankverbindung!", "Versuchen Sie es später erneut."));
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
            if (userToValidate.Lastname == null)
            {
                ModelState.AddModelError("Lastname", "Bitte geben Sie einen Nachnamen ein");
            }
            if(userToValidate.Lastname.Trim().Length < 1)
            {
                ModelState.AddModelError("Lastname", "Bitte geben Sie einen sinnvollen Nachnamen ein");
            }
            if (userToValidate.Email == null)
            {
                ModelState.AddModelError("EMail", "Bitte geben Sie Ihre gültige Email ein");
            }
            if (userToValidate.Birthdate >= DateTime.Now)
            {
                ModelState.AddModelError("Birthdate", "Kommen Sie aus der Zukunft????");
            }
            if (userToValidate.Username == null)
            {
                ModelState.AddModelError("Username", "Bitte geben Sie einen Benutzernamen ein.");
            }
            if ((userToValidate.Password == null) || (userToValidate.Password.Length < 8))
            {
                ModelState.AddModelError("Password", "Das Passwort muss mindestens 8 Zeichen beinhalten");
            }
        }

    }
}