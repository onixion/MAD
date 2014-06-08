using System;
using System.Net;

namespace MAD.JobSystem
{
    public class JobOptions
    {
        #region members

        public string jobName;
        public JobType jobType;
        public enum JobType { Null, PingRequest, PortRequest, HttpRequest }
        public JobTime jobTime = new JobTime();

        #endregion

        #region constructors

        public JobOptions(string jobName, int jobDelay, JobType jobType)
        {
            this.jobName = jobName;
            this.jobTime.jobDelay = jobDelay;
            jobTime.type = JobTime.TimeType.Relativ;
            this.jobType = jobType; 
        }

        public JobOptions(string jobName, DateTime[] times, JobType jobType)
        {
            this.jobName = jobName;
            this.jobTime.jobTimes = times;
            jobTime.type = JobTime.TimeType.Absolute;
            this.jobType = jobType;
        }

        #endregion
    }
}
