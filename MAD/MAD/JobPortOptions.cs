using System;
using System.Net;

namespace MAD
{
    public class JobPortOptions : JobOptions
    {
        public int port;

        public JobPortOptions(string jobName, JobType jobType, int delay, IPAddress targetAddress, int port)
        {
            this.jobName = jobName;
            this.jobType = jobType;
            this.delay = delay;
            this.targetAddress = targetAddress;
            this.port = port;
        }
    }
}
