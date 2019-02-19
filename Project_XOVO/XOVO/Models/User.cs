﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XOVO.Models
{
    public enum Gender
    {
        male, female, notSpecified
    }

    public class User
    {
        

        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime Birthdate { get; set; }
        public Gender Gender { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string PasswordWH { get; set; }
        public string Email { get; set; }


        public User() : this("", "", DateTime.MinValue, Gender.notSpecified, "", "", "", "") { }
        public User(string firstname, string lastname, DateTime birthdate, Gender gender, string username, string password, string passwordwh, string email)
        {
            this.Firstname = firstname;
            this.Lastname = lastname;
            this.Birthdate = birthdate;
            this.Gender = gender;
            this.Username = username;
            this.Password = password;
            this.PasswordWH = passwordwh;
            this.Email = email;
        }



    }
}