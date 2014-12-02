using System;
using System.Data;
using System.Data.SQLite;

using MAD.Database;

namespace MAD.JobSystemCore
{
    public class DBHandler : IDisposable
    {
        private SQLiteConnection _con;

        public DBHandler(SQLiteConnection con)
        {
            _con = con;
            //CreateDB();
        }

        private void CreateDB()
        {
            using (SQLiteCommand _command = new SQLiteCommand(_con))
            {
                // create Device_Table
                string buffer = "create table if not exists Device_Table ( GUID TEXT, HOSTNAME TEXT, IP TEXT, MAC TEXT, ONLINE INTEGER, MEMO1 TEXT, MEMO2 TEXT);";
                _command.CommandText = buffer;
                _command.ExecuteNonQuery();

                // create Event_Table
                buffer = "create table if not exists Event_Table ( GUID TEXT, JOBNAME INTEGER, JOBTYPE INTEGER, PROTOCOL INTEGER, SUCCESS INTEGER, STARTTIME TEXT, STOPTIME TEXT, DELAYTIME TEXT, CUSTOM1 TEXT, CUSTOM2 INTEGER);";
                _command.CommandText = buffer;
                _command.ExecuteNonQuery();

                // create Job_Type_Table
                buffer = "create table if not exists Job_Type_Table ( ID INTEGER, JobType TEXT);";
                _command.CommandText = buffer;
                _command.ExecuteNonQuery();

                // create Job_Name_Table
                buffer = "create table if not exists Job_Name_Table ( ID INTEGER, JobNames TEXT);";
                _command.CommandText = buffer;
                _command.ExecuteNonQuery();

                // create Protocol_Table
                buffer = "create table if not exists Protocol_Table ( ID INTEGER, Protocol TEXT);";
                _command.CommandText = buffer;
                _command.ExecuteNonQuery();

                // create Summary_Table
                buffer = "create table if not exists Summary_Table ( GUID TEXT, DATE TEXT, JOBTYPE INTEGER, PROTOCOL INTEGER, OutState INTEGER);";
                _command.CommandText = buffer;
                _command.ExecuteNonQuery();
            }
        }

        public void InsertNode(JobNode node)
        {
            using (SQLiteCommand _command = new SQLiteCommand(_con))
            { 
                _command.CommandText = "select GUID from Device_Table where GUID='" + node .guid.ToString() + "';";
                DataTable _table = _command.ExecuteReader().GetSchemaTable();

                // check if a device with the GUID exists
                if (_table.Rows.Count == 1)
                {
                    // device does not exist -> create new one
                    _command.CommandText = "insert into Device_Table (GUID, ";
                }
                else
                { 
                    // device exists -> update device
                }
            }
        }

        public void InsertJob(Job job)
        { 
            // inserts job ..
        }

        public void Dispose()
        {
            _con.Dispose();
        }
    }
}
