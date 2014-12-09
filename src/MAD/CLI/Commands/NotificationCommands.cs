using System;
using System.Net.Mail;

using MAD.JobSystemCore;
using MAD.Notification;

namespace MAD.CLICore
{
    public class SetMailSettingsCommand : NotificationConCommand
    {
        public SetMailSettingsCommand()
        {
            description = "This command sets the default notification for all jobs.";
        }

        public override string Execute(int consoleWidth)
        {
            JobNotificationSettings _settings = ParseJobNotificationSettings(pars);

            return "<color><red>Default mail-settings set.";
        }
    }

}
