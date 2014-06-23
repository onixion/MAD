using System;
using System.Windows.Forms;

using MAD.jobSys;
using MAD.cli;

namespace MAD
{
    class Program
    {
        /* MAD - Network Monitoring v0.0.2.9 RC2 */

        private const string dataPath = "data";

        [STAThread]
        static int Main(string[] args)
        {
            JobSystem js = new JobSystem(dataPath);
            CLIServer cliServer = new CLIServer(dataPath, 999);
            CLI cli = new CLI(dataPath, js, cliServer);
            
            if (args.Length == 0)
            { 
                // START GUI
                Application.EnableVisualStyles();
                Application.Run(new Form());

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
