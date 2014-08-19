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

        public const string JOB_NOTI_PAR = "rule";
        public const string JOB_NOTI_MAIL = "mail";
        public const string JOB_NOTI_PRIO = "prio";

        public static JobTime ParseJobTime(Command c)
        {
            JobTime _buffer = new JobTime();

            if (c.OParUsed(JOB_TIME_PAR))
            {
                Type _argType = c.GetArgType(JOB_TIME_PAR);

                if (_argType == typeof(int))
                {
                    _buffer.type = JobTime.TimeMethod.Relative;
                    _buffer.jobDelay = new JobDelayHandler((int)c.pars.GetPar(JOB_TIME_PAR).argValues[0]);
                }
                else if (_argType == typeof(string))
                {
                    _buffer.type = JobTime.TimeMethod.Absolute;
                    _buffer.jobTimes = JobTime.ParseStringArray(c.pars.GetPar(JOB_TIME_PAR).argValues);
                }
            }
            else
            {
                // default settings
                _buffer.jobDelay = new JobDelayHandler(20000);
                _buffer.type = JobTime.TimeMethod.Relative;
            }

            return _buffer;
        }

        public static JobNotification ParseJobNotification(Command c, List<OutputDesc> outDesc)
        {
            JobNotification _buffer = new JobNotification();

            // PARSE MAILADDRESSES
            if (c.OParUsed(JOB_NOTI_MAIL))
            {
                MailAddress[] _mails = (MailAddress[])c.pars.GetPar(JOB_NOTI_MAIL).argValues;
                string[] _mailsString = new string[_mails.Length];

                for (int i = 0; i < _mails.Length; i++)
                    _mailsString[i] = _mails[i].Address;

                _buffer.mailAddr = _mailsString;
            }

            // PARSE PRIO
            if (c.OParUsed(JOB_NOTI_PRIO))
            {
                string _arg = (string)c.pars.GetPar(JOB_NOTI_PRIO).argValues[0];
                _buffer.priority = ParsePrio(_arg);
            }

            // PARSE RULES
            if (c.OParUsed(JOB_NOTI_PAR))
            {
                object[] _args = c.pars.GetPar(JOB_NOTI_PAR).argValues;
                for (int i = 0; i < _args.Length; i++)
                    _buffer.rules.Add(ParseRule(outDesc, (string)_args[i]));
            }

            return _buffer;
        }

        public static JobRule ParseRule(List<OutputDesc> outDesc, string data)
        {
            JobRule _rule = new JobRule();
            bool _operatorKnown = false;

            string[] _buffer;

            while (true)
            {
                _buffer = SplitByOperator(data, "=");
                if (_buffer.Length == 2)
                {
                    _rule.oper = JobRule.Operation.Equal;
                    _operatorKnown = true;
                    break;
                }

                _buffer = SplitByOperator(data, "!=");
                if (_buffer.Length == 2)
                {
                    _rule.oper = JobRule.Operation.NotEqual;
                    _operatorKnown = true;
                    break;
                }

                _buffer = SplitByOperator(data, "<");
                if (_buffer.Length == 2)
                {
                    _rule.oper = JobRule.Operation.Smaller;
                    _operatorKnown = true;
                    break;
                }

                _buffer = SplitByOperator(data, ">");
                if (_buffer.Length == 2)
                {
                    _rule.oper = JobRule.Operation.Bigger;
                    _operatorKnown = true;
                    break;
                }

                break;
            }

            if (_operatorKnown == false)
                throw new Exception("Operation not known!");

            OutputDesc _desc = GetOutDesc(outDesc, _buffer[0]);
            if (_desc == null)
                throw new Exception("OutDescriptor not known!");

            _rule.obj = _desc.dataObject;

            if(!_rule.IsOperatorSupported())
                throw new Exception("Operator does not support this descriptor!");

            _rule.obj2 = _buffer[1];

            return _rule;
        }

        private static string[] SplitByOperator(string toSplit, string i)
        {
            return toSplit.Split(new string[] { i }, StringSplitOptions.RemoveEmptyEntries);
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
