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
            description = "This command shows you the content of a specific table.";
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
                DataTable _table = _db.ReadTable("JobTable");

                DataColumnCollection _columns = _table.Columns;
                DataRowCollection _rows = _table.Rows;

        

            }

            return output;
        }
    }
}
