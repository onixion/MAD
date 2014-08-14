using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite; //include SQLite database library
using System.IO;  
using System.Data;

namespace MAD
{
    public class DB
    {
        private string _DBname;
        private SQLiteConnection _dbConnection;

        //check if a databese already exists if not create one
        public DB(string DBname)
        {
            _DBname = DBname;
            if (!File.Exists(_DBname))
            {
                SQLiteConnection.CreateFile(_DBname);
            }
        }

        //connect to database
        public void ConnectDB()
        {
            _dbConnection = new SQLiteConnection("Data Source=MyDatabase.sqlite;Version=3;");
            _dbConnection.Open();
        }

 //       //check if table excists, create if not, connect and write results
        public table(string TableName, string IP, int Port)
        {
            //check if table exists if not create it
            string sql = "create table if not exists " + TableName + "()";
            SQLiteCommand command = new SQLiteCommand(sql, _dbConnection);
            //write results in the table
            sql = "insert into " + TableName + " (IP, port) values (" + IP + ", " + Port + ")";
            command = new SQLiteCommand(sql, _dbConnection);
            command.ExecuteNonQuery();
        }






/*
        public bool CheckTableExists(string tableName)
        {
           bool tableExists = false;
           string query1 = "SELECT name FROM sqlite_master WHERE name ='" + tableName + "';";

           SQLiteConnection sqlConnection = OpenConnection();
           SQLiteCommand sqlCommand = new SQLiteCommand(query1, sqlConnection);
        
            try
            {

                SQLiteDataReader sqlDataReader = sqlCommand.ExecuteReader();

                if(sqlDataReader.HasRows)
                {
                    tableExists = true;
                }

               // sqlDataReader.Close();
               // sqlConnection.Close();
            }
      
            catch (Exception ex)
            {
           //exception
            }

            return tableExists;
        }
*/








    }
}
