using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace MAD.JobSystemCore
{
    public class JobNotificationSettings
    {
        #region members

        public MailAddress[] mailAddr { get; set; }
        public MailPriority priority { get; set; }

        #endregion

        #region constructors

        public JobNotificationSettings() 
        {
            mailAddr = new MailAddress[0];
            priority = MailPriority.High;
        }

        public JobNotificationSettings(MailAddress[] mailAddr)
        {
            this.mailAddr = mailAddr;
        }

        public JobNotificationSettings(MailAddress[] mailAddr, MailPriority priority) 
        {
            this.mailAddr = mailAddr;
            this.priority = priority;
        }

        #endregion
    }
}
