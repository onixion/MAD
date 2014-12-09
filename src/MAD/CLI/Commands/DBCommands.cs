using System;

using System.Data;
using System.Data.SQLite;
using MAD.Database;

namespace MAD.CLICore
{
    public class DBInsertTest : Command
    {
        private DB _db;

        public void DBSelect(object[] args)
        {
            _db = (DB)args[0];
        }

        public override string Execute(int consoleWidth)
        {
            using (SQLiteCommand _command = new SQLiteCommand(_db._dbConnection))
            {
                // Test writing into the database.

                // Test reading from the database.
            }
            return "Worked.";
        }
    }
}
