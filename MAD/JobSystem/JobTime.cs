using System;
using System.Threading;
using System.Collections.Generic;

namespace MAD.jobSys
{
    public class JobTime
    {
        #region members

        public enum TimeType { NULL, Absolute, Relative }
        public TimeType type;

        public JobDelayHandler jobDelay;
        public List<JobTimeHandler> jobTimes;

        #endregion

        #region constructors

        public JobTime()
        {
            this.type = TimeType.NULL;
        }

        public JobTime(List<JobTimeHandler> jobTimes)
        {
            type = TimeType.Absolute;
            this.jobTimes = jobTimes;
        }

        public JobTime(JobDelayHandler jobDelay)
        {
            type = TimeType.Relative;
            this.jobDelay = jobDelay;
        }

        #endregion

        #region methodes

        public JobTimeHandler GetJobTimeHandler(DateTime time)
        {
            foreach (JobTimeHandler _handler in jobTimes)
            {
                if (_handler.CheckTime(time))
                {
                    return _handler;
                }
            }

            return null;
        }

        public static List<JobTimeHandler> ParseStringArray(object[] data)
        {
            List<JobTimeHandler> _buffer = new List<JobTimeHandler>();

                for (int i = 0; i < data.Length; i++)
                {
                    string[] _split = data[i].ToString().Split(new char[] { ';' });

                    int _minute = 0;
                    int _hour = 0;
                    int _day = 0;
                    int _month = 0;
                    int _year = 0;

                    if (_split.Length == 1)
                    {
                        // 19:30 | Daily at 19:30
                        ParseHourMinute(_split[0], ref _hour, ref _minute);
                        _buffer.Add(new JobTimeHandler(_hour, _minute));
                    }
                    else if (_split.Length == 2)
                    {
                        /* WARNING: NOT EVERY MONTH HAVE THE SAME AMOUNT OF DAYS!
                         * JOB WILL NOT BE TRIGGERED IF THE DAY DOES
                         * NOT EXIST FOR THIS MONTH! e.g. 31 Febuary */

                        ParseHourMinute(_split[1], ref _hour, ref _minute);
                        ParseYearMonthDay(_split[0], ref _year, ref _month, ref _day);

                        if (_month == 0 && _year == 0)
                        {
                            // 5;19:30 | Monthly at 5th 19:30
                            _buffer.Add(new JobTimeHandler(_hour, _minute, _day));
                        }
                        else if (_month != 0 && _year == 0) 
                        {
                            // 5.6;19:30 | Yearly at 5th June 19:30
                            _buffer.Add(new JobTimeHandler(_hour, _minute, _day, _month));
                        }
                        else 
                        {
                            // 5.6.2015;19:30 | Unique at 5th June 2015 19:30
                            _buffer.Add(new JobTimeHandler(_hour, _minute, _day, _month, _year));
                        }
                    }
                    else
                    {
                        throw new Exception("Syntax-Error! Maybe too many ';'?");
                    }
                }

            return _buffer;
        }

        private static void ParseHourMinute(string data, ref int hour, ref int minute)
        {
            string[] _split = data.Split(new char[] { ':' });

            if (_split.Length == 2)
            {
                int _hour;
                int _minute;

                try
                {
                    _hour = Convert.ToInt32(_split[0]);
                    _minute = Convert.ToInt32(_split[1]);
                }
                catch (Exception)
                {
                    throw new Exception("Could not parse time argument(s)!");
                }

                if (_hour <= 23 && _hour >= 0)
                {
                    if (_minute <= 59 && _minute >= 0)
                    {
                        hour = _hour;
                        minute = _minute;
                    }
                    else
                    {
                        throw new Exception("Minute can't be '" + _minute + "'!");
                    }
                }
                else
                {
                    throw new Exception("Hour can't be '" + _hour + "'!");
                }
            }
            else
            {
                throw new Exception("Syntax-Error! Maybe too many ':'?");
            }
        }

