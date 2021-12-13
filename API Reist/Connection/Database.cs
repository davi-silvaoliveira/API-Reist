using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace API_Reist.Connection
{
    public class Database : IDisposable
    {
        public MySqlConnection connection;
        public MySqlCommand command;

        public Database()
        {
            //connection = new MySqlConnection("server = localhost; port = 3306; database = reist_2021; user id = root; password = root; Allow User Variables=True");
            connection = new MySqlConnection("server = reistdatabaseazure.mysql.database.azure.com; port = 3306; database = reist_2021; user id = brunodatabase@reistdatabaseazure; password = brunoDB2021; Allow User Variables = True");
            if (connection.State == ConnectionState.Closed)
                connection.Open();
        }

        public void ExecuteCommand(string comando)
        {
            command = new MySqlCommand(comando, connection);
            command.ExecuteNonQuery();
        }

        public MySqlDataReader ReturnCommand(string comando)
        {
            command = new MySqlCommand(comando, connection);
            return command.ExecuteReader();

            /*try
            {
                command = new MySqlCommand(comando, connection);
                return command.ExecuteReader();
            }
            catch
            {
                connection.Close(); connection.Open();
                connection = connection = new MySqlConnection("server = localhost; port = 3306; database = reist_2021; user id = root; password = root");
                command = new MySqlCommand(comando, connection);
                return command.ExecuteReader();
            }*/
        }

        public void Dispose()
        {
            if (connection.State == ConnectionState.Open)
                connection.Close();
        }
    }
}
