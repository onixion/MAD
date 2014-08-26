using System;
using System.Net;
using System.Net.Mail;
using MAD.Logging;

namespace Notification
{
    public class NotificationSystem
    {
        //#Declaration

        //Declaration of static Parameters (Public)
        public MailAddress eMailFrom;
        public string smtpClient;
        public int port;
        public string messageSuccess, messageFailed, attempt;
        //_[DefaultValue("")]
        


        //Declaration of static Parameters (Private)
        private string password;
        private bool eMailSent = false;
        
        
        //Decleration of Objects
        private SmtpClient client;
        private Object thisLock = new Object();
        public 
        MailMessage mail;


        //#Methods

        //Method for getting basic information (static (have to set once))
        public NotificationSystem(string smtpClient,MailAddress eMailFrom, string password,  int port)
        {
            //Convertation of income to intern
            this.smtpClient = smtpClient;
            this.eMailFrom = eMailFrom;
            this.password = password;
            this.port = port;
        }
        
        //Method to send mail with mail parameters (dynamic (have to set for every mail))
        public bool SendMail(MailAddress[] eMailTo, string subject, string body, bool highPriority, int retryCounter,
            MailAddress[] eMailToCC = null, MailAddress[] eMailToBCC = null, Attachment[] eMailAttachment = null)
        {
            lock (thisLock)
            {
                //Declaration of Variables
                int tryCounter = 1;

                //Initialization of mail and setting parameters
                mail = new MailMessage();

                mail.From = eMailFrom;

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
                        client = new SmtpClient(smtpClient, port);
                        lock (client) { };
                        client.Credentials = new NetworkCredential(eMailFrom.ToString(), password);
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
                        Logger.Log(messageFailed, Logger.MessageType.ERROR);//ex gives a report of problems
                    }

                } return eMailSent;
            }
        }
    }
}