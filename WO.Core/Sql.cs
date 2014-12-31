using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Text;

namespace WO.Core
{
    public class SqlData
    {
        public string host;
        public string db;
        public string user;
        public string pass;
    }

    public class Sql
    {
        public Sql(SqlData d)
        {
            Host = d.host;
            Db = d.db;
            User = d.user;
            Pass = d.pass;
        }

        public void ExecuteQuery(string q)
        {
            try
            {
                conn = new MySqlConnection("SERVER=" + Host + ";DATABASE=" + Db + ";UID=" + User + ";PASSWORD=" + Pass + ";");
                command = new MySqlCommand(q);

                command.Connection = conn;
                command.CommandTimeout = 10000;
                conn.Open();
                reader = command.ExecuteReader();
            }
            catch (MySqlException m)
            {
                
                System.Console.WriteLine(m.ToString());
            }
        }

        public void Dispose()
        {
            conn.Close();
            command.Dispose();
            reader.Close();
        }

        private string Host;
        private string User;
        private string Pass;
        private string Db;

        private MySqlConnection conn;
        private MySqlCommand command;
        public MySqlDataReader reader;
    }
}
