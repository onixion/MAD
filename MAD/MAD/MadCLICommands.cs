using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAD
{
    class HelpCommand : Command
    {
        MadCLI cli;

        public HelpCommand(MadCLI cli) 
        {
            this.cli = cli;
        }

        public override int Execute()
        {
            Console.WriteLine();
            Console.WriteLine(" <---- H E L P ---- P A G E ---->");
            Console.WriteLine();
            Headline(" <-- General commands -->");
            Console.WriteLine();
            Console.WriteLine(" > help                      print this help page            (WORKS)");
            Console.WriteLine(" > version                   show all versions               (WORKS)");
            Console.WriteLine(" > clear                     clear console                   (WORKS)");
            Console.WriteLine(" > logo                      print the MAD logo              (WORKS)");
            Console.WriteLine(" > exit/close                exit program                    (WORKS)");
            Console.WriteLine(" > info                      informations about the program  (WORKS)");
            Console.WriteLine(" > cursor -t <NEW CURSOR>    change the CLI cursor           (WORKS)");
            Console.WriteLine(" > gui                       start the GUI                   (NIY)");                
            Console.WriteLine();
            Headline(" <-- MadJobSystem commands -->");
            Console.WriteLine();
            Console.WriteLine(" > jobstatus [-id <ID>]      status of the jobs              (WORKS)");
            Console.WriteLine(" > jobadd -t <TYPE> -n <JOBNAME> -ip <IPADDRESS> -d <DELAY>  (WORKS)");
            Console.WriteLine("     > -t <TYPE=PING> -ttl <TTL>");
            Console.WriteLine("     > -t <TYPE=HTTP> -p <PORT>");
            Console.WriteLine("     > -t <TYPE=PORT> -p <PORT>");
            Console.WriteLine("                             add a job to the jobsystem      (WORKS)");
            Console.WriteLine();
            Headline(" <-- MadNotificationSystem commands -->");
            Console.WriteLine(" ______________________________________________________");
            Console.WriteLine();
            return 0;
        }

        public void Headline(string text)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(text);
            Console.ForegroundColor = cli.textColor;
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
        MadCLI cli;

        public LogoCommand(MadCLI cli)
        {
            this.cli = cli;
        }

        public override int Execute()
        {
            cli.PrintLogo();
            return 0;
        }
    }

    class ExitCommand : Command
    {
        public ExitCommand() { }

        public override int Execute()
        {
            Environment.Exit(0);
            return 0;
        }
    }

    class VersionCommand : Command
    {
        MadCLI cli;

        public VersionCommand(MadCLI cli) 
        {
            this.cli = cli;
        }

        public override int Execute()
        {
            Console.WriteLine(@" __  __ 2 _   ____ ");
            Console.WriteLine(@"|  \/  |/ _ \|  _  \    MadCLI VERSION                  v " + cli.cliVersion);
            Console.WriteLine(@"| .  . / /_\ \ | | |    MadNotificationSystem           v " + "unknown");
            Console.WriteLine(@"| |\/| |  _  | | | |    MadJobSystem                    v " + "unknown");
            Console.WriteLine(@"| |  | | | | | |_/ |    MadMemoryManagmentSysystem      v " + "unknown");
            Console.WriteLine(@"|_|  |_\_| |_/____/ ________________________________________________");
            Console.WriteLine();
            return 0;
        }
    }

    class CursorCommand : Command
    {
        private MadCLI cli;

        public CursorCommand(MadCLI cli)
        {
            this.cli = cli;
            requiredIndicators.Add(new object[] {"t", typeof(string)}); // TEXT
            //optionalIndicators.Add("c"); // COLOR
        }

        public override int Execute()
        {
            cli.cursor = GetArgument("t") + " ";
            return 0;
        }
    }
}