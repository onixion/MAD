using System;
using System.Net;

namespace JobSystemTest
{
    class JobOptions
    {
        public string jobName;
        public int delayTime;
        public IPAddress targetAddress;

        public enum JobType
        { 
            PingRequest,
            HttpRequest,
            PortRequest
        }

        public JobOptions(string jobName, JobType jobType, int delayTime, IPAddress targetAddress)
        {
            this.jobName = jobName;
            this.delayTime = delayTime;
            this.targetAddress = targetAddress;
        }
    }
}
