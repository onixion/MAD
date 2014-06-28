using System;

namespace MAD.jobSys
{
    public class OutDescriptor
    {
        #region members

        public string name;
        public Type dataType;
        public object dataObject;

        #endregion

        #region constructor

        public OutDescriptor(string name, Type dataType)
        {
            this.name = name;
            this.dataType = dataType;
        }

        #endregion

        #region methodes

        public void SetData(object dataObject)
        {
            if (dataObject.GetType() == dataType)
            {
                this.dataObject = dataObject;
            }
            else
            {
                throw new Exception("WRONG TYPE!");
            }
        }

        #endregion
    }
    
}
