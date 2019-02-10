using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XOVO.Models
{
    public class User
    {
        private int _age;

        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int Age
        {
            get { return this._age; }
            set
            {
                if(value <= 0)
                {
                    this._age = value;
                }
            }
        }
        public string Username { get; set; }
        public string Password { get; set; }


        public User() : this("", "", 0, "", "") { }
        public User(string firstname, string lastname, int age, string username, string password)
        {
            this.Firstname = firstname;
            this.Lastname = lastname;
            this.Age = age;
            this.Username = username;
            this.Password = password;
        }



    }
}