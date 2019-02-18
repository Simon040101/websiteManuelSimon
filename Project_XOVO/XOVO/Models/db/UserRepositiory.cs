﻿using System;
using MySql.Data.MySqlClient;
using System.Data;


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
                MySqlCommand cmdInsert = this._connection.CreateCommand();
                cmdInsert.CommandText = "INSERT INTO users VALUES(null, @firstname, @lastname, null, null, @username, @email, sha2(@pwd, 256))";
                cmdInsert.Parameters.AddWithValue("firstname", userToAdd.Firstname);
                cmdInsert.Parameters.AddWithValue("lastname", userToAdd.Lastname);
                cmdInsert.Parameters.AddWithValue("email", userToAdd.Email);
                cmdInsert.Parameters.AddWithValue("username", userToAdd.Username);
                cmdInsert.Parameters.AddWithValue("pwd", userToAdd.Password);

                return cmdInsert.ExecuteNonQuery() == 1;
            }
            catch (MySqlException)
            {
                throw;
            }
        }

        public User Authenticate(string emailOrUsername, string Passwort)
        {
            throw new NotImplementedException();
        }


    }
}