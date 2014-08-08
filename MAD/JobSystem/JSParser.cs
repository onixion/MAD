using System;
using System.Collections.Generic;

using MAD.CLICore;

namespace MAD.JobSystemCore
{
    /* This static class is used to parse the jobs and nodes. */

    public static class JSParser
    {
        public const string JOB_TIME_PAR = "t";
        public const string JOB_NOTI_PAR = "nRule";

        // This method parses the args and creates the JobTime.
        public static JobTime ParseJobTime(Command c)
        {
            JobTime _buffer = new JobTime();

            if (c.OParUsed(JOB_TIME_PAR))
            {
                Type _argType = c.GetArgType(JOB_TIME_PAR);

                if (_argType == typeof(int))
                {
                    _buffer.type = JobTime.TimeType.Relative;
                    _buffer.jobDelay = new JobDelayHandler((int)c.pars.GetPar(JOB_TIME_PAR).argValues[0]);
                }
                else if (_argType == typeof(string))
                {
                    _buffer.type = JobTime.TimeType.Absolute;
                    _buffer.jobTimes = JobTime.ParseStringArray(c.pars.GetPar(JOB_TIME_PAR).argValues);
                }
            }
            else
            {
                // default settings
                _buffer.jobDelay = new JobDelayHandler(20000);
                _buffer.type = JobTime.TimeType.Relative;
            }

            return _buffer;
        }

        public static JobNotification ParseJobNotification(Command c, List<OutputDesc> outDesc)
        {
            JobNotification _buffer = new JobNotification();

            object[] _args = c.pars.GetPar(JOB_NOTI_PAR).argValues;

            for (int i = 0; i < _args.Length; i++)
            {

                // HERE


            
            }

            return _buffer;
        }
    }
}
