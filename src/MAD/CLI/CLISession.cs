using System;
using System.Reflection;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

using MAD.Database;
using MAD.MacFinders;
using MAD.CLICore;
using MAD.JobSystemCore;
using MAD.Logging;

using MadNet;

namespace MAD.CLIServerCore
{
    public class CLISession : CLIFramework
    {
        #region members

        private NetworkStream _stream;
        private AES _aes;

        private JobSystem _js;
        private DHCPReader _dhcpReader;
        private DB _db;

        #endregion

        #region constructor

        public CLISession(NetworkStream stream, AES aes, JobSystem js, DHCPReader dhcpReader, DB db)
            : base()
        {
            _stream = stream;
            _aes = aes;
            _js = js;
            _dhcpReader = dhcpReader;
            _db = db;
        }

        #endregion

        #region methods

        public void InitCommands()
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
            commands.Add(new CommandOptions("dhcp reader start", typeof(CatchBasicInfoStartCommand), new object[] { _dhcpReader }));                             //Outdated!
            commands.Add(new CommandOptions("dhcp reader stop", typeof(CatchBasicInfoStopCommand), new object[] { _dhcpReader }));                               //Outdated!
            commands.Add(new CommandOptions("dhcp reader set time", typeof(CatchBasicInfoSetTimeIntervallCommand), new object[] { _dhcpReader }));               //Outdated!
            commands.Add(new CommandOptions("print list", typeof(CatchBasicInfoPrintHostsCommand), new object[] { _dhcpReader }));
            commands.Add(new CommandOptions("arp reader list interfaces", typeof(PrintArpReadyInterfaces), null));
            commands.Add(new CommandOptions("arp reader start", typeof(ArpReaderStart), new object[] { _js }));
            commands.Add(new CommandOptions("arp reader stop", typeof(StopArpReaderCommand), null));

            commands.Add(new CommandOptions("check list", typeof(CheckList), new object[] { _js }));

            // JOBSYSTEM
            commands.Add(new CommandOptions("js", typeof(JobSystemStatusCommand), new object[] { _js }));
            commands.Add(new CommandOptions("js load", typeof(JobSystemLoadTableCommand), new object[] { _js }));
            commands.Add(new CommandOptions("js save", typeof(JobSystemSaveTableCommand), new object[] { _js }));
            commands.Add(new CommandOptions("js nodes", typeof(JobSystemStatusNodesCommand), new object[] { _js }));
            commands.Add(new CommandOptions("js jobs", typeof(JobSystemStatusJobsCommand), new object[] { _js }));

            // Schedule
            commands.Add(new CommandOptions("schedule start", typeof(JobScheduleStartCommand), new object[] { _js }));
            commands.Add(new CommandOptions("schedule stop", typeof(JobScheduleStopCommand), new object[] { _js }));

            // NODES
            commands.Add(new CommandOptions("node add", typeof(JobSystemAddNodeCommand), new object[] { _js }));
            commands.Add(new CommandOptions("node remove", typeof(JobSystemRemoveNodeCommand), new object[] { _js }));
            commands.Add(new CommandOptions("node edit", typeof(JobSystemEditNodeCommand), new object[] { _js }));
            commands.Add(new CommandOptions("node start", typeof(JobSystemStartNodeCommand), new object[] { _js }));
            commands.Add(new CommandOptions("node stop", typeof(JobSystemStartNodeCommand), new object[] { _js }));
            //commands.Add(new CommandOptions("node sync", typeof(JobSystemSyncNodeCommand), new object[] { js, macFeeder }));
            commands.Add(new CommandOptions("node save", typeof(JobSystemSaveNodeCommand), new object[] { _js }));
            commands.Add(new CommandOptions("node load", typeof(JobSystemLoadNodeCommand), new object[] { _js }));

            // JOBS
            commands.Add(new CommandOptions("job info", typeof(JobInfoCommand), new object[] { _js }));
            commands.Add(new CommandOptions("job desc", typeof(JobOutDescInfoCommand), new object[] { _js }));
            commands.Add(new CommandOptions("job start", typeof(JobSystemStartJobCommand), new object[] { _js }));
            commands.Add(new CommandOptions("job stop", typeof(JobSystemStopJobCommand), new object[] { _js }));
            commands.Add(new CommandOptions("job remove", typeof(JobSystemRemoveJobCommand), new object[] { _js }));
            commands.Add(new CommandOptions("job edit", typeof(JobSystemEditJobCommand), new object[] { _js }));
            commands.Add(new CommandOptions("job setmail", typeof(JobSystemSetJobMailSettingsCommand), new object[] { _js }));

            commands.Add(new CommandOptions("job add ping", typeof(JobSystemAddPingCommand), new object[] { _js }));
            commands.Add(new CommandOptions("job add http", typeof(JobSystemAddHttpCommand), new object[] { _js }));
            commands.Add(new CommandOptions("job add port", typeof(JobSystemAddPortCommand), new object[] { _js }));
            commands.Add(new CommandOptions("job add read traffic", typeof(JobSystemAddReadTrafficCommand), new object[] { _js }));
            commands.Add(new CommandOptions("job add hostdetect", typeof(JobSystemAddHostDetectCommand), new object[] { _js }));
            commands.Add(new CommandOptions("job add ftpcheck", typeof(JobSystemAddCheckFtpCommand), new object[] { _js }));
            commands.Add(new CommandOptions("job add dnscheck", typeof(JobSystemAddCheckDnsCommand), new object[] { _js }));
            commands.Add(new CommandOptions("job add snmpcheck", typeof(JobSystemAddCheckSnmpCommand), new object[] { _js }));

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
            commands.Add(new CommandOptions("db show", typeof(DBShowTables), new object[] { _db }));
        }

        public void Start()
        {
            using(DataStringPacket _packet = new DataStringPacket(_stream))
            {
                _packet.ReceivePacket(_aes);
                _packet.data = GetBanner( System.Convert.ToInt32(_packet.data));
                _packet.SendPacket(_aes);
            }

            using (CLIPacket _cliP = new CLIPacket(_stream))
            {
                Command _command = null;

                while (true)
                {
                    _cliP.ReceivePacket(_aes);
                    _cliP.serverAnswer = CLIInterpreter(ref _command, _cliP.cliInput);

                    try
                    {
                        if (_cliP.serverAnswer == "VALID_PARAMETERS")
                            _cliP.serverAnswer = _command.Execute(_cliP.consoleWidth);
                    }
                    catch (Exception e)
                    {
                        _cliP.serverAnswer = e.Message;
                    }

                    _cliP.SendPacket(_aes);

                    if (_cliP.serverAnswer == "EXIT_CLI")
                        break;
                }
            }
        }

        #endregion
    }
}
