using System;
using System.Net.Mail;

namespace MAD.CLICore
{
    public class SendMail : Command
    {
        public SendMail()
        {
            rPar.Add(new ParOption("s", "SMTP-Server", "", false, false, new Type[] { typeof(string) }));
            rPar.Add(new ParOption("p", "", "", false, false, new Type[] { typeof(int) }));
            rPar.Add(new ParOption("e", "EMAIL-FROM", "", false, false, new Type[] { typeof(MailAddress) }));
            rPar.Add(new ParOption("pass", "PASSWORD", "", false, false, new Type[] { typeof(string) }));
            rPar.Add(new ParOption("t", "EMAIL-TO", "", false, false, new Type[] { typeof(string) }));
            rPar.Add(new ParOption("m", "MESSAGE", "", false, false, new Type[] { typeof(string) }));
        }

        public override string Execute(int consoleWidth)
        {
            return "NIY";
        }
    }
}
