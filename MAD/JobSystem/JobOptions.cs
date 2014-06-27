using System;
using System.Net;

namespace MAD.jobSys
{
    public class JobOptions
    {
        #region members

        public string jobName;
        public JobType jobType;
        public enum JobType { NULL, Ping, PortScan, Http, HostDetect, ServiceCheck }
        public JobTime jobTime = new JobTime();
        public JobNotification jobNotification = new JobNotification();

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
