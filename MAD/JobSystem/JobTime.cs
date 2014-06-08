using System;

namespace MAD.JobSystem
{
    public class JobTime
    {
        public TimeType type;
        public enum TimeType { Absolute, Relativ, Null }

        public DateTime[] jobTimes;
        public int jobDelay;

        public JobTime()
        {
            this.type = TimeType.Null;
        }

        public JobTime(DateTime[] jobTimes)
        {
            type = TimeType.Absolute;
            this.jobTimes = jobTimes;
        }

        public JobTime(int jobDelay)
        {
            type = TimeType.Relativ;
            this.jobDelay = jobDelay;
        }
    }
}
