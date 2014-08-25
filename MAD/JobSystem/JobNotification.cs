using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace MAD.JobSystemCore
{
    public class JobNotification
    {
        #region members

        public MailAddress[] mailAddr { get; set; }
        public MailPriority priority { get; set; }

        #endregion

        #region constructors

        public JobNotification() 
        {
            mailAddr = new MailAddress[0];
            priority = MailPriority.High;
        }

        public JobNotification(MailAddress[] mailAddr)
        {
            this.mailAddr = mailAddr;
        }

        public JobNotification(MailAddress[] mailAddr, MailPriority priority) 
        {
            this.mailAddr = mailAddr;
            this.priority = priority;
        }

        #endregion
    }
}
