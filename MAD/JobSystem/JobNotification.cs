using System;
using System.Net.Mail;
using System.Collections.Generic;

namespace MAD.jobSys
{
    public class JobNotification
    {
        #region members

        public MailAddress[] mailAddresses;
        public List<JobNotificationRule> rules = new List<JobNotificationRule>();

        #endregion

        #region constructors

        public JobNotification()
        {
            this.mailAddresses = new MailAddress[0];
            this.rules = new List<JobNotificationRule>();
        }

        public JobNotification(MailAddress[] mailAddresses, List<JobNotificationRule> rules)
        {
            this.mailAddresses = mailAddresses;
            this.rules = rules;
        }

        #endregion

        #region methodes

        public static List<JobNotificationRule> ParseNotification(string text)
        {
            return null;
        }

        public static MailAddress[] ParseMailAddresses(params string[] mailAddresses)
        {
            return null;
        }

        public List<JobNotificationRule> GetValidityRules()
        {
            List<JobNotificationRule> _buffer = new List<JobNotificationRule>();

            for(int i = 0; i < rules.Count; i++)
            {
                if (rules[i].CheckRuleValidity())
                {
                    _buffer.Add(rules[i]);
                }
            }

            return _buffer;
        }

        #endregion
    }
}
