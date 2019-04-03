﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using XOVO.Models;

namespace XOVO.Models.db
{
    public class FeedRepository : IFeedRepository
    {
        private string _connectionString = "Server=localhost;Database=XOVO;Uid=root;Pwd=alpine;SslMode=none";
        private MySqlConnection _connection;

        public void Open()
        {
            if (this._connection == null)
            {
                this._connection = new MySqlConnection(_connectionString);
            }

            if (this._connection.State != ConnectionState.Open)
            {
                this._connection.Open();
            }
        }

        public void Close()
        {
            if ((this._connection != null) && (this._connection.State == ConnectionState.Open))
            {
                this._connection.Close();
            }
        }

        public List<FeedItem> GetFeedItems()
        {
            List<FeedItem> allItems = new List<FeedItem>();

            try
            {
                MySqlCommand cmdGetAllItems = this._connection.CreateCommand();
                cmdGetAllItems.CommandText = "SELECT * FROM Feed";

                using (MySqlDataReader reader = cmdGetAllItems.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            // TODO? - aufgrund der user_id den kompletten User ermittl"creationDateTime"n
                            //      Problem - aktuelle Lösung belegt nur das ID-Feld des Users, die restlichen Felder des Users sind unbelegt.
                            allItems.Add(new FeedItem(Convert.ToInt32(reader["feed_id"]),
                                Convert.ToInt32(reader["user_id"]), Convert.ToDateTime(reader["creationDateTime"]),
                                Convert.ToString(reader["imagePath"]), Convert.ToString(reader["content"]),
                                Convert.ToInt32(reader["likeCount"])));
                        }
                    }
                }

                return allItems.Count > 0 ? allItems : null;
            }
            catch (MySqlException)
            {
                throw;
            }
        }

        public List<FeedItem> GetFeedItemsByID(int id)
        {
            List<FeedItem> allItems = new List<FeedItem>();

            try
            {
                MySqlCommand cmdGetIDItems = this._connection.CreateCommand();
                cmdGetIDItems.CommandText = "SELECT * FROM Feed where id = @id";
                cmdGetIDItems.Parameters.AddWithValue("id", id);

                using (MySqlDataReader reader = cmdGetIDItems.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            allItems.Add(new FeedItem(Convert.ToInt32(reader["feed_id"]),
                                Convert.ToInt32(reader["user_id"]), Convert.ToDateTime(reader["creationDateTime"]),
                                Convert.ToString(reader["imagePath"]), Convert.ToString(reader["content"]),
                                Convert.ToInt32(reader["likeCount"])));
                        }
                    }
                }

                return allItems.Count > 0 ? allItems : null;
            }
            catch (MySqlException)
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

                using (MySqlDataReader reader = cmdGetId.ExecuteReader())
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
                                Gender = (Gender) Convert.ToInt32(reader["gender"]),
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

        public bool InsertFeedItem(FeedItem itemToInsert)
        {
            if ((this._connection == null) || (this._connection.State != ConnectionState.Open))
            {
                return false;
            }

            try
            {
                DateTime dateToInsert;
                dateToInsert = DateTime.Now;
                MySqlCommand cmdInsert = this._connection.CreateCommand();
                cmdInsert.CommandText =
                    "INSERT INTO feed VALUES(NULL, @id, @creationDateTime, @imgPath, @textarea, 0)";
                cmdInsert.Parameters.AddWithValue("id", itemToInsert.UserForFeedID);
                cmdInsert.Parameters.AddWithValue("creationDateTime", dateToInsert);
                cmdInsert.Parameters.AddWithValue("imgPath", itemToInsert.Image);
                cmdInsert.Parameters.AddWithValue("textarea", itemToInsert.FeedContent);

                return cmdInsert.ExecuteNonQuery() == 1;
            }

            catch (MySqlException)
            {
                throw;
            }
        }

        public bool UserLikeFeed(int userID, int feedID)
        {
            try
            {
                MySqlCommand cmdLike = _connection.CreateCommand();
                cmdLike.CommandText = "Insert into UsersLikeFeed Values (@u_id, @f_id)";
                cmdLike.Parameters.AddWithValue("u_id", userID);
                cmdLike.Parameters.AddWithValue("f_id", feedID);

                return cmdLike.ExecuteNonQuery() == 1;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int CountLike(int feedID)
        {
            try
            {
                MySqlCommand cmdCount = _connection.CreateCommand();
                cmdCount.CommandText = "Select Count(uid) as likes from userslikefeed where fid = @id";
                cmdCount.Parameters.AddWithValue("id", feedID);

                using (MySqlDataReader reader = cmdCount.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        return (int) reader["likes"];
                    }
                }

                return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}