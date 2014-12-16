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
        private static readonly DateTime _timeconst =  new DateTime (2000, 1, 1);
        public DB(string DBname)
        {
            if(!File.Exists(DBname))
                SQLiteConnection.CreateFile(DBname);

            Connect(DBname);

            using (SQLiteCommand _command = new SQLiteCommand(_con))
            {
                // GUIDNodeTable 
                _command.CommandText = "create table if not exists GUIDNodeTable (ID INTEGER PRIMARY KEY AUTOINCREMENT, GUID_NODE TEXT);";
                _command.ExecuteNonQuery();

                // GUIDJobTable 
                _command.CommandText = "create table if not exists GUIDJobTable (ID INTEGER PRIMARY KEY AUTOINCREMENT, GUID_JOB TEXT);";
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
                _command.CommandText = "create table if not exists MemoTable (ID, MEMO1 text, MEMO2 text);";
                _command.ExecuteNonQuery();

                // JobTable
                _command.CommandText = "create table if not exists JobTable (" +
                    "ID_RECORD integer PRIMARY KEY AUTOINCREMENT, " +
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
                    "OUTDESC text, " +
                    "ID_MEMO integer);";
                _command.ExecuteNonQuery();

                // SummaryTable
                _command.CommandText = "create table if not exists SummaryTable (ID_NODE integer, STARTTIME text, STOPTIME text, SUCCESS text, AVERAGE_DURATION text, MAX_DURATION text, MIN_DURATION text);";
                _command.ExecuteNonQuery();
            }
        }

        private void Connect(string DBname)
        {
            _con = new SQLiteConnection("Data Source=" + DBname);
            _con.Open();
        }

        private void Disconnect()
        {
            _con.Close();
        }

        public void AddMemoToNode(string guid, string memo1, string memo2, int id)
        {
            InsertIDNode(guid); //saftyinstruction
            int _node_id = GetIDNode(guid);

            if(Insert)
            {
                using (SQLiteCommand command = new SQLiteCommand("insert into MemoTable (MEMO1, MEMO2) values (" + memo1 + ", " + memo2 + ");" + _con))
                    command.ExecuteNonQuery();
            }
            else
            {
                int _node_id = GetIDNode(guid);
                using (SQLiteCommand command = new SQLiteCommand("update MemoTable set MEMO1 = " + memo1 + ", MEMO2  " + memo2 + ") where ID = " + id + ";" + _con))
                    command.ExecuteNonQuery();                
            }
        }

        public DataTable ReadTable(string TableName)
        {
            string sql = "SELECT * FROM " + TableName + ";";
            SQLiteCommand command = new SQLiteCommand(sql, _con);
            SQLiteDataReader reader = command.ExecuteReader();
            DataTable TempResult = new DataTable();
            command.Dispose();
            TempResult.Load(reader);
            return TempResult;  
        }

        public DataTable ReadJobs(string limitcommand)
        {
            string sql = "select " +
                         "ID_RECORD, " +
                         "GUIDNodeTable.GUID_NODE, " +
                         "HostTable.HOST, " +
                         "IPTable.IP, " +
                         "MacTable.MAC, " +
                         "GUIDJobTable.GUID_JOB, " +
                         "JobNameTable.JOBNAME, " +
                         "JobTypeTable.JOBTYPE, " +
                         "ProtocolTable.PROTOCOL, " +
                         "OutStateTable.OUTSTATE, " +
                         "STARTTIME, " +
                         "STOPTIME, " +
                         "DELAYTIME, " +
                         "MemoTable.MEMO1, " +
                         "MemoTable.MEMO2 " +
                         "from JobTable " +
                         "inner join GUIDNodeTable on JobTable.ID_NODE = GUIDNodeTable.ID " +
                         "inner join HostTable on JobTable.ID_HOST = HostTable.ID " +
                         "inner join IPTable on JobTable.ID_IP = IPTable.ID " +
                         "inner join MACTable on JobTable.ID_MAC = MACTable.ID " +
                         "inner join GUIDJobTable on JobTable.ID_JOB = GUIDJobTable.ID " +
                         "inner join JobNameTable on JobTable.ID_JOBNAME = JobNameTable.ID " +
                         "inner join JobTypeTable on JobTable.ID_JOBTYPE = JobTypeTable.ID " +
                         "inner join ProtocolTable on JobTable.ID_PROTOCOL = ProtocolTable.ID " +
                         "inner join OutStateTable on JobTable.ID_OUTSTATE = OutStateTable.ID " +
                         "left join MemoTable on JobTable.ID_MEMO = MemoTable.ID " + limitcommand + ";";

            using (SQLiteCommand command = new SQLiteCommand(sql, _con))
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                DataTable TempResult = new DataTable();
                TempResult.Load(reader);
                return TempResult;
            }
        }

        public DataTable ReadTables()
        {
            string sql = "SELECT name FROM sqlite_master WHERE type='table' ORDER BY name;";
            using (SQLiteCommand command = new SQLiteCommand(sql, _con))
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                DataTable TempResult = new DataTable();
                TempResult.Load(reader);
                return TempResult;
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
            InsertIDProtocol(job.prot.ToString());
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
                _command.CommandText = "insert into JobTable (ID_NODE, ID_HOST, ID_IP, ID_MAC, ID_JOB, ID_JOBNAME, ID_JOBTYPE, ID_PROTOCOL, ID_OUTSTATE, STARTTIME, STOPTIME, DELAYTIME, OUTDESC) values ('"
                    + _idNode + "', '"
                    + _idHost + "', '"
                    + _idIP + "', '"
                    + _idMAC + "', '"
                    + _idJob + "', '"
                    + _idJobName + "', '"
                    + _idJobType + "', '"
                    + _idProtocol + "', '"
                    + _idOutState + "', '"
                    + DateTimToMADTimestamp(job.tStart) + "', '"
                    + DateTimToMADTimestamp(job.tStop) + "', '"
                    + job.tSpan.Milliseconds.ToString() + "', '"
                    + job.outp.GetStrings() + "');";
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
            return GetID("GUIDNodeTable", "GUID_NODE", guid);
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

        #endregion

        #region Get ID job

        public int GetIDJob(string guid)
        {
            return GetID("GUIDJobTable", "GUID_JOB", guid);
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
            if(Insert("GUIDNodeTable", "GUID_NODE", guid))
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

        public bool InsertIDMemo(string mac)
        {
            if (Insert("MemoTable", "", mac))
                return true;
            else
                return false;
        }

        #endregion

        #region Insert ID job

        public bool InsertIDJob(string guid)
        {
            if (Insert("GUIDJobTable", "GUID_JOB", guid))
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
            if(Insert("ProtocolTable", "PROTOCOL", protocol))
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

        #region timstamp

        private static DateTime MADTimestampToDateTime(Int64 MADTimeStamp)
        {
            return _timeconst.AddTicks(MADTimeStamp);
        }

        public static Int64 DateTimToMADTimestamp(DateTime date)
        {
            return date.Ticks - _timeconst.Ticks;
        }
        #endregion

        public void Dispose()
        {
            Disconnect();
        }
    }
}