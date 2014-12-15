using System;

using System.Data;
using System.Data.SQLite;
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
                output += "<color><yellow>Available Tables:\n";
                output += "<color><yellow> -> <color><white>GUIDTable\n";
                output += "<color><yellow> -> <color><white>HostTable\n";
                output += "<color><yellow> -> <color><white>IPTable\n";
                output += "<color><yellow> -> <color><white>MacTable\n";
                output += "<color><yellow> -> <color><white>JobNameTable\n";
                output += "<color><yellow> -> <color><white>JobTypeTable\n";
                output += "<color><yellow> -> <color><white>ProtocolTable\n";
                output += "<color><yellow> -> <color><white>OutStateTable\n";
                output += "<color><yellow> -> <color><white>JobTable\n";
                output += "<color><yellow> -> <color><white>SummaryTable\n";

                return output;
            }
            else
            {
                int _amountOfRows = 10;
                if(OParUsed("r"))
                    _amountOfRows = (int)pars.GetPar("r").argValues[0];

                DataTable _table = _db.ReadTable((string)pars.GetPar("t").argValues[0]);
                DataColumnCollection _columns = _table.Columns;
                DataRowCollection _rows = _table.Rows;

                if(_amountOfRows > _rows.Count)
                    _amountOfRows = _rows.Count;

                output += "<color><yellow>" + ConsoleTable.GetSplitline(consoleWidth);
                output += ConsoleTable.FormatStringArray(consoleWidth, DBCLIHelper.GetColumnNames(_columns));
                output += ConsoleTable.GetSplitline(consoleWidth) + "<color><white>";

                for (int i = _rows.Count - _amountOfRows; i < _rows.Count; i++)
                    output += ConsoleTable.FormatStringArray(consoleWidth, DBCLIHelper.GetRowValues(_columns, _rows[i]));
            }

            return output;
        }
    }

    public class DBJob : Command
    { 
        private DB _db;

        public DBJob(object[] args)
            :base()
        {
            _db = (DB)args[0];
            oPar.Add(new ParOption("r", "AMOUNT-ROWS", "Amount of rows to show.", false, false, new Type[] { typeof(int) }));
            description = "This command shows you the joined content of the job table.";
        }

        public override string Execute(int consoleWidth)
        {
            int _amountOfRows = 10;
            if (OParUsed("r"))
                _amountOfRows = (int)pars.GetPar("r").argValues[0];

            DataTable _table = _db.ReadTable("JobTable");
            DataColumnCollection _columns = _table.Columns;
            DataRowCollection _rows = _table.Rows;

            if (_amountOfRows > _rows.Count)
                _amountOfRows = _rows.Count;

            output += "<color><yellow>" + ConsoleTable.GetSplitline(consoleWidth);
            output += ConsoleTable.FormatStringArray(consoleWidth, DBCLIHelper.GetColumnNames(_columns));
            output += ConsoleTable.GetSplitline(consoleWidth) + "<color><white>";

            for (int i = _rows.Count - _amountOfRows; i < _rows.Count; i++)
                output += ConsoleTable.FormatStringArray(consoleWidth, DBCLIHelper.GetRowValues(_columns, _rows[i]));

            return output;
        }
    }

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
