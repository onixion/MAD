using System;
using System.Net.Mail;
using System.Collections.Generic;

namespace MAD.jobSys
{
    public class JobNotification
    {
        private MailAddress[] _mailAddresses = new MailAddress[0];
        public List<JobNotificationRule> rules = new List<JobNotificationRule>();

        public JobNotification() { }

        public JobNotification(params MailAddress[] mailAddresses)
        {
            _mailAddresses = mailAddresses;
        }

        public void ParseAndSetNotification(string textToParse)
        { 
            // TODO
        }

        public bool CheckRules()
        { 
            for(int i = 0; i < rules.Count; i++)
            {
                if (!rules[i].CheckRuleValidity())
                {
                    return false;
                }
            }

            return true;
        }
    }
}
