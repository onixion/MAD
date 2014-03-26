using System;
using System.Net;

namespace MAD
{
    public abstract class JobOptions
    {
        public string jobName;
        public enum JobType { PingRequest, PortRequest, HttpRequest }
        public JobType jobType;
        public int delay;
        public IPAddress targetAddress;
    }
}
