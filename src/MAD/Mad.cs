using System;
using System.Windows.Forms;
using System.IO;
using System.Security.AccessControl;

using MAD.JobSystemCore;
using MAD.CLICore;
using MAD.CLIServerCore;
using MAD.MacFinders;
using MAD.Logging;
using MAD.Notification;

namespace MAD
{
    public class Mad
    {
        public static readonly string DATADIR = Path.Combine("data");
        public static readonly string CONFFILE = Path.Combine(DATADIR, "mad.conf");

        [STAThread]
        static int Main(string[] args)
       {
            Console.WriteLine("(WARNING) THIS SOFTWARE IS STILL UNDER DEVELOPMENT!");
            Console.WriteLine("(WARNING) DO NOT RELY ON THIS SOFTWARE!");
            
            MadConf.TryCreateDir(DATADIR);
            if (File.Exists(CONFFILE))
            {
                try
                {
                    MadConf.LoadConf(CONFFILE);
                    Console.WriteLine("(CONFIG) Config loaded.");
                }
                catch (Exception e)
                {
                    Console.WriteLine("(CONFIG) Config could not be loaded: " + e.Message);
                    MadConf.SetToDefault();
                    Console.WriteLine("(CONFIG) Loaded default config.");
                    Console.WriteLine("(CONFIG) Default config does not use all possible features!");
                }
            }
            else
            {
                Console.WriteLine("(CONFIG) No config file found!");
                MadConf.SetToDefault();
                Console.WriteLine("(CONFIG) Loaded default config.");
                MadConf.SaveConf(CONFFILE);
                Console.WriteLine("(CONFIG) Saved default config to '" + CONFFILE + "'.");
                Console.WriteLine("(CONFIG) Default config does not use all possible features!");
            }

            JobSystem js = new JobSystem();
            DHCPReader dhcpReader = new DHCPReader(js);

            NotificationSystem.SetOrigin(MadConf.conf.SMTP_SERVER, new System.Net.Mail.MailAddress(MadConf.conf.SMTP_USER), MadConf.conf.SMTP_PASS, MadConf.conf.SERVER_PORT);

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
                        CLI cli = new CLI(js, dhcpReader);
                        cli.Start();
                        break;
                    case "-cliserver":
                        Logger.Log("Programm Start. CLI Server Start.", Logger.MessageType.INFORM);
                        try
                        {
                            CLIServer cliServer = new CLIServer(Path.Combine(DATADIR, "certificate.ptx"), js);
                            cliServer.Start();
                            Console.ReadKey();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Could not start server: " + e.Message);
                            Console.ReadKey();
                        }
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

            js.Shutdown();

            Logger.Log("Programm Exited Successfully. See Ya!", Logger.MessageType.INFORM);
            Logger.ForceWriteToLog();

            return 0;
        }
    }
}
