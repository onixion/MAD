using System;

namespace MAD.JobSystem
{
    public class JobTime
    {
        public TimeType type;
        public enum TimeType { Absolute, Relativ }

        public DateTime[] times;
        public int delay;

        public JobTime(DateTime[] times)
        {
            type = TimeType.Absolute;
            this.times = times;
        }
        
        public JobTime(int delay)
        {
            type = TimeType.Relativ;
            this.delay = delay;
        }
    }
}
