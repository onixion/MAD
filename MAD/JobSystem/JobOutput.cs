using System;
using System.Collections.Generic;

namespace MAD.jobSys
{
    /*
     * Every job has a JobOutput object. It contains a state, which describes if
     * the job has ended successfully or not and a list of JobOutputDescriptors.
     * Every job can have multiple JobOutputDescriptors.
     * 
     * One JobOutputDescriptors describes one job-output object. */

    public class JobOutput
    {
        #region members

        public State jobState = State.NULL;
        public enum State { NULL, Success, Failed, Exception }

        public List<JobOutputDescriptor> jobOutputDescriptors = new List<JobOutputDescriptor>();

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

        public JobOutputDescriptor GetJobDescriptor(string name)
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

    public class JobOutputDescriptor
    {
        #region members

        public string name;
        public Type dataType;
        public object data;

        #endregion

        #region constructor

        public JobOutputDescriptor(string name, Type dataType, object data)
        {
            this.name = name;
            this.dataType = dataType;
            this.data = data;
        }

        #endregion
    }
}
