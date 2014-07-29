using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Runtime.Serialization;

namespace MAD.JobSystemCore
{
    [Serializable]
    public class JobNotification : ISerializable
    {
        #region members

        public MailAddress[] mailAddr;
        public MailPriority priority;

        #endregion

        #region constructors

        public JobNotification() { }

        public JobNotification(MailAddress[] mailAddr)
        {
            this.mailAddr = mailAddr;
            this.priority = MailPriority.High;
        }

        public JobNotification(MailAddress[] mailAddr, MailPriority priority) 
        {
            this.mailAddr = mailAddr;
            this.priority = priority;
        }

        // for serialization only
        public JobNotification(SerializationInfo info, StreamingContext context)
        {
            mailAddr = (MailAddress[])info.GetValue("SER_MAILADDR", typeof(MailAddress[]));
            priority = (MailPriority)info.GetValue("SER_MAILPRIORITY", typeof(MailPriority));
        }

        #endregion

        #region methodes

        public MailPriority ParsePrio(string text)
        {
            text = text.ToLower();
            switch(text)
            {
                case "low":
                    return MailPriority.Low;
                case "normal":
                    return MailPriority.Normal;
                case "high":
                    return MailPriority.High;
                default:
                    throw new Exception("Could not parse '" + text + "' to a mail-priority!");
            }
        }

        #region for serialization only

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("SER_MAILADDR", mailAddr);
            info.AddValue("SER_MAILPRIORITY", priority);
        }

        #endregion

        #endregion
    }
}
