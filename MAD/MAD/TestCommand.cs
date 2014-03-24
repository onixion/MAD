using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAD
{
    class TestCommand : Command
    {
        public TestCommand()
        {
            mainCommand = "test";

            requiredIndicators = new List<string>();
            requiredIndicators.Add("-a");
            requiredIndicators.Add("-d");
        }

        public override void Execute()
        {

        }
    }
}
