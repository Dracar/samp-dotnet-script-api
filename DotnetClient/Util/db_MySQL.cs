/*
 * Iain Gilbert
 * 2011
 *
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MySql.Data.MySqlClient;

namespace Samp.Util
{
    public class db_MySQL : Database
    {
        public string SQLServer = "localhost";
        public string SQLDatabase = "samp";
        public string SQLUsername = "root";
        public string SQLPassword = "pass";

        public MySqlConnection Connection = null;

        public override bool Connect()
        {
            //Log.Debug("Conncting to DB");
            string MyConString = "SERVER=" + SQLServer + ";" +
                "DATABASE=" + SQLDatabase + ";" +
                "UID=" + SQLUsername + ";" +
                "PASSWORD=" + SQLPassword + ";";
            Connection = new MySqlConnection(MyConString);
            Connection.Open();

            MySqlCommand command = Connection.CreateCommand();


            return true;
        }

        public override bool Disconnect()
        {
            //Log.Debug("Disconnecting from DB");
            if (Connection == null) { return false; }
            Connection.Close();
            Connection = null;
            return true;
        }

        public MySqlDataReader ReadData(string query)
        {
            if (Connection == null) Connect();
            MySqlCommand command = Connection.CreateCommand();
            command.CommandText = query;

            MySqlDataReader Reader = null;
            try
            {
                Reader = command.ExecuteReader();
            }
            catch (Exception ex) { Log.Exception(ex); };
            //Disconnect();
            return Reader;
        }

        public MySqlCommand InsertData(string query)
        {
            if (Connection == null) Connect();
            MySqlCommand command = Connection.CreateCommand();
            command.CommandText = query;

            MySqlDataReader Reader = null;
            try
            {
                Reader = command.ExecuteReader();
            }
            catch (Exception ex) { Log.Exception(ex); };
            Reader.Close();
            //Disconnect();
            return command;
        }

    }
}
