using System;
using System.Collections.Generic;
using System.Net.Mail;

using MAD.Notification;

namespace MAD.JobSystemCore
{
    public class JobNotificationSettings
    {
        #region members

        public MailLogin login { get; set; }
        public MailAddress[] mailAddr { get; set; }

        #endregion

        #region constructors

        public JobNotificationSettings() 
        {
            this.login = new MailLogin();
        }

        public JobNotificationSettings(MailAddress[] mailAddr)
        {
            this.mailAddr = mailAddr;
        }

        public JobNotificationSettings(MailAddress[] mailAddr, MailPriority priority) 
        {
            this.mailAddr = mailAddr;
        }

        #endregion
    }
}
