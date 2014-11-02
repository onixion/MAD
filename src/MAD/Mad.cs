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

            // -------------------------
            // init components

            JobSystem js = new JobSystem();
            DHCPReader dhcpReader = new DHCPReader(js);

            NotificationSystem.SetOrigin(MadConf.conf.SMTP_SERVER, new System.Net.Mail.MailAddress(MadConf.conf.SMTP_USER), MadConf.conf.SMTP_PASS, MadConf.conf.SERVER_PORT);

            // -------------------------

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
                        if (MadConf.conf.LOG_MODE)
                            Logger.Log("Programm Start. CLI Start.", Logger.MessageType.INFORM);
                        CLI cli = new CLI(js, dhcpReader);
                        cli.Start();
                        break;
                    case "-cliserver":
                        Logger.Log("Programm Start. CLI Server Start.", Logger.MessageType.INFORM);
                        try
                        {
                            CLIServer cliServer = new CLIServer(MadConf.conf.SERVER_CERT, js);
                            cliServer.Start();
                            Console.WriteLine("Server listening on port " + cliServer.serverPort + ".");
                            if (MadConf.conf.LOG_MODE)
                                Logger.Log("CLIServer started on port " + cliServer.serverPort, Logger.MessageType.ERROR);
                            Console.ReadKey();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Could not start server: " + e.Message);
                            if (MadConf.conf.LOG_MODE)
                                Logger.Log("CLIServer could not start: " + e.Message, Logger.MessageType.ERROR);
                            Console.ReadKey();
                        }
                        break;
                    default:
                        Console.WriteLine("ERROR! Argument '" + args[0] + "' not known!\nPress any key to close ...");
                        if (MadConf.conf.LOG_MODE)
                            Logger.Log("Programm Aborted. False Call Argument!", Logger.MessageType.EMERGENCY);
                        Logger.ForceWriteToLog();
                        Console.ReadKey();
                        return 1;
                }
            }
            else
            {
                Console.WriteLine("ERROR! Too many arguments!\nPress any key to close ...");
                if (MadConf.conf.LOG_MODE)
                {
                    Logger.Log("Programm Aborted. Too many arguments!", Logger.MessageType.EMERGENCY);
                    Logger.ForceWriteToLog();
                }
                Console.ReadKey();
                return 1;
            }

            js.Shutdown();

            if (MadConf.conf.LOG_MODE)
            {
                Logger.Log("Programm Exited Successfully. See Ya!", Logger.MessageType.INFORM);
                Logger.ForceWriteToLog();
            }

            return 0;
        }
    }
}
