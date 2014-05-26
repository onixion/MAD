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

            // IMPORTANT!
            // WHEN YOU PUSH THIS TO GITHUB, MAKE SURE THE MAIN METHOD (THIS ON HERE) IS EMPTY.
            // IF THIS IS NOT EMPTY, THE MERGE PROCESS COULD CAUSE AN ERROR. TO AVOID THAT JUST
            // KEEP THIS METHOD HERE CLEAN. THANKS!

            // Test your underneath here:


           

            return 0;
        }
    }
}
