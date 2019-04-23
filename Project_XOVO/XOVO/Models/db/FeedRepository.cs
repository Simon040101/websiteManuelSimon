using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
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

        public bool CheckIfLiked(int uid, int fid)
        {
            try
            {
                MySqlCommand cmdGetAllItems = this._connection.CreateCommand();
                cmdGetAllItems.CommandText = "SELECT * FROM userslikefeed where uid = @uid and fid = @fid";
                cmdGetAllItems.Parameters.AddWithValue("uid", uid);
                cmdGetAllItems.Parameters.AddWithValue("fid", fid);

                using (MySqlDataReader reader = cmdGetAllItems.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<FeedItem> GetFeedItems()
        {
            List<FeedItem> allItems = new List<FeedItem>();

            try
            {
                MySqlCommand cmdGetAllItems = this._connection.CreateCommand();
                cmdGetAllItems.CommandText = "SELECT * FROM Feed order by feed_id desc";

                using (MySqlDataReader reader = cmdGetAllItems.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            // TODO? - aufgrund der user_id den kompletten User ermittl"creationDateTime"n
                            //      Problem - aktuelle Lösung belegt nur das ID-Feld des Users, die restlichen Felder des Users sind unbelegt.
                            allItems.Add(
                                new FeedItem(Convert.ToInt32(reader["feed_id"]), Convert.ToString(reader["username"]), Convert.ToInt32(reader["user_id"]), Convert.ToDateTime(reader["creationDateTime"]),
                                        Convert.ToString(reader["imagePath"]), Convert.ToString(reader["content"]), 0, null));
                        }
                    }
                }

                foreach (var t in allItems)
                {
                    t.LikeCount = CountLike(t.Id);
                }
                foreach (var t in allItems)
                {
                    t.Comments = GetAllComments(t.Id);
                }

                foreach (var feedItem in allItems)
                {
                    feedItem.Comments = GetAllComments(feedItem.Id);
                }

                return allItems.Count > 0 ? allItems : null;
            }

            catch (MySqlException ex)
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
                            allItems.Add(new FeedItem(Convert.ToInt32(reader["feed_id"]), Convert.ToString(reader["username"]),
                                Convert.ToInt32(reader["user_id"]), Convert.ToDateTime(reader["creationDateTime"]),
                                Convert.ToString(reader["imagePath"]), Convert.ToString(reader["content"]),
                                Convert.ToInt32(reader["likeCount"]), GetAllComments(Convert.ToInt32(reader["feed_id"]))));
                        }
                    }
                }

                return allItems.Count > 0 ? allItems : null;
            }
            catch (MySqlException ex)
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
                cmdInsert.CommandText = "INSERT INTO feed VALUES(NULL ,@username, @user_id, @creationDateTime, @imgPath, @textarea)";
                cmdInsert.Parameters.AddWithValue("username", itemToInsert.Username);
                cmdInsert.Parameters.AddWithValue("user_id", itemToInsert.UserForFeedID);
                cmdInsert.Parameters.AddWithValue("creationDateTime", dateToInsert);
                cmdInsert.Parameters.AddWithValue("imgPath", itemToInsert.Image);
                cmdInsert.Parameters.AddWithValue("textarea", itemToInsert.FeedContent);



                return cmdInsert.ExecuteNonQuery() == 1;
            }

            catch (MySqlException ex)
            {
                throw;
            }
        }

        public bool UserLikeFeed(int userID, int feedID)
        {
            try
            {
                if (CheckIfLiked(userID, feedID) == false)
                {
                    MySqlCommand cmdLike = _connection.CreateCommand();
                    cmdLike.CommandText = "Insert into UsersLikeFeed Values (@u_id, @f_id)";
                    cmdLike.Parameters.AddWithValue("u_id", userID);
                    cmdLike.Parameters.AddWithValue("f_id", feedID);

                    return cmdLike.ExecuteNonQuery() == 1;
                }
                else
                {
                    MySqlCommand cmdDislike = _connection.CreateCommand();
                    cmdDislike.CommandText = "Delete from userslikefeed where uid = @id and fid= @fid";
                    cmdDislike.Parameters.AddWithValue("id", userID);
                    cmdDislike.Parameters.AddWithValue("fid", feedID);

                    return cmdDislike.ExecuteNonQuery() == 1;
                }

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
                        return Convert.ToInt32(reader["likes"]);
                    }
                }

                return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool UserCommentFeed(string username, int feedID, string Comment)
        {
            try
            {
                MySqlCommand cmdComment = _connection.CreateCommand();
                cmdComment.CommandText = "Insert into UserCommentFeed Values (@username, @f_id, @content, null)";
                cmdComment.Parameters.AddWithValue("username", username);
                cmdComment.Parameters.AddWithValue("f_id", feedID);
                cmdComment.Parameters.AddWithValue("content", Comment);

                return cmdComment.ExecuteNonQuery() == 1;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public bool DeleteFeed(int id)
        {
            try
            {
                MySqlCommand cmdLike = _connection.CreateCommand();
                cmdLike.CommandText = "Delete from Feed where feed_id = @id";
                cmdLike.Parameters.AddWithValue("id", id);
                return cmdLike.ExecuteNonQuery() == 1;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<Comment> GetAllComments(int feedID)
        {
            try
            {
                List<Comment> AllComments = new List<Comment>();

                MySqlCommand cmdGetComments = this._connection.CreateCommand();
                cmdGetComments.CommandText = "Select * from usercommentfeed where fid = @id";
                cmdGetComments.Parameters.AddWithValue("id", feedID);

                using (MySqlDataReader reader = cmdGetComments.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                           
                            AllComments.Add(
                                new Comment(Convert.ToString(reader["username"]), Convert.ToInt32(reader["fid"]), Convert.ToString(reader["content"])));
                        }
                    }
                }

                return AllComments.Count > 0 ? AllComments : null;


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}