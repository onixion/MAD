using System;
using System.Collections.Generic;
using System.IO;  
using System.Data;
using System.Data.SQLite;

using MAD.JobSystemCore;
using MAD.Logging;

namespace MAD.Database
{ 
    public class DB
    {
        private SQLiteConnection _con;
        private static readonly DateTime _timeconst =  new DateTime (2010, 1, 1);

        public DB(string DBname)
        {
            if (!File.Exists(DBname))
            {
                Console.WriteLine("(DB) No database found at '" + DBname + "'!");
                Console.WriteLine("(DB) Creating new one.");
                SQLiteConnection.CreateFile(DBname);
            }

            Console.WriteLine("(DB) Connected to database '" + DBname + "'.");
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

                // MemoTable ! ID = ID_NODE
                _command.CommandText = "create table if not exists MemoTable (ID integer PRIMARY KEY, MEMO1 text, MEMO2 text);";
                _command.ExecuteNonQuery();

                // JobTable
                _command.CommandText = "create table if not exists JobTable (" +
                    "ID integer PRIMARY KEY AUTOINCREMENT, " +
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
                _command.CommandText = "create table if not exists SummaryTable (ID integer PRIMARY KEY AUTOINCREMENT, ID_NODE integer, STARTTIME text, STOPTIME text, SUCCESSRATE text, AVERAGE_DURATION text, MAX_DURATION text, MIN_DURATION text);";
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

        #region basic methods

        private int InsertCommand(string command)
        {
            using (SQLiteCommand _command = new SQLiteCommand(_con))
            {
                _command.CommandText = command;
                return _command.ExecuteNonQuery();
            }
        }

        private DataTable SelectCommand(string command)
        {
            using (SQLiteCommand _command = new SQLiteCommand(_con))
            {
                _command.CommandText = command;

                using (SQLiteDataReader _reader = _command.ExecuteReader())
                {
                    DataTable _table = new DataTable();
                    _table.Load(_reader);
                    return _table;
                }
            }
        }

        #endregion

        #region read tables

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

        public DataTable SelectAllFromTable(string TableName)
        {
            using(SQLiteCommand command = new SQLiteCommand("select * from " + TableName + ";", _con))
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                DataTable TempResult = new DataTable();
                TempResult.Load(reader);
                return TempResult;
            }
        }

        #endregion

        #region Get joined tables

        public DataTable GetJobTable(string subcommand)
        {
            string sql = "select " +
                         "JobTable.ID, " +
                         "GUIDNodeTable.GUID_NODE, " +
                         "HostTable.HOST, " +
                         "IPTable.IP, " +
                         "MacTable.MAC, " +
                         "GUIDJobTable.GUID_JOB, " +
                         "JobNameTable.JOBNAME, " +
                         "JobTypeTable.JOBTYPE, " +
                         "ProtocolTable.PROTOCOL, " +
                         "OutStateTable.OUTSTATE, " +
                         "JobTable.STARTTIME, " +
                         "JobTable.STOPTIME, " +
                         "JobTable.DELAYTIME, " +
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
                         "left join MemoTable on JobTable.ID_NODE = MemoTable.ID " + subcommand + ";"; ;

            using (SQLiteCommand command = new SQLiteCommand(sql, _con))
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                DataTable TempResult = new DataTable();
                TempResult.Load(reader);
                return TempResult;
            }
        }

