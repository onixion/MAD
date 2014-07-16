using System;
using System.Windows.Forms;

using MAD.JobSystemCore;
using MAD.CLICore;
using MAD.CLIServerCore;

namespace MAD
{
    class Mad
    {
        /* MAD - Network Monitoring v0.0.4.0 RC1 */

        [STAThread]
        static int Main(string[] args)
        {
            JobSystem js = new JobSystem();
            CLIServer cliServer = new CLIServer(999, js);

            if (args.Length == 0)
            { 
                // No args -> start gui.
                //Application.EnableVisualStyles();
                //Application.Run(new Form());
                
                CLI cli = new CLI(js, cliServer);
                cli.Start();
            }
            else if (args.Length == 1)
            {
                switch (args[0])
                {
                    case "-console":
                        CLI cli = new CLI(js, cliServer);
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
