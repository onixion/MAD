using System;
using System.Data;
using System.Data.SQLite;

using MAD.JobSystemCore;
using MAD.Database;

namespace MAD.CLICore
{
    public class DBSelectParameters : Command
    {
        public DBSelectParameters()
        {
            oPar.Add(new ParOption("id", "DATAROW-ID", "Show all datarows with this/those id(s).", false, false, new Type[] { typeof(string) }));
            oPar.Add(new ParOption("c", "COUNT-ROWS", "Amount of the last rows to show.", false, false, new Type[] { typeof(int) }));
            oPar.Add(new ParOption("b", "BEGIN-DATE", "Show all datarows since this date.", false, false, new Type[] { typeof(DateTime) }));
            oPar.Add(new ParOption("e", "END-DATE", "Show all datarows before this date.", false, false, new Type[] { typeof(DateTime) }));
            oPar.Add(new ParOption("m", "DISPLAY-MODE", "Show all datarows before this date.", false, false, new Type[] { typeof(string) }));
        }

        public override string Execute(int consoleWidth)
        {
            throw new NotImplementedException();
        }

        protected string GetSQLSubCommand(string tablename, bool joined)
        {
            string _buffer = " ";

            // ID
            if (OParUsed("id"))
            {
                string[] _buffer2 = pars.GetPar("id").argValues[0].ToString().Split(new string[] { "-" }, StringSplitOptions.None);
                int _idMIN = 0;
                int _idMAX = 0;

                if (_buffer2.Length == 2)
                {
                    _idMIN = Convert.ToInt32(_buffer2[0]);
                    _idMAX = Convert.ToInt32(_buffer2[1]);
                }
                else if (_buffer2.Length == 1)
                {
                    _idMIN = Convert.ToInt32(_buffer2[0]);
                    _idMAX = Convert.ToInt32(_buffer2[0]);
                }
                else
                {
                    throw new Exception("Could not parse '" + (string)pars.GetPar("id").argValues[0] + "' into an ID.");
                }

                if(joined)
                    _buffer += "where " + tablename + ".ID>='" + _idMIN + "' and " + tablename + ".ID<='" + _idMAX + "' ";
                else
                    _buffer += "where ID>='" + _idMIN + "' and ID<='" + _idMAX + "' ";
            }

            // BEGIN-DATE & END-DATE
            if (OParUsed("b") && OParUsed("e"))
            {
                if (OParUsed("id"))
                    _buffer += "and ";
                else
                    _buffer += "where ";

                _buffer += "STARTTIME>='" + DB.DateTimeToMADTimestamp((DateTime)pars.GetPar("b").argValues[0]) + "'" +
                    " and STARTTIME<='" + DB.DateTimeToMADTimestamp((DateTime)pars.GetPar("e").argValues[0]) + "' ";
            }
            else
            {
                if (OParUsed("b"))
                {
                    if (OParUsed("id"))
                        _buffer += "and ";
                    else
                        _buffer += "where ";

                    _buffer += "STARTTIME>='" + DB.DateTimeToMADTimestamp((DateTime)pars.GetPar("b").argValues[0]) + "' ";
                }

                if (OParUsed("e"))
                {
                    if (OParUsed("id"))
                        _buffer += "and ";
                    else
                        _buffer += "where ";

                    _buffer += "STARTTIME<='" + DB.DateTimeToMADTimestamp((DateTime)pars.GetPar("e").argValues[0]) + "' ";
                }
            }

            if (OParUsed("c"))
            {
                if(joined)
                    _buffer += "order by " + tablename + ".ID desc limit " + (int)pars.GetPar("c").argValues[0];
                else
                    _buffer += "order by ID desc limit " + (int)pars.GetPar("c").argValues[0];
            }
            else
            {
                if (joined)
                    _buffer += "order by " + tablename + ".ID desc limit 20";
                else
                    _buffer += "order by ID desc limit 20";
            }

            return _buffer;
        }
    }

    public class DBShowTables : DBSelectParameters
    {
        private DB _db;

