using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XOVO.Models
{
    public class Login
    {
        public string UsernameOrEmail { get; set; }
        public string Password { get; set; }

        public Login() : this("", "") { }

        public Login(string usernameOrEmail, string password)
        {
            this.UsernameOrEmail = usernameOrEmail;
            this.Password = password;
        }
    }
}