using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace MAD.JobSystemCore
{
    public class JobTime
    {
        #region members

        public enum TimeMethod { Absolute, Relative }
        public TimeMethod type;

        public bool delayed = false;
        public JobDelayHandler jobDelay { get; set; }
        public List<JobTimeHandler> jobTimes { get; set; }

        #endregion

        #region constructors

        public JobTime() 
        {
            jobDelay = new JobDelayHandler();
        }

        // for delay (e.g. 2000ms)
        public JobTime(JobDelayHandler jobDelay)
        {
            this.type = TimeMethod.Relative;
            this.jobDelay = jobDelay;
        }

        // for times (e.g. 19:30)
        public JobTime(List<JobTimeHandler> jobTimes)
        {
            this.type = TimeMethod.Absolute;
            this.jobTimes = jobTimes;
        }

        #endregion

        #region methods

        public JobTimeHandler GetJobTimeHandler(DateTime time)
        {
            foreach (JobTimeHandler _handler in jobTimes)
                if (_handler.CheckTime(time))
                    return _handler;
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
                        ParseHourMinute(_split[0], _hour, _minute);
                        _buffer.Add(new JobTimeHandler(_hour, _minute));
                    }
                    else if (_split.Length == 2)
                    {
                        /* WARNING: NOT EVERY MONTH HAVE THE SAME AMOUNT OF DAYS!
                         * JOB WILL NOT BE TRIGGERED IF THE DAY DOES
                         * NOT EXIST FOR THIS MONTH! e.g. 31 Febuary */

                        ParseHourMinute(_split[1], _hour, _minute);
                        ParseYearMonthDay(_split[0], _year, _month, _day);

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
                        throw new Exception("Syntax-Error! Maybe too many ';'?");
                }

            return _buffer;
        }

        private static void ParseHourMinute(string data, int hour, int minute)
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
                    throw new Exception("Could not parse time arg(s)!");
                }

                if (_hour <= 23 && _hour >= 0)
                    if (_minute <= 59 && _minute >= 0)
                    {
                        hour = _hour;
                        minute = _minute;
                    }
                    else
                        throw new Exception("Minute can't be '" + _minute + "'!");
                else
                    throw new Exception("Hour can't be '" + _hour + "'!");
            }
            else
                throw new Exception("Syntax-Error! Maybe too many ':'?");
        }

        private static void ParseYearMonthDay(string data, int year, int month, int day)
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
                    throw new Exception("Could not parse time arg(s)!");
                }

                if (_day <= 31 && _day >= 1)
                    day = _day;
                else
                    throw new Exception("Day can't be '" + _day + "'!");
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
                    throw new Exception("Could not parse time arg(s)!");
                }

                if (_day <= 31 && _day >= 1)
                    if (_month <= 12 && _month >= 1)
                    {
                        day = _day;
                        month = _month;
                    }
                    else
                        throw new Exception("Month can't be '" + _month + "'!");
                else
                    throw new Exception("Day can't be '" + _day + "'!");
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
                    throw new Exception("Could not parse time arg(s)!");
                }

                if (_day <= 31 && _day >= 1) 
                    if (_month <= 12 && _month >= 1)
                    {
                        day = _day;
                        month = _month;
                        year = _year;
                    }
                    else
                        throw new Exception("Month can't be '" + _month + "'!");
                else
                    throw new Exception("Day can't be '" + _day + "'!");
            }
            else
                throw new Exception("Syntax-Error! Maybe too many '.'?");
        }

        public string GetValues()
        {
            if (type == TimeMethod.Relative)
                return jobDelay.delayTimeRemaining.ToString() + "/" + jobDelay.delayTime.ToString() + "ms";
            else if (type == TimeMethod.Absolute)
            {
                string _temp = "";
                foreach (JobTimeHandler _buffer in jobTimes)
                    _temp += _buffer.JobTimeStatus() + " ";
                return _temp;
            }
            else
                return "NULL";
        }

        #endregion
    }

    public class JobTimeHandler
    {
        #region members

        public enum TimeType { NULL, Daily, Monthly, Yearly, Unique }
        public TimeType timeType = TimeType.NULL;

        public int year { get; set; }
        public int month { get; set; }
        public int day { get; set; }
        public int hour { get; set; }
        public int minute { get; set; }

        [JsonIgnore]
        private bool _blockSignal = false;
        [JsonIgnore]
        public bool blockSignal { get { return _blockSignal; } }
        [JsonIgnore]
        public int minuteAtBlock = 100;
        
        #endregion

        #region constructors

        public JobTimeHandler()
        { }

        public JobTimeHandler(int hour, int minute)
        {
            this.hour = hour;
            this.minute = minute;
            this.timeType = TimeType.Daily;
        }

        public JobTimeHandler(int hour, int minute, int day)
        {
            this.hour = hour;
            this.minute = minute;
            this.day = day;
            this.timeType = TimeType.Monthly;
        }

        public JobTimeHandler(int hour, int minute, int day, int month)
        {
            this.hour = hour;
            this.minute = minute;
            this.day = day;
            this.month = month;
            this.timeType = TimeType.Yearly;
        }

        public JobTimeHandler(int hour, int minute, int day, int month, int year)
        {
            this.hour = hour;
            this.minute = minute;
            this.day = day;
            this.month = month;
            this.year = year;
            this.timeType = TimeType.Unique;
        }

        #endregion

        #region methods

        public bool IsBlocked(DateTime now)
        {
            if (now.Minute == minuteAtBlock)
                return true;
            else
                return false;
        }

        public bool CheckTime(DateTime now)
        {
            switch (timeType)
            {
                case TimeType.Daily:
                    if (now.Hour == hour && now.Minute == minute)
                        return true;
                    else
                        return false;

                case TimeType.Monthly:
                    if (now.Hour == hour && now.Minute == minute && now.Day == day)
                        return true;
                    else
                        return false;

                case TimeType.Yearly:
                    if (now.Hour == hour && now.Minute == minute && now.Day == day && now.Month == month)
                        return true;
                    else
                        return false;

                case TimeType.Unique:
                    if (now.Hour == hour && now.Minute == minute && now.Day == day && now.Month == month && now.Year == year)
                        return true;
                    else
                        return false;

                default:
                    return false;
            }
        }

        #region for cli only

        public string JobTimeStatus()
        {
            switch (timeType)
            {
                case TimeType.Daily:
                    return hour + ":" + minute;
                case TimeType.Monthly:
                    return day + ";" + hour + ":" + minute;
                case TimeType.Yearly:
                    return day + "." + month + ";" + hour + ":" + minute;
                case TimeType.Unique:
                    return day + "." + month + "." + year + ";" + hour + ":" + minute;
                case TimeType.NULL:
                    return "NULL";
                default:
                    return null;
            }
        }

        #endregion

        #endregion
    }

    public class JobDelayHandler
    {
        #region members

        public int delayTime;
        public int delayTimeRemaining { get; set; }

        #endregion

        #region constructors

        public JobDelayHandler()
        { }

        public JobDelayHandler(int delayTime)
        {
            this.delayTime = delayTime;
            this.delayTimeRemaining = delayTime;
        }

        #endregion

        #region methods

        public bool CheckTime()
        {
            if (delayTimeRemaining <= 0)
                return true;
            else
                return false;
        }

        public void Reset()
        {
            delayTimeRemaining = delayTime;
        }

        public void SubtractFromDelaytime(int deltaTime)
        {
            delayTimeRemaining = delayTimeRemaining - deltaTime;
        }

        #endregion
    }
}