        public DBShowTables(object[] args)
            :base()
        {
            _db = (DB)args[0];
            oPar.Add(new ParOption("t", "TABLE-NAME", "Name of the table.", false, false, new Type[] { typeof(string) }));
            description = "This command shows you the raw content of a specific table.";
        }

        public override string Execute(int consoleWidth)
        {
            if (!OParUsed("t"))
            {
                using (DataTable _table = _db.ReadTables())
                {
                    DataColumnCollection _columns = _table.Columns;
                    DataRowCollection _rows = _table.Rows;

                    output += "<color><yellow>" + ConsoleTable.GetSplitline(consoleWidth);
                    output += ConsoleTable.FormatStringArray(consoleWidth, DBCLIHelper.GetColumnNames(_columns));
                    output += ConsoleTable.GetSplitline(consoleWidth) + "<color><white>";

                    for (int i = 0; i < _rows.Count; i++)
                        output += ConsoleTable.FormatStringArray(consoleWidth, DBCLIHelper.GetRowValues(_columns, _rows[i]));
                }
            }
            else
            {
                string _dataTable = (string)pars.GetPar("t").argValues[0];

                using (DataTable _table = _db.SelectAllFromTable(_dataTable + GetSQLSubCommand(_dataTable,false)))
                {
                    DataColumnCollection _columns = _table.Columns;
                    DataRowCollection _rows = _table.Rows;

                    output += "<color><yellow>" + ConsoleTable.GetSplitline(consoleWidth);
                    output += ConsoleTable.FormatStringArray(consoleWidth, DBCLIHelper.GetColumnNames(_columns));
                    output += ConsoleTable.GetSplitline(consoleWidth) + "<color><white>";

                    for (int i = 0; i < _rows.Count; i++)
                        output += ConsoleTable.FormatStringArray(consoleWidth, DBCLIHelper.GetRowValues(_columns, _rows[i]));
                }
            }

            return output;
        }
    }

    public class DBJobs : DBSelectParameters
    { 
        private DB _db;

        public DBJobs(object[] args)
            :base()
        {
            _db = (DB)args[0];
            description = "This command shows the joined content of the job-table.";
        }

        public override string Execute(int consoleWidth)
        {
            using (DataTable _table = _db.GetJobTable(GetSQLSubCommand("JobTable", true)))
            {
                DataColumnCollection _columns = _table.Columns;
                DataRowCollection _rows = _table.Rows;

                output += "<color><yellow>" + ConsoleTable.GetSplitline(consoleWidth);
                output += ConsoleTable.FormatStringArray(consoleWidth, DBCLIHelper.GetColumnNames(_columns));
                output += ConsoleTable.GetSplitline(consoleWidth) + "<color><white>";

                for (int i = 0; i < _rows.Count; i++)
                    output += ConsoleTable.FormatStringArray(consoleWidth, DBCLIHelper.GetRowValues(_columns, _rows[i]));

                return output;
            }
        }
    }

    public class DBJobsDelAll : Command
    {
        private DB _db;

        public DBJobsDelAll(object[] args)
        {
            _db = (DB) args[0];
            description = "This command deletes all datarows from the JobTable.";
        }

        public override string Execute(int consoleWidth)
        {
            return "<color><green>" + _db.DeleteAllFromJobTable() + " rows deleted.";
        }
    }

    #region summarize commands

    public class DBSumShow : DBSelectParameters
    {
        private DB _db;

        public DBSumShow(object[] args)
        {
            _db = (DB)args[0];
            description = "This command shows the joined content of the summary-table.";
        }

        public override string Execute(int consoleWidth)
        {
            using (DataTable _table = _db.GetSummaryTable(GetSQLSubCommand("SummaryTable",true)))
            {
                DataColumnCollection _columns = _table.Columns;
                DataRowCollection _rows = _table.Rows;

                output += "<color><yellow>" + ConsoleTable.GetSplitline(consoleWidth);
                output += ConsoleTable.FormatStringArray(consoleWidth, DBCLIHelper.GetColumnNames(_columns));
                output += ConsoleTable.GetSplitline(consoleWidth) + "<color><white>";

                for (int i = 0; i < _rows.Count; i++)
                    output += ConsoleTable.FormatStringArray(consoleWidth, DBCLIHelper.GetRowValues(_columns, _rows[i]));

                return output;
            }
        }
    }

