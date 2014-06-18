using System;
using System.Threading;
using System.Collections.Generic;

namespace MAD.JobSystem
{
    public class JobTime
    {
        #region members

        public TimeType type;
        public enum TimeType { NULL, Absolute, Relativ }

        public List<JobTimeHandler> jobTimes;
        public int jobDelay;

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

                        if (_month == 0 && _year == 0) // 5;19:30 | Monthly at 5th 19:30
                        {
                            _buffer.Add(new JobTimeHandler(_hour, _minute, _day));
                        }
                        else if (_month != 0 && _year == 0) // 5.6;19:30 | Yearly at 5th June 19:30
                        {
                            _buffer.Add(new JobTimeHandler(_hour, _minute, _day, _month));
                        }
                        else // 5.6.2015;19:30 | Unique at 5th June 2015 19:30
                        {
                            _buffer.Add(new JobTimeHandler(_hour, _minute, _day, _month, _year));
                        }
                    }
                    else
                    {
                        throw new Exception("<color><red>JOB-TIME: NOT INITILIZED YET!");
                    }
                }

            return _buffer;
        }

        private void ParseHourMinute(string data, ref int hour, ref int minute)
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
                    throw new Exception("<color><red>JOB-TIME: Could not parse time argument(s)!");
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
                        throw new Exception("<color><red>JOB-TIME: Minute can't be '" + _minute + "'!");
                    }
                }
                else
                {
                    throw new Exception("<color><red>JOB-TIME: Hour can't be '" + _hour + "'!");
                }
            }
            else
            {
                throw new Exception("<color><red>JOB-TIME: Syntax-Error! Maybe too many ':'?");
            }
        }

        private void ParseYearMonthDay(string data, ref int year, ref int month, ref int day)
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
                    throw new Exception("<color><red>JOB-TIME: Could not parse job time!");
                }

                if (_day <= 31 && _day >= 1)
                {
                    day = _day;
                }
                else
                {
                    throw new Exception("<color><red>JOB-TIME: Day cannot be bigger than 31!");
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
                    throw new Exception("<color><red>JOB-TIME: Could not parse job time!");
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
                        throw new Exception("<color><red>JOB-TIME: Day cannot be bigger than 12 or smaller than 1!");
                    }
                }
                else
                {
                    throw new Exception("<color><red>JOB-TIME: Day cannot be bigger than 31 or smaller than 1!");
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
                    throw new Exception("<color><red>JOB-TIME: Could not parse job time!");
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
                        throw new Exception("<color><red>JOB-TIME: Day cannot be bigger than 12 or smaller than 1!");
                    }
                }
                else
                {
                    throw new Exception("<color><red>JOB-TIME: Day cannot be bigger than 31 or smaller than 1!");
                }
            }
            else
            {
                throw new Exception("<color><red>JOB-TIME: Syntax-Error! Maybe too many '.'?");
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

        public bool CheckTime(DateTime now)
        {
            if (!blockSignal)
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
            else
            {
                return false;
            }
        }

        public void BlockHandler()
        {
            Thread _block = new Thread(WaitToUnBlock);
            _block.Start();
        }

        private void WaitToUnBlock()
        {
            _blockSignal = true;
            Thread.Sleep(70000);
            _blockSignal = false;
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
}
