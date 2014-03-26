using System;
using System.Net;

namespace MAD
{
    public class JobOptions
    {
        public string jobName;
        public string jobType;
        public int delayTime;
        public IPAddress targetAddress;

        public JobOptions(string jobName, string jobType, int delayTime, IPAddress targetAddress)
        {
            this.jobName = jobName;
            this.jobType = jobType;
            this.delayTime = delayTime;
            this.targetAddress = targetAddress;
        }
    }

    public class JobHttpOptions
    {
        public int port;

        public JobHttpOptions(int port)
        {
            this.port = port;
        }
    
    }
}
