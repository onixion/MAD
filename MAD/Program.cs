﻿using System;
using System.Windows.Forms;

using MAD.JobSystemCore;
using MAD.CLICore;
using MAD.CLIServerCore;

namespace MAD
{
    class Program
    {
        /* MAD - Network Monitoring v0.0.3.6 */

        private const string dataPath = "data";

        [STAThread]
        static int Main(string[] args)
        {
            JobSystem js = new JobSystem(dataPath);
            CLIServer cliServer = new CLIServer(999, dataPath, js);

            if (args.Length == 0)
            { 
                // START GUI
                //Application.EnableVisualStyles();
                //Application.Run(new Form());

                CLI cli = new CLI(dataPath, js, cliServer);
                cli.Start();
            }
            else if (args.Length == 1)
            {
                switch (args[0])
                {
                    case "-console":
                        CLI cli = new CLI(dataPath, js, cliServer);
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
