//just a Example Program for Testing



using System;
using System.Net.Mail;
using System.Net;

namespace Notification
{   
    class Program
    {
        static MailAddress eID = new MailAddress("mad.group@outlook.com");
        
        
        static void Main(string[] args)
        {
            NotificationSystem Config = new NotificationSystem("smtp-mail.outlook.com", eID, "Mad-21436587", 587);

            MailAddress[] _eIDF = new MailAddress[1]{new MailAddress("singh.manpreet@live.at")};
            MailAddress[] _eMailToCC = new MailAddress[1]{new MailAddress("singh.manpreet.iphone@gmail.com")};
            MailAddress[] _eMailToBCC = new MailAddress[1]{new MailAddress("singh.manpreet.iphone@gmail.com")};
            Attachment[] _data = new Attachment[1]{new Attachment(@"Attachment.txt")}; 

            Config.SendMail(_eIDF, "TestMail", "I_Am_Jack",true , 3, eMailToBCC: _eMailToBCC, eMailAttachment:_data);

            Console.ReadKey();
        }
    }
}
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            /*Notification eMailSend = new Notification();
                      eMailSend.SendMail(  , 
                                    , 
                                    "smtp-mail.outlook.com", 
                                    , 
                                    "alin.porcic@gmail.com", 
                                    "Try", 
                                    "Hej Spasst",
                                     3 );

            if(eMailSend==true){Console.WriteLine("Your email has successfully been sent");}
            if(eMailSend==false){Console.WriteLine("A problem has occurred\n Please check your username and password");}
            Console.ReadKey();*/
