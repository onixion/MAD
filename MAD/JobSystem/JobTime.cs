using System;
using System.Collections.Generic;

namespace MAD.JobSystem
{
    public class JobTime
    {
        #region members

        public TimeType type;
        public enum TimeType { Absolute, Relativ, Null }

        public List<JobTimeHandler> jobTimes;
        public int jobDelay;

        #endregion

        #region constructor

        public JobTime()
        {
            this.type = TimeType.Null;
        }

        public JobTime(List<JobTimeHandler> jobTimes)
        {
            type = TimeType.Absolute;
            this.jobTimes = jobTimes;
        }

        public JobTime(int jobDelay)
        {
            type = TimeType.Relativ;
            this.jobDelay = jobDelay;
        }

        #endregion

        #region methodes

        public List<JobTimeHandler> ParseStringArray(object[] data)
        {
            List<JobTimeHandler> _buffer = new List<JobTimeHandler>();

                for (int i = 0; i < data.Length; i++)
                {
                    string[] _split = data[i].ToString().Split(new char[] { '.' });
                    string[] _split2;

                    if (_split.Length == 1)
                    {
                        // DAILY

                        _split2 = _split[0].Split(new char[] { ':' });

                        if (_split2.Length == 2)
                        {
                            // NEED TO BE TRY {}
                            int _hour = Convert.ToInt32(_split2[0]);
                            int _minute = Convert.ToInt32(_split2[1]);

                            if (_hour <= 23 && _hour >= 0)
                            {
                                if (_minute <= 59 && _minute >= 0)
                                {
                                    _buffer.Add(new JobTimeHandler(_hour, _minute));
                                }
                                else
                                {
                                    throw new Exception("<color><red>Minute can't be '" + _minute + "'!");
                                }
                            }
                            else
                            {
                                throw new Exception("<color><red>Hour can't be '" + _minute + "'!");
                            }
                        }
                        else
                        {
                            throw new Exception("<color><red>Syntax-Error! Maybe to many ':'?");
                        }
                    }
                    else if (_split.Length == 2)
                    {
                        // MONTHLY
                    }
                    else if (_split.Length == 3)
                    {
                        // YEARLY
                    }
                    else if (_split.Length == 4)
                    {
                        // UNIQUE
                    }
                    else
                    {
                        throw new Exception("JOBTIME NOT INITILIZED YET!");
                    }
                }

            return _buffer;
        }

        #endregion
    }

    public class JobTimeHandler
    {
        #region members

        public Type type = Type.NULL;
        public enum Type { NULL, Daily, Monthly, Yearly, Unique }

        public int year { get; set; }
        public int month { get; set; }
        public int day { get; set; }
        public int hour { get; set; }
        public int minute { get; set; }

        #endregion

        #region constructors

        public JobTimeHandler(int hour, int minute)
        {
            this.hour = hour;
            this.minute = minute;
            this.type = Type.Daily;
        }

        public JobTimeHandler(int hour, int minute, int day)
        {
            this.hour = hour;
            this.minute = minute;
            this.day = day;
            this.type = Type.Monthly;
        }

        public JobTimeHandler(int hour, int minute, int day, int month)
        {
            this.hour = hour;
            this.minute = minute;
            this.day = day;
            this.month = month;
            this.type = Type.Yearly;
        }

        public JobTimeHandler(int hour, int minute, int day, int month, int year)
        {
            this.hour = hour;
            this.minute = minute;
            this.day = day;
            this.month = month;
            this.year = year;
            this.type = Type.Unique;
        }

        #endregion

        #region methodes

        public bool CheckTime()
        {
            DateTime _now = DateTime.Now;

            switch (type)
            { 
                case Type.Daily:

                    if (_now.Hour == hour && _now.Minute == minute)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case Type.Monthly:

                    if (_now.Hour == hour && _now.Minute == minute && _now.Day == day)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case Type.Yearly:

                    if (_now.Hour == hour && _now.Minute == minute && _now.Day == day && _now.Month == month)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case Type.Unique:

                    if (_now.Hour == hour && _now.Minute == minute && _now.Day == day && _now.Month == month && _now.Year == year)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                default:
                    return false;
            }

        }

        #endregion
    }
}
