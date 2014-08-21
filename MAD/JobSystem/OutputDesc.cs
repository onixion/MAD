using System;

namespace MAD.JobSystemCore
{
    public class OutputDesc
    {
        #region members

        public string name;
        public Type dataType;
        public object dataObject;

        #endregion

        #region constructors

        public OutputDesc()
        { }

        public OutputDesc(string name, Type dataType)
        {
            this.name = name;
            this.dataType = dataType;
        }

        public OutputDesc(string name, Type dataType, object dataObject)
        {
            this.name = name;
            this.dataType = dataType;
            this.dataObject = dataObject;
        }

        #endregion
    }  
}
