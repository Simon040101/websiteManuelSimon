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
                cmdInsert.CommandText = "INSERT INTO users VALUES(null, @firstname, @lastname, @birthdate, @gender, @username, @email, sha2(@pwd, 256))";
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

        public bool Authenticate(string emailOrUsername, string Passwort)
        {
            try
            {
                MySqlCommand cmdAut = this._connection.CreateCommand();
                cmdAut.CommandText = "Select * from users where ((username = @usernameOrEMail) AND (passwrd = sha2(@password, 256)) OR ((email = @usernameOrEMail) AND (passwrd = sha2(@password, 256))))";
                cmdAut.Parameters.AddWithValue("usernameOrEMail", emailOrUsername);
                cmdAut.Parameters.AddWithValue("password", Passwort);

                using(MySqlDataReader reader = cmdAut.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
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
                                Firstname = Convert.ToString(reader["firstname"]),
                                Lastname = Convert.ToString(reader["lastname"]),
                                Birthdate = Convert.ToDateTime(reader["birthdate"]),
                                Gender = (Gender)Convert.ToInt32(reader["gender"]),
                                Username = Convert.ToString(reader["username"]),
                                Email = Convert.ToString(reader["email"])


                            }

                            );
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
    }
}