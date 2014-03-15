using System;
using System.Net;

namespace JobSystemTest
{
    class JobPingOptions : JobOptions
    {
        public int ttl;
        
        public JobPingOptions(string jobName, int delayTime, IPAddress targetAddress, int ttl)
        {
            jobType = "PingRequest";

            this.jobName = jobName;
            this.delayTime = delayTime;
            this.targetAddress = targetAddress;

            this.ttl = ttl;
        }

        public bool jobSuccess { get; set; }
    }
}
