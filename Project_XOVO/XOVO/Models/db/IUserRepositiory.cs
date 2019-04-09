using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XOVO.Models.db
{
    public enum UserRole
    {
        Administrator, RegisteredUser, NoUser, IsLocked
    }

    public interface IUserRepositiory
    {
        void Open();
        void Close();

        bool Insert(User userToAdd);
        bool Delete(int id);
        UserRole Authenticate(string emailOrUsername, string passwort);
        bool CheckDoubleUsername(User user);
        List<User> GetAllUser();
        bool LockUser(int id);
        bool CheckDoubleEmail(User user);
        bool ChangeData(User user);
        User GetUser(string emailOrUsername, string passwort);
        List<User> SearchUserByUsername(string username);
        User GetUserById(int id);
        bool ChangeLayout(User user);
        string GetLayoutColor(int id);
        string GetBackgroundLogin(int id);
        bool ChangeBackground(User user);
    }
}