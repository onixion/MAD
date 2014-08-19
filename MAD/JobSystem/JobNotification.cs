using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Runtime.Serialization;

namespace MAD.JobSystemCore
{
    [Serializable()]
    public class JobNotification
    {
        #region members

        public string[] mailAddr = new string[0];
        public MailPriority priority = MailPriority.High;
        public List<JobRule> rules = new List<JobRule>();

        #endregion

        #region constructors

        public JobNotification() { }

        public JobNotification(string[] mailAddr)
        {
            this.mailAddr = mailAddr;
        }

        public JobNotification(string[] mailAddr, MailPriority priority) 
        {
            this.mailAddr = mailAddr;
            this.priority = priority;
        }

        // for serialization only
        public JobNotification(SerializationInfo info, StreamingContext context)
        {
            //mailAddr = (MailAddress[])info.GetValue("SER_MAILADDR", typeof(MailAddress[]));
            //priority = (MailPriority)info.GetValue("SER_MAILPRIORITY", typeof(MailPriority));
        }

        #endregion

        #region methodes

        #region for serialization only

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("SER_MAILADDR", mailAddr);
            info.AddValue("SER_MAILPRIORITY", priority);
        }

        #endregion

        public List<JobRule> GetBrokenRules()
        {
            List<JobRule> _buffer = new List<JobRule>();
            foreach (JobRule _temp in rules)
                if (!_temp.CheckValidity())
                    _buffer.Add(_temp);
            return _buffer;
        }

        #endregion
    }
}
