using System;

using System.Data;
using System.Data.SQLite;
using MAD.Database;

namespace MAD.CLICore
{
    public class DBInsertTest : Command
    {
        private DB _db;

        public DBInsertTest(object[] args)
            :base()
        {
            _db = (DB)args[0];
        }

        public override string Execute(int consoleWidth)
        {
            try
            {
                
            }
            catch (Exception e)
            {
                output += "<color><red>SQL-ERROR: " + e.Message;
            }

            return output;
        }
    }
}