        public DataTable GetSummaryTable(string subcommand)
        {
            string sql = "select " +
             "SummaryTable.ID, " +
             "GUIDNodeTable.GUID_NODE, " +
             "SummaryTable.STARTTIME, " +
             "SummaryTable.STOPTIME, " +
             "SummaryTable.SUCCESSRATE, " + 
             "SummaryTable.AVERAGE_DURATION, " +
             "SummaryTable.MAX_DURATION, " +
             "SummaryTable.MIN_DURATION " +
             "from SummaryTable " +
             "inner join GUIDNodeTable on SummaryTable.ID_NODE = GUIDNodeTable.ID " +
             "" + subcommand + ";";

            using (SQLiteCommand command = new SQLiteCommand(sql, _con))
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                DataTable TempResult = new DataTable();
                TempResult.Load(reader);
                return TempResult;
            }
        }

        #endregion

        #region job method

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
                    + DateTimeToMADTimestamp(job.tStart) + "', '"
                    + DateTimeToMADTimestamp(job.tStop) + "', '"
                    + job.tSpan.Milliseconds.ToString() + "', '"
                    + job.outp.GetStrings() + "');";
                _command.ExecuteNonQuery();
            }
        }

        public int DeleteAllFromJobTable()
        {
            return InsertCommand("delete from JobTable;");
        }

        #endregion

        #region job / node database operations

        public bool InsertID(string tablename, string column, string value)
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
            if(InsertID("GUIDNodeTable", "GUID_NODE", guid))
                return true;
            else
                return false;
        }

        public bool InsertIDHost(string host)
        {
            if (InsertID("HostTable", "HOST", host))
                return true;
            else
                return false;
        }

        public bool InsertIDIP(string ip)
        {
            if (InsertID("IPTable", "IP", ip))
                return true;
            else
                return false;
        }

        public bool InsertIDMac(string mac)
        { 
            if(InsertID("MACTable", "MAC", mac))
                return true;
            else
                return false;
        }

        #endregion

        #region Insert ID job

        public bool InsertIDJob(string guid)
        {
            if (InsertID("GUIDJobTable", "GUID_JOB", guid))
                return true;
            else
                return false;
        }

        public bool InsertIDJobName(string name)
        {
            if (InsertID("JobNameTable", "JOBNAME", name))
                return true;
            else
                return false;
        }

        public bool InsertIDJobType(string type)
        {
            if (InsertID("JobTypeTable", "JOBTYPE", type))
                return true;
            else
                return false;
        }

        public bool InsertIDProtocol(string protocol)
        {
            if(InsertID("ProtocolTable", "PROTOCOL", protocol))
                return true;
            else
                return false;
        }

        public bool InsertIDOutState(string outstate)
        {
            if (InsertID("OutStateTable", "OUTSTATE", outstate))
                return true;
            else
                return false;
        }

        #endregion

        #endregion

        #region summarize methods

        public int[] SummarizeJobTable(DateTime from, DateTime to, long blocksize, bool removedSummarizedData)
        {
            TimeSpan _summarytime = to.Subtract(from);
            if (_summarytime.TotalMilliseconds <= 0)
                throw new Exception("Times are invalid!");

            long _starttimestamp = DB.DateTimeToMADTimestamp(from);
            long _stoptimestamp = DB.DateTimeToMADTimestamp(to);

            // check if summarytime has already been summerized
            using (DataTable _table = SelectCommand("select ID from SummaryTable where " +
                "STARTTIME between " + _starttimestamp + " and " +  _stoptimestamp + 
                " or " +
                "STOPTIME between " + _starttimestamp + " and " + _stoptimestamp + ";"))
            {
                if (_table.Rows.Count > 0)
                    throw new Exception("This time has already been summerized before!");
            }

            int _rowsSummarizedTotal = 0;
            int _rowsWritten = 0;

            long _blockStart = _starttimestamp; // begin of a certain block
            long _blockStop = _starttimestamp + blocksize; // end of a certain block

            while (_blockStop <= _stoptimestamp)
            {
                // get all nodes in block
                Guid[] _nodes = ReadNodes(_blockStart, _blockStop);

                for (int i = 0; i < _nodes.Length; i++)
                {
                    // Now we need to generate one datarow for each node.

                    int _id_node = GetIDNode(_nodes[i].ToString());

                    using (DataTable _table = SelectCommand("select * from JobTable " +
                        "where STARTTIME between " + _blockStart + " and " + _blockStop + " and ID_NODE=" + _id_node + ";"))
                    {
                        // SUCCESSRATE
                        int _buffer = 0;
                        for (int i2 = 0; i2 < _table.Rows.Count; i2++)
                        {
                            using (DataTable _outTable = SelectCommand("select OUTSTATE from " +
                                "OutStateTable where ID=" + Convert.ToString(_table.Rows[i2]["ID_OUTSTATE"]) + ";"))
                            {
                                if ((string)_outTable.Rows[0]["OUTSTATE"] == "Success")
                                    _buffer += 1;
                            }

                            _rowsSummarizedTotal++;
                        }
                        int _successrate = Convert.ToInt32(((decimal)_buffer / (decimal)_table.Rows.Count) * (decimal)100);

                        // AVERAVE_DURATION, MAX_DURATION, MIN_DURATION
                        _buffer = 0;
                        int _average = 0;
                        int _max = 0;
                        int _min = 10000;
                        for (int i2 = 0; i2 < _table.Rows.Count; i2++)
                        {
                            _buffer = (int)(Convert.ToUInt64(_table.Rows[i2]["DELAYTIME"]));

                            if (_buffer > _max)
                                _max = _buffer;
                            if (_buffer < _min)
                                _min = _buffer;

                            _average += _buffer;
                        }
                        _average = _average / _table.Rows.Count;

                        InsertCommand("insert into SummaryTable (ID_NODE, STARTTIME, STOPTIME, " +
                            "SUCCESSRATE, AVERAGE_DURATION, MAX_DURATION, MIN_DURATION) values " +
                            "(" + _id_node + ", " + _blockStart + ", " + _blockStop + ", " + 
                            _successrate + ", " + _average + ", " + _max + ", " + _min + ");");

                        _rowsWritten++;
                    }
                }

                // set pointers to the next block
                _blockStart = _blockStop;
                _blockStop += blocksize;
            }

            if (removedSummarizedData)
            {
                InsertCommand("delete from JobTable where STARTTIME between " + _starttimestamp + " and " + _blockStop + ";");
            }

            return new int[]{_rowsSummarizedTotal, _rowsWritten};
        }

        private Guid[] ReadNodes(long from, long to)
        {
            using (DataTable _table = SelectCommand("select distinct ID_NODE from JobTable " +
                "where STARTTIME between " + from + " and " + to + ";"))
            {
                Guid[] _buffer = new Guid[_table.Rows.Count];
                for (int i = 0; i < _buffer.Length; i++)
                {
                    string _temp = Convert.ToString(_table.Rows[i]["ID_NODE"]);

                    using (DataTable _table2 = SelectCommand("select GUID_NODE from GUIDNodeTable " +
                        "where ID=" + _temp + ";"))
                    {
                        _buffer[i] = Guid.Parse((string)_table2.Rows[0]["GUID_NODE"]);
                    }
                }

                return _buffer;
            }
        }

        public int DeleteFromSummaryTable(DateTime from, DateTime to)
        {
            return InsertCommand("delete from SummaryTable " +
                "where STARTTIME >= " + DateTimeToMADTimestamp(from) + " and " +
                "STOPTIME <= " + DateTimeToMADTimestamp(to) + ";");
        }

        public int DeleteAllFromSummaryTable()
        {
            return InsertCommand("delete from SummaryTable;");
        }

        #endregion

        #region memos

        public bool AddMemoToNode(string guid, string memo1, string memo2)
        {
                InsertIDNode(guid);
                int _nodeID = GetIDNode(guid);

                using (SQLiteCommand _command = new SQLiteCommand(_con))
                {
                        _command.CommandText = "select * from MemoTable where ID='" + _nodeID + "';";
                        using (SQLiteDataReader _reader = _command.ExecuteReader())
                        {
                                if (_reader.Read())
                                {
                                        _reader.Close();
                                        _command.CommandText = "update MemoTable set " +
                                            "MEMO1='" + memo1 + "', " +
                                            "MEMO2='" + memo2 + "' " +
                                            "where ID='" + _nodeID + "';";
                                        _command.ExecuteNonQuery();
                                }
                                else
                                {
                                        _reader.Close();
                                        _command.CommandText = "insert into MemoTable (ID, MEMO1, MEMO2) values " +
                                            "('" + _nodeID + "', '" + memo1 + "', '" + memo2 + "');";
                                        _command.ExecuteNonQuery();
                                }
                        }
                }

                return true;
        }

        public string[] GetMemoFromNode(string guid)
        {
                int _nodeID = GetIDNode(guid);
                if (_nodeID == 0)
                {
                    InsertIDNode(guid);
                    _nodeID = GetIDNode(guid);
                }

                string[] _buffer = new string[2] { "", "" };

                using (SQLiteCommand _command = new SQLiteCommand(_con))
                {
                        _command.CommandText = "select * from MemoTable where ID='" + _nodeID + "';";

                        using (SQLiteDataReader _reader = _command.ExecuteReader())
                        {
                                if (_reader.Read())
                                {
                                        _buffer[0] = (string)_reader["MEMO1"];
                                        _buffer[1] = (string)_reader["MEMO2"];
                                }
                        }
                }

                return _buffer;
        }

        #endregion

        #region timstamp

        // We use our own timestamp. It counts the milliseconds since 1.1.2010.

        public static DateTime MADTimestampToDateTime(Int64 MADTimeStamp)
        {
            return _timeconst.AddMilliseconds(MADTimeStamp);
        }

        public static Int64 DateTimeToMADTimestamp(DateTime date)
        {
            return Convert.ToInt64((date - _timeconst).TotalMilliseconds);
        }

        #endregion

        public void Dispose()
        {
            Disconnect();
        }
    }
}