    public class DBSumCreate : Command
    {
        private DB _db;

        public DBSumCreate(object[] args)
        {
            _db = (DB)args[0];

            rPar.Add(new ParOption("b", "BEGIN-DATE", "", false, false, new Type[] { typeof(DateTime) }));
            rPar.Add(new ParOption("e", "END-DATE", "", false, false, new Type[] { typeof(DateTime) }));
            rPar.Add(new ParOption("b", "BLOCK-SIZE", "Size of each block to summarize the jobs to (e.g. 10y, 10w, 10m, 10d, 5h, 15min, ...).", false, false, new Type[] { typeof(string) }));
            oPar.Add(new ParOption("del", "", "Delete the data which have been summarized.", true, false, null)); 
        }

        public override string Execute(int consoleWidth)
        {
            // parsing blocksize
            long _blocksize = 0;
            string _buffer = (string)pars.GetPar("b").argValues[0];
            string[] _buffer2;
            if ((_buffer2 = _buffer.Split(new string[] { "min" }, StringSplitOptions.None)).Length == 2)
            {
                _blocksize = (long)new TimeSpan(0, Convert.ToInt32(_buffer2[0]), 0).TotalMilliseconds;
            }
            else if ((_buffer2 = _buffer.Split(new string[] { "h" }, StringSplitOptions.None)).Length == 2)
            {
                _blocksize = (long)new TimeSpan(Convert.ToInt32(_buffer2[0]), 0, 0).TotalMilliseconds;
            }
            else if ((_buffer2 = _buffer.Split(new string[] { "d" }, StringSplitOptions.None)).Length == 2)
            {
                _blocksize = (long)new TimeSpan(Convert.ToInt32(_buffer2[0]), 0, 0, 0).TotalMilliseconds;
            }
            else if ((_buffer2 = _buffer.Split(new string[] { "w" }, StringSplitOptions.None)).Length == 2)
            {
                _blocksize = (long)new TimeSpan(Convert.ToInt32(_buffer2[0]) * 7, 0, 0, 0).TotalMilliseconds;
            }
            else if ((_buffer2 = _buffer.Split(new string[] { "m" }, StringSplitOptions.None)).Length == 2)
            {
                _blocksize = (long)new TimeSpan(Convert.ToInt32(_buffer2[0]) * 30, 0, 0, 0).TotalMilliseconds;
            }
            else if ((_buffer2 = _buffer.Split(new string[] { "y" }, StringSplitOptions.None)).Length == 2)
            {
                _blocksize = (long)new TimeSpan(Convert.ToInt32(_buffer2[0]) * 365, 0, 0).TotalMilliseconds;
            }
            else
                return "<color><red>Could not parse '" + _buffer + "'!";

            int[] _info = _db.SummarizeJobTable((DateTime)pars.GetPar("s").argValues[0],
                (DateTime)pars.GetPar("e").argValues[0], _blocksize, OParUsed("del") ? true : false);

            output += "<color><yellow>DataRows summerized: <color><white>" + _info[0] + "\n";
            output += "<color><yellow>DataRows written:    <color><white>" + _info[1] + "\n";
            return output;
        }
    }

    public class DBSumDel : Command
    {
        private DB _db;

        public DBSumDel(object[] args)
        {
            _db = (DB)args[0];

            rPar.Add(new ParOption("b", "BEGIN-DATE", "", false, false, new Type[] { typeof(DateTime) }));
            rPar.Add(new ParOption("e", "END-DATE", "", false, false, new Type[] { typeof(DateTime) }));
        }

