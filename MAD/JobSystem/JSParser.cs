using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

using MAD.CLICore;

namespace MAD.JobSystemCore
{
    public static class JSParser
    {
        public const string JOB_TIME_PAR = "t";
        public const string JOB_NOTI_PAR = "nRule";

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

            if (c.OParUsed(JOB_NOTI_PAR))
            {
                object[] _args = c.pars.GetPar(JOB_NOTI_PAR).argValues;

                for (int i = 0; i < _args.Length; i++)
                {
                    // HERE
                }
            }

            return _buffer;
        }

        public static JobRule ParseRule(List<OutputDesc> outDesc, string data)
        {
            JobRule _rule = null;

            string[] _buffer = data.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
            if (_buffer.Length == 2)
                _rule.oper = JobRule.Operation.Equal;

            _buffer = data.Split(new string[] { "!=" }, StringSplitOptions.RemoveEmptyEntries);
            if (_buffer.Length == 2)
                _rule.oper = JobRule.Operation.NotEqual;

            _buffer = data.Split(new string[] { "<" }, StringSplitOptions.RemoveEmptyEntries);
            if (_buffer.Length == 2)
                _rule.oper = JobRule.Operation.Smaller;

            _buffer = data.Split(new string[] { ">" }, StringSplitOptions.RemoveEmptyEntries);
            if (_buffer.Length == 2)
                _rule.oper = JobRule.Operation.Bigger;

            if(_rule.oper == null)
                throw new Exception("Operator not known!");

            OutputDesc _desc = GetOutDesc(outDesc, data);
            if (_desc == null)
                throw new Exception("OutDescriptor not known!");

            // CHECK OPERATION IS SUPPORTED!

            // HERE

            return _rule;
        }

        public static OutputDesc GetOutDesc(List<OutputDesc> outDesc, string name)
        {
            foreach (OutputDesc _temp in outDesc)
                if (_temp.name == name)
                    return _temp;
            return null;
        }

        public static MailPriority ParsePrio(string text)
        {
            text = text.ToLower();
            switch (text)
            {
                case "low":
                    return MailPriority.Low;
                case "normal":
                    return MailPriority.Normal;
                case "high":
                    return MailPriority.High;
                default:
                    throw new Exception("Could not parse '" + text + "' to a mail-priority!");
            }
        }
    }
}
