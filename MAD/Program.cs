using System;

using MAD.jobSys;
using MAD.cli;

namespace MAD
{
    class Program
    {
        /* MAD - Network Monitoring v0.0.2.9 RC1 */

        private const string dataPath = "data";

        static int Main(string[] args)
        {
            JobSystem js = new JobSystem(dataPath, 100);

            CLIServer cliServer = new CLIServer(dataPath, 999);
            CLI cli = new CLI(dataPath, js, cliServer);
            

            if (args.Length == 0)
            { 
                // START GUI
                cli.Start();
            }
            else if (args.Length == 1)
            {
                switch (args[0])
                {
                    case "-console":
                        cli.Start();
                        break;
                    default:
                        Console.WriteLine("ERROR! Argument '" + args[0] + "' not known!\nPress any key to close ...");
                        Console.ReadKey();
                        return 1;
                }
            }
            else
            {
                Console.WriteLine("ERROR! Too many arguments!\nPress any key to close ...");
                Console.ReadKey();
                return 1;
            }

            return 0;
        }
    }
}
