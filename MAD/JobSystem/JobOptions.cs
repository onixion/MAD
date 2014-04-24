using System;
using System.Net;

namespace MAD
{
    public abstract class JobOptions
    {
        public string jobName;
        public JobType jobType;
        public enum JobType { PingRequest, PortRequest, HttpRequest }
        public int delay;
        public IPAddress targetAddress;
    }
}
