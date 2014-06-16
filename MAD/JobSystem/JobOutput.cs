using System;
using System.Collections.Generic;

namespace MAD.JobSystem
{
    public class JobOutput
    {
        public List<JobDescriptor> jobOutputDescriptors = new List<JobDescriptor>();

        public State jobState = State.NULL;
        public enum State { NULL, Success, Failed, Exception}

        public JobOutput()
        {
        }
    }

    /*
     * A JobDescriptor describes one job output.
     * A job can have multiple outputs. */
    public class JobDescriptor
    {
        public string jobDescription;
        Type jobDataType;
        object jobData;

        public JobDescriptor(string jobDescription, Type jobDataType, object jobData)
        {
            this.jobDescription = jobDescription;
            this.jobDataType = jobDataType;
            this.jobData = jobData;
        }
    }
}
