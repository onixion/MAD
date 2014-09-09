using System;
using System.Collections.Generic;

using MAD.Logging;

using Newtonsoft.Json;

namespace MAD.JobSystemCore
{
    public class JobNotification
    {
        public JobNotificationSettings settings = null;
        public List<JobRule> rules = null;

        public JobNotification()
        { }

        public JobNotification(JobNotificationSettings settings)
        {
            this.settings = settings;
        }

        public List<JobRule> GetBrokenRules(JobOutput outp)
        {
            List<JobRule> _brokenRules = new List<JobRule>();

            if (rules == null)
                return _brokenRules;

            foreach (JobRule _rule in rules)
            {
                if (!_rule.CheckRuleValidity(outp))
                    _brokenRules.Add(_rule);
            }
            return _brokenRules;
        }
    }
}
