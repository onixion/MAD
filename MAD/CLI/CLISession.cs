using System;
using System.Reflection;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

using MAD.CLICore;
using MAD.JobSystemCore;

using MadNet;

namespace MAD.CLIServerCore
{
    public class CLISession : CLIFramework
    {
        #region members

        private static uint _sessionsCount = 0;
        private uint _sessionID;
        public uint sessionID { get { return _sessionID; } }

        private object _sessionInitLock = new object();

        private NetworkStream _stream;
        private AES _aes;

        private Command _command;
        private string _clientCLIInput;
        private string _cliInterpreter;

        private JobSystem _js;

        #endregion

        public CLISession(NetworkStream stream, AES aes, JobSystem js)
            : base()
        {
            lock (_sessionInitLock)
            {
                _sessionID = _sessionsCount;
                _sessionsCount++;
            }

            _stream = stream;
            _aes = aes;
            _js = js;
        }

        #region methodes

        public void InitCommands()
        {
            // general purpose
            commands.Add(new CommandOptions("exit", typeof(ExitCommand), null));
            commands.Add(new CommandOptions("help", typeof(HelpCommand), new object[] { commands }));
            commands.Add(new CommandOptions("colortest", typeof(ColorTestCommand), null));
            commands.Add(new CommandOptions("info", typeof(InfoCommand), null));

            // MAC AND IP READER
            /*
            commands.Add(new CommandOptions("mac finder start", typeof(CatchBasicInfoStartCommand), new object[] { macFeeder }));
            commands.Add(new CommandOptions("mac finder stop", typeof(CatchBasicInfoStopCommand), new object[] { macFeeder }));
            commands.Add(new CommandOptions("mac finder set time", typeof(CatchBasicInfoSetTimeIntervallCommand), new object[] { macFeeder }));
            commands.Add(new CommandOptions("mac finder print list", typeof(CatchBasicInfoPrintHostsCommand), new object[] { macFeeder }));
            */
            // JOBSYSTEM
            commands.Add(new CommandOptions("js", typeof(JobSystemStatusCommand), new object[] { _js }));
            commands.Add(new CommandOptions("js nodes", typeof(JobSystemStatusNodesCommand), new object[] { _js }));
            commands.Add(new CommandOptions("js jobs", typeof(JobSystemStatusJobsCommand), new object[] { _js }));

            // SCEDULE
            commands.Add(new CommandOptions("scedule", typeof(JobSceduleCommand), new object[] { _js }));
            commands.Add(new CommandOptions("scedule start", typeof(JobSceduleStartCommand), new object[] { _js }));
            commands.Add(new CommandOptions("scedule stop", typeof(JobSceduleStopCommand), new object[] { _js }));

            // NODES
            commands.Add(new CommandOptions("node add", typeof(JobSystemAddNodeCommand), new object[] { _js }));
            commands.Add(new CommandOptions("node remove", typeof(JobSystemRemoveNodeCommand), new object[] { _js }));
            commands.Add(new CommandOptions("node start", typeof(JobSystemStartNodeCommand), new object[] { _js }));
            commands.Add(new CommandOptions("node stop", typeof(JobSystemStartNodeCommand), new object[] { _js }));
            commands.Add(new CommandOptions("node save", typeof(JobSystemSaveNodeCommand), new object[] { _js }));
            commands.Add(new CommandOptions("node load", typeof(JobSystemLoadNodeCommand), new object[] { _js }));

            // JOBS
            commands.Add(new CommandOptions("job status", typeof(JobStatusCommand), new object[] { _js }));
            commands.Add(new CommandOptions("job remove", typeof(JobSystemRemoveJobCommand), new object[] { _js }));
            commands.Add(new CommandOptions("job start", typeof(JobSystemStartJobCommand), new object[] { _js }));
            commands.Add(new CommandOptions("job stop", typeof(JobSystemStopJobCommand), new object[] { _js }));

            commands.Add(new CommandOptions("add ping", typeof(JobSystemAddPingCommand), new object[] { _js }));
            commands.Add(new CommandOptions("add http", typeof(JobSystemAddHttpCommand), new object[] { _js }));
            commands.Add(new CommandOptions("add port", typeof(JobSystemAddPortCommand), new object[] { _js }));
            commands.Add(new CommandOptions("add detect", typeof(JobSystemAddHostDetectCommand), new object[] { _js }));
            commands.Add(new CommandOptions("add serviceCheck", typeof(JobSystemAddServiceCheckCommand), new object[] { _js }));
        }

        public void Start()
        {
            DataPacket _dataP = new DataPacket(_stream, _aes);
            CLIPacket _cliP = new CLIPacket(_stream, _aes);

            while (true)
            {
                _cliP.ReceivePacket();
                _clientCLIInput = Encoding.Unicode.GetString(_cliP.cliInput);

                _cliInterpreter = AnalyseInput(ref _command, _clientCLIInput);

                if (_cliInterpreter == "VALID_PARAMETERS")
                {
                    _dataP.data = Encoding.Unicode.GetBytes(_command.Execute(_cliP.consoleWidth));
                }
                else
                {
                    _dataP.data = Encoding.Unicode.GetBytes(_cliInterpreter);
                }
                _dataP.SendPacket();
            }

            _dataP.Dispose();
            _cliP.Dispose();
        }

        #endregion
    }
}
