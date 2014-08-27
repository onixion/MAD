using System;

using System.Net.Mail;

namespace MAD.JobSystemCore
{
    public class JobNotificationLogin
    {
        public string smtpAddr { get; set; }
        public int port { get; set; }
        public MailAddress mail { get; set; }
        public string password { get; set; }

        public JobNotificationLogin()
        { }
    }
}
