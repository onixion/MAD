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


namespace MAD.Database
{ 
    public class DB
    {
        private SQLiteConnection _con;

        public DB(string DBname)
        {
            // if file exists
            SQLiteConnection.CreateFile(DBname);
            // connect
            ConnectDB(DBname);

            using (SQLiteCommand _command = new SQLiteCommand(_con))
            {
                // GUIDTable 
                _command.CommandText = "create table if not exists GUIDTable (ID INTEGER auto increment primary key, GUID TEXT);";
                _command.ExecuteNonQuery();

                // DeviceTable
                _command.CommandText = "create table if not exists DeviceTable (ID_NODE integer, HOSTNAME text, IP text, MAC text, ONLINE integer, MEMO1 varchar(5), MEMO2 text);";
                _command.ExecuteNonQuery(); // TODO

                _command.CommandText = "create table if not exists JobTable (ID_NODE integer, ID_JOB integer, JOBNAME integer, JOBTYPE integer, OUTSTATE integer, STARTTIME text, STOPTIME text, DELAYTIME text, OUTDESC text);";
                _command.ExecuteNonQuery();

                _command.CommandText = "create table if not exists JobNameTable (JOB_ID integer auto increment primary key, NAME text);";
                _command.ExecuteNonQuery();

                _command.CommandText = "create table if not exists JobTypeTable (JOBTYPE_ID integer auto increment primary key, JOBTYPE text)";
                _command.ExecuteNonQuery();
                /*
                _command.CommandText = "create table if not exists ProtocolTable (Protocol_ID integer auto increment primary key, Protocol text)";
                _command.ExecuteNonQuery();
                */

                _command.CommandText = "create table if not exists OutStateTable (ID INTEGER auto increment primary key, OUTSTATE TEXT);";
                _command.ExecuteNonQuery();
                
                _command.CommandText = "create table if not exists SummaryTable (ID_NODE integer, STARTTIME text, STOPTIME text, SUCCESS text, AVERAGE_DURATION text, MAX_DURATION text, MIN_DURATION text);";
                _command.ExecuteNonQuery();
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

            SQLiteCommand command = new SQLiteCommand(sql, _con);
            command.ExecuteNonQuery();
            command.Dispose();
        }
    
        //read entire table
        public DataTable ReadTable(string TableName)
        {
            string sql = "SELECT * FROM " + TableName + "";
            SQLiteCommand command = new SQLiteCommand(sql, _con);
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
            SQLiteCommand command = new SQLiteCommand(sql, _con);
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
            SQLiteCommand command = new SQLiteCommand(sql, _con);
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
                        _command.CommandText = "insert into DeviceTable (ID_NODE, HOSTNAME, IP, MAC) values ('" + _int + "', '" + node.name + "', '" + node.ip + "', '" + node.mac + ");";
                        _command.ExecuteNonQuery();
                    }
                    else
                    {
                        // update
                        _command.CommandText = "update DeviceTable set HOSTNAME='" + node.name + "', IP='" + node.ip + "', MAC='" + node.mac + "' where ID_NODE='" + _int + "';";
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

            using (SQLiteCommand _command = new SQLiteCommand(_con))
            {
                //_command.CommandText = "select * from DeviceTable where ID_NODE='" + _id + "';";

                DataTable _table = _command.ExecuteReader().GetSchemaTable();
                DataRowCollection _rows = _table.Rows;

                _command.CommandText = "insert into JobTable (ID_NODE, ID_JOB, JOBNAME, JOBTYPE, OUTSTATE, STARTTIME, STOPTIME, DELAYTIME, OUTDESC) values ('"
                    + GetGUIDID(node.guid.ToString()) + "', '"
                    + GetGUIDID(job.guid.ToString()) + "', '"
                    + GetJobNameID(job.name) + "', '"
                    + GetJobTypeID(job.type.ToString()) + "', '"
                    + GetOutState(job.outp.outState.ToString()) + "', '"
                    + UnixTimestampFromDateTime(job.tStart) + "', '"
                    + UnixTimestampFromDateTime(job.tStop) + "', '"
                    + job.tSpan.Milliseconds.ToString() + "', '";
                    // outdesc
                _command.ExecuteNonQuery();

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

        public int GetOutState(string outstate)
        {
            using (SQLiteCommand _commmand = new SQLiteCommand(_con))
            {
                _commmand.CommandText = "select * from OutStateTable where GUID='" + outstate + "';";

                DataTable _table = _commmand.ExecuteReader().GetSchemaTable();
                DataRowCollection _rows = _table.Rows;

                if (_rows.Count > 0)
                {
                    DataRow _row = _rows[0];
                    return (int)_row[0];
                }
                else
                    throw new Exception("OutState not found!");
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

        public int GetJobTypeID(string typename)
        {
            using (SQLiteCommand _commmand = new SQLiteCommand(_con))
            {
                _commmand.CommandText = "select * from JobTypeTable where JOBTYPE='" + typename + "';";

                DataTable _table = _commmand.ExecuteReader().GetSchemaTable();
                DataRowCollection _rows = _table.Rows;

                if (_rows.Count > 0)
                {
                    DataRow _row = _rows[0];
                    return (int)_row[0];
                }
                else
                    throw new Exception("JOBTYPE not found!");
            }
        }

        public void InsertOnline(int online, int nodeID)
        {
            using (SQLiteCommand _command = new SQLiteCommand(_con))
            {
                _command.CommandText = "update DeviceTable set ONLINE='" + online + "' where ID_NODE='" + nodeID + "';";
                _command.ExecuteNonQuery();
            }
        }

        public void InsertMemo1(string memo, int nodeID)
        {
            using (SQLiteCommand _command = new SQLiteCommand(_con))
            {
                _command.CommandText = "update DeviceTable set MEMO1='" + memo + "' where ID_NODE='" + nodeID + "';";
                _command.ExecuteNonQuery();
            }
        }

        public void InsertMemo2(string memo, int nodeID)
        {
            using (SQLiteCommand _command = new SQLiteCommand(_con))
            {
                _command.CommandText = "update DeviceTable set MEMO2='" + memo + "' where ID_NODE='" + nodeID + "';";
                _command.ExecuteNonQuery();
            }
        }
        
        // ---

        private static DateTime TimeFromUnixTimestamp(int unixTimestamp)
        {
            DateTime unixYear0 = new DateTime(1970, 1, 1);
            long unixTimeStampInTicks = unixTimestamp * TimeSpan.TicksPerSecond;
            DateTime dtUnix = new DateTime(unixYear0.Ticks + unixTimeStampInTicks);
            return dtUnix;
        }

        public static long UnixTimestampFromDateTime(DateTime date)
        {
            long unixTimestamp = date.Ticks - new DateTime(1970, 1, 1).Ticks;
            unixTimestamp /= TimeSpan.TicksPerSecond;
            return unixTimestamp;
        }

        public void Dispose()
        {
            DisconnectDB();
        }
    }
}