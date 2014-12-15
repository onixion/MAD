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
            if(!File.Exists(DBname))
                SQLiteConnection.CreateFile(DBname);
            // connect
            ConnectDB(DBname);

            using (SQLiteCommand _command = new SQLiteCommand(_con))
            {
                // DeviceTable
                //_command.CommandText = "create table if not exists DeviceTable (ID_NODE integer, HOSTNAME text, IP text, MAC text, ONLINE integer, MEMO1 varchar(5), MEMO2 text);";
                //_command.ExecuteNonQuery(); // TODO

                // GUIDTable 
                _command.CommandText = "create table if not exists GUIDTable (ID INTEGER PRIMARY KEY AUTOINCREMENT, GUID TEXT);";
                _command.ExecuteNonQuery();

                // HostTable
                _command.CommandText = "create table if not exists HostTable (ID integer PRIMARY KEY AUTOINCREMENT, HOST text);";
                _command.ExecuteNonQuery();

                // IPTable
                _command.CommandText = "create table if not exists IPTable (ID integer PRIMARY KEY AUTOINCREMENT, IP text);";
                _command.ExecuteNonQuery();

                // MACTable
                _command.CommandText = "create table if not exists MACTable (ID integer PRIMARY KEY AUTOINCREMENT, MAC text);";
                _command.ExecuteNonQuery();

                // JobNameTable
                _command.CommandText = "create table if not exists JobNameTable (ID integer PRIMARY KEY AUTOINCREMENT, JOBNAME text);";
                _command.ExecuteNonQuery();

                // JobTypeTable
                _command.CommandText = "create table if not exists JobTypeTable (ID integer PRIMARY KEY AUTOINCREMENT, JOBTYPE text)";
                _command.ExecuteNonQuery();

                // ProtocolTable
                _command.CommandText = "create table if not exists ProtocolTable (ID integer PRIMARY KEY AUTOINCREMENT, PROTOCOL text)";
                _command.ExecuteNonQuery();

                // OutStateTable
                _command.CommandText = "create table if not exists OutStateTable (ID integer PRIMARY KEY AUTOINCREMENT, OUTSTATE text);";
                _command.ExecuteNonQuery();

                // MemoTable
                _command.CommandText = "create table if not exists MemoTable (ID integer PRIMARY KEY AUTOINCREMENT, MEMO1 text, MEMO2 text);";
                _command.ExecuteNonQuery();

                // JobTable
                _command.CommandText = "create table if not exists JobTable (" + 
                    "ID_NODE integer, " +
                    "ID_HOST integer, " +
                    "ID_IP integer, " +
                    "ID_MAC integer, " +
                    "ID_JOB integer, " + 
                    "ID_JOBNAME integer, " +
                    "ID_JOBTYPE integer, " +
                    "ID_PROTOCOL integer, " + 
                    "ID_OUTSTATE integer, " + 
                    "STARTTIME text, " +
                    "STOPTIME text, " +
                    "DELAYTIME text, " +
                    "OUTDESC text);";
                _command.ExecuteNonQuery();

                // SummaryTable
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

        public void InsertJob(JobNode node, Job job)
        {
            InsertIDNode(node.guid.ToString());
            InsertIDHost(node.name);
            InsertIDIP(node.ip.ToString());
            InsertIDMac(node.mac.ToString());

            InsertIDJob(job.guid.ToString());
            InsertIDJobName(job.name);
            InsertIDJobType(job.type.ToString());
            InsertIDProtocol(job.type.ToString());
            InsertIDOutState(job.outp.outState.ToString());

            int _idNode = GetIDNode(node.guid.ToString());
            int _idHost = GetIDHost(node.name);
            int _idIP = GetIDIP(node.ip.ToString());
            int _idMAC = GetIDMac(node.mac.ToString());

            int _idJob = GetIDJob(job.guid.ToString());
            int _idJobName = GetIDJobName(job.name);
            int _idJobType = GetIDJobType(job.type.ToString());
            int _idProtocol = GetIDProtocol(job.prot.ToString());
            int _idOutState = GetIDOutState(job.outp.outState.ToString());

            using (SQLiteCommand _command = new SQLiteCommand(_con))
            {
                _command.CommandText = "insert into JobTable (ID_NODE, ID_HOST, ID_IP, ID_MAC, ID_JOB, ID_JOBNAME, ID_JOBTYPE, ID_PROTOCOL, ID_OUTSTATE, STARTTIME, STOPTIME, DELAYTIME) values ('"
                    + _idNode + "', '"
                    + _idHost + "', '"
                    + _idIP + "', '"
                    + _idMAC + "', '"
                    + _idJob + "', '"
                    + _idJobName + "', '"
                    + _idJobType + "', '"
                    + _idProtocol + "', '"
                    + _idOutState + "', '"
                    + job.tStart.ToString() + "', '"
                    + job.tStop.ToString() + "', '"
                    + job.tSpan.Milliseconds.ToString() + "');";
                    // outdesc
                _command.ExecuteNonQuery();
            }
        }

        // ------------------------------------------------------------------
        // UNIVERSELL

        public bool Insert(string tablename, string column, string value)
        {
            using (SQLiteCommand _command = new SQLiteCommand(_con))
            {
                _command.CommandText = "select * from " + tablename + " where " + column + "='" + value + "';";

                using (SQLiteDataReader _reader = _command.ExecuteReader())
                {
                    if (!_reader.Read())
                    {
                        _reader.Close();
                        _command.CommandText = "insert into " + tablename + " (" + column + ") values ('" + value + "');";
                        _command.ExecuteNonQuery();
                        return true;
                    }
                }
                return false;
            }
        }

        public int GetID(string tablename, string column, string pattern)
        {
            using (SQLiteCommand _commmand = new SQLiteCommand(_con))
            {
                _commmand.CommandText = "select * from " + tablename + " where " + column + "='" + pattern + "';";

                using (SQLiteDataReader _reader = _commmand.ExecuteReader())
                {
                    if (_reader.Read())
                        return Convert.ToInt32((Int64)_reader["ID"]);
                    else
                        return 0;
                }
            }
        }

        // ------------------------------------------------------------------

        #region Get ID node

        public int GetIDNode(string guid)
        {
            return GetID("GUIDTable", "GUID", guid);
        }

        public int GetIDHost(string host)
        {
            return GetID("HostTable", "HOST", host);
        }

        public int GetIDIP(string ip)
        {
            return GetID("IPTable", "IP", ip);
        }

        public int GetIDMac(string mac)
        {
            return GetID("MACTable", "MAC", mac);
        }

        /*
        public int GetIDOnline(string online)
        {
            return GetID("OnlineTable", "ONLINE", online);
        }
        */
        #endregion

        #region Get ID job

        public int GetIDJob(string guid)
        {
            return GetID("GUIDTable", "GUID", guid);
        }

        public int GetIDJobName(string name)
        {
            return GetID("JobNameTable", "JOBNAME", name);
        }

        public int GetIDJobType(string typename)
        {
            return GetID("JobTypeTable", "JOBTYPE", typename);
        }

        public int GetIDProtocol(string protocol)
        {
            return GetID("ProtocolTable", "PROTOCOL", protocol);
        }

        public int GetIDOutState(string outstate)
        {
            return GetID("OutStateTable", "OUTSTATE", outstate);
        }

        #endregion

        #region Insert ID node

        public bool InsertIDNode(string guid)
        { 
            if(Insert("GUIDTable", "GUID", guid))
                return true;
            else
                return false;
        }

        public bool InsertIDHost(string host)
        {
            if (Insert("HostTable", "HOST", host))
                return true;
            else
                return false;
        }

        public bool InsertIDIP(string ip)
        {
            if (Insert("IPTable", "IP", ip))
                return true;
            else
                return false;
        }

        public bool InsertIDMac(string mac)
        { 
            if(Insert("MACTable", "MAC", mac))
                return true;
            else
                return false;
        }

        /*
        public bool InsertIDOnline(string online)
        {
            if (Insert("OnlineTable", "ONLINE", online))
                return true;
            else
                return false;
        }
        */
        #endregion

        #region Insert ID job

        public bool InsertIDJob(string guid)
        {
            if (Insert("GUIDTable", "GUID", guid))
                return true;
            else
                return false;
        }

        public bool InsertIDJobName(string name)
        {
            if (Insert("JobNameTable", "JOBNAME", name))
                return true;
            else
                return false;
        }

        public bool InsertIDJobType(string type)
        {
            if (Insert("JobTypeTable", "JOBTYPE", type))
                return true;
            else
                return false;
        }

        public bool InsertIDProtocol(string protocol)
        {
            if (Insert("ProtocolTable", "PROTOCOL", protocol))
                return true;
            else
                return false;
        }

        public bool InsertIDOutState(string outstate)
        {
            if (Insert("OutStateTable", "OUTSTATE", outstate))
                return true;
            else
                return false;
        }

        #endregion

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

        public void Dispose()
        {
            DisconnectDB();
        }
    }
}