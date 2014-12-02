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


namespace MAD.Database
{ 
    public class DB
    {
        private string _DBname;
        public SQLiteConnection _dbConnection;

        public DB(string DBname)
        {
            _DBname = DBname;
            SQLiteConnection.CreateFile(_DBname);
            ConnectDB();

            CreateDeviceTable();
            CreateEventTable();
            CreateSummaryTable();

            CreateProtocolTable();
            CreateJobTypeTable();
            CreateJobNameTable();
        }

        //connect to database
        private void ConnectDB()
        {
            _dbConnection = new SQLiteConnection("Data Source=" + _DBname);
            _dbConnection.Open();
        }

        //disconnect
        private void DisconnectDB()
        {
            _dbConnection.Close();
        }

        public void CreateDeviceTable()
        {
            string sql = "CREATE TABLE IF NOT EXISTS Device_Table ( GUID TEXT, HOSTNAME TEXT, IP TEXT, MAC TEXT, Online INTEGER, Memo1 VARCHAR(5), Memo2 TEXT);";

            SQLiteCommand command = new SQLiteCommand(sql, _dbConnection);
            command.ExecuteNonQuery();
            command.Dispose();
        }

        public void CreateEventTable()
        {
            string sql = "CREATE TABLE IF NOT EXISTS Event_Table ( GUID TEXT, JOBNAME INTEGER, JOBTYPE INTEGER, PROTOCOL INTEGER, SUCCESS INTEGER, StartTime TEXT, StopTime TEXT, DelayTime TEXT, Custom1 TEXT, Custom2 INTEGER);";
            
            SQLiteCommand command = new SQLiteCommand(sql, _dbConnection);
            command.ExecuteNonQuery();
            command.Dispose();
        }

        public void CreateJobTypeTable()
        {
            string sql = "CREATE TABLE IF NOT EXISTS Job_Type_Table ( ID INTEGER PRIMARY KEY AUTOINCREMENT, JobType TEXT);";

            SQLiteCommand command = new SQLiteCommand(sql, _dbConnection);
            command.ExecuteNonQuery();
            command.Dispose();
        }

        public void CreateJobNameTable()
        {
            string sql = "CREATE TABLE IF NOT EXISTS Job_Name_Table ( ID INTEGER PRIMARY KEY AUTOINCREMENT, JobNames TEXT);";

            SQLiteCommand command = new SQLiteCommand(sql, _dbConnection);
            command.ExecuteNonQuery();
            command.Dispose();
        }

        public void CreateProtocolTable()
        {
            string sql = "CREATE TABLE IF NOT EXISTS Protocol_Table ( ID INTEGER PRIMARY KEY AUTOINCREMENT, Protocol TEXT);";

            SQLiteCommand command = new SQLiteCommand(sql, _dbConnection);
            command.ExecuteNonQuery();
            command.Dispose();
        }

        public void CreateSummaryTable()
        {
            string sql = "CREATE TABLE IF NOT EXISTS Summary_Table ( GUID TEXT, DATE TEXT, JOBTYPE INTEGER, PROTOCOL INTEGER, OutState INTEGER);";
            
            SQLiteCommand command = new SQLiteCommand(sql, _dbConnection);
            command.ExecuteNonQuery();
            command.Dispose();
        }

        //insert data
        public void InsertToTable(string TableName, params Insert[] insdata)
        {
            // sql = "insert into " + TableName + " (IP, port) values (" + IP + ", " + Port + "); ...";
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
            command.Dispose();
        }
    
        //read entire table
        public DataTable ReadTable(string TableName)
        {
            string sql = "SELECT * FROM " + TableName + "";
            SQLiteCommand command = new SQLiteCommand(sql, _dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            DataTable TempResult = new DataTable();
            command.Dispose();
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
        
        public DataTable ReadEvents()
        {
            //string sql = "SELECT GUID, JobNames FROM " + TableName + " INNER JOIN Job_Name_Table ON Event_Table.JOBNAME = Job_Name_Table.JobNames WHERE GUID='" + TableID + "'";
            //string sql = "SELECT * FROM Job_Name_Table, Job_Type_Table where Job_Name_Table.ID = Job_Type_Table.ID";
            string sql = "SELECT * FROM event_Table INNER JOIN job_Name_Table ON event_Table.JOBNAME = job_Name_Table.ID INNER JOIN job_Type_Table ON event_Table.JOBTYPE = job_Type_Table.ID INNER JOIN protocol_Table ON event_Table.PROTOCOL = protocol_Table.ID;";
            SQLiteCommand command = new SQLiteCommand(sql, _dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            /*
            while (reader.Read())
                Console.WriteLine("ID: " + reader["ID"] + "\tJobname: " + reader["JobNames"] + "\tID: " + reader["ID"] + "\tJobType: " + reader["JobType"]);
            Console.ReadLine();
            command.Dispose();
            */
            DataTable TempResult = new DataTable();
            TempResult.Load(reader);
            return TempResult;
        }

        public void Dispose()
        {
            DisconnectDB();
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

/* =========copy too main:
 
            DB MADstore = new DB("MAD.sqlite");
            MADstore.ConnectDB("MAD.sqlite");

            MADstore.CreateDeviceTable();
            MADstore.CreateEventTable();
            MADstore.CreateJobTypeTable();
            MADstore.CreateProtocolTable();
            MADstore.CreateJobNameTable();
            MADstore.CreateSummaryTable();


            MADstore.InsertToTable("Device_Table",
                        new Insert("GUID", "0001"), 
                        new Insert("HOSTNAME", "Router1"), 
                        new Insert("IP", "192.168.17.17"), 
                        new Insert("MAC", "00-50-56-C0-00-08"), 
                        new Insert("Online", "1"), 
                        new Insert("Memo1","NAT-Router"), 
                        new Insert ("Memo2", "Router for Ineternet-connection"));
            MADstore.InsertToTable("Event_Table", 
                        new Insert("GUID", "0001"), 
                        new Insert("JOBNAME", "0"), 
                        new Insert("JOBTYPE", "0"), 
                        new Insert("PROTOCOL", "0"), 
                        new Insert("Success", "1"), 
                        new Insert("StartTime", "12:20:20:20"), 
                        new Insert("StopTime", "12:20:20:50"), 
                        new Insert("DelayTime", "00:30"), 
                        new Insert("Custom1", "IPStatus"), 
                        new Insert("Custom2", "17"));
            MADstore.InsertToTable("Job_Type_Table", 
                        new Insert("ID", "0"), 
                        new Insert("JobType", "Ping-Request"));
            MADstore.InsertToTable("Job_Type_Table", 
                        new Insert("ID", "1"), 
                        new Insert("JobType", "FTP-Request"));
            MADstore.InsertToTable("Protocol_Table", 
                        new Insert("ID", "0"), 
                        new Insert("Protocol", "TCP"));
            MADstore.InsertToTable("Job_Name_Table", 
                        new Insert("ID", "0"), 
                        new Insert("JobName", "Ping1"));
            MADstore.InsertToTable("Summary_Table", 
                        new Insert("GUID", "0001"), 
                        new Insert("DATE", "12.03.2015"), 
                        new Insert("JOBTYPE", "0"), 
                        new Insert("Protocol", "0"), 
                        new Insert("OutState", "60"));
 */
