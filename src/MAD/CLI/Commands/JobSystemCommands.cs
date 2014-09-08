﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.IO; 

using MAD.JobSystemCore;
using MAD.DHCPReader;
using MAD.Notification;

namespace MAD.CLICore
{
    #region commands for JOBSYSTEM

    public class JobSystemStatusCommand : Command
    {
        private JobSystem _js;

        public JobSystemStatusCommand(object[] args)
            : base()
        {
            _js = (JobSystem)args[0];
            description = "This command shows informations about the jobsystem.";
        }

        public override string Execute(int consoleWidth)
        {
            output += "<color><yellow>\n JOBSYSTEM version " + JobSystem.VERSION + "\n\n";
            output += " Nodes stored in RAM: <color><white>" + _js.NodesInitialized() + "<color><yellow>\t\t(MAX=" + JobSystem.MAXNODES + ")\n";
            output += " Jobs  stored in RAM: <color><white>" + _js.JobsInitialized() + "<color><yellow>\t\t(MAX=" + JobSystem.MAXNODES * JobNode.MAX_JOBS + ")\n";
            output += "\n\n Scedule-State: ";
            if (_js.sceduleState == JobScedule.State.Active)
                output += "<color><green>" + _js.sceduleState.ToString() + "<color><yellow>";
            else
                output += "<color><red>" + _js.sceduleState.ToString() + "<color><yellow>";
            output += "\n";

            return output;
        }
    }

    public class JobSystemSaveTableCommand : Command
    {
        private JobSystem _js;

        public JobSystemSaveTableCommand(object[] args)
        {
            _js = (JobSystem)args[0];
            rPar.Add(new ParOption("file", "FILE", "File to save to.", false, false, new Type[] { typeof(string) }));
        }

        public override string Execute(int consoleWidth)
        {
            _js.SaveTable((string)pars.GetPar("file").argValues[0]);
            return "<color><green>Table saved (contains " + _js.nodes.Count + " Nodes).";
        }
    }

    public class JobSystemLoadTableCommand : Command
    {
        private JobSystem _js;

        public JobSystemLoadTableCommand(object[] args)
        {
            _js = (JobSystem)args[0];
            rPar.Add(new ParOption("file", "FILE", "File to load from.", false, false, new Type[] { typeof(string) }));
        }

        public override string Execute(int consoleWidth)
        {
            int _loadedNodes = _js.LoadTable((string)pars.GetPar("file").argValues[0]);
            return "<color><green>Table loaded (loaded " + _loadedNodes + " Nodes).";
        }
    }

    #endregion

    #region commands for SCEUDLE

    public class JobSceduleCommand : Command
    { 
        JobSystem _js;

        public JobSceduleCommand(object[] args)
            : base()
        {
            _js = (JobSystem)args[0];
            description = "This command shows the current state of the scedule.";
        }

        public override string Execute(int consoleWidth)
        {
            output = "<color><yellow>Scedule-state: ";
            if (_js.sceduleState == JobScedule.State.Active)
                output += "<color><green>" + _js.sceduleState.ToString() + "<color><yellow>";
            else
                output += "<color><red>" + _js.sceduleState.ToString() + "<color><yellow>";
            return output;
        }
    }

    public class JobSceduleStartCommand : Command
    {
        JobSystem _js;

        public JobSceduleStartCommand(object[] args)
            : base()
        {
            _js = (JobSystem)args[0];
        }

        public override string Execute(int consoleWidth)
        {
            _js.StartScedule();
            return "<color><green>Scedule started.";
        }
    }

    public class JobSceduleStopCommand : Command
    {
        JobSystem _js;

        public JobSceduleStopCommand(object[] args)
            : base()
        {
            _js = (JobSystem)args[0];
        }

        public override string Execute(int consoleWidth)
        {
            _js.StopScedule();
            return "<color><green>Scedule stopped.";
        }
    }

    #endregion

    #region commands for NODES

    public class JobSystemStatusNodesCommand : Command
    {
        JobSystem _js;

