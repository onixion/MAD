using System;
using System.Collections.Generic;
using System.IO;

using MAD.JobSystemCore;
using MAD.CLIServerCore;
using MAD.MacFinders;
using MAD.Database;
using MAD.Logging;
using CLIIO;

namespace MAD.CLICore
{
    public class CLI : CLIFramework
    {
        #region members

        private string _cursor = "MAD> ";
        private ConsoleColor _cursorColor = ConsoleColor.Cyan;
        private ConsoleColor _inputColor = ConsoleColor.White;

        #endregion

        #region constructor

        public CLI(JobSystem js, DHCPReader dhcpReader, DB db)
            :base()
        {
            // GENERAL
            commands.Add(new CommandOptions("exit", typeof(ExitCommand), null));
            commands.Add(new CommandOptions("help", typeof(HelpCommand), new object[] { commands }));
            commands.Add(new CommandOptions("set-width", typeof(SetWidthCommand), null));
            commands.Add(new CommandOptions("colortest", typeof(ColorTestCommand), null));
            commands.Add(new CommandOptions("info", typeof(InfoCommand), null));

            commands.Add(new CommandOptions("conf", typeof(ConfShowCommand), null));
            
            // INTERNET CONECTIVITY
            commands.Add(new CommandOptions("check connection", typeof(ConnectivityTestCommand), null));

            // LOGGER
            commands.Add(new CommandOptions("change logBuffer", typeof(ChangeBufferSize), null));
            commands.Add(new CommandOptions("change logName", typeof(ChangeLogFileName), null));

            // MAC AND IP READER
            commands.Add(new CommandOptions("dhcp reader start", typeof(CatchBasicInfoStartCommand), new object[] { dhcpReader }));                             //Outdated!
            commands.Add(new CommandOptions("dhcp reader stop", typeof(CatchBasicInfoStopCommand), new object[] { dhcpReader }));                               //Outdated!
            commands.Add(new CommandOptions("dhcp reader set time", typeof(CatchBasicInfoSetTimeIntervallCommand), new object[] { dhcpReader }));               //Outdated!
            commands.Add(new CommandOptions("print list", typeof(CatchBasicInfoPrintHostsCommand), new object[] { dhcpReader }));
            commands.Add(new CommandOptions("arp reader list interfaces", typeof(PrintArpReadyInterfaces), null));
            commands.Add(new CommandOptions("arp reader start", typeof(ArpReaderStart), new object[] { js }));
            commands.Add(new CommandOptions("arp reader stop", typeof(StopArpReaderCommand), null));

            commands.Add(new CommandOptions("check list", typeof(CheckList), new object[] { js }));

            // JOBSYSTEM
            commands.Add(new CommandOptions("js", typeof(JobSystemStatusCommand), new object[] { js }));
            commands.Add(new CommandOptions("js load", typeof(JobSystemLoadTableCommand), new object[] { js }));
            commands.Add(new CommandOptions("js save", typeof(JobSystemSaveTableCommand), new object[] { js }));
            commands.Add(new CommandOptions("js nodes", typeof(JobSystemStatusNodesCommand), new object[] { js }));
            commands.Add(new CommandOptions("js jobs", typeof(JobSystemStatusJobsCommand), new object[] { js }));

            // Schedule
            commands.Add(new CommandOptions("schedule start", typeof(JobScheduleStartCommand), new object[] { js }));
            commands.Add(new CommandOptions("schedule stop", typeof(JobScheduleStopCommand), new object[] { js }));

            // NODES
            commands.Add(new CommandOptions("node add", typeof(JobSystemAddNodeCommand), new object[] { js }));
            commands.Add(new CommandOptions("node remove", typeof(JobSystemRemoveNodeCommand), new object[] { js }));
            commands.Add(new CommandOptions("node edit", typeof(JobSystemEditNodeCommand), new object[] { js }));
            commands.Add(new CommandOptions("node start", typeof(JobSystemStartNodeCommand), new object[] { js }));
            commands.Add(new CommandOptions("node stop", typeof(JobSystemStartNodeCommand), new object[] { js }));
            //commands.Add(new CommandOptions("node sync", typeof(JobSystemSyncNodeCommand), new object[] { js, macFeeder }));
            commands.Add(new CommandOptions("node save", typeof(JobSystemSaveNodeCommand), new object[] { js }));
            commands.Add(new CommandOptions("node load", typeof(JobSystemLoadNodeCommand), new object[] { js }));

            // JOBS
            commands.Add(new CommandOptions("job info", typeof(JobInfoCommand), new object[] { js }));
            commands.Add(new CommandOptions("job desc", typeof(JobOutDescInfoCommand), new object[] { js }));
            commands.Add(new CommandOptions("job start", typeof(JobSystemStartJobCommand), new object[] { js }));
            commands.Add(new CommandOptions("job stop", typeof(JobSystemStopJobCommand), new object[] { js }));
            commands.Add(new CommandOptions("job remove", typeof(JobSystemRemoveJobCommand), new object[] { js }));
            commands.Add(new CommandOptions("job edit", typeof(JobSystemEditJobCommand), new object[] { js }));
            commands.Add(new CommandOptions("job setmail", typeof(JobSystemSetJobMailSettingsCommand), new object[] { js }));

            commands.Add(new CommandOptions("job add ping", typeof(JobSystemAddPingCommand), new object[] { js }));
            commands.Add(new CommandOptions("job add http", typeof(JobSystemAddHttpCommand), new object[] { js }));
            commands.Add(new CommandOptions("job add port", typeof(JobSystemAddPortCommand), new object[] { js }));
            commands.Add(new CommandOptions("job add read traffic", typeof(JobSystemAddReadTrafficCommand), new object[] { js }));
            commands.Add(new CommandOptions("job add hostdetect", typeof(JobSystemAddHostDetectCommand), new object[] { js }));
            commands.Add(new CommandOptions("job add ftpcheck", typeof(JobSystemAddCheckFtpCommand), new object[] { js }));
            commands.Add(new CommandOptions("job add dnscheck", typeof(JobSystemAddCheckDnsCommand), new object[] { js }));
            commands.Add(new CommandOptions("job add snmpcheck", typeof(JobSystemAddCheckSnmpCommand), new object[] { js }));

            // SNMP
            commands.Add(new CommandOptions("snmpinterface", typeof(SnmpInterfaceCommand), null));

            // CLIServer (these commands cannot be used by cli!)
            /*
            commands.Add(new CommandOptions("cliserver", typeof(CLIServerInfo), new object[] { cliServer }));
            commands.Add(new CommandOptions("cliserver start", typeof(CLIServerStart), new object[] { cliServer }));
            commands.Add(new CommandOptions("cliserver stop", typeof(CLIServerStop), new object[] { cliServer }));
            commands.Add(new CommandOptions("cliserver changeport", typeof(CLIChangePort), new object[] { cliServer }));
            */

            // Database commands
            commands.Add(new CommandOptions("db show", typeof(DBShowTables), new object[] { db }));
            commands.Add(new CommandOptions("db jobs", typeof(DBJobs), new object[] { db }));
            commands.Add(new CommandOptions("db summary create", typeof(DBSumCreate), new object[] { db }));
            commands.Add(new CommandOptions("db add memo", typeof(DBMemoAdd), new object[] { js, db }));
            commands.Add(new CommandOptions("db show memo", typeof(DBMemoShow), new object[] { js, db }));

            Console.CancelKeyPress += new ConsoleCancelEventHandler(StrgCHandler);
        }

