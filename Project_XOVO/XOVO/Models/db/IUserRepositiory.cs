using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XOVO.Models.db
{
    public interface IUserRepositiory
    {
        void Open();
        void Close();

        bool Insert(User userToAdd);
        User Authenticate(string emailOrUsername, string Passwort);
    }
}