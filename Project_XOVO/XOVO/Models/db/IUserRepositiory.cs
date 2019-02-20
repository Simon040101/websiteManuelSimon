using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XOVO.Models.db
{
    public enum UserRole
    {
        Administrator, RegisteredUser, NoUser
    }

    public interface IUserRepositiory
    {
        void Open();
        void Close();

        bool Insert(User userToAdd);
        UserRole Authenticate(string emailOrUsername, string passwort);
        bool CheckDoubleUsername(User user);
        List<User> GetAllUser();
    }
}