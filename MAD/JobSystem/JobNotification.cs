using System;
using System.Net.Mail;
using System.Collections.Generic;

namespace MAD.jobSys
{
    public class JobNotification
    {
        private List<MailAddress> _mailAddresses = new List<MailAddress>();
        public List<JobNotificationRule> rules = new List<JobNotificationRule>();

        public JobNotification(List<MailAddress> mailAddresses)
        {
            _mailAddresses = mailAddresses;
        }

        public bool CheckAllRules()
        {
            foreach (JobNotificationRule _rule in rules)
            {
                if (_rule.CheckRule() == JobNotificationRule.Result.RuleObserved)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }
    }

    public class JobNotificationArgs
    { 
        
    }
}