        private static void ParseYearMonthDay(string data, ref int year, ref int month, ref int day)
        {
            string[] _split = data.Split(new char[]{'.'});
            
            if(_split.Length == 1)
            {
                int _day = 0;

                try
                {
                    _day = Convert.ToInt32(_split[0]);
                }
                catch (Exception)
                {
                    throw new Exception("Could not parse time argument(s)!");
                }

                if (_day <= 31 && _day >= 1)
                {
                    day = _day;
                }
                else
                {
                    throw new Exception("Day can't be '" + _day + "'!");
                }

            }
            else if (_split.Length == 2)
            {
                int _day = 0;
                int _month = 0;

                try
                {
                    _day = Convert.ToInt32(_split[0]);
                    _month = Convert.ToInt32(_split[1]);
                }
                catch (Exception)
                {
                    throw new Exception("Could not parse time argument(s)!");
                }

                if (_day <= 31 && _day >= 1)
                {
                    if (_month <= 12 && _month >= 1)
                    {
                        day = _day;
                        month = _month;
                    }
                    else
                    {
                        throw new Exception("Month can't be '" + _month + "'!");
                    }
                }
                else
                {
                    throw new Exception("Day can't be '" + _day + "'!");
                }
            }
            else if (_split.Length == 3)
            {
                int _day = 0;
                int _month = 0;
                int _year = 0;

                try
                {
                    _day = Convert.ToInt32(_split[0]);
                    _month = Convert.ToInt32(_split[1]);
                    _year = Convert.ToInt32(_split[2]);
                }
                catch (Exception)
                {
                    throw new Exception("Could not parse time argument(s)!");
                }

                if (_day <= 31 && _day >= 1)
                {
                    if (_month <= 12 && _month >= 1)
                    {
                        day = _day;
                        month = _month;
                        year = _year;
                    }
                    else
                    {
                        throw new Exception("Month can't be '" + _month + "'!");
                    }
                }
                else
                {
                    throw new Exception("Day can't be '" + _day + "'!");
                }
            }
            else
            {
                throw new Exception("Syntax-Error! Maybe too many '.'?");
            }
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

        private bool _blockSignal = false;
        public bool blockSignal { get { return _blockSignal; } }
        public int minuteAtBlock = 100;
        
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

        public bool IsBlocked(DateTime now)
        {
            if (now.Minute == minuteAtBlock)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckTime(DateTime now)
        {
            switch (type)
            {
                case Type.Daily:

                    if (now.Hour == hour && now.Minute == minute)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case Type.Monthly:

                    if (now.Hour == hour && now.Minute == minute && now.Day == day)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case Type.Yearly:

                    if (now.Hour == hour && now.Minute == minute && now.Day == day && now.Month == month)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case Type.Unique:

                    if (now.Hour == hour && now.Minute == minute && now.Day == day && now.Month == month && now.Year == year)
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

        public string JobTimeStatus()
        { 
            switch(type)
            {
                case Type.Daily:
                    return hour + ":" + minute;
                case Type.Monthly:
                    return day + ";" + hour + ":" + minute;
                case Type.Yearly:
                    return day + "." + month + ";" + hour + ":" + minute;
                case Type.Unique:
                    return day + "." + month + "." + year + ";" + hour + ":" + minute;
                case Type.NULL:
                    return "NULL";
                default:
                    return null;
            }
        }

        #endregion
    }

    public class JobDelayHandler
    {
        #region members

        private int _delayTime;
        public int delayTime { get { return _delayTime; } }

        private int _delayTimeRemaining;
        public int delayTimeRemaining { get { return _delayTimeRemaining; } }

        #endregion

        #region constructor

        public JobDelayHandler(int delayTime)
        {
            _delayTime = delayTime;
            _delayTimeRemaining = delayTime;
        }

        #endregion

        #region methodes

        public bool CheckTime()
        {
            if (_delayTimeRemaining <= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Reset()
        {
            _delayTimeRemaining = _delayTime;
        }

        public void SubtractFromDelaytime(int deltaTime)
        {
            _delayTimeRemaining = _delayTimeRemaining - deltaTime;
        }

        #endregion
    }
}