        public JobSystemStatusNodesCommand(object[] args)
            : base()
        {
            _js = (JobSystem)args[0];
            description = "This command prints a table with all nodes.";
        }

        public override string Execute(int consoleWidth)
        {
            string[] _tableRow = new string[] { "Node-ID", "Node-Name", "Node-State", "MAC-Address", "IP-Address", "Jobs Init." };
            output += "\n";
            output += " <color><yellow>Nodes max:         <color><white>" + JobSystem.MAXNODES + "\n";
            output += " <color><yellow>Nodes initialized: <color><white>" + _js.NodesInitialized() + "\n";
            output += " <color><yellow>Nodes active:      <color><white>" + _js.NodesActive() + "\n";
            output += " <color><yellow>Nodes inactive:    <color><white>" + _js.NodesInactive() + "\n\n";
            output += "<color><yellow>" + ConsoleTable.splitline; 
            output += ConsoleTable.FormatStringArray(consoleWidth, _tableRow);
            output += ConsoleTable.splitline;
            output += "<color><white>";

            foreach (JobNode _temp in _js.nodes)
            {
                _tableRow[0] = _temp.id.ToString();
                _tableRow[1] = _temp.name;
                _tableRow[2] = _temp.state.ToString();
                _tableRow[3] = _temp.macAddress.ToString();
                _tableRow[4] = _temp.ipAddress.ToString();
                _tableRow[5] = _temp.jobs.Count.ToString();
                output += ConsoleTable.FormatStringArray(consoleWidth, _tableRow);
            }

            return output;
        }
    }

    public class JobSystemStartNodeCommand : Command
    {
        private JobSystem _js;

        public JobSystemStartNodeCommand(object[] args)
        {
            _js = (JobSystem)args[0];
            rPar.Add(new ParOption("id", "<NODE-ID>", "Id of the node.", false, false, new Type[] { typeof(int) }));
            description = "This command sets the node-state to active.";
        }

        public override string Execute(int consoleWidth)
        {
            _js.StartNode((int)pars.GetPar("id").argValues[0]);
            return "<color><green>Node is active.";
        }
    }

    public class JobSystemStopNodeCommand : Command
    { 
        private JobSystem _js;

        public JobSystemStopNodeCommand(object[] args)
        {
            _js = (JobSystem)args[0];
            rPar.Add(new ParOption("id", "<NODE-ID>", "Id of the node.", false, false, new Type[] { typeof(int) }));
            description = "This command sets the node-state to inactive.";
        }

        public override string Execute(int consoleWidth)
        {
            _js.StartNode((int)pars.GetPar("id").argValues[0]);
            return "<color><green>Node is inactive.";
        }
    }

    public class JobSystemAddNodeCommand : Command
    { 
        private JobSystem _js;

        public JobSystemAddNodeCommand(object[] args)
        {
            _js = (JobSystem)args[0];
            rPar.Add(new ParOption("n", "NODE-NAME", "Name of the node.", false, false, new Type[] { typeof(string) }));
            rPar.Add(new ParOption("mac", "MAC-ADDRESS", "MAC-Address of the target.", false, false, new Type[] { typeof(PhysicalAddress) }));
            rPar.Add(new ParOption("ip", "IP-ADDRESS", "IP-Address of the target.", false, false, new Type[] { typeof(IPAddress) }));
            description = "This command creates a node.";
        }

        public override string Execute(int consoleWidth)
        {
            JobNode _node = new JobNode();

            _node.name = (string)pars.GetPar("n").argValues[0];
            _node.macAddress = (PhysicalAddress)pars.GetPar("mac").argValues[0];
            _node.ipAddress = (IPAddress)pars.GetPar("ip").argValues[0];

            _js.AddNode(_node); 

            return "<color><green>Node created (ID " + _node.id + ").";
        }
    }

    public class JobSystemRemoveNodeCommand : Command
    {
        private JobSystem _js;

