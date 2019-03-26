using System;
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
        private string _connectionString = "Server=localhost;Database=XOVO;Uid=root;Password=alpine;SslMode=none";
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
                            allItems.Add(
                                new FeedItem(Convert.ToInt32(reader["feed_id"]),  Convert.ToInt32(reader["user_id"]), Convert.ToDateTime(reader["creationDateTime"]),
                                        Convert.ToString(reader["imagePath"]), Convert.ToString(reader["content"])));
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
                cmdInsert.CommandText = "INSERT INTO feed VALUES(NULL, @id, @creationDateTime, @imgPath, @textarea)";
                cmdInsert.Parameters.AddWithValue("id", itemToInsert.UserForFeed.ID);
                cmdInsert.Parameters.AddWithValue("creationDateTime", dateToInsert);
                cmdInsert.Parameters.AddWithValue("imgPath", itemToInsert.ImgPath);
                cmdInsert.Parameters.AddWithValue("textarea", itemToInsert.FeedContent);



                return cmdInsert.ExecuteNonQuery() == 1;
            }
            
            catch (MySqlException ex)
            {
                throw;
            }
        }

        private 
    }
}