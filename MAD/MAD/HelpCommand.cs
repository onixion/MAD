using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAD
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
            Console.WriteLine("----< HELP PAGE >---------------------------------------");
            Console.WriteLine("--< general commands >----------------------------------");
            Console.WriteLine();
            Console.WriteLine(" help                        print this help page");
            Console.WriteLine();
        }
    }
}
