using System;
using System.Collections.Generic;

using MAD.Logging;
using MAD.Notification;

using Newtonsoft.Json;

namespace MAD.JobSystemCore
{
    public class JobNotification
    {
        public const bool LOG = false;

        public List<JobRule> rules = new List<JobRule>();

        public JobNotification()
        { 
        
        }

        public void CheckRulesAndNotify(JobOutput outp, JobNotificationSettings settings)
        {
            List<JobRule> _brokenRules = GetBrokenRules(outp);
            if (_brokenRules.Count != 0)
            {
                // MAKE NOTIFICATION

            }
        }

        private List<JobRule> GetBrokenRules(JobOutput outp)
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
                    Logger.Log("JobNoti.: " + e.Message, Logger.MessageType.ERROR);
                }
            }
            return _brokenRules;
        }
    }
}
