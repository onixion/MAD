using System;
using System.Collections.Generic;

namespace MAD.jobSys
{
    public class OutDescriptor
    {
        #region members

        public string name;
        public Type dataType;
        public object[] data;

        #endregion

        #region constructor

        public OutDescriptor(string name, Type dataType, params object[] data)
        {
            this.name = name;
            this.dataType = dataType;
            this.data = data;
        }

        #endregion
    }
    
}
