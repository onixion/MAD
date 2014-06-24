using System;
using System.Net.Mail;
using System.Collections.Generic;

namespace MAD.jobSys
{
    public class JobNotification
    {
        private MailAddress[] _mailAddresses = new MailAddress[0];
        public List<JobNotificationRule> rules = new List<JobNotificationRule>();

        public JobNotification(params MailAddress[] mailAddresses)
        {
            _mailAddresses = mailAddresses;
        }
    }
}
