using System.Net.Mail;
using System;


namespace MAD.Notification
{
    class Program
    {
        static void Main(string[] args)
        {
            
            
            NotificationGetParams.SetSendMail("Fick de scheise", " Fick sie sowas von", 3);
            for (; Notification.NotificationGetParams.sendMailThread.IsAlive == true; )
            {
                Console.WriteLine("Hi");
            }
        }
    }
}
