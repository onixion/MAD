using System;
using System.Net;
namespace MAD.JobSystem
{
    public static class JobDefaultValues
    {
        public static JobOptions defaultJobOptions = new JobOptions("unknown", 20000, JobOptions.JobType.Null);

        public static IPAddress defaultTargetAddress = IPAddress.Loopback;
        public static int defaultPort = 80;
        public static int defaultTTL = 200;
        public static bool defaultDontFragment = true;
    }
}