        public JobSystemRemoveNodeCommand(object[] args)
        {
            _js = (JobSystem)args[0];
            rPar.Add(new ParOption("id", "NODE-ID", "ID of the node.", false, false, new Type[] { typeof(int) }));
            description = "This command removes a node.";
        }

        public override string Execute(int consoleWidth)
        {
            _js.RemoveNode((int)pars.GetPar("id").argValues[0]);
            return "<color><green>Node removed.";
        }
    }

    public class JobSystemSyncNodeCommand : Command
    {
        private JobSystem _js;
        private MAD.DHCPReader.MACFeeder _feeder;

        public JobSystemSyncNodeCommand(object[] args)
        {
            _js = (JobSystem)args[0];
            _feeder = (MAD.DHCPReader.MACFeeder)args[1];

            description = "This command syncs the hosts, which were found by the macfeeder, and updates the nodes of the Jobsystem.";
        }

        public override string Execute(int consoleWidth)
        {
            // HERE
            SyncResult _result = _js.SyncNodes(MACFeeder._dummyList);

            output += "<color><yellow>Nodes added:   <color><white>" + _result.nodesAdded + "\n";
            output += "<color><yellow>Nodes updated: <color><white>" + _result.nodesUpdated + "\n";

            return output;
        }
    }

    public class JobSystemSaveNodeCommand : Command
    { 
        private JobSystem _js;

        public JobSystemSaveNodeCommand(object[] args)
        {
            _js = (JobSystem)args[0];
            rPar.Add(new ParOption("file", "FILENAME", "Name of the file to load node from.", false, false, new Type[] { typeof(string) }));
            rPar.Add(new ParOption("id", "NODE-ID", "ID of the node which should be saved.", false, false, new Type[] { typeof(int) }));
            description = "This command save a node.";
        }

        public override string Execute(int consoleWidth)
        {
            string _fileName = (string)pars.GetPar("file").argValues[0];

            try
            {
                _js.SaveNode(_fileName, (int)pars.GetPar("id").argValues[0]);
                return "<color><green>Nodes saved.";
            }
            catch(Exception e)
            {
                return "<color><red>EXCEPTION: " + e.Message;
            }
        }
    }

    public class JobSystemLoadNodeCommand : Command
    {
        private JobSystem _js;

        public JobSystemLoadNodeCommand(object[] args)
        {
            _js = (JobSystem)args[0];
            rPar.Add(new ParOption("file", "FILENAME", "Name of the file to load node from.", false, false, new Type[] { typeof(string) }));
            description = "This command loads nodes.";
        }

        public override string Execute(int consoleWidth)
        {
            string _fileName = (string)pars.GetPar("file").argValues[0];

            try
            {
                JobNode _node = _js.LoadNode(_fileName);
                _js.AddNode(_node);

                return "<color><green>Node loaded.";
            }
            catch(Exception e)
            {
                return "<color><red>XML: " + e.Message;
            }
        }
    }

    // TODO
    public class JobSystemEditNode : Command
    {
        public override string Execute(int consoleWidth)
        {
            throw new NotImplementedException();
        }
    }

    #endregion

    #region commands for jobs

    public class JobSystemStatusJobsCommand : Command
    {
        JobSystem _js;

        public JobSystemStatusJobsCommand(object[] args)
            : base()
        {
            _js = (JobSystem)args[0];
            description = "This command prints a table with all initialized jobs.";
        }

