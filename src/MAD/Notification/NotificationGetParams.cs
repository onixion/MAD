using System;
using System.Net;
using System.Net.Mail;
using System.Collections.Generic;
using System.Threading;



namespace MAD.Notification
{
    internal class NotificPackIn
    {
        internal MailAddress[] eMailToPackIn;
        internal string subjectPackIn;
        internal string bodyPackIn;
        internal int retryCounterPackIn;
        internal string smtpClient_specialPackIn;
        internal MailAddress eMailFrom_specialPackIn;
        internal string password_specialPackIn;
        internal int? port_specialPackIn;
        internal bool highPriorityPackIn;
        internal MailAddress[] eMailToCCPackIn;
        internal MailAddress[] eMailToBCCPackIn;
        internal Attachment[] eMailAttachmentPackIn;
    }

    public static class NotificationGetParams
    {
        #region Declaration

        //Declaration of static Parameters (Private)

        internal static object lockObject = new object();
                
        //Decleration of Objects

        public static Queue<Object> NotificPackQueue = new Queue<object>();
        internal static NotificPackIn NotificPackIn = new NotificPackIn();

        public static Thread sendMailThread = null;

        
        #endregion

        #region Methods
        //Method to send mail with mail parameters (dynamic (have to set for every mail))

        //#1 for default Origin
        public static bool SetSendMail(string subject, string body, int retryCounter,
            bool highPriority = false, MailAddress[] eMailToCC = null, MailAddress[] eMailToBCC = null, Attachment[] eMailAttachment = null)
        {
            return NotificationGetParams.SetSendMail(null, subject, body, retryCounter, null, null, null, null,
                highPriority, eMailToCC, eMailToBCC, eMailAttachment);
        } 
        
        //#2 for Special Orign - the only one which is used (contains #1)
        public static bool SetSendMail(MailAddress[] eMailTo, string subject, string body, int retryCounter, string smtpClient_special, MailAddress eMailFrom_special, string password_special, int? port_special,
            bool highPriority = false, MailAddress[] eMailToCC = null, MailAddress[] eMailToBCC = null, Attachment[] eMailAttachment = null)
        {
            NotificPackIn NotificPackIn = new NotificPackIn();
            NotificPackIn.eMailToPackIn = eMailTo;
            NotificPackIn.subjectPackIn = subject;
            NotificPackIn.bodyPackIn = body;
            NotificPackIn.retryCounterPackIn = retryCounter;
            NotificPackIn.smtpClient_specialPackIn = smtpClient_special;
            NotificPackIn.eMailFrom_specialPackIn = eMailFrom_special;
            NotificPackIn.password_specialPackIn = password_special;
            NotificPackIn.port_specialPackIn = port_special;
            NotificPackIn.highPriorityPackIn = highPriority;
            NotificPackIn.eMailToCCPackIn = eMailToCC;
            NotificPackIn.eMailToBCCPackIn = eMailToBCC;
            NotificPackIn.eMailAttachmentPackIn = eMailAttachment;

            lock (lockObject)
            {
                NotificPackQueue.Enqueue(NotificPackIn);
            }

            if (sendMailThread == null)
            {
                try
                {
                    sendMailThread = new Thread(new ThreadStart(NotificationSend.SendMail));
                    sendMailThread.Start();
                    return true;
                }

                catch(Exception ex)
                {
                    Logging.Logger.Log("Failed to send Mail because:" + ex, Logging.Logger.MessageType.ERROR);
                    return true;
                }
            }

            else
            {
                return true;
            }

            
        }
       
        
        #endregion
    }

}
