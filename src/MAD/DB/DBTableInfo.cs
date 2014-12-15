using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAD.Database
{
    public class TableInfo
    {
        public string name;
        public types type;
        public enum types
        {
            INTEGER,
            TEXT
        }

        public TableInfo(string name, types type)
        {
            this.name = name;
            this.type = type;
        }

        public string GetCommand()
        {
            return name + " " + GetInfoType();
        }

        private string GetInfoType()
        {
            switch (type)
            {
                case types.INTEGER:
                    return "INTERGER";
                case types.TEXT:
                    return "TEXT";

                default:
                    throw new Exception();
            }
        }
    }

    public class Insert
    {
        public string column;
        public string data;

        public Insert(string column, string data)
        {
            this.column = column;
            this.data = data;
        }
    }
}