        public override string Execute(int consoleWidth)
        {
            string[] _tableRow = new string[] { "Node-ID", "Job-ID", "Job-Name", "Job-Type", "Job-State", "Time-Type", "Time-Value(s)", "Output-State" };
            output += "\n";
            output += " <color><yellow>Jobs max:             <color><white>" + JobSystem.MAXNODES * JobNode.MAX_JOBS + "\n";
            output += " <color><yellow>Jobs initialized:     <color><white>" + _js.JobsInitialized() + "\n";
            output += " <color><yellow>Jobs waiting/running: <color><white>" + _js.NodesActive() + "\n";
            output += " <color><yellow>Jobs stopped:         <color><white>" + _js.NodesInactive() + "\n\n";
            output += "<color><yellow>" + ConsoleTable.splitline;
            output += ConsoleTable.FormatStringArray(consoleWidth, _tableRow);
            output += ConsoleTable.splitline;
            output += "<color><white>";

            foreach (JobNode _temp in _js.nodes)
                foreach (Job _temp2 in _temp.jobs)
                {
                    _tableRow[0] = _temp.id.ToString();
                    _tableRow[1] = _temp2.id.ToString();
                    _tableRow[2] = _temp2.name;
                    _tableRow[3] = _temp2.type.ToString();
                    _tableRow[4] = _temp2.state.ToString();
                    _tableRow[5] = _temp2.time.type.ToString();
                    _tableRow[6] = _temp2.time.GetValues();
                    _tableRow[7] = _temp2.outp.outState.ToString();
                    output += ConsoleTable.FormatStringArray(consoleWidth, _tableRow);
                }
            return output;
        }
    }

    public class JobOutDescriptorListCommand : Command
    {
        private JobSystem _js;

        public JobOutDescriptorListCommand(object[] args)
        {
            _js = (JobSystem)args[0];

            rPar.Add(new ParOption("id", "JOB-ID", "Id of job.", false, false, new Type[] { typeof(int) }));

            description = "Show the outdescriptors of a specific job.";
        }

        public override string Execute(int consoleWidth)
        {
            Job _job = _js.GetJob((int)pars.GetPar("id").argValues[0]);
            if (_job != null)
            {
                output += "<color><yellow>";

                foreach (OutputDescriptor _temp in _job.outp.outputs)
                {
                    output += "-> OutDesc.: " + _temp.name + "\n";
                    output += "   DataType: " + _temp.dataType.ToString() + "\n";
                }

                return output;
            }
            else
            {
                return "<color><red>Job does not exist!";
            }
        }
    }

    public class JobStatusCommand : Command
    {
        private JobSystem _js;

        public JobStatusCommand(object[] args)
            : base()
        {
            _js = (JobSystem)args[0];
            oPar.Add(new ParOption("id", "JOB-ID", "ID of the job.", false, false, new Type[] { typeof(int) }));
        }

        public override string Execute(int consoleWidth)
        {
            if (OParUsed("id"))
            {
                Job _job = _js.GetJob((int)pars.GetPar("id").argValues[0]);
                if (_job != null)
                    output = _job.Status();
                else
                    output = "<color><red>Job does not exist!";
                return output;
            }
            else
                foreach (JobNode _node in _js.nodes)
                    foreach (Job _job in _node.jobs)
                        output += _job.Status() + "\n";
            return output;
        }
    }

    public class JobSystemStartJobCommand : Command
    {
        private JobSystem _js;

        public JobSystemStartJobCommand(object[] args)
        {
            _js = (JobSystem)args[0];
            rPar.Add(new ParOption("id", "<JOB-ID>", "Id of the job.", false, false, new Type[] { typeof(int) }));
            description = "This command sets the job to active.";
        }

        public override string Execute(int consoleWidth)
        {
            _js.StartJob((int)pars.GetPar("id").argValues[0]);
            return "<color><green>Job is active.";
        }
    }

    public class JobSystemStopJobCommand : Command
    {
        private JobSystem _js;

        public JobSystemStopJobCommand(object[] args)
        {
            _js = (JobSystem)args[0];
            rPar.Add(new ParOption("id", "<JOB-ID>", "Id of the job.", false, false, new Type[] { typeof(int) }));
            description = "This command sets the job to inactive.";
        }

        public override string Execute(int consoleWidth)
        {
            _js.StopJob((int)pars.GetPar("id").argValues[0]);
            return "<color><green>Job is inactive.";
        }
    }

    public class JobSystemRemoveJobCommand : Command
    {
        private JobSystem _js;

