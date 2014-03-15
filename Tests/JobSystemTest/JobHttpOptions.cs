using System;
using System.Net;

namespace JobSystemTest
{
    class JobHttpOptions : JobOptions
    {
        public int port;

        public JobHttpOptions(string jobName, int delayTime, IPAddress targetAddress, int port)
        {
            jobType = "HttpRequest";

            this.jobName = jobName;
            this.delayTime = delayTime;
            this.targetAddress = targetAddress;

            this.port = port;
        }
    }
}
