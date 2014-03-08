using System;
using System.Net;

namespace JobSystemTest
{
    class JobOptions
    {
        public string jobName;
        public int delayTime;
        public JobTypes type;
        public IPAddress target;

        public JobOptions(string jobName, int delayTime, JobTypes type, IPAddress target)
        {
            this.jobName = jobName;
            this.delayTime = delayTime;
            this.type = type;
            this.target = target;
        }

        public enum JobTypes
        {
            PingRequest,
            HttpRequest,
            PortScan
        }
    }
}
