using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Data.Common;
using System.Data.SQLite; //include SQLite database library
using System.IO;  
using System.Data;

using MAD.JobSystemCore;

/*
 //add to main!!
            DB MADstore = new DB("MAD.sqlite");
            MADstore.ConnectDB("MAD.sqlite");
 
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


    public class DB : IDisposable // <- this is needed when the DB gets destroyed at the end of program
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

            CreateJobNameTable(); //this one is not necessary ... makes everthing more complicated
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

        private void CreateDeviceTable()
        {
            string sql = "CREATE TABLE IF NOT EXISTS Device_Table ( GUID TEXT, HOSTNAME TEXT, IP TEXT, MAC TEXT, Online INTEGER, Memo1 VARCHAR(5), Memo2 TEXT);";

            SQLiteCommand command = new SQLiteCommand(sql, _dbConnection);
            command.ExecuteNonQuery();
            command.Dispose();
        }

        private void CreateEventTable()
        {
            string sql = "CREATE TABLE IF NOT EXISTS Event_Table ( GUID TEXT, GUIDdt TEXT, JOBNAME INTEGER, JOBTYPE INTEGER, PROTOCOL INTEGER, Success INTEGER, StartTime TEXT, StopTime TEXT, DelayTime TEXT, Custom1 TEXT, Custom2 INTEGER);";

            SQLiteCommand command = new SQLiteCommand(sql, _dbConnection);
            command.ExecuteNonQuery();
            command.Dispose();
        }

        private void CreateJobTypeTable()
        {
            string sql = "CREATE TABLE IF NOT EXISTS Job_Type_Table ( ID INTEGER PRIMARY KEY AUTOINCREMENT, JobType TEXT);";

            SQLiteCommand command = new SQLiteCommand(sql, _dbConnection);
            command.ExecuteNonQuery();
            command.Dispose();
        }

        private void CreateJobNameTable()
        {
            string sql = "CREATE TABLE IF NOT EXISTS Job_Name_Table ( ID INTEGER PRIMARY KEY AUTOINCREMENT, JobNames TEXT);";

            SQLiteCommand command = new SQLiteCommand(sql, _dbConnection);
            command.ExecuteNonQuery();
            command.Dispose();
        }

        private void CreateProtocolTable()
        {
            string sql = "CREATE TABLE IF NOT EXISTS Protocol_Table ( ID INTEGER PRIMARY KEY AUTOINCREMENT, Protocol TEXT);";

            SQLiteCommand command = new SQLiteCommand(sql, _dbConnection);
            command.ExecuteNonQuery();
            command.Dispose();
        }

        void CreateSummaryTable()
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

        public void InsertNode(JobNode node)
        {

            
        }

        public void InsertJob(JobNode node, Job job)
        {

        }

        public void Update(string TableName, string GUID, string IP)
        {
            string sql = "UPDATE Device_Table SET IP = '" + IP + "' WHERE GUID = '" + GUID + "'";
        }


        static class QUERYHELPER
        {
            public const string READ_TABLE = "SELECT * FROM {0}";
            public const string READ_RESULT = "SELECT * FROM {0} WHERE ID='{1}'";
            public const string READ_EVENTS = "SELECT * FROM event_Table INNER JOIN job_Name_Table ON event_Table.JOBNAME = job_Name_Table.ID INNER JOIN job_Type_Table ON event_Table.JOBTYPE = job_Type_Table.ID INNER JOIN protocol_Table ON event_Table.PROTOCOL = protocol_Table.ID;";
            public const string READ_EVENT = "SELECT * FROM event_Table INNER JOIN job_Name_Table ON event_Table.JOBNAME = job_Name_Table.ID INNER JOIN job_Type_Table ON event_Table.JOBTYPE = job_Type_Table.ID INNER JOIN protocol_Table ON event_Table.PROTOCOL = protocol_Table.ID; WHERE GUID='{1}'";
            public const string DELETE_CONTENT = "DELETE * FROM Event_Table";
        }

        public DataTable ReadTable(string tableName)
        {
            return readDT(String.Format(QUERYHELPER.READ_TABLE, tableName));
        }


        public DataTable ReadResult(string tableName, int tableId)
        {
            return readDT(String.Format(QUERYHELPER.READ_RESULT, tableName, tableId));

        }

        public DataTable ReadEvents()
        {
            return readDT(QUERYHELPER.READ_EVENTS);
        }

        public DataTable ReadEvent()
        {
            return readDT(QUERYHELPER.READ_EVENTS);
        }
        /*
    public DataTable DoSomething(string tableName, string SQLOrder)
    {
        return do(String.Format(SQLOrder, tableName));
    }*/


        private DataTable readDT(string sql)
        {
            DataTable result = new DataTable();
            using (SQLiteCommand command = new SQLiteCommand(sql, _dbConnection))
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                result.Load(reader);
            }
            return result;
        }

        /*
        public void DataTable DeleteContent(string tableName)
        {
            deleteDT(String.Format(QUERYHELPER.DELETE_CONTENT, tableName));
        }
    

        private DataTable deleteDT(string sql)
        {
            DataTable result = new DataTable();
            using (SQLiteCommand command = new SQLiteCommand(sql, _dbConnection))
        }
     
        public void DataTable DoSomething(string tableName)
        {
            do(String.Format(QUERYHELPER.DELETE_CONTENT, tableName));
        }
     
         private DataTable doDT(string sql)
        {
            DataTable result = new DataTable();
            using (SQLiteCommand command = new SQLiteCommand(sql, _dbConnection))
        }
         */

        public void Dispose()
        {
            DisconnectDB();
        }

        private static DateTime TimeFromUnixTimestamp(int unixTimestamp)
        {
            DateTime unixYear0 = new DateTime(1970, 1, 1);
            long unixTimeStampInTicks = unixTimestamp * TimeSpan.TicksPerSecond;
            DateTime dtUnix = new DateTime(unixYear0.Ticks + unixTimeStampInTicks);
            return dtUnix;
        }
    }
}







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