        public JobSystemRemoveJobCommand(object[] args)
            : base()
        {
            _js = (JobSystem)args[0];
            rPar.Add(new ParOption("id", "JOB-ID", "ID of the job.", false, true, new Type[] { typeof(int) }));
        }

        public override string Execute(int consoleWidth)
        {
            _js.RemoveJob((int)pars.GetPar("id").argValues[0]);
            return "<color><green>Job removed.";
        }
    }

    public class JobSystemSetJobNotiCommand : NotificationCommand
    {
        private JobSystem _js;

        public JobSystemSetJobNotiCommand(object[] args)
        {
            _js = (JobSystem)args[0];
            rPar.Add(new ParOption("id", "JOB-ID", "ID of the job.", false, false, new Type[] { typeof(int) }));
        }

        public override string Execute(int consoleWidth)
        {
            Job _job = _js.GetJob((int)pars.GetPar("id").argValues[0]);
            if (_job == null)
                throw new Exception("Job does not exist!");

            _job.noti = ParseJobNotification(pars, _job.outp);

            return "<color><green>Notification-Settings set.";
        }
    }

    #region commands for adding jobs

    public class JobSystemAddPingCommand : JobCommand
    {
        private JobSystem _js;

        public JobSystemAddPingCommand(object[] args)
            : base()
        {
            _js = (JobSystem)args[0];

            oPar.Add(new ParOption("ttl", "TTL", "TTL of the ping.", false, false, new Type[] { typeof(int) }));
            oPar.Add(new ParOption("tout", "TIMEOUT", "Timeout of the ping.", false, false, new Type[] { typeof(int) })); 
            description = "This command adds a job with the jobtype 'PingRequest' to the node with the given ID.";
        }

        public override string Execute(int consoleWidth)
        {
            // Create an empty job.
            JobPing _job = new JobPing();

            // Set name.
            _job.name = (string)pars.GetPar("n").argValues[0];

            // Set jobTime.
            _job.time = ParseJobTime(this);
            
            // Set job-specific settings.
            if (OParUsed("ttl"))
                _job.ttl = (int)pars.GetPar("ttl").argValues[0];

            if (OParUsed("tout"))
                _job.timeout = (int)pars.GetPar("tout").argValues[0];

            // So now the JobPing is finished and set properly. 

            int _nodeID = (int)pars.GetPar("id").argValues[0];
            _js.AddJobToNode(_nodeID, _job);
            return "<color><green>Job (ID " + _job.id + ") added to node (ID " + _nodeID + ").";
        }
    }

    public class JobSystemAddHttpCommand : JobCommand
    {
        private JobSystem _js;

        public JobSystemAddHttpCommand(object[] args)
            : base()
        {
            _js = (JobSystem)args[0];
            oPar.Add(new ParOption("p", "PORT", "Port-Address of the target.", false, false, new Type[] { typeof(int) }));
        }

        public override string Execute(int consoleWidth)
        {
            JobHttp _job = new JobHttp();

            _job.name = (string)pars.GetPar("n").argValues[0];
            _job.time = ParseJobTime(this);

            _job.port = (int)pars.GetPar("p").argValues[0];

            int _nodeID = (int)pars.GetPar("id").argValues[0];
            _js.AddJobToNode(_nodeID, _job);
            return "<color><green>Job (ID " + _job.id + ") added to node (ID " + _nodeID + ").";
        }
    }

    public class JobSystemAddPortCommand : JobCommand
    {
        private JobSystem _js;

        public JobSystemAddPortCommand(object[] args)
            : base()
        {
            _js = (JobSystem)args[0];
            rPar.Add(new ParOption("p", "PORT", "Port of the target.", false, false, new Type[] { typeof(int) }));
        }

        public override string Execute(int consoleWidth)
        {
            JobPort _job = new JobPort();

            _job.name = (string)pars.GetPar("n").argValues[0];
            _job.time = ParseJobTime(this);

            _job.port = (int)pars.GetPar("p").argValues[0];

            int _nodeID = (int)pars.GetPar("id").argValues[0];
            _js.AddJobToNode(_nodeID, _job);
            return "<color><green>Job (ID " + _job.id + ") added to node (ID " + _nodeID + ").";
        }
    }

