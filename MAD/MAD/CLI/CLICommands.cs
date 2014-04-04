﻿using System;

namespace MAD
{
    class HelpCommand : Command
    {
        public HelpCommand() {}

        public override int Execute()
        {
            Console.WriteLine();
            StartEndHeadline(" <---- H E L P ---- P A G E ---->");
            Console.WriteLine();
            Headline(" <-- General commands -->");
            Console.WriteLine();
            Console.WriteLine(" > help                      print this help page");
            Console.WriteLine(" > versions                  show versions");
            Console.WriteLine(" > clear                     clear console");
            Console.WriteLine(" > logo                      print the MAD logo");
            Console.WriteLine(" > exit/close                exit program");
            Console.WriteLine(" > info                      informations about the program");
            Console.WriteLine(" > cursor -t <NEW CURSOR>    change the CLI cursor");
            Console.WriteLine(" > gui                       start the GUI (NIY)");                
            Console.WriteLine();
            Headline(" <-- MadJobSystem commands -->");
            Console.WriteLine();
            Console.WriteLine(" > jobsystem status          status of the job-system");
            Console.WriteLine(" > job status [-id <ID>]     status of the jobs");
            Console.WriteLine(" > job add ping -n <JOB NAME> -ip <IPADDRESS> -d <DELAY> -ttl <TTL>");
            Console.WriteLine("                             add a ping-job");
            Console.WriteLine(" > job add http -n <JOB NAME> -ip <IPADDRESS> -d <DELAY> -p <PORT>");
            Console.WriteLine("                             add a http-job");
            Console.WriteLine(" > job add port -n <JOB NAME> -ip <IPADDRESS> -d <DELAY> -p <PORT>");
            Console.WriteLine("                             add a port-job");
            Console.WriteLine(" > job remove -id <JOB ID>   remove a job");
            Console.WriteLine(" > job start -id <JOB ID>    start a job");
            Console.WriteLine(" > job stop -id <JOB ID>     stop a job");
            Console.WriteLine();
            Headline(" <-- MadMemoryManagmentSystem commands -->");
            Console.WriteLine();
            Headline(" <-- MadNotificationSystem commands -->");
            Console.WriteLine();
            StartEndHeadline(" <---- E N D ---->");
            Console.WriteLine();
            return 0;
        }

        public void StartEndHeadline(string text)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(text);
            Console.ForegroundColor = MadComponents.components.cli.textColor;
        }

        public void Headline(string text)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(text);
            Console.ForegroundColor = MadComponents.components.cli.textColor;
        }
    }

    class ClearCommand : Command
    {
        public ClearCommand() { }

        public override int Execute()
        {
            Console.Clear();
            return 0;
        }
    }

    class LogoCommand : Command
    {
        public LogoCommand() {}

        public override int Execute()
        {
            MadComponents.components.cli.PrintLogo();
            return 0;
        }
    }

    class ExitCommand : Command
    {
        public ExitCommand() {}

        public override int Execute()
        {
            Environment.Exit(0);
            return 0;
        }
    }

    class VersionsCommand : Command
    {
        public VersionsCommand() {}

        public override int Execute()
        {
            Console.WriteLine("Mad-Project VERSION " + MadComponents.components.version);
            Console.WriteLine("MadCLI VERSION VERSION " + MadComponents.components.cli.version);
            Console.WriteLine("MadNotificationSystem VERSION " + "unknown");
            Console.WriteLine("MadJobSystem VERSION " + MadComponents.components.jobSystem.version);
            Console.WriteLine("MadMemoryManagmentSysystem VERSION " + "unknown");
            return 0;
        }
    }

    class InfoCommand : Command
    {
        public InfoCommand() { }

        public override int Execute()
        {
            Console.WriteLine();
            Console.WriteLine("███╗   ███2 █████╗ ██████╗ ");
            Console.WriteLine("████╗ ████║██╔══██╗██╔══██╗");
            Console.WriteLine("██╔████╔██║███████║██║  ██║");
            Console.WriteLine("██║╚██╔╝██║██╔══██║██║  ██║");
            Console.WriteLine("██║ ╚═╝ ██║██║  ██║██████╔╝");
            Console.WriteLine("╚═╝     ╚═╝╚═╝  ╚═╝╚═════╝ ");
            Console.WriteLine();
            Console.WriteLine("Network Monitoring Program written fully in C#.");
            Console.WriteLine();
            return 0;
        }
    }

    class CursorCommand : Command
    {
        public CursorCommand()
        {
            requiredIndicators.Add(new object[] {"t", false, typeof(String)});
        }

        public override int Execute()
        {
            MadComponents.components.cli.cursor = GetArgument("t") + " ";
            return 0;
        }
    }
}