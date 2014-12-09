using System;

using System.Data;
using System.Data.SQLite;
using MAD.Database;

namespace MAD.CLICore
{
    public class DBShow : Command
    {
        private DB _db;

        public DBShow(object[] args)
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
                output += "<color><yellow>Tables:\n";
                output += "<color><yellow> -> <color><white>GUIDTable\n";
                output += "<color><yellow> -> <color><white>DeviceTable\n";
                output += "<color><yellow> -> <color><white>EventTable\n";
                output += "<color><yellow> -> <color><white>JobNameTable\n";
                output += "<color><yellow> -> <color><white>SummaryTable\n";

                return output;
            }
            else
            { 
                // check if table exists
                // read content
                // create console table
            }

            return output;
        }
    }
}
