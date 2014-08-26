using System;
using System.Windows.Forms;
using System.IO;
using System.Security.AccessControl;

using MAD.JobSystemCore;
using MAD.CLICore;
using MAD.CLIServerCore;
using MAD.DHCPReader;
using MAD.Logging;

namespace MAD
{
    class Mad
    {
        /* MAD - Network Monitoring v0.0.5.5 */

        public static readonly string VERSION = "v0.0.6.0";
        public static readonly string DATADIR = Path.Combine("data");

        [STAThread]
        static int Main(string[] args)
        {
            Console.WriteLine("WARNING! THIS SOFTWARE IS STILL UNDER DEVELOMPENT!");
            if (!Directory.Exists(DATADIR))
                Directory.CreateDirectory(DATADIR);

            JobSystem js = new JobSystem();
            MACFeeder macFeeder = new MACFeeder();

            if (Logger.PathFileExists())
                Logger.ReadPathToLogFile();
            else
                Logger.CreateNewPathFile();

            if (args.Length == 0)
            { 
                // No args -> start gui.
                Logger.Log("Programm Start. GUI Start.", Logger.MessageType.INFORM);
                Logger.Log("Programm Aborted. No GUI!", Logger.MessageType.EMERGENCY);
                Logger.ForceWriteToLog();

                throw new NotImplementedException("NO GUI!");
            }
            else if (args.Length == 1)
            {
                switch (args[0])
                {
                    case "-cli":
                        Logger.Log("Programm Start. CLI Start.", Logger.MessageType.INFORM);

                        CLI cli = new CLI(js, macFeeder);
                        cli.Start();
                        break;
                    case "-cliserver":
                        Logger.Log("Programm Start. CLI Server Start.", Logger.MessageType.INFORM);

                        CLIServer cliServer = new CLIServer(999, true, false, js);
                        cliServer.Start();
                        break;
                    default:
                        Logger.Log("Programm Aborted. False Call Argument!", Logger.MessageType.EMERGENCY);
                        Logger.ForceWriteToLog();

                        Console.WriteLine("ERROR! Argument '" + args[0] + "' not known!\nPress any key to close ...");
                        Console.ReadKey();
                        return 1;
                }
            }
            else
            {
                Logger.Log("Programm Aborted. Too many arguments!", Logger.MessageType.EMERGENCY);
                Logger.ForceWriteToLog();

                Console.WriteLine("ERROR! Too many arguments!\nPress any key to close ...");
                Console.ReadKey();
                return 1;
            }

            Logger.ForceWriteToLog();
            return 0;
        }
    }
}
