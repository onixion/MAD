using System;
using System.Collections.Generic;

namespace MAD.JobSystem
{
    public class JobTime
    {
        public TimeType type;
        public enum TimeType { Absolute, Relativ, Null }

        public List<DayTime> jobTimes;
        public int jobDelay;

        public JobTime()
        {
            this.type = TimeType.Null;
        }

        public JobTime(List<DayTime> jobTimes)
        {
            type = TimeType.Absolute;
            this.jobTimes = jobTimes;
        }

        public JobTime(int jobDelay)
        {
            type = TimeType.Relativ;
            this.jobDelay = jobDelay;
        }

        private List<DayTime> ParseTime(string[] data)
        {
            List<DayTime> _buffer = new List<DayTime>();

            foreach (string _temp in data)
            {
                string[] _temp2 = _temp.Split(new char[] { ':' });

                if (_temp2.Length == 2)
                {
                    try
                    {
                        int _hour = Convert.ToInt32(_temp2[0]);
                        int _minute = Convert.ToInt32(_temp2[1]);

                        if (_hour <= 23 && _hour >= 0 && _minute <= 59 && _minute >= 0)
                        {
                            _buffer.Add(new DayTime(_hour, _minute));
                        }
                    }
                    catch (Exception) { return _buffer; }
                }

                if (_buffer.Count == 0)
                {
                    _buffer.Add(new DayTime(0, 0));
                }
            }

            return _buffer;
        }
    }

    public class DayTime
    { 
        public int hour { get; set; }
        public int minute { get; set; }

        public DayTime(int hour, int minute)
        {
            this.hour = hour;
            this.minute = minute;
        }
    }
}
