using System;
using System.Collections.Generic;
using System.IO;

using MAD.JobSystemCore;
using MAD.CLIServerCore;
using MAD.DHCPReader;
using CLIIO;

namespace MAD.CLICore
{
    public class CLI : CLIFramework
    {
        #region members

        public string cursor = "MAD> ";
        private ConsoleColor _cursorColor = ConsoleColor.Cyan;
        private ConsoleColor _inputColor = ConsoleColor.White;

        #endregion

        #region constructor

        public CLI(JobSystem js, MACFeeder macFeeder)
            :base()
        {
            // GENERAL
            commands.Add(new CommandOptions("exit", typeof(ExitCommand), null));
            commands.Add(new CommandOptions("help", typeof(HelpCommand), new object[] { commands }));
            commands.Add(new CommandOptions("set-width", typeof(SetWidthCommand), null));
            commands.Add(new CommandOptions("colortest", typeof(ColorTestCommand), null));
            commands.Add(new CommandOptions("info", typeof(InfoCommand), null));

            commands.Add(new CommandOptions("conf", typeof(LoadConfigFileCommand), null));
            commands.Add(new CommandOptions("conf-default", typeof(LoadDefaultConfigCommand), null));
            commands.Add(new CommandOptions("conf-save", typeof(SaveConfigCommand), null));
            commands.Add(new CommandOptions("conf-show", typeof(ConfShowCommand), null));

            // MAIL-SETTINGS GLOBAL
            commands.Add(new CommandOptions("set-mail", typeof(SetMailSettingsCommand), null));

            // LOGGER
            commands.Add(new CommandOptions("change logBuffer", typeof(ChangeBufferSize), null));
            commands.Add(new CommandOptions("change log direction", typeof(ChangePathFile), null));
            commands.Add(new CommandOptions("change logName", typeof(ChangeLogFileName), null));

            // MAC AND IP READER
            commands.Add(new CommandOptions("mac finder start", typeof(CatchBasicInfoStartCommand), new object[] { macFeeder }));
            commands.Add(new CommandOptions("mac finder stop", typeof(CatchBasicInfoStopCommand), new object[] { macFeeder }));
            commands.Add(new CommandOptions("mac finder set time", typeof(CatchBasicInfoSetTimeIntervallCommand), new object[] { macFeeder }));
            commands.Add(new CommandOptions("mac finder print list", typeof(CatchBasicInfoPrintHostsCommand), new object[] { macFeeder }));

            // JOBSYSTEM
            commands.Add(new CommandOptions("js", typeof(JobSystemStatusCommand), new object[] { js }));
            commands.Add(new CommandOptions("js load", typeof(JobSystemLoadTableCommand), new object[] { js }));
            commands.Add(new CommandOptions("js save", typeof(JobSystemSaveTableCommand), new object[] { js }));
            commands.Add(new CommandOptions("js nodes", typeof(JobSystemStatusNodesCommand), new object[] { js }));
            commands.Add(new CommandOptions("js jobs", typeof(JobSystemStatusJobsCommand), new object[] { js }));

            // SCEDULE
            commands.Add(new CommandOptions("scedule", typeof(JobSceduleCommand), new object[] { js }));
            commands.Add(new CommandOptions("scedule start", typeof(JobSceduleStartCommand), new object[] { js }));
            commands.Add(new CommandOptions("scedule stop", typeof(JobSceduleStopCommand), new object[] { js }));

            // NODES
            commands.Add(new CommandOptions("node add", typeof(JobSystemAddNodeCommand), new object[] { js }));
            commands.Add(new CommandOptions("node remove", typeof(JobSystemRemoveNodeCommand), new object[] { js }));
            commands.Add(new CommandOptions("node start", typeof(JobSystemStartNodeCommand), new object[] { js }));
            commands.Add(new CommandOptions("node stop", typeof(JobSystemStartNodeCommand), new object[] { js }));
            commands.Add(new CommandOptions("node sync", typeof(JobSystemSyncNodeCommand), new object[] { js, macFeeder }));
            commands.Add(new CommandOptions("node save", typeof(JobSystemSaveNodeCommand), new object[] { js }));
            commands.Add(new CommandOptions("node load", typeof(JobSystemLoadNodeCommand), new object[] { js }));

            // JOBS
            commands.Add(new CommandOptions("job status", typeof(JobStatusCommand), new object[] { js }));
            commands.Add(new CommandOptions("job output", typeof(JobOutDescriptorListCommand), new object[] { js }));
            commands.Add(new CommandOptions("job remove", typeof(JobSystemRemoveJobCommand), new object[] { js }));
            commands.Add(new CommandOptions("job start", typeof(JobSystemStartJobCommand), new object[] { js }));
            commands.Add(new CommandOptions("job stop", typeof(JobSystemStopJobCommand), new object[] { js }));

            commands.Add(new CommandOptions("job add ping", typeof(JobSystemAddPingCommand), new object[] { js }));
            commands.Add(new CommandOptions("job add http", typeof(JobSystemAddHttpCommand), new object[] { js }));
            commands.Add(new CommandOptions("job add port", typeof(JobSystemAddPortCommand), new object[] { js }));
            commands.Add(new CommandOptions("job add hostdetect", typeof(JobSystemAddHostDetectCommand), new object[] { js }));
            commands.Add(new CommandOptions("job add ftpcheck", typeof(JobSystemAddCheckFtpCommand), new object[] { js }));
            commands.Add(new CommandOptions("job add dnscheck", typeof(JobSystemAddCheckDnsCommand), new object[] { js }));
            commands.Add(new CommandOptions("job add snmpcheck", typeof(JobSystemAddCheckSnmpCommand), new object[] { js }));

            commands.Add(new CommandOptions("job setmail", typeof(JobSystemSetJobMailSettingsCommand), new object[] { js }));
            commands.Add(new CommandOptions("job setrules", typeof(JobSystemSetJobRulesCommand), new object[] { js }));

            // SNMP
            commands.Add(new CommandOptions("snmpinterface", typeof(SnmpInterfaceCommand), null));

            // CLIServer (these commands cannot be used by cli!)
            /*
            commands.Add(new CommandOptions("cliserver", typeof(CLIServerInfo), new object[] { cliServer }));
            commands.Add(new CommandOptions("cliserver start", typeof(CLIServerStart), new object[] { cliServer }));
            commands.Add(new CommandOptions("cliserver stop", typeof(CLIServerStop), new object[] { cliServer }));
            commands.Add(new CommandOptions("cliserver changeport", typeof(CLIChangePort), new object[] { cliServer }));*/
        }