    public class JobSystemAddHostDetectCommand : JobCommand
    {
        private JobSystem _js;

        public JobSystemAddHostDetectCommand(object[] args)
            : base()
        {
            _js = (JobSystem)args[0];
            rPar.Add(new ParOption("m", "SUBNETMASK", "Subnetmask of the target Net", false, false, new Type[] { typeof(IPAddress) }));
            description = "Checks the given Network for all IPAddresses. Mind that it won't work if Ping is blocked.";
        }

        public override string Execute(int consoleWidth)
        {
            JobHostDetect _job = new JobHostDetect();

            _job.name = (string)pars.GetPar("n").argValues[0];
            _job.time = ParseJobTime(this);

            _job.Subnetmask = (IPAddress)pars.GetPar("m").argValues[0];

            int _nodeID = (int)pars.GetPar("id").argValues[0];
            _js.AddJobToNode(_nodeID, _job);
            return "<color><green>Job (ID " + _job.id + ") added to node (ID " + _nodeID + ").";
        }
    }

    public class JobSystemAddCheckSnmpCommand : JobCommand
    {
        private JobSystem _js;

        public JobSystemAddCheckSnmpCommand(object[] args)
            : base()
        {
            _js = (JobSystem)args[0];
            rPar.Add(new ParOption("ver", "VERSION", "Version of SNMP to use.", false, false, new Type[] { typeof(string) }));
        }

        public override string Execute(int consoleWidth)
        {
            JobSnmp _job = new JobSnmp();

            _job.name = (string)pars.GetPar("n").argValues[0];
            _job.time = ParseJobTime(this);

            _job.version = (uint)pars.GetPar("ver").argValues[0];

            int _nodeID = (int)pars.GetPar("id").argValues[0];
            _js.AddJobToNode(_nodeID, _job);
            return "<color><green>Job (ID " + _job.id + ") added to node (ID " + _nodeID + ").";
        }
    }

    public class JobSystemAddCheckFtpCommand : JobCommand
    {
        private JobSystem _js;

        public JobSystemAddCheckFtpCommand(object[] args)
            : base()
        {
            _js = (JobSystem)args[0];
            rPar.Add(new ParOption("u", "USERNAME", "Username on the server.", false, false, new Type[] { typeof(string) }));
            rPar.Add(new ParOption("p", "PASSWORD", "Password on the server.", false, false, new Type[] { typeof(string) }));
        }

        public override string Execute(int consoleWidth)
        {
            JobCheckFtp _job = new JobCheckFtp();

            _job.name = (string)pars.GetPar("n").argValues[0];
            _job.time = ParseJobTime(this);

            _job.username = (string)pars.GetPar("u").argValues[0];
            _job.password = (string)pars.GetPar("p").argValues[0];

            int _nodeID = (int)pars.GetPar("id").argValues[0];
            _js.AddJobToNode(_nodeID, _job);
            return "<color><green>Job (ID " + _job.id + ") added to node (ID " + _nodeID + ").";
        }
    }

    public class JobSystemAddCheckDnsCommand : JobCommand
    {
        private JobSystem _js;

        public JobSystemAddCheckDnsCommand(object[] args)
            : base()
        {
            _js = (JobSystem)args[0];
        }

        public override string Execute(int consoleWidth)
        {
            JobCheckDns _job = new JobCheckDns();

            _job.name = (string)pars.GetPar("n").argValues[0];
            _job.time = ParseJobTime(this);

            int _nodeID = (int)pars.GetPar("id").argValues[0];
            _js.AddJobToNode(_nodeID, _job);
            return "<color><green>Job (ID " + _job.id + ") added to node (ID " + _nodeID + ").";
        }
    }

    #endregion

    #region commands for editing jobs

    // TODO

    #endregion

