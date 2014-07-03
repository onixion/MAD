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
        public static List<JobNotificationRule> ParseNotification(object[] data, List<OutDescriptor> outDescriptors)
        {
            List<JobNotificationRule> _buffer = new List<JobNotificationRule>();

            return _buffer;
        }

        // TODO
        private static void ParseNotificationInput(string data, ref string head, ref char operationTag, ref string value)
        {

        }

        private static OutDescriptor GetOutDescriptor(List<OutDescriptor> outDescriptors, string name)
        {
            foreach (OutDescriptor _temp in outDescriptors)
            {
                if (_temp.name == name)
                {
                    return _temp;
                }
            }

            return null;
        }

        // TODO
        public static List<MailAddress> ParseMailAddresses(params string[] mailAddresses)
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
