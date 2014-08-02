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
        private string _DBname = "MyDatabase.sqlite";
        private SQLiteConnection _dbConnection;

        private object _lock = new object();

        public DB(string DBname)
        {
            _DBname = DBname;
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
           lock(_lock)
           {
                if (_dbConnection.State == System.Data.ConnectionState.Open )
                {

                    SQLiteCommand _command = new SQLiteCommand(_dbConnection);
                    _command
                }
           }
        }
    }
}
