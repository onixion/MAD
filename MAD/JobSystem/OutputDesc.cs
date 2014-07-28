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

        #region constructor

        public OutputDesc(string name, Type dataType)
        {
            this.name = name;
            this.dataType = dataType;
        }

        #endregion
    }  
}
