using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XOVO.Models
{
    public class User
    {
        

        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime Birthdate { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }


        public User() : this("", "", DateTime.MinValue, "", "", "") { }
        public User(string firstname, string lastname, DateTime birthdate, string username, string password, string email)
        {
            this.Firstname = firstname;
            this.Lastname = lastname;
            this.Birthdate = birthdate;
            this.Username = username;
            this.Password = password;
            this.Email = email;
        }



    }
}