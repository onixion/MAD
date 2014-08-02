using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite; //include SQLite database library
using System.IO;  

namespace MAD
{
    public class DB
    {
        private string _DBname = "MyDatabase.sqlite";
        private SQLiteConnection _dbConnection;

        public DB(string filename)
        {
            if (!File.Exists(_DBname))
            {
                SQLiteConnection.CreateFile(_DBname); 
            }
        }
        public void Connect()
        {
            _dbConnection = new SQLiteConnection("Data Source=MyDatabase.sqlite;Version=3;");
            _dbConnection.Open();
        }

        public void CreateDB(string DBname)
        {
           if (_dbConnection.State == SQLiteConnection. )
        }
    }
}
