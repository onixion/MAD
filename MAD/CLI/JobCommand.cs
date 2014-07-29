using System;
using System.Net.Mail;

namespace MAD.CLICore
{
    public abstract class JobCommand : Command
    {
        public JobCommand()
            : base()
        {
            rPar.Add(new ParOption("n", "JOB-NAME", "Name of the job.", false, false, new Type[] { typeof(string) }));
            rPar.Add(new ParOption("id", "NODE-ID", "ID of the node to add the job to.", false, false, new Type[] { typeof(int) }));
            oPar.Add(new ParOption("t", "TIME", "Delaytime or time on which th job should be executed.", false, true, new Type[] { typeof(Int32), typeof(string) }));
            oPar.Add(new ParOption("nAddr", "NOTIFICATION-Addresses", "Mailaddresses to send notification to.", false, true, new Type[] { typeof(MailAddress) }));
            oPar.Add(new ParOption("nPrio", "NOTIFICATION-Priority", "Priority of the mails.", false, true, new Type[] { typeof(string) }));
        }
    }
}
