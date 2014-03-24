using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MadCLI
{
    class HelpCommand : Command
    {
        public HelpCommand()
        {
            mainCommand = "help";
        }

        public override void Execute()
        {
            Console.WriteLine();
            Console.WriteLine(" < HELP PAGE >");
            Console.WriteLine(" < General commands >");
            Console.WriteLine();
            Console.WriteLine(" help                print this help page");
            Console.WriteLine(" header              print the MAD header");
            Console.WriteLine();
            Console.WriteLine(" < JobSystem commands >");
            Console.WriteLine();
            Console.WriteLine();
        }
    }

    class ClearCommand : Command
    {
        public ClearCommand()
        {
            mainCommand = "clear";
        }

        public override void Execute()
        {
            Console.Clear();
        }
    }

    class HeaderCommand : Command
    {
        MadCLI cli;

        public HeaderCommand(MadCLI cli)
        {
            mainCommand = "header";

            this.cli = cli;
        }

        public override void Execute()
        {
            cli.PrintHeader();
        }
    }
}