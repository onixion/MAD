using System;
using System.Collections.Generic;

using MAD.Logging;

using Newtonsoft.Json;

namespace MAD.JobSystemCore
{
    public class JobNotification
    {
        public JobNotificationSettings settings = null;
        public List<JobRule> rules = new List<JobRule>();

        public JobNotification()
        { }

        public JobNotification(JobNotificationSettings settings)
        {
            this.settings = settings;
        }

        public List<JobRule> GetBrokenRules(JobOutput outp)
        {
            List<JobRule> _brokenRules = new List<JobRule>();
            foreach (JobRule _rule in rules)
            {
                try
                {
                    if (!_rule.CheckRuleValidity(outp))
                        _brokenRules.Add(_rule);
                }
                catch (Exception e)
                {
                    if (MadConf.conf.DEBUG_MODE)
                        Console.WriteLine("JobNoti.:" + e.Message);
                    if (MadConf.conf.LOG_MODE)
                        Logger.Log("JobNoti.: " + e.Message, Logger.MessageType.ERROR);
                }
            }
            return _brokenRules;
        }
    }
}