    public class JobCommand : Command
    {
        public const string JOB_NAME = "n";
        public const string JOB_ID = "id";
        public const string JOB_TIME_PAR = "t";
 

        public JobCommand()
            : base()
        {
            // GENERAL
            rPar.Add(new ParOption(JOB_NAME, "JOB-NAME", "Name of the job.", false, false, new Type[] { typeof(string) }));
            rPar.Add(new ParOption(JOB_ID, "NODE-ID", "ID of the node to add the job to.", false, false, new Type[] { typeof(int) }));
            
            // TIME
            oPar.Add(new ParOption(JOB_TIME_PAR, "TIME", "Delaytime or time on which th job should be executed.", false, true, new Type[] { typeof(Int32), typeof(string) }));

        }

        public override string Execute(int consoleWidth)
        {
            throw new NotImplementedException();
        }

        public static JobTime ParseJobTime(Command c)
        {
            JobTime _buffer = new JobTime();
            if (c.OParUsed(JOB_TIME_PAR))
            {
                Type _argType = c.GetArgType(JOB_TIME_PAR);

                if (_argType == typeof(int))
                {
                    _buffer.type = JobTime.TimeMethod.Relative;
                    _buffer.jobDelay = new JobDelayHandler((int)c.pars.GetPar(JOB_TIME_PAR).argValues[0]);
                }
                else if (_argType == typeof(string))
                {
                    _buffer.type = JobTime.TimeMethod.Absolute;
                    _buffer.jobTimes = JobTime.ParseStringArray(c.pars.GetPar(JOB_TIME_PAR).argValues);
                }
            }
            else
            {
                // default settings
                _buffer.jobDelay = new JobDelayHandler(20000);
                _buffer.type = JobTime.TimeMethod.Relative;
            }
            return _buffer;
        }
    }

    public class NotificationCommand : Command
    {
        public const string NOTI_SMTP_ENDPOINT = "smtp";
        public const string NOTI_SMTP_LOGIN = "login";
        public const string NOTI_SMTP_MAILS = "mail";
        public const string JOB_NOTI_RULE = "rule";

        public NotificationCommand()
            : base()
        {
            // NOTIFICATION-LOGIN-PARAMETER
            rPar.Add(new ParOption(NOTI_SMTP_ENDPOINT, "SMTP-ADDR>:<SMTP-PORT", "Serveraddress and Serverport.", false, false, new Type[] { typeof(string) }));
            rPar.Add(new ParOption(NOTI_SMTP_LOGIN, "SMTP-MAIL>:<SMTP-PASS", "From Mailaddress.", false, false, new Type[] { typeof(string) }));

            // NOTIFICATION-SETTINGS
            rPar.Add(new ParOption(NOTI_SMTP_MAILS, "TO-MAIL-ADDR", "Mailaddresses to send notifications to.", false, true, new Type[] { typeof(MailAddress) }));

            // NOTIFICATION-RULES
            rPar.Add(new ParOption(JOB_NOTI_RULE, "NOT.-RULE", "Define Rule(s).", false, true, new Type[] { typeof(string) }));
        }

        public override string Execute(int consoleWidth)
        {
            throw new NotImplementedException();
        }

        public JobNotification ParseJobNotification(ParInput pars, JobOutput outp)
        {
            JobNotification _noti = new JobNotification();
            _noti.settings = ParseJobNotificationSettings(pars);
            _noti.rules = ParseJobNotificationRules(pars, outp);
            return _noti;
        }

