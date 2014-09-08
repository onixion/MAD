using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAD
{
    public class TableInfo
    {
        public string name;
        public OType type;
        public enum OType 
        {
            INTEGER,
            CHAR20,
            CHAR30,
            boolean1 
        }

        public TableInfo(string name, OType type)
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
                case OType.INTEGER:
                    return "INTERGER";
                case OType.CHAR20:
                    return "CHAR(20)";
                case OType.CHAR30:
                    return "CHAR(30)";
                case OType.boolean1:
                    return "BOOLEAN1";

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
