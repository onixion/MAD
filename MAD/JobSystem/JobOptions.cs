using System;
using System.Net;

namespace MAD.jobSys
{
    public class JobOptions
    {
        #region members

        public string jobName;
        public JobType jobType;
        public enum JobType { NULL, PingRequest, PortRequest, HttpRequest }
        public JobTime jobTime = new JobTime();
        public JobOutput jobOutput = new JobOutput();

        #endregion

        #region constructors

        public JobOptions(string jobName, JobTime jobTime, JobType jobType)
        {
            this.jobName = jobName;
            this.jobTime = jobTime;
            this.jobType = jobType; 
        }

        #endregion
    }
}
