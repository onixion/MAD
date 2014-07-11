using System;
using System.Text;
using System.Net.Mail;
using System.Collections.Generic;

namespace MAD.JobSystemCore
{
    public class JobNotification
    {
        #region members

        public List<MailAddress> mailAddresses = new List<MailAddress>();
        public List<JobNotificationRule> rules = new List<JobNotificationRule>();

        #endregion

        #region constructors

        public JobNotification() { }

        public JobNotification(List<MailAddress> mailAddresses, List<JobNotificationRule> rules)
        {
            this.mailAddresses = mailAddresses;
            this.rules = rules;
        }

        #endregion

        #region methodes

        // TODO
        public static List<JobNotificationRule> ParseJobNotification(List<OutDescriptor> outDesc, object[] data)
        {
            List<JobNotificationRule> _buffer = new List<JobNotificationRule>();

            return _buffer;
        }

        // TODO
        private static JobNotificationRule ParseJobNotificationInput(List<OutDescriptor> outDesc, string data)
        {
            /* Syntax: <head><operationTag><value>
             * E.G.  : TTLLeft==20 */

            return null;
        }

        private static OutDescriptor GetOutDescriptor(List<OutDescriptor> outDesc, string name)
        {
            foreach (OutDescriptor _temp in outDesc)
            {
                if (_temp.name == name)
                {
                    return _temp;
                }
            }

            return null;
        }

        public List<JobNotificationRule> GetBrokenRules()
        {
            List<JobNotificationRule> _buffer = new List<JobNotificationRule>();

            for(int i = 0; i < rules.Count; i++)
            {
                if (!rules[i].CheckRuleValidity())
                {
                    _buffer.Add(rules[i]);
                }
            }

            return _buffer;
        }

        #endregion
    }
}
