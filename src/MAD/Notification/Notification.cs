using System;
using System.Net;
using System.Net.Mail;
using MAD.Logging;

namespace MAD.Notification
{
    public static class NotificationSystem
    {
        //#Declaration

        //Declaration of static Parameters (Public)
        private static MailAddress eMailFrom_intern;
        private static string smtpClient_intern;
        private static int port_intern;
        public static string messageSuccess, messageFailed, attempt;


        //Declaration of static Parameters (Private)
        private static string password_intern;
        private static bool eMailSent = false;


        //Decleration of Objects
        private static SmtpClient client;
        private static Object thisLock = new Object();
        public static MailMessage mail;


        //#Methods
        #region Methods

        
        //Method for getting basic information (static (have to set once))
        public static void SetOrigin(string smtpClient, MailAddress eMailFrom, string password, int port)
        {
            //Convertation of income to intern
            smtpClient_intern = smtpClient;
            eMailFrom_intern = eMailFrom;
            password_intern = password;
            port_intern = port;
        }

        //Method to send mail with mail parameters (dynamic (have to set for every mail))
        //#1 for default Origin
        public static bool SendMail(MailAddress[] eMailTo, string subject, string body, int retryCounter,
            bool highPriority = false, MailAddress[] eMailToCC = null, MailAddress[] eMailToBCC = null, Attachment[] eMailAttachment = null)
        {
            return NotificationSystem.SendMail(eMailTo, subject, body, retryCounter, smtpClient_intern, eMailFrom_intern, password_intern, port_intern, 
                highPriority, eMailToCC, eMailToBCC, eMailAttachment);  
        }
        
        //#2 for Special Orign
        public static bool SendMail(MailAddress[] eMailTo, string subject, string body, int retryCounter, string smtpClient_special, MailAddress eMailFrom_special, string password_special, int port_special,
            bool highPriority = false, MailAddress[] eMailToCC = null, MailAddress[] eMailToBCC = null, Attachment[] eMailAttachment = null)
        {
            lock (thisLock)
            {
                //Declaration of Variables
                int tryCounter = 1;

                //Initialization of mail and setting parameters
                mail = new MailMessage();

                mail.From = eMailFrom_intern;

                foreach (MailAddress element in eMailTo)//to get all eMailIds from incoming Array
                {
                    mail.To.Add(element);
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

                mail.Subject = subject;
                mail.Body = body;


                if (highPriority == true)
                {
                    mail.Priority = MailPriority.High;
                }
                //Authentification and sending process
                for (eMailSent = false; (eMailSent == false) && (tryCounter <= retryCounter); tryCounter++)//to retry because shit happens
                {
                    try
                    {
                        attempt = tryCounter + ".Attempt";
                        Logger.Log(attempt, Logger.MessageType.INFORM);
                        //Initialization of SMTPclient and setting parameters and sending mail
                        client = new SmtpClient(smtpClient_special, port_special);
                        lock (client) { };
                        client.Credentials = new NetworkCredential(eMailFrom_special.ToString(), password_special);
                        client.EnableSsl = true;
                        client.Send(mail); //Send order
                        eMailSent = true;
                        messageSuccess = "(" + tryCounter + ".Attempt) Success";
                        Logger.Log(messageSuccess, Logger.MessageType.INFORM);
                        //Console.WriteLine("Success");

                    }

                    catch (Exception ex)
                    {
                        eMailSent = false;
                        messageFailed = "(" + tryCounter + ".Attempt) Sending mail failed becuase: " + ex.Message;
                        Logger.Log(messageFailed, Logger.MessageType.ERROR);//ex gives a report_intern of problems
                    }

                } return eMailSent;

            }
        }
         #endregion
    }
       
}