        public static JobNotificationSettings ParseJobNotificationSettings(ParInput pars)
        {
            JobNotificationSettings _buffer = new JobNotificationSettings();

            string[] _temp = pars.GetPar(NOTI_SMTP_ENDPOINT).argValues[0].ToString().Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
            if (_temp.Length == 2)
            {
                _buffer.login.smtpAddr = _temp[0];

                try
                {
                    _buffer.login.port = Int32.Parse(_temp[1]);
                }
                catch (Exception)
                {
                    throw new Exception(CLIError.Error(CLIError.ErrorType.argTypeError, "Could not parse SMTP-Port!", true));
                }
            }
            else
                throw new Exception(CLIError.Error(CLIError.ErrorType.SyntaxError, "SMTP-Endpoint could not be parsed correctly!", true));

            string[] _temp2 = pars.GetPar(NOTI_SMTP_LOGIN).argValues[0].ToString().Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
            if (_temp2.Length == 2)
            {
                try
                {
                    _buffer.login.mail = new MailAddress(_temp2[0]);
                }
                catch (Exception)
                {
                    throw new Exception(CLIError.Error(CLIError.ErrorType.argTypeError, "Could not parse MailAddress!", true));
                }

                _buffer.login.password = _temp2[1];
            }
            else
                throw new Exception(CLIError.Error(CLIError.ErrorType.SyntaxError, "SMTP-Login could not be parsed correctly!", true));

            object[] _mailsTo = pars.GetPar(NOTI_SMTP_MAILS).argValues;
            _buffer.mailAddr = new MailAddress[_mailsTo.Length];

            for(int i = 0; i < _mailsTo.Length; i++)
                _buffer.mailAddr[i] = (MailAddress)_mailsTo[i];

            return _buffer;
        }

        public static MailPriority ParsePrio(string text)
        {
            text = text.ToLower();
            switch (text)
            {
                case "low":
                    return MailPriority.Low;
                case "normal":
                    return MailPriority.Normal;
                case "high":
                    return MailPriority.High;
                default:
                    throw new Exception("Could not parse '" + text + "' to a mail-priority!");
            }
        }

        public static List<JobRule> ParseJobNotificationRules(ParInput pars, JobOutput outp)
        {
            List<JobRule> _rules = new List<JobRule>();

            object[] _args = pars.GetPar(JOB_NOTI_RULE).argValues;
            string _temp;

            foreach (object _arg in _args)
            {
                _temp = (string)_arg;

                // parse rule.
                JobRule _rule = ParseRule(_temp);

                // then check if there is a outdescriptor matching the name
                OutputDescriptor _desc = outp.GetOutputDesc(_rule.outDescName);
                if (_desc == null)
                    throw new Exception("No OutputDescriptor with the name '" + _rule.outDescName + "' found!");

                // then check if the type is supported
                if (!_rule.IsOperatorSupported(_desc.dataType))
                    throw new Exception("OutputDescriptor-Type is not supported!");

                _rules.Add(_rule);
            }
            return _rules;
        }

        public static JobRule ParseRule(string data)
        {
            JobRule _rule = new JobRule();
            bool _operatorKnown = false;

            string[] _buffer;

            while (true)
            {
                _buffer = SplitByOperator(data, "=");
                if (_buffer.Length == 2)
                {
                    _rule.oper = JobRule.Operation.Equal;
                    _operatorKnown = true;
                    break;
                }

                _buffer = SplitByOperator(data, "!=");
                if (_buffer.Length == 2)
                {
                    _rule.oper = JobRule.Operation.NotEqual;
                    _operatorKnown = true;
                    break;
                }

                _buffer = SplitByOperator(data, "<");
                if (_buffer.Length == 2)
                {
                    _rule.oper = JobRule.Operation.Smaller;
                    _operatorKnown = true;
                    break;
                }

                _buffer = SplitByOperator(data, ">");
                if (_buffer.Length == 2)
                {
                    _rule.oper = JobRule.Operation.Bigger;
                    _operatorKnown = true;
                    break;
                }

                break;
            }

            if (_buffer.Length == 2)
            {
                _rule.outDescName = _buffer[0];
                _rule.compareValue = _buffer[1];
            }


            if (_operatorKnown == false)
                throw new Exception("Operation not known!");
            return _rule;
        }

        private static string[] SplitByOperator(string toSplit, string i)
        {
            return toSplit.Split(new string[] { i }, StringSplitOptions.RemoveEmptyEntries);
        }
    }

    #endregion
}
