using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            Console.WriteLine(" > help                      print this help page            (WORKS)");
            Console.WriteLine(" > versions                  show versions                   (WORKS)");
            Console.WriteLine(" > clear                     clear console                   (WORKS)");
            Console.WriteLine(" > logo                      print the MAD logo              (WORKS)");
            Console.WriteLine(" > exit/close                exit program                    (WORKS)");
            Console.WriteLine(" > info                      informations about the program  (WORKS)");
            Console.WriteLine(" > cursor -t <NEW CURSOR>    change the CLI cursor           (WORKS)");
            Console.WriteLine(" > gui                       start the GUI                   (NIY)");                
            Console.WriteLine();
            Headline(" <-- MadJobSystem commands -->");
            Console.WriteLine();
            Console.WriteLine(" > jobsystem status          status of the job-system        (WORKS)");
            Console.WriteLine(" > job status [-id <ID>]     status of the jobs              (WORKS)");
            Console.WriteLine(" > job add -t <TYPE> -n <JOBNAME> -ip <IPADDRESS> -d <DELAY> (WORKS)");
            Console.WriteLine("     > -t <TYPE=PING> -ttl <TTL>");
            Console.WriteLine("     > -t <TYPE=HTTP> -p <PORT>");
            Console.WriteLine("     > -t <TYPE=PORT> -p <PORT>");
            Console.WriteLine("                             add a job to the jobsystem      (WORKS)");
            Console.WriteLine(" > job remove -id <JOB ID>   remove a job from the jobsystem (WORKS)");
            Console.WriteLine(" > job start -id <JOB ID>    start a job from the jobsystem  (WORKS)");
            Console.WriteLine(" > job stop -id <JOB ID>     stop a job from the jobsystem   (WORKS)");
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
            int consoleWidth = Console.BufferWidth;
            Console.WriteLine();
            Console.CursorLeft = consoleWidth / 2 - 28 / 2;
            Console.WriteLine("███╗   ███2 █████╗ ██████╗ ");
            Console.CursorLeft = consoleWidth / 2 - 28 / 2;
            Console.WriteLine("████╗ ████║██╔══██╗██╔══██╗");
            Console.CursorLeft = consoleWidth / 2 - 28 / 2;
            Console.WriteLine("██╔████╔██║███████║██║  ██║");
            Console.CursorLeft = consoleWidth / 2 - 28 / 2;
            Console.WriteLine("██║╚██╔╝██║██╔══██║██║  ██║");
            Console.CursorLeft = consoleWidth / 2 - 28 / 2;
            Console.WriteLine("██║ ╚═╝ ██║██║  ██║██████╔╝");
            Console.CursorLeft = consoleWidth / 2 - 28 / 2;
            Console.WriteLine("╚═╝     ╚═╝╚═╝  ╚═╝╚═════╝ ");
            Console.WriteLine();
            Console.CursorLeft = consoleWidth / 2 - 24;
            Console.WriteLine("Network Monitoring Program written fully in C#.");
            return 0;
        }
    }

    class CursorCommand : Command
    {
        public CursorCommand()
        {
            requiredIndicators.Add(new object[] {"t", typeof(string)});
        }

        public override int Execute()
        {
            MadComponents.components.cli.cursor = GetArgument("t") + " ";
            return 0;
        }
    }
}