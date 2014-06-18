using System;
using System.Collections.Generic;

namespace MAD.JobSystem
{
    public class JobOutput
    {
        #region members

        public List<JobDescriptor> jobOutputDescriptors = new List<JobDescriptor>();
        public State jobState = State.NULL;
        public enum State { NULL, Success, Failed, Exception }

        #endregion
    }

    /*
     * A JobDescriptor describes one job output.
     * A job can have multiple outputs. */
    public class JobDescriptor
    {
        #region members

        public string jobDescription;
        Type jobDataType;
        object jobData;

        #endregion

        #region constructor

        public JobDescriptor(string jobDescription, Type jobDataType, object jobData)
        {
            this.jobDescription = jobDescription;
            this.jobDataType = jobDataType;
            this.jobData = jobData;
        }

        #endregion
    }
}
