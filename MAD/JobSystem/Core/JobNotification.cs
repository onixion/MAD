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
        public static List<JobNotificationRule> ParseJobNotiRules(List<OutputDesc> outputDesc, object[] data)
        {
            List<JobNotificationRule> _buffer = new List<JobNotificationRule>();



            return _buffer;
        }

        // TODO
        private static JobNotificationRule ParseJobNotificationInput(List<OutputDesc> outputDesc, string data)
        {
            /* Syntax: <head><operationTag><value>
             * E.G.  : TTLLeft==20 */

            for (int i = 0; i < outputDesc.Count; i++)
            { 
                
            
            }


            return null;
        }

        private static OutputDesc GetOutDescriptor(List<OutputDesc> outputDesc, string name)
        {
            foreach (OutputDesc _temp in outputDesc)
                if (_temp.name == name)
                    return _temp;
            return null;
        }

        public List<JobNotificationRule> GetBrokenRules()
        {
            List<JobNotificationRule> _buffer = new List<JobNotificationRule>();

            for(int i = 0; i < rules.Count; i++)
                if (!rules[i].CheckRuleValidity())
                    _buffer.Add(rules[i]);
            return _buffer;
        }

        #endregion
    }
}
