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
        /* MAD - Network Monitoring v0.0.4.0 RC3 */

        private const string dataPath = "data";

        [STAThread]
        static int Main(string[] args)
        {
            Console.WriteLine("WARNING! THIS SOFTWARE IS STILL UNDER DEVELOMPENT!");

            JobSystem js = new JobSystem();
            MACFeeder macFeeder = new MACFeeder();

            if (args.Length == 0)
            { 
                // No args -> start gui.

                //Application.EnableVisualStyles();
                //Application.Run(new Form());

                CLI cli = new CLI(dataPath, js, macFeeder);
                cli.Start();
            }
            else if (args.Length == 1)
            {
                switch (args[0])
                {
                    case "-cli":
                        CLI cli = new CLI(dataPath, js, macFeeder);
                        cli.Start();
                        break;
                    case "-cliserver":
                        CLIServer cliServer = new CLIServer(999, js);
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
