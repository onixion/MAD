using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAD
{
    class HelpCommand : Command
    {
        public HelpCommand() { }

        public override int Execute()
        {
            Console.WriteLine();
            Console.WriteLine(" ______________________________________________________");
            Console.WriteLine();
            Console.WriteLine(" < HELP PAGE >");
            Console.WriteLine();
            Console.WriteLine(" < General commands >");
            Console.WriteLine();
            Console.WriteLine(" > help                print this help page            (WORKS)");
            Console.WriteLine(" > clear               clear console                   (WORKS)");
            Console.WriteLine(" > logo                print the MAD logo              (WORKS)");
            Console.WriteLine(" > exit/close          exit program                    (WORKS)");
            Console.WriteLine(" > info                informations about the program  (WORKS)");
            Console.WriteLine(" > cursor -t <NEW CURSOR>");      
            Console.WriteLine("                       change the CLI cursor           (NOT WORKING)");
            Console.WriteLine(" > gui                 start the GUI                   (NIY)");                
            Console.WriteLine();
            Console.WriteLine(" < JobSystem commands >");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(" < NotificationSystem commands >");
            Console.WriteLine(" ______________________________________________________");
            Console.WriteLine();
            return 0;
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

    class InfoCommand : Command
    {
        MadCLI cli;

        public InfoCommand(MadCLI cli) 
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
            Console.WriteLine(@"                             THE AMAZING TEAM");
            Console.WriteLine();
            Console.WriteLine(@"    PORCIC Alin  &  RANALTER Daniel  &  SINGH Manpreet  &  STOJANOVIC Marko");
            return 0;
        }
    }

    class CursorCommand : Command
    {
        private MadCLI cli;

        public CursorCommand(MadCLI cli)
        {
            this.cli = cli;

            requiredIndicators.Add("t"); // TEXT
            //optionalIndicators.Add("c"); // COLOR
        }

        public override int Execute()
        {
            if (!ArgumentEmpty("t"))
            {
                cli.cursor = GetArgument("t") + " ";
                return 0;
            }
            else
                return 1;
        }
    
    }
}