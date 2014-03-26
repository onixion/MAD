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
            Console.WriteLine(" < HELP PAGE >");
            Console.WriteLine(" < General commands >");
            Console.WriteLine();
            Console.WriteLine(" help                print this help page");
            Console.WriteLine(" clear               clear console");
            Console.WriteLine(" header              print the MAD header");
            Console.WriteLine(" exit/close          exit program");
            Console.WriteLine(" info                informations about the program");
            Console.WriteLine();
            Console.WriteLine(" < JobSystem commands >");
            Console.WriteLine();
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

    class HeaderCommand : Command
    {
        MadCLI cli;

        public HeaderCommand(MadCLI cli)
        {
            this.cli = cli;
        }

        public override int Execute()
        {
            cli.PrintHeader();
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
        public InfoCommand() { }

        public override int Execute()
        {
            Console.WriteLine("");
            return 0;
        }
    }
}