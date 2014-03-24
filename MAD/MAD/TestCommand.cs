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
        }

        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
