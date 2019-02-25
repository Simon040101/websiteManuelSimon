using System;
using MySql.Data.MySqlClient;
using System.Data;
using System.Collections.Generic;

namespace XOVO.Models.db
{
    public class UserRepositiory : IUserRepositiory
    {
        private string _connenctionString = "Server=localhost;Database=XOVO;Uid=root;SslMode=none";
        private MySqlConnection _connection;

        public void Open()
        {
            if (this._connection == null)
            {
                this._connection = new MySqlConnection(_connenctionString);
            }
            if (this._connection.State != ConnectionState.Open)
            {
                this._connection.Open();
            }
        }

        public void Close()
        {
            if((this._connection != null) && (this._connection.State == ConnectionState.Open))
            {
                this._connection.Close();
            }
        }

        public bool Insert(User userToAdd)
        {
            if((this._connection == null)||(this._connection.State != ConnectionState.Open))
            {
                return false;
            }
            try
            {
                string dateToInsert = userToAdd.Birthdate.ToString("yyyy-M-d");
                MySqlCommand cmdInsert = this._connection.CreateCommand();
                cmdInsert.CommandText = "INSERT INTO users VALUES(null, @firstname, @lastname, @birthdate, @gender, @username, @email, sha2(@pwd, 256), 1)";
                cmdInsert.Parameters.AddWithValue("firstname", userToAdd.Firstname);
                cmdInsert.Parameters.AddWithValue("lastname", userToAdd.Lastname);
                cmdInsert.Parameters.AddWithValue("birthdate", dateToInsert);
                cmdInsert.Parameters.AddWithValue("gender", userToAdd.Gender);
                cmdInsert.Parameters.AddWithValue("email", userToAdd.Email);
                cmdInsert.Parameters.AddWithValue("username", userToAdd.Username);
                cmdInsert.Parameters.AddWithValue("pwd", userToAdd.Password);

                return cmdInsert.ExecuteNonQuery() == 1;
            }
            catch (MySqlException ex)
            {
                throw;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                MySqlCommand cmdDelete = this._connection.CreateCommand();
                cmdDelete.CommandText = "Delete from users where id = @id and isAdmin != 0";
                cmdDelete.Parameters.AddWithValue("id", id);

                return cmdDelete.ExecuteNonQuery() == 1;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public UserRole Authenticate(string emailOrUsername, string passwort)
        {
            try
            {
                MySqlCommand cmdAut = this._connection.CreateCommand();
                cmdAut.CommandText = "SELECT isAdmin FROM users WHERE ((username = @usernameOrEMail) AND (passwrd = sha2(@password, 256)) OR ((email = @usernameOrEMail) AND (passwrd = sha2(@password,256))))";
                cmdAut.Parameters.AddWithValue("usernameOrEMail", emailOrUsername);
                cmdAut.Parameters.AddWithValue("password", passwort);

                using(MySqlDataReader reader = cmdAut.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        if(Convert.ToInt32(reader["isAdmin"]) == 3)
                        {
                            return UserRole.IsLocked;
                        }
                        else if (Convert.ToInt32(reader["isAdmin"]) == 0)
                        {
                            return UserRole.Administrator;
                        }

                        else if(Convert.ToInt32(reader["isAdmin"]) == 1)
                        {
                            return UserRole.RegisteredUser;
                        }
                    }

                   
                    return UserRole.NoUser;
                    
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool CheckDoubleUsername(User user)
        {
            try
            {
                MySqlCommand cmdCheck = this._connection.CreateCommand();
                cmdCheck.CommandText = "Select * from users where username = @username";
                cmdCheck.Parameters.AddWithValue("username", user.Username);

                using (MySqlDataReader reader = cmdCheck.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<User> GetAllUser()
        {
            List<User> allUsers = new List<User>();

            try
            {
                MySqlCommand cmdGetAllUsers = this._connection.CreateCommand();
                cmdGetAllUsers.CommandText = "Select * from users";

                using (MySqlDataReader reader = cmdGetAllUsers.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {



                            allUsers.Add(new User
                            {
                                ID = Convert.ToInt32(reader["id"]),
                                Firstname = Convert.ToString(reader["firstname"]),
                                Lastname = Convert.ToString(reader["lastname"]),
                                Birthdate = Convert.ToDateTime(reader["birthdate"]),
                                Gender = (Gender)Convert.ToInt32(reader["gender"]),
                                Username = Convert.ToString(reader["username"]),
                                Email = Convert.ToString(reader["email"]),
                                IsLocked = Convert.ToInt32(reader["isAdmin"]) == 3


                                } );

                            }
                        }
                    }
                

                return allUsers.Count > 0 ? allUsers : null;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool UnlockUser(int id)
        {
            try
            {
                MySqlCommand cmdUnLock = this._connection.CreateCommand();
                cmdUnLock.CommandText = "Update users set isAdmin = 1 where id = @id";
                cmdUnLock.Parameters.AddWithValue("id", id);

                return cmdUnLock.ExecuteNonQuery() == 1;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool LockUser(int id)
        {
            try
            {
                MySqlCommand cmdLock = this._connection.CreateCommand();
                cmdLock.CommandText = "Update users set isAdmin = 3 where id = @id and isAdmin != 0";
                cmdLock.Parameters.AddWithValue("id", id);

                return cmdLock.ExecuteNonQuery() == 1;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<User> SearchUser(string firstname, string lastname)
        {
            List<User> foundUser = new List<User>();
            try
            {
                MySqlCommand cmdSearchUser = this._connection.CreateCommand();
                cmdSearchUser.CommandText = "SELECT * FROM users WHERE (firstname = @firstname) AND (lastname = @lastname)";
                cmdSearchUser.Parameters.AddWithValue("firstname", firstname);
                cmdSearchUser.Parameters.AddWithValue("lastname", lastname);

                using (MySqlDataReader reader = cmdSearchUser.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {

                            foundUser.Add(
                                new User
                                {
                                    ID = Convert.ToInt32(reader["id"]),
                                    Firstname = Convert.ToString(reader["firstname"]),
                                    Lastname = Convert.ToString(reader["lastname"]),
                                    Birthdate = Convert.ToDateTime(reader["birthdate"]),
                                    Gender = (Gender) Convert.ToInt32(reader["gender"]),
                                    Username = Convert.ToString(reader["username"]),
                                    Email = Convert.ToString(reader["email"]),
                                    IsLocked = Convert.ToInt32(reader["isAdmin"]) == 3
                                });
                        }


                    }

                    return foundUser.Count > 0 ? foundUser : null;
                }
            }
            catch (MySqlException)
            {
                throw;
            }

            return null;
        }


    }
}