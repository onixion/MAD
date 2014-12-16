using System;
using System.Data;
using System.Data.SQLite;

using MAD.JobSystemCore;
using MAD.Database;

namespace MAD.CLICore
{
    public class DBShowTables : Command
    {
        private DB _db;

        public DBShowTables(object[] args)
            :base()
        {
            _db = (DB)args[0];
            oPar.Add(new ParOption("t", "TABLE-NAME", "Name of the table.", false, false, new Type[] { typeof(string) }));
            oPar.Add(new ParOption("r", "AMOUNT-ROWS", "Amount of rows to show.", false, false, new Type[] { typeof(int) }));
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
                int _amountOfRows = 20;
                if(OParUsed("r"))
                    _amountOfRows = (int)pars.GetPar("r").argValues[0];

                string _dataTable = (string)pars.GetPar("t").argValues[0];

                using (DataTable _table = _db.ReadTable(" (select * from " + _dataTable + " order by ID desc limit " + _amountOfRows + ") order by ID asc" )) 
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

    public class DBJobs : Command
    { 
        private DB _db;

        public DBJobs(object[] args)
            :base()
        {
            _db = (DB)args[0];
            oPar.Add(new ParOption("r", "AMOUNT-ROWS", "Amount of rows to show.", false, false, new Type[] { typeof(int) }));
            description = "This command shows the joined content of the job-table.";
        }

        public override string Execute(int consoleWidth)
        {
            int _amountOfRows = 10;
            if (OParUsed("r"))
                _amountOfRows = (int)pars.GetPar("r").argValues[0];

            DataTable _table = _db.ReadJobs(" order by JobTable.ID desc limit " + _amountOfRows);
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
            rPar.Add(new ParOption("m1", "MEMO1", "First memo to attach to node.", false, false, new Type[] { typeof(string) }));
            rPar.Add(new ParOption("m2", "MEMO2", "Second memo to attach to node.", false, false, new Type[] { typeof(string) }));
        }

        public override string Execute(int consoleWidth)
        {
            int _id = (int)pars.GetPar("id").argValues[0];
            string _memo1 = (string)pars.GetPar("m1").argValues[0];
            string _memo2 = (string)pars.GetPar("m2").argValues[0];

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
                _buffer[i] = row[columns[i]].ToString();

            return _buffer;
        }
    }
}
