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

        public readonly JobOptions defaultJobOptions = new JobOptions("unknown", 20000, JobOptions.JobType.Null);
        public readonly IPAddress defaultTargetAddress = IPAddress.Loopback;
        public readonly int defaultPort = 80;
        public readonly int defaultTTL = 200;
        public readonly bool defaultDontFragment = true;
    }
}
