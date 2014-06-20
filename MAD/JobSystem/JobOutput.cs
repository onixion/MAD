using System;
using System.Collections.Generic;

namespace MAD.jobSys
{
    /*
     * Every job has a JobOutput object. It contains a state, which describes if
     * the job has ended successful or not, and a list of JobDescriptors.
     * Every job can have multiple jobDescriptors. These describes job output objects.
     * 
     * EXAMPLE:
     *  PingRequest have two JobDescriptor, one for the remaining TTL and one for
     *  the passed time.
     * */

    public class JobOutput
    {
        #region members

        public State jobState = State.NULL;
        public enum State { NULL, Success, Failed, Exception }

        public List<JobDescriptor> jobOutputDescriptors = new List<JobDescriptor>();

        #endregion

        #region methodes

        public bool SetDataObject(string name, object data)
        {
            for(int i = 0; i < jobOutputDescriptors.Count; i++)
            {
                if (jobOutputDescriptors[i].name == name)
                {
                    jobOutputDescriptors[i].data = data;
                    return true;
                }
            }

            return false;
        }

        public JobDescriptor GetJobDescriptor(string name)
        {
            for (int i = 0; i < jobOutputDescriptors.Count; i++)
            {
                if (jobOutputDescriptors[i].name == name)
                {
                    return jobOutputDescriptors[i];
                }
            }

            return null;
        }

        #endregion
    }

    public class JobDescriptor
    {
        #region members

        public string name;
        public Type dataType;
        public object data;

        #endregion

        #region constructor

        public JobDescriptor(string name, Type dataType, object data)
        {
            this.name = name;
            this.dataType = dataType;
            this.data = data;
        }

        #endregion
    }
}
