using System;

namespace MAD.JobSystemCore
{
    public class OutputDescriptor
    {
        #region members

        public string name;
        public Type dataType;
        public object dataObject;

        #endregion

        #region constructors

        public OutputDescriptor()
        { }

        public OutputDescriptor(string name, Type dataType)
        {
            this.name = name;
            this.dataType = dataType;
        }

        public OutputDescriptor(string name, Type dataType, object dataObject)
        {
            this.name = name;
            this.dataType = dataType;
            this.dataObject = dataObject;
        }

        #endregion
    }  
}
