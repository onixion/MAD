using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Data.Common;
using System.Data.SQLite; //include SQLite database library
using System.IO;  
using System.Data;

namespace MAD
{
    /*      //don't forget at the end .sqlite
            DB MADmemory = new DB("MAD7.sqlite");
            //connect to DB
            MADmemory.ConnectDB("MAD7.sqlite");
     * 
     *      MADmemory.CreateTable("mac_id", new TableInfo("IP", TableInfo.OType.CHAR20), new TableInfo("PORT", TableInfo.OType.CHAR30), new TableInfo("subnetzmaske", TableInfo.OType.CHAR30));

            MADmemory.InsertToTable("mac_id", new Insert("PORT", "17"), new Insert("IP", "192.168.1.8"));

            DataTable TableTest = MADmemory.ReadTable("mac_id");

            Console.WriteLine();

            MADmemory.AllTables();
     *
     * */

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
        public void ConnectDB(string DBname)
        {
            //_dbConnection = new SQLiteConnection("Data Source=MyDatabase.sqlite;Version=3;");
            _dbConnection = new SQLiteConnection("Data Source=" + DBname + ";Version=3;");
            _dbConnection.Open();
        }

        //disconnect
        public void DisconnectDB()
        {
            _dbConnection.Close();
        }

        public bool TableCheck(string TabelName)
        { 
           // _dbConnection.
            return true;
        }

        //check if table excists, create if not and connect
        public void CreateTable(string TableName, params TableInfo[] tableInfos)
        {
            string sql = "create table if not exists '" + TableName + "' (";
            sql += "id INTEGER PRIMERY KEY AUTO_INCREMENT,";
            for (int i = 0; i < tableInfos.Length; i++)
            {
                sql += tableInfos[i].GetCommand();
                if (i != tableInfos.Length - 1)
                    sql += ",";
            }
            sql += ")";
            
            SQLiteCommand command = new SQLiteCommand(sql, _dbConnection);
          
            command.ExecuteNonQuery();
        }

        public int Insert(string Command)
        {
            SQLiteCommand cmd = new SQLiteCommand(Command, _dbConnection);
            return cmd.ExecuteNonQuery();
        }

        public DataTable Select(string Command)
        {
            SQLiteCommand cmd = new SQLiteCommand(Command, _dbConnection);
            return cmd.ExecuteReader().GetSchemaTable();
        }

        public int Create(string Command)
        {
            SQLiteCommand cmd = new SQLiteCommand(Command, _dbConnection);
            return cmd.ExecuteNonQuery();
        }

        //insert data
        public void InsertToTable(string TableName, params Insert[] insdata)
        {
            // sql = "insert into " + TableName + " (IP, port) values (" + IP + ", " + Port + ");";
            string sql = "INSERT into " + TableName + " (";
            for (int i = 0; i < insdata.Length; i++)
            {
                sql += insdata[i].column;
                if (i != insdata.Length - 1)
                    sql += ",";
            }
            sql += ") VALUES ('";

            for (int i = 0; i < insdata.Length; i++)
            {
                sql += insdata[i].data;
                if (i != insdata.Length - 1)
                    sql += "','";
            }
            sql += "')";

            SQLiteCommand command = new SQLiteCommand(sql, _dbConnection);

            command.ExecuteNonQuery();
        }

        //see all tables
        
        public DataTable AllTables()
        {
            //string sql = "SELECT name FROM '" + DBName + "'_master WHERE type='table'";
            string sql = "SELECT name FROM sqlite_master WHERE type = 'table'";
            SQLiteCommand command = new SQLiteCommand(sql, _dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            DataTable TempResult = new DataTable();
            TempResult.Load(reader);
            return TempResult;
        }
     

        //read entire table
        public DataTable ReadTable(string TableName)
        {
            //string sql = "SELECT * FROM '" + TableName + "' ORDER BY ID DESC";
            string sql = "SELECT * FROM '" + TableName + "'";
            SQLiteCommand command = new SQLiteCommand(sql, _dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            DataTable TempResult = new DataTable();
            TempResult.Load(reader);
            return TempResult;
        }

        //read one result from table
        public DataTable ReadResult(string TableName, int TableID)
        {
            string sql = "SELECT * FROM " + TableName + " WHERE ID='" + TableID + "'";
            SQLiteCommand command = new SQLiteCommand(sql, _dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            DataTable TempResult = new DataTable();
            TempResult.Load(reader);
            return TempResult;
        }


    }
}

       





/*
        public bool CheckTableExists(string TableName)
        {
           bool tableExists = false;
           string query1 = "SELECT name FROM sqlite_master WHERE name ='" + TableName + "';";

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

         //write results in the table
         //  sql = "insert into " + TableName + " (IP, port) values (" + IP + ", " + Port + ");";
        //Public Output[] SelectFromTable(string tablename, ) 






