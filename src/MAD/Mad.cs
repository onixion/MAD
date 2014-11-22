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

using MadNet;
using System.Text;

namespace MAD
{
    public class Mad
    {
        public static readonly string DATADIR = Path.Combine("data");
        public static readonly string CONFFILE = Path.Combine(DATADIR, "mad.conf");

        [STAThread]
        public static int Main(string[] args)
        {
            // load config-file
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
                Console.WriteLine("(CONFIG) Default config may not use all possible features!");
            }

            // init components
            JobSystem js = new JobSystem();
            DHCPReader dhcpReader = new DHCPReader(js);
            
            NotificationSystem.SetOrigin(MadConf.conf.SMTP_SERVER, new System.Net.Mail.MailAddress(MadConf.conf.SMTP_USER), MadConf.conf.SMTP_PASS, MadConf.conf.SERVER_PORT);
            MailNotification.Start();

            // start interface
            if (args.Length == 0)
            { 
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
                        if (MadConf.conf.LOG_MODE)
                            Logger.Log("Programm Start. CLI Start.", Logger.MessageType.INFORM);
                        CLI cli = new CLI(js, dhcpReader);
                        cli.Start();
                        break;

                    case "-cliserver":
                        Logger.Log("Programm Start. CLI Server Start.", Logger.MessageType.INFORM);
                        try
                        {
                            CLIServer cliServer = new CLIServer(js);
                            cliServer.Start();

                            Console.WriteLine("(SERVER) Listening on port " + cliServer.serverPort + ".");
                            if (MadConf.conf.LOG_MODE)
                                Logger.Log("CLIServer started on port " + cliServer.serverPort, Logger.MessageType.INFORM);

                            Console.ReadKey(true);
                            cliServer.Stop();

                            Console.WriteLine("(SERVER) Stopped.");
                            if (MadConf.conf.LOG_MODE)
                                Logger.Log("Server stopped", Logger.MessageType.INFORM);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("(SERVER) Could not start: " + e.Message);
                            if (MadConf.conf.LOG_MODE)
                                Logger.Log("CLIServer could not start: " + e.Message, Logger.MessageType.ERROR);
                        }

                        PressAnyKeyToClose();
                        break;

                    default:
                        Console.WriteLine("ERROR! Argument '" + args[0] + "' not known!");
                        if (MadConf.conf.LOG_MODE)
                        {
                            Logger.Log("Programm Aborted. False Call Argument!", Logger.MessageType.EMERGENCY);
                            Logger.ForceWriteToLog();
                        }

                        PressAnyKeyToClose();
                        break;
                }
            }
            else
            {
                Console.WriteLine("ERROR! Too many arguments!");
                if (MadConf.conf.LOG_MODE)
                {
                    Logger.Log("Programm Aborted. Too many arguments!", Logger.MessageType.EMERGENCY);
                    Logger.ForceWriteToLog();
                }

                PressAnyKeyToClose();
            }

            js.Shutdown();
            MailNotification.Stop();

            if (MadConf.conf.LOG_MODE)
            {
                Logger.Log("Programm Exited Successfully. See Ya!", Logger.MessageType.INFORM);
                Logger.ForceWriteToLog();
            }

            return 0;
        }

        private static void PressAnyKeyToClose()
        {
            Console.WriteLine("Press any key to close program ...");
            Console.ReadKey(true);
        }
    }
}
