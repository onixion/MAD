using System;
using System.Windows.Forms;

using MAD.JobSystemCore;
using MAD.CLICore;
using MAD.CLIServerCore;
using MAD.DHCPReader;

namespace MAD
{
    class Mad
    {
        /* MAD - Network Monitoring v0.0.5.2 */

        private const string DATAPATH = "data";

        [STAThread]
        static int Main(string[] args)
        {
            Console.WriteLine("WARNING! THIS SOFTWARE IS STILL UNDER DEVELOMPENT!");

            JobSystem js = new JobSystem();
            MACFeeder macFeeder = new MACFeeder();

            if (args.Length == 0)
            { 
                // No args -> start gui.
                throw new NotImplementedException("NO GUI!");
            }
            else if (args.Length == 1)
            {
                switch (args[0])
                {
                    case "-cli":
                        CLI cli = new CLI(DATAPATH, js, macFeeder);
                        cli.Start();
                        break;
                    case "-cliserver":
                        CLIServer cliServer = new CLIServer(999, true, false, js);
                        cliServer.Start();
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
