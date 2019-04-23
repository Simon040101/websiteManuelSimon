using System;
using MySql.Data.MySqlClient;
using System.Data;
using System.Collections.Generic;

namespace XOVO.Models.db
{
    public class UserRepositiory : IUserRepositiory
    {
        private string _connenctionString = "Server=localhost;Database=XOVO;Uid=root;Pwd=alpine;SslMode=none";
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

        public bool ChangeData(User user)
        {
            try
            {
                MySqlCommand cmdChange = this._connection.CreateCommand();
                cmdChange.CommandText = "Update users set firstname = @firstname, lastname = @lastname, birthdate = @birthdate, gender = @gender, username = @username, email = @email, passwrd = sha2(@passwort, 256), profilpic = @pfb where id = @id";
                cmdChange.Parameters.AddWithValue("firstname", user.Firstname);
                cmdChange.Parameters.AddWithValue("lastname", user.Lastname);
                cmdChange.Parameters.AddWithValue("birthdate", user.Birthdate);
                cmdChange.Parameters.AddWithValue("gender", user.Gender);
                cmdChange.Parameters.AddWithValue("username", user.Username);
                cmdChange.Parameters.AddWithValue("email", user.Email);
                cmdChange.Parameters.AddWithValue("passwort", user.Password);
                cmdChange.Parameters.AddWithValue("id", user.ID);
                cmdChange.Parameters.AddWithValue("pfb", user.Profilpicture);
                
                return cmdChange.ExecuteNonQuery() == 1;
            }
            catch (Exception ex)
            {

                throw;
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
                cmdInsert.CommandText = "INSERT INTO users VALUES(null, @firstname, @lastname, @birthdate, @gender, @username, @email, sha2(@pwd, 256), 1, 'rot', '/Content/img/background_login_registration.jpg', @profilpic)";
                cmdInsert.Parameters.AddWithValue("firstname", userToAdd.Firstname);
                cmdInsert.Parameters.AddWithValue("lastname", userToAdd.Lastname);
                cmdInsert.Parameters.AddWithValue("birthdate", dateToInsert);
                cmdInsert.Parameters.AddWithValue("gender", userToAdd.Gender);
                cmdInsert.Parameters.AddWithValue("email", userToAdd.Email);
                cmdInsert.Parameters.AddWithValue("username", userToAdd.Username);
                cmdInsert.Parameters.AddWithValue("pwd", userToAdd.Password);
                cmdInsert.Parameters.AddWithValue("profilpic", userToAdd.Profilpicture);

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

        public User GetUser(string emailOrUsername, string passwort)
        {
            try
            {
                MySqlCommand cmd = _connection.CreateCommand();
                cmd.CommandText = "SELECT * FROM users WHERE (username = @emailOrUsername OR email = @emailOrUsername) AND passwrd = sha2(@password, 256) LIMIT 1";
                cmd.Parameters.AddWithValue("emailOrUsername", emailOrUsername);
                cmd.Parameters.AddWithValue("password", passwort);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        return new User
                        {
                            ID = Convert.ToInt32(reader["id"]),
                            Firstname = Convert.ToString(reader["firstname"]),
                            Lastname = Convert.ToString(reader["lastname"]),
                            Birthdate = Convert.ToDateTime(reader["birthdate"]),
                            Gender = (Gender)Convert.ToInt32(reader["gender"]),
                            Username = Convert.ToString(reader["username"]),
                            Email = Convert.ToString(reader["email"]),
                            Layout_color = Convert.ToString(reader["layout_color"]),
                            Background_login = Convert.ToString(reader["background_login"]),
                            Profilpicture = Convert.ToString(reader["profilpic"])
                        };
                    }
                }
                return null;
            }
            catch (MySqlException ex)
            {

                throw;
            }
        }

