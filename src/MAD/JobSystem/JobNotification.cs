using System;
using System.Collections.Generic;

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

        public void CheckRulesAndNotify(Job job)
        {
            List<JobRule> _brokenRules = GetBrokenRules(job.outp);
            if (_brokenRules.Count != 0)
            {
                string _mailSubject = "[MAD][!RULE-BREAK!] Job (JOB-ID: " + job.id + ") broke one or more rules!";
                string _mailContent = "";
                foreach (JobRule _brokenRule in _brokenRules)
                {
                    _mailContent =  "___RULE-BROKEN______________________\n";
                    _mailContent += "-> OutDescriptor:  " + _brokenRule.outDescName + "\n";
                    _mailContent += "-> Operation:      " + _brokenRule.oper.ToString() + "\n";
                    _mailContent += "-> CompareValue:   " + _brokenRule.compareValue.ToString() + "\n";
                    _mailContent += "=> CurrentValue:   " + job.outp.GetOutputDesc(_brokenRule.outDescName).dataObject.ToString() + "\n\n";
                    _mailContent += "____________________________________\n\n";
                }

                if (settings != null)
                {
                    // there are settings for sending an email
                }
                else
                { 
                    // there are no settings for sending an email
                }
            }
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
                catch (Exception)
                {
                    //Logger.Log("JobNoti.: " + e.Message, Logger.MessageType.ERROR);
                }
            }
            return _brokenRules;
        }
    }
}
