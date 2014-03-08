using System;
using System.Net;

namespace JobSystemTest
{
    class JobOptions
    {
        public string jobName;
        public int delayTime;
        public JobTypes type;
        public IPAddress targetAddress;
        public int targetPort;

        public JobOptions(string jobName, JobTypes type, int delayTime, IPAddress targetAddress, int targetPort)
        {
            this.jobName = jobName;
            this.type = type;
            this.delayTime = delayTime;
            this.targetAddress = targetAddress;
            this.targetPort = targetPort;
        }

        public enum JobTypes
        {
            PingRequest,
            HttpRequest,
            PortScan
        }
    }
}
