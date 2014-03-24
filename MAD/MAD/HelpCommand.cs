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

            requiredIndicators = new List<string>();
            optionalIndicators = new List<string>();
            optionalIndicators.Add("-a");
        }

        public override void Execute()
        {
            Console.WriteLine("Help Page --------------- ");
        }
    }
}