        public override string Execute(int consoleWidth)
        {
            int _rows = _db.DeleteFromSummaryTable((DateTime)pars.GetPar("s").argValues[0],
                (DateTime)pars.GetPar("e").argValues[0]);
            return "<color><green>" + _rows + " DataRows deleted.";
        }
    }

    public class DBSumDelAll : Command
    {
        private DB _db;

        public DBSumDelAll(object[] args)
        {
            _db = (DB)args[0];
        }

        public override string Execute(int consoleWidth)
        {
            return "<color><green>" + _db.DeleteAllFromSummaryTable() + " rows deleted.";
        }
    }

    #endregion

    #region memo commands

    public class DBMemoAdd : Command
    {
        private DB _db;
        private JobSystem _js;

        public DBMemoAdd(object[] args)
        {
            _js = (JobSystem)args[0];
            _db = (DB)args[1];

            rPar.Add(new ParOption("id", "NODE-ID", "ID of the node.", false, false, new Type[] { typeof(int) }));
            rPar.Add(new ParOption("m1", "MEMO1", "First memo to attach to node.", false, true, new Type[] { typeof(string) }));
            rPar.Add(new ParOption("m2", "MEMO2", "Second memo to attach to node.", false, true, new Type[] { typeof(string) }));
        }

        public override string Execute(int consoleWidth)
        {
            int _id = (int)pars.GetPar("id").argValues[0];
            string _memo1 = "";
            string _memo2 = "";

            object[] args = pars.GetPar("m1").argValues;
            for (int i = 0; i < args.Length; i++)
            {
                _memo1 += (string)args[i] + " ";
            }

            args = pars.GetPar("m2").argValues;
            for (int i = 0; i < args.Length; i++)
            {
                _memo2 += (string)args[i] + " ";
            }

            string _guid = "";

            lock (_js.jsLock)
            {
                JobNode _node = _js.LGetNode(_id);
                _guid = _node.guid.ToString();
            }

            _db.AddMemoToNode(_guid, _memo1, _memo2);

            return "<color><green>DB updated.";
        }
    }

    public class DBMemoShow : Command
    {
        private DB _db;
        private JobSystem _js;

        public DBMemoShow(object[] args)
        {
            _js = (JobSystem)args[0];
            _db = (DB)args[1];

            rPar.Add(new ParOption("id", "NODE-ID", "ID of the node.", false, false, new Type[] { typeof(int) }));
        }

        public override string Execute(int consoleWidth)
        {
            int _id = (int)pars.GetPar("id").argValues[0];
            string _guid = "";

            lock (_js.jsLock)
            {
                JobNode _node = _js.LGetNode(_id);
                _guid = _node.guid.ToString();
            }

            string[] _buffer = _db.GetMemoFromNode(_guid);

            if(_buffer == null)
                return "<color><red>NODE [GUID: " + _guid + "] does not exist!";

            output += "<color><green>NODE [GUID: " + _guid + "]\n\n";
            output += "<color><white>" + _buffer[0] + "\n\n" + _buffer[1];

            return output;
        }
    }

    #endregion

    public static class DBCLIHelper
    {
        public static string[] GetColumnNames(DataColumnCollection columns)
        {
            string[] _buffer = new string[columns.Count];

            for (int i = 0; i < _buffer.Length; i++)
                _buffer[i] = columns[i].ColumnName;

            return _buffer;
        }

        public static string[] GetRowValues(DataColumnCollection columns, DataRow row)
        {
            string[] _buffer = new string[columns.Count];

            for (int i = 0; i < _buffer.Length; i++)
            {
                // not nice, but works for now
                if (columns[i].ColumnName == "STARTTIME" ||
                    columns[i].ColumnName == "STOPTIME")
                {
                    _buffer[i] = DB.MADTimestampToDateTime(Int64.Parse(row[columns[i]].ToString())).ToString("HH:mm:ss:fff dd.MM.yyyy");
                }
                else
                {
                    _buffer[i] = row[columns[i]].ToString();
                }
            }

            return _buffer;
        }
    }
}
