using System;
using System.Collections.Generic;
using System.IO;

using MAD.JobSystemCore;
using MAD.CLIServerCore;
using MAD.CLIIO;
using MAD.DHCPReader;

namespace MAD.CLICore
{
    public class CLI : CLIFramework
    {
        #region members

        private CLIInput _input = new CLIInput();
        
        private string _dataPath;

        #endregion

        #region constructor

        public CLI(string dataPath, JobSystem js, CLIServer cliServer, MACFeeder macFeeder)
            :base()
        {
            _dataPath = dataPath;

            // !! INIT COMMANDS !!

            // general purpose
            commands.Add(new CommandOptions("exit", typeof(ExitCommand), null));
            commands.Add(new CommandOptions("help", typeof(HelpCommand), new object[] { commands }));
            commands.Add(new CommandOptions("colortest", typeof(ColorTestCommand), null));
            commands.Add(new CommandOptions("info", typeof(InfoCommand), null));

            // MAC AND IP READER
            commands.Add(new CommandOptions("mac finder start", typeof(CatchBasicInfoStartCommand), new object[] { macFeeder }));
            commands.Add(new CommandOptions("mac finder stop", typeof(CatchBasicInfoStopCommand), new object[] { macFeeder }));
            commands.Add(new CommandOptions("mac finder set time", typeof(CatchBasicInfoSetTimeIntervallCommand), new object[] { macFeeder }));

            // JOBSYSTEM
            commands.Add(new CommandOptions("js", typeof(JobSystemStatusCommand), new object[] { js }));
            commands.Add(new CommandOptions("js nodes", typeof(JobSystemStatusNodesCommand), new object[] { js }));
            commands.Add(new CommandOptions("js jobs", typeof(JobSystemStatusJobsCommand), new object[] { js }));

            // SCEDULE
            commands.Add(new CommandOptions("scedule start", typeof(JobSceduleStartCommand), new object[] { js }));
            commands.Add(new CommandOptions("scedule stop", typeof(JobSceduleStopCommand), new object[] { js }));

            // NODES
            commands.Add(new CommandOptions("js add node", typeof(JobSystemAddNodeCommand), new object[] { js }));
            commands.Add(new CommandOptions("js remove node", typeof(JobSystemRemoveNodeCommand), new object[] { js }));
            commands.Add(new CommandOptions("js node start", typeof(JobSystemStartNodeCommand), new object[] { js }));
            commands.Add(new CommandOptions("js node stop", typeof(JobSystemStartNodeCommand), new object[] { js }));

            // JOBS
            commands.Add(new CommandOptions("js job status", typeof(JobStatusCommand), new object[] { js }));
            commands.Add(new CommandOptions("js remove job", typeof(JobSystemRemoveJobCommand), new object[] { js }));
            commands.Add(new CommandOptions("js job start", typeof(JobSystemStartJobCommand), new object[] { js }));
            commands.Add(new CommandOptions("js job stop", typeof(JobSystemStopJobCommand), new object[] { js }));

            commands.Add(new CommandOptions("js add ping", typeof(JobSystemAddPingCommand), new object[] { js }));
            commands.Add(new CommandOptions("js add http", typeof(JobSystemAddHttpCommand), new object[] { js }));
            commands.Add(new CommandOptions("js add port", typeof(JobSystemAddPortCommand), new object[] { js }));
            commands.Add(new CommandOptions("js add detect", typeof(JobSystemAddHostDetectCommand), new object[] { js }));
            commands.Add(new CommandOptions("js add serviceCheck", typeof(JobSystemAddServiceCheckCommand), new object[] { js }));

            // CLIServer
            commands.Add(new CommandOptions("cliserver", typeof(CLIServerInfo), new object[] { cliServer }));
            commands.Add(new CommandOptions("cliserver start", typeof(CLIServerStart), new object[] { cliServer }));
            commands.Add(new CommandOptions("cliserver stop", typeof(CLIServerStop), new object[] { cliServer }));
            commands.Add(new CommandOptions("cliserver changeport", typeof(CLIChangePort), new object[] { cliServer }));
        }

        #endregion

        #region methodes

        public void Start()
        {
            CLIOutput.WriteToConsole(GetBanner());

            while (true)
            {
                Command _command = null;

                _input.WriteCursor();

                //string _cliInput = Console.ReadLine();
                string _cliInput = _input.ReadInput();

                _cliInput = _cliInput.Trim();

                if (_cliInput != "")
                {
                    string response = AnalyseInput(ref _command, _cliInput);

                    // Check if the parameter and arguments are valid.
                    if (response == "VALID_PARAMETER")
                    {
                        // Execute command and get response from command. // TODO: pass console width to .Execute()
                        response = _command.Execute();

                        // When command response with 'EXIT_CLI' the CLI closes.
                        if (response == "EXIT_CLI")
                            break;

                        // Write command ouput to console.
                        CLIOutput.WriteToConsole(response);
                    }
                    else
                    {
                        // Something must be wrong with the input (parameter does not exist, to many arguments, ..).
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
