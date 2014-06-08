using System;
using System.Net;
namespace MAD.JobSystem
{
    public class JobDefaultValues
    {
        private static JobDefaultValues _defaultValues;
        public static JobDefaultValues defaultValues
        {
            get
            {
                if (_defaultValues == null)
                    _defaultValues = new JobDefaultValues();

                return _defaultValues;
            }
        }

        public JobOptions defaultJobOptions = new JobOptions("unknown", 20000, JobOptions.JobType.Null);
        public IPAddress defaultTargetAddress = IPAddress.Loopback;
        public int defaultPort = 80;
        public int defaultTTL = 200;
        public bool defaultDontFragment = true;
    }
}