        #endregion

        #region methodes

        public void Start()
        {
            CLIOutput.WriteToConsole(GetBanner());

            while (true)
            {
                Command _command = null;

                // This was the old method to read input from cli.
                //string _cliInput = Console.ReadLine();

                Console.ForegroundColor = _cursorColor;
                Console.Write(cursor);

                Console.ForegroundColor = _inputColor;
                string _cliInput = CLIInput.ReadInput(Console.CursorLeft);

                _cliInput = _cliInput.Trim();

                if (_cliInput != "")
                {
                    // It is not necessery to use 'ref', but then
                    // it is obvious that the command-object gets
                    // modified.
                    string response = CLIInterpreter(ref _command, _cliInput);

                    // Check if the par and args are valid.
                    if (response == "VALID_PARAMETERS")
                    {
                        try
                        {
                            // Execute command and get response from command.
                            response = _command.Execute(Console.BufferWidth);

                            // When command response with 'EXIT_CLI' the CLI closes.
                            if (response == "EXIT_CLI")
                                break;
                        }
                        catch (Exception e)
                        {
                            response = "<color><red>" + e.Message;
                        }

                        // Write output to console.
                        CLIOutput.WriteToConsole(response);
                    }
                    else
                    {
                        // Something must be wrong with the input (par does not exist, to many args, ..).
                        CLIOutput.WriteToConsole(response);
                    }
                }
            }
        }

        private string GetBanner()
        {
            string _buffer = "";

            _buffer += @"<color><cyan> ___  ___  ___ ______ " + "\n";
            _buffer += @"<color><cyan> |  \/  | / _ \|  _  \" + "\n";
            _buffer += @"<color><cyan> |      |/ /_\ \ | | |" + "\n";
            _buffer += @"<color><cyan> | |\/| ||  _  | | | | <color><yellow>VERSION <color><white>" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version + "\n";
            _buffer += @"<color><cyan> | |  | || | | | |_/ | <color><yellow>TIME: <color><white>" + DateTime.Now.ToString("HH:mm:ss") + " <color><yellow>DATE: <color><white>" + DateTime.Now.ToString("dd.MM.yyyy") + "\n";
            _buffer += @"<color><cyan> \_|  |_/\_| |_/_____/_________________________________" + "\n";

            return _buffer;
        }

        #endregion
    }
}
