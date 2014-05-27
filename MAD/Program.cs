using System;

namespace MAD
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                // start gui
                MadComponents.components.cli.Start();
            }
            else if (args.Length == 1)
            {
                switch (args[0])
                { 
                    case "-console":
                        MadComponents.components.cli.Start();
                        break;
                    default:
                        return 1;
                }
            }

            // ignore more than one Argument (for now)

            return 0;
        }
    }
}
