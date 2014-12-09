using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Data.Common;
using System.Data.SQLite; //include SQLite database library
using System.IO;  
using System.Data;

using MAD.JobSystemCore;
using MAD.Logging;

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
        private SQLiteConnection _con;

        public DB(string DBname)
        {
            // ob datei existiert
            SQLiteConnection.CreateFile(DBname);
            // verbinden
            ConnectDB(DBname);

            using (SQLiteCommand _command = new SQLiteCommand(_con))
            {
                // GUIDTable 
                _command.CommandText = "create table if not exists GUIDTable (ID INTEGER auto increment primary key, GUID TEXT);";
                _command.ExecuteNonQuery();

                // DeviceTable
                _command.CommandText = "";
                _command.ExecuteNonQuery(); // TODO
            }
        }

        private void ConnectDB(string DBname)
        {
            _con = new SQLiteConnection("Data Source=" + DBname);
            _con.Open();
        }

        private void DisconnectDB()
        {
            _con.Close();
        }

        /*
        public void CreateDeviceTable()
        {
            string sql = "CREATE TABLE IF NOT EXISTS Device_Table ( GUID TEXT, HOSTNAME TEXT, IP TEXT, MAC TEXT, Online INTEGER, Memo1 VARCHAR(5), Memo2 TEXT);";

            SQLiteCommand command = new SQLiteCommand(sql, _con);
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
        */
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

        //---

        public DataRowCollection Select(string sql)
        {
            using (SQLiteCommand _command = new SQLiteCommand(_con))
            {
                _command.CommandText = sql;
                return _command.ExecuteReader().GetSchemaTable().Rows;
            }
        }

        public void Insert(string sql)
        {
            using (SQLiteCommand _command = new SQLiteCommand(_con))
            {
                _command.CommandText = sql;
                _command.ExecuteNonQuery();
            }
        }

        // ---

        public void InsertNode(JobNode node)
        {
            try
            {
                InsertGUID(node.guid.ToString());

                int _int = GetGUIDID(node.guid.ToString()); // <- NODE ID
                using (SQLiteCommand _command = new SQLiteCommand(_con))
                {
                    _command.CommandText = "select * from DeviceTable where NODEID='" + _int + "';";

                    DataTable _table = _command.ExecuteReader().GetSchemaTable();
                    DataRowCollection _rows = _table.Rows;

                    if (_rows.Count == 0)
                    {
                        // make new datarow
                        _command.CommandText = "insert into DeviceTable (NODEID, ...) values ('" + _int + "', '" + node.name + "', ');";
                        _command.ExecuteNonQuery();
                    }
                    else
                    {
                        // update
                        _command.CommandText = "update into DeviceTable (...) values ('" + node.name + "', ');";
                        _command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Log("(DB) SQL-Error: " + e.Message, Logger.MessageType.ERROR);
            }
        }

        public void InsertJob(JobNode node, Job job)
        {
            InsertJobName(job.name);

            int _id = GetJobNameID(job.name);
            using (SQLiteCommand _command = new SQLiteCommand(_con))
            {
                _command.CommandText = "select * from DeviceTable where NODEID='';";
                
            }
        }

        public bool InsertGUID(string guid)
        {
            using (SQLiteCommand _command = new SQLiteCommand(_con))
            {
                _command.CommandText = "select * from GUIDTable where GUID='" + guid + "';";

                DataTable _table = _command.ExecuteReader().GetSchemaTable();
                DataRowCollection _rows = _table.Rows;

                if (_rows.Count == 0)
                {
                    _command.CommandText = "insert into GUIDTable (GUID) values ('" + guid + "');";
                    _command.ExecuteNonQuery();
                    return true;
                }
                else
                    return false;
            }
        }

        public int GetGUIDID(string guid)
        {
            using (SQLiteCommand _commmand = new SQLiteCommand(_con))
            {
                _commmand.CommandText = "select * from GUIDTable where GUID='" + guid + "';";

                DataTable _table = _commmand.ExecuteReader().GetSchemaTable();
                DataRowCollection _rows = _table.Rows;

                if (_rows.Count > 0)
                {
                    DataRow _row = _rows[0];
                    return (int)_row[0];
                }
                else
                    throw new Exception("GUID not found!");
            }
        }

        public bool InsertJobName(string name)
        {
            using (SQLiteCommand _commmand = new SQLiteCommand(_con))
            {
                _commmand.CommandText = "select * from JobNameTable where JOBNAME='" + name + "';";

                DataTable _table = _commmand.ExecuteReader().GetSchemaTable();
                DataRowCollection _rows = _table.Rows;

                if (_rows.Count == 0)
                {
                    _commmand.CommandText = "insert into JobNameTable (JOBNAME) values ('" + name + "');";
                    _commmand.ExecuteNonQuery();
                    return true;
                }
                else
                    return false;
            }
        }

        public int GetJobNameID(string name)
        {
            using (SQLiteCommand _commmand = new SQLiteCommand(_con))
            {
                _commmand.CommandText = "select * from JobNameTable where JOBNAME='" + name + "';";

                DataTable _table = _commmand.ExecuteReader().GetSchemaTable();
                DataRowCollection _rows = _table.Rows;

                if (_rows.Count > 0)
                {
                    DataRow _row = _rows[0];
                    return (int)_row[0];
                }
                else
                    throw new Exception("JOBNAME not found!");
            }
        }

        // ---

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