        #endregion

        #region methods

        public void Start()
        {
            CLIOutput.WriteToConsole(GetBanner(Console.BufferWidth) + "\n");

            while (true)
            {
                Command _command = null;

                Console.ForegroundColor = _cursorColor;
                Console.Write(_cursor);
                Console.ForegroundColor = _inputColor;
                string _cliInput = CLIInput.ReadInput(Console.CursorLeft);
                string _response;

                // This is the old method to read input from cli.
                //string _cliInput = Console.ReadLine();

                _cliInput = _cliInput.Trim();
                if (_cliInput != "")
                {
                    _response = CLIInterpreter(ref _command, _cliInput);
                    if (_response == "VALID_PARAMETERS")
                    {
                        try
                        {
                            _response = _command.Execute(Console.BufferWidth);
                            if (_response == "EXIT_CLI")
                                break;
                        }
                        catch (Exception e)
                        {
                            _response = "<color><red>" + e.Message;
                        }
                    }

                    CLIOutput.WriteToConsole(_response + "\n");
                }
            }
        }

        private static void StrgCHandler(object sender, ConsoleCancelEventArgs args)
        {
            Logger.Log("Exited by sending Ctrl+C", Logger.MessageType.INFORM);
            Logger.ForceWriteToLog();
        }

        #endregion
    }
}
