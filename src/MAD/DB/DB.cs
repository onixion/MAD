using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Data.Common;
using System.Data.SQLite; //include SQLite database library
using System.IO;  
using System.Data;

/*
 //add to main!!
            DB MADstore = new DB("MAD.sqlite");
            MADstore.ConnectDB("MAD.sqlite");

            MADstore.CreateDeviceTable();
            MADstore.CreateEventTable();
            MADstore.CreateJobTypeTable();
            MADstore.CreateProtocolTable();
            MADstore.CreateStatusTable();
            MADstore.CreateSummaryTable();
 
            MADstore.DisconnectDB();
 */
//how to insert e.g. ... 
/*
MADstore.InsertToTable("Device_Table", new Insert("GUID", "0001"), new Insert("HOST", "YOLO"), new Insert("IP", "192.168.17.17"));
                          Tablename                column    value ...
*/
//reading function will be finished soon


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

        public void CreateDeviceTable()
        {
            string sql = "CREATE TABLE IF NOT EXISTS Device_Table ( GUID INTEGER, HOST VARCHAR(30), IP VARCHAR(30), MAC VARCHAR(30), Memo1 VARCHAR(30), Memo2 VARCHAR(100));";

            SQLiteCommand command = new SQLiteCommand(sql, _dbConnection);

            command.ExecuteNonQuery();
        }

        public void CreateEventTable()
        {
            string sql = "CREATE TABLE IF NOT EXISTS Event_Table ( GUID INTEGER, JOBNAME VARCHAR(30), JOBTYPE VARCHAR(30), PROTOCOL VARCHAR(30),Out_State VARCHAR(30), Discription VARCHAR(100), Start_Time VARCHAR(30), Stop_Time VARCHAR(30), Delay_Time VARCHAR(30), Custom1 VARCHAR(30), Custom2 INTEGER);";

            SQLiteCommand command = new SQLiteCommand(sql, _dbConnection);

            command.ExecuteNonQuery();
        }

        public void CreateJobTypeTable()
        {
            string sql = "CREATE TABLE IF NOT EXISTS Job_Type_Table ( ID INTEGER, JOBTYPE VARCHAR(30));";
            SQLiteCommand command = new SQLiteCommand(sql, _dbConnection);

            command.ExecuteNonQuery();
        }

        public void CreateProtocolTable()
        {
            string sql = "CREATE TABLE IF NOT EXISTS Protocol_Table ( ID INTEGER, Protocol VARCHAR(30));";
            SQLiteCommand command = new SQLiteCommand(sql, _dbConnection);

            command.ExecuteNonQuery();
        }

        public void CreateStatusTable()
        {
            string sql = "CREATE TABLE IF NOT EXISTS Status_Table ( GUID INTEGER, Online INTEGER, Time_of_Execution VARCHAR(30));";
            SQLiteCommand command = new SQLiteCommand(sql, _dbConnection);

            command.ExecuteNonQuery();
        }


        public void CreateSummaryTable()
        {
            string sql = "CREATE TABLE IF NOT EXISTS Summary_Table ( GUID INTEGER, DATE VARCHAR(30), JOBTYPE INTEGER, PROTOCOL INTEGER, Successful_Outstate_[%] INTEGER, Average_Delay_Time VARCHAR(30), Online_[%] INTEGER);";
            SQLiteCommand command = new SQLiteCommand(sql, _dbConnection);

            command.ExecuteNonQuery();
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





//at the moment useless
/*
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
*/