        public bool CheckDoubleEmail(User user)
        {
            try
            {
                MySqlCommand cmdCeckEmail = this._connection.CreateCommand();
                cmdCeckEmail.CommandText = "Select * from users where email = @email";
                cmdCeckEmail.Parameters.AddWithValue("email", user.Email);

                using (MySqlDataReader reader = cmdCeckEmail.ExecuteReader())
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
                                IsLocked = Convert.ToInt32(reader["isAdmin"]) == 3,
                                Layout_color = Convert.ToString(reader["layout_color"]),
                                Background_login = Convert.ToString(reader["background_login"]),
                                Profilpicture = Convert.ToString(reader["profilpic"]),


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

        public User GetUserById(int id)
        {
            try
            {
                MySqlCommand cmdGetId = this._connection.CreateCommand();
                cmdGetId.CommandText = "Select * from users where id = @id";
                cmdGetId.Parameters.AddWithValue("id", id);

                using(MySqlDataReader reader = cmdGetId.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {



                             return new User
                            {
                                 ID = Convert.ToInt32(reader["id"]),
                                 Firstname = Convert.ToString(reader["firstname"]),
                                 Lastname = Convert.ToString(reader["lastname"]),
                                 Birthdate = Convert.ToDateTime(reader["birthdate"]),
                                 Gender = (Gender)Convert.ToInt32(reader["gender"]),
                                 Username = Convert.ToString(reader["username"]),
                                 Email = Convert.ToString(reader["email"]),
                                 IsLocked = Convert.ToInt32(reader["isAdmin"]) == 3,
                                 Layout_color = Convert.ToString(reader["layout_color"]),
                                 Background_login = Convert.ToString(reader["background_login"]),
                                 Profilpicture = Convert.ToString(reader["profilpic"]),


                             };

                        }
                    }
                    
                }
                return null;
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
                if (firstname.Trim() == "")
                {
                    cmdSearchUser.CommandText = "SELECT * FROM users WHERE (lastname LIKE Concat('%', @lastname, '%'))";
                    cmdSearchUser.Parameters.AddWithValue("lastname", lastname);
                }
                else if (lastname.Trim() == "")
                {
                    cmdSearchUser.CommandText = "SELECT * FROM users WHERE (firstname LIKE Concat('%', @firstname, '%'))";
                    cmdSearchUser.Parameters.AddWithValue("firstname", firstname);
                }
                else
                {
                    cmdSearchUser.CommandText = "SELECT * FROM users WHERE (firstname LIKE Concat('%', @firstname, '%')) AND (lastname LIKE Concat('%', @lastname, '%'))";
                    cmdSearchUser.Parameters.AddWithValue("firstname", firstname);
                    cmdSearchUser.Parameters.AddWithValue("lastname", lastname);
                }
                

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
                                    Gender = (Gender)Convert.ToInt32(reader["gender"]),
                                    Username = Convert.ToString(reader["username"]),
                                    Email = Convert.ToString(reader["email"]),
                                    IsLocked = Convert.ToInt32(reader["isAdmin"]) == 3,
                                    Background_login = Convert.ToString(reader["background_login"]),
                                    Layout_color = Convert.ToString(reader["layout_color"]),
                                    Profilpicture = Convert.ToString(reader["profilpic"]),
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
        }

        public List<User> SearchUserByUsername(string username)
        {
            List<User> foundUser = new List<User>();
            try
            {
                MySqlCommand cmdSearchUser = this._connection.CreateCommand();
                if (username.Trim() != "")
                {
                    cmdSearchUser.CommandText = "SELECT * FROM users WHERE (username LIKE Concat('%', @username, '%'))";
                    cmdSearchUser.Parameters.AddWithValue("username", username);
                }
                else
                {
                    cmdSearchUser.CommandText = "SELECT * FROM users";
                }



                using (MySqlDataReader reader = cmdSearchUser.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {

                            foundUser.Add(
                                new User
                                {
                                    Username = Convert.ToString(reader["username"])
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
        }

        public string GetBackgroundLogin(int id)
        {
            try
            {
                MySqlCommand cmdGetBackground = this._connection.CreateCommand();
                cmdGetBackground.CommandText = "Select background_login from users where id = @id";
                cmdGetBackground.Parameters.AddWithValue("id", id);

                string Background;
            
                using (MySqlDataReader reader = cmdGetBackground.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            return Background = Convert.ToString(reader["background_login"]);
                        }
                    }

                    return null;
                    
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string GetProfilPic(int id)
        {
            try
            {
                MySqlCommand cmdGetBackground = this._connection.CreateCommand();
                cmdGetBackground.CommandText = "Select profilpic from users where id = @id";
                cmdGetBackground.Parameters.AddWithValue("id", id);

                string Picture;

                using (MySqlDataReader reader = cmdGetBackground.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            return Picture = Convert.ToString(reader["profilpic"]);
                        }
                    }

                    return null;

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool ChangeBackground(User user)
        {
            try
            {
                MySqlCommand cmdChangeBackground = this._connection.CreateCommand();
                cmdChangeBackground.CommandText = "Update users set background_login = @background_login where id = @id";
                cmdChangeBackground.Parameters.AddWithValue("background_login", user.Background_login);
                cmdChangeBackground.Parameters.AddWithValue("id", user.ID);

                return cmdChangeBackground.ExecuteNonQuery() == 1;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool ChangeLayout(User user)
        {
            try
            {
                MySqlCommand cmdChangeLayout = this._connection.CreateCommand();
                cmdChangeLayout.CommandText = "Update users set layout_color = @layout_color where id=@id";
                cmdChangeLayout.Parameters.AddWithValue("layout_color", user.Layout_color);
                cmdChangeLayout.Parameters.AddWithValue("id", user.ID);

                return cmdChangeLayout.ExecuteNonQuery() == 1;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public string GetLayoutColor(int id)
        {
            try
            {
                MySqlCommand cmdGetColor = this._connection.CreateCommand();
                cmdGetColor.CommandText = "Select layout_color from users where id = @id";
                cmdGetColor.Parameters.AddWithValue("id", id);

                string Color;

                using (MySqlDataReader reader = cmdGetColor.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            return Color = Convert.ToString(reader["layout_color"]);
                        }
                    }

                    return null;
                    
                }
                
            }

            catch (Exception)
            {

                throw;
            }
        }


        
    }
}