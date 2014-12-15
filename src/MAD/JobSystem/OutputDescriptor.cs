using System;

namespace MAD.JobSystemCore
{
    public class OutputDescriptor
    {
        #region members

        public string name;
        public Type dataType;
        public object dataObject;

        public bool dbIgnore = false;

        #endregion

        #region constructors

        public OutputDescriptor()
        { }

        public OutputDescriptor(string name, Type dataType, bool dbIgnore)
        {
            this.name = name;
            this.dataType = dataType;
            this.dbIgnore = dbIgnore;
        }

        public OutputDescriptor(string name, Type dataType, object dataObject)
        {
            this.name = name;
            this.dataType = dataType;
            this.dataObject = dataObject;
        }

        public string GetString()
        {
            return name + ":" + dataType + ":" + Convert.ToString(dataObject); // NOT TESTED
        }

        #endregion
    }  
}
