using System;
using System.Net;
using System.Net.Mail;
using System.Collections.Generic;
using MAD.Logging;

namespace MAD.Notification
{
    public static class NotificationSend
    {

        #region Declaration

        //Declaration of static Parameters (Public)

        public static string eMailSendingSucceed, eMailSendingFailed, eMailSendingAttempt;

        //Decleration of Objects

        private static SmtpClient client;
        public static MailMessage mail;
        private static NotificPackIn tempNotificPackIn = new NotificPackIn();
        private static object lockQueueForReading = new object();

        #endregion

        #region Methods

        //Method to send mail with mail parameters (dynamic (have to set for every mail))
        private static void SendMailDo(MailAddress[] eMailTo, string subject, string body, int retryCounter, MailAddress eMailFrom_special, string smtpClient_special, string password_special, int? port_special,
            bool highPriority, MailAddress[] eMailToCC, MailAddress[] eMailToBCC, Attachment[] eMailAttachment)
        {
            //Declaration of Variables
            int tryCounter = 1;
            int port_special_intern;

            #region settings

            mail = new MailMessage();
            mail.Subject = subject;
            mail.Body = body;

            if (eMailTo == null)
            {
                foreach (MailAddress element in MAD.MadConf.conf.MAIL_DEFAULT)//to get all eMailIds from incoming Array
                {
                    mail.To.Add(element);
                }
            }

            else
            {
                foreach (MailAddress element in eMailTo)//to get all eMailIds from incoming Array
                {
                    mail.To.Add(element);
                }
            }

            if (eMailToCC != null)
            {
                foreach (MailAddress element in eMailToCC)
                {
                    mail.CC.Add(element);
                }
            }

            if (eMailToBCC != null)
            {
                foreach (MailAddress element in eMailToBCC)
                {
                    mail.Bcc.Add(element);
                }
            }

            if (eMailAttachment != null)
            {
                foreach (Attachment element in eMailAttachment)
                {
                    mail.Attachments.Add(element);
                }
            }
            
            if (highPriority == true)
            {
                mail.Priority = MailPriority.High;
            }

            if (eMailFrom_special == null)
            {
                eMailFrom_special = MAD.MadConf.conf.SMTP_USER;
                mail.From = eMailFrom_special;
            }

            if (smtpClient_special == null)
            {
                smtpClient_special = MAD.MadConf.conf.SMTP_SERVER;
            }

            if (password_special == null)
            {
                password_special = MAD.MadConf.conf.SMTP_PASS;
            }

            if (port_special == null)
            {
                port_special = MAD.MadConf.conf.SMTP_PORT;
            }

            port_special_intern = port_special.GetValueOrDefault();

            
            
            #endregion

            #region Authentification and sending process
            for (;tryCounter <= retryCounter; tryCounter++)//to retry because shit happens
            {
                try
                {
                    eMailSendingAttempt = tryCounter + ".Attempt";
                    Logger.Log(eMailSendingAttempt, Logger.MessageType.INFORM);
                    //Initialization of SMTPclient and setting parameters and sending mail
                    client = new SmtpClient(smtpClient_special, port_special_intern);
                    client.Credentials = new NetworkCredential(eMailFrom_special.ToString(), password_special);
                    client.EnableSsl = true;
                    client.Send(mail); //Send order
                    eMailSendingSucceed = "(" + tryCounter + ".Attempt) Success";
                    Logger.Log(eMailSendingSucceed, Logger.MessageType.INFORM);
                    client.Dispose();
                    break;
                }

                catch (Exception ex)
                {
                    eMailSendingFailed = "(" + tryCounter + ".Attempt) Sending mail failed becuase: " + ex.Message;
                    Logger.Log(eMailSendingFailed, Logger.MessageType.ERROR);//ex gives a report_intern of problems
                    client.Dispose();
                    continue;
                }

            }

            #endregion

        }

        public static void SendMail()
        {
            for(;NotificationGetParams.NotificPackQueue.Count != 0;)
            {
                lock (lockQueueForReading)
                {
                    tempNotificPackIn = (NotificPackIn)NotificationGetParams.NotificPackQueue.Dequeue();
                }

                SendMailDo(tempNotificPackIn.eMailToPackIn, tempNotificPackIn.subjectPackIn, tempNotificPackIn.bodyPackIn, tempNotificPackIn.retryCounterPackIn,
                tempNotificPackIn.eMailFrom_specialPackIn,tempNotificPackIn.smtpClient_specialPackIn,  tempNotificPackIn.password_specialPackIn, tempNotificPackIn.port_specialPackIn,
                tempNotificPackIn.highPriorityPackIn, tempNotificPackIn.eMailToCCPackIn, tempNotificPackIn.eMailToBCCPackIn, tempNotificPackIn.eMailAttachmentPackIn); 
            }            
        }
        #endregion
    }
}