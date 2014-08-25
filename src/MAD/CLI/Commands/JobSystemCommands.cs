﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.IO; 

using MAD.JobSystemCore;

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
            output += "<color><yellow>\n JOBSYSTEM version " + _js.VERSION + "\n\n";
            output += " Nodes stored in RAM: <color><white>" + _js.NodesInitialized() + "<color><yellow>\t\t(MAX=" + JobSystem.MAX_NODES + ")\n";
            output += " Jobs  stored in RAM: <color><white>" + _js.JobsInitialized() + "<color><yellow>\t\t(MAX=" + JobSystem.MAX_NODES * JobNode.MAX_JOBS + ")\n";
            output += "\n\n Scedule-State: ";
            if (_js.sceduleState == JobScedule.State.Active)
                output += "<color><green>" + _js.sceduleState.ToString() + "<color><yellow>";
            else
                output += "<color><red>" + _js.sceduleState.ToString() + "<color><yellow>";
            output += "\n";

            return output;
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
            output += " <color><yellow>Nodes max:         <color><white>" + JobSystem.MAX_NODES + "\n";
            output += " <color><yellow>Nodes initialized: <color><white>" + _js.NodesInitialized() + "\n";
            output += " <color><yellow>Nodes active:      <color><white>" + _js.NodesActive() + "\n";
            output += " <color><yellow>Nodes inactive:    <color><white>" + _js.NodesInactive() + "\n\n";
            output += "<color><yellow>" + ConsoleTable.splitline; 
            output += ConsoleTable.FormatStringArray(Console.BufferWidth, _tableRow);
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
                output += ConsoleTable.FormatStringArray(Console.BufferWidth, _tableRow);
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
            try
            {
                _js.StartNode((int)pars.GetPar("id").argValues[0]);
                return "<color><green>Node is active.";
            }
            catch(Exception e)
            {
                return "<color><red>" + e.Message;
            }
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
            try
            {
                _js.StartNode((int)pars.GetPar("id").argValues[0]);
                return "<color><green>Node is inactive.";
            }
            catch (Exception e)
            {
                return "<color><red>" + e.Message;
            }
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

            try
            { _js.AddNode(_node); }
            catch (JobNodeException e)
            {
                return "<color><red>" + e.Message;
            }

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
            try
            {
                _js.RemoveNode((int)pars.GetPar("id").argValues[0]);
                return "<color><green>Node removed.";
            }
            catch (Exception e)
            { 
                return "<color><green>" + e.Message;
            }
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
            output += " <color><yellow>Jobs max:             <color><white>" + JobSystem.MAX_NODES * JobNode.MAX_JOBS + "\n";
            output += " <color><yellow>Jobs initialized:     <color><white>" + _js.JobsInitialized() + "\n";
            output += " <color><yellow>Jobs waiting/running: <color><white>" + _js.NodesActive() + "\n";
            output += " <color><yellow>Jobs stopped:         <color><white>" + _js.NodesInactive() + "\n\n";
            output += "<color><yellow>" + ConsoleTable.splitline;
            output += ConsoleTable.FormatStringArray(Console.BufferWidth, _tableRow);
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
                    output += ConsoleTable.FormatStringArray(Console.BufferWidth, _tableRow);
                }
            return output;
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

    #region commands for adding jobs

    public class JobSystemAddServiceCheckCommand : JobCommand
    {
        private JobSystem _js;
        string serviceDescript = "Choose the service to check \n" +
            "        Choose between following: \n" +
            "           dns -> doesn't need any more args\n" +
            "           ftp -> needs -a, -u, -p; leave username and password empty if there is no\n" +
            "           NYI\n";

        public JobSystemAddServiceCheckCommand(object[] args)
            : base()
        {
            _js = (JobSystem)args[0];
            rPar.Add(new ParOption("s", "SERVICE", serviceDescript, false, false, new Type[] { typeof(string) }));
            //there will be more, still working
            oPar.Add(new ParOption("u", "USERNAME", "Username on the server, e.g. ftp", false, false, new Type[] { typeof(string) }));
            oPar.Add(new ParOption("p", "PASSWORD", "Password on the server, e.g. ftp", false, false, new Type[] { typeof(string) }));
            description = "Checks the given Service for availibility. See in 'job serviceCheck help' for a list of available jobs"; //empty promises yet
        }

        public override string Execute(int consoleWidth)
        {
            // WORKING ON THIS

            /*JobServiceCheck _job = new JobServiceCheck();

            _job.name = (string)pars.GetPar("n").argValues[0];
            _job.time = ParseJobTime(this);
            _job.noti = ParseJobNotification(this);

            _job.arg = (string)pars.GetPar("s").argValues[0];

            if (OParUsed("u"))
                _job.username = (string)pars.GetPar("u").argValues[0];
            if (OParUsed("p"))
                _job.password = (string)pars.GetPar("p").argValues[0];

            int _nodeID = (int)pars.GetPar("id").argValues[0];
            _js.AddJobToNode(_nodeID, _job);
            return "<color><green>Job (ID " + _job.id + ") added to node (ID " + _nodeID + ").";*/
            return null;
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
            _job.noti = ParseJobNotification(this);

            _job.Subnetmask = (IPAddress)pars.GetPar("m").argValues[0];

            int _nodeID = (int)pars.GetPar("id").argValues[0];
            _js.AddJobToNode(_nodeID, _job);
            return "<color><green>Job (ID " + _job.id + ") added to node (ID " + _nodeID + ").";
        }
    }

    public class JobSystemAddPingCommand : JobCommand
    {
        private JobSystem _js;

        public JobSystemAddPingCommand(object[] args)
            : base()
        {
            _js = (JobSystem)args[0];

            oPar.Add(new ParOption("ttl", "TTL", "TTL of the ping.", false, false, new Type[] { typeof(int) })); 
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
            // Set notification.
            _job.noti = ParseJobNotification(this);

            // Set job-specific settings.
            if (OParUsed("ttl"))
                _job.ttl = (int)pars.GetPar("ttl").argValues[0];

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
            _job.noti = ParseJobNotification(this);

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
            _job.noti = ParseJobNotification(this);

            _job.port = (int)pars.GetPar("p").argValues[0];

            int _nodeID = (int)pars.GetPar("id").argValues[0];
            _js.AddJobToNode(_nodeID, _job);
            return "<color><green>Job (ID " + _job.id + ") added to node (ID " + _nodeID + ").";
        }
    }

    public class JobSystemAddSnmpCommand : JobCommand
    {
        private JobSystem _js;

        public JobSystemAddSnmpCommand(object[] args)
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
            _job.noti = ParseJobNotification(this);

            _job.version = (uint)pars.GetPar("ver").argValues[0];

            int _nodeID = (int)pars.GetPar("id").argValues[0];
            _js.AddJobToNode(_nodeID, _job);
            return "<color><green>Job (ID " + _job.id + ") added to node (ID " + _nodeID + ").";
        }
    }

    #endregion

    public class JobCommand : Command
    {
        public const string JOB_TIME_PAR = "t";
        public const string JOB_NOTI_PAR = "rule";
        public const string JOB_NOTI_MAIL = "mail";
        public const string JOB_NOTI_PRIO = "prio";

        public JobCommand()
            : base()
        {
            // GENERAL
            rPar.Add(new ParOption("n", "JOB-NAME", "Name of the job.", false, false, new Type[] { typeof(string) }));
            rPar.Add(new ParOption("id", "NODE-ID", "ID of the node to add the job to.", false, false, new Type[] { typeof(int) }));
            
            // TIME
            oPar.Add(new ParOption("t", "TIME", "Delaytime or time on which th job should be executed.", false, true, new Type[] { typeof(Int32), typeof(string) }));

            // NOTIFICATION
            oPar.Add(new ParOption("mail", "NOT.-ADDR", "Mailaddresses to send notifications to.", false, true, new Type[] { typeof(MailAddress) }));
            oPar.Add(new ParOption("prio", "NOT.-PRIO", "Priority of the mails.", false, true, new Type[] { typeof(string) }));
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

        public static JobNotification ParseJobNotification(Command c)
        {
            JobNotification _buffer = new JobNotification();



            return _buffer;
        }

        public static JobNotificationSettings ParseJobNotificationSettings(Command c)
        {
            JobNotificationSettings _buffer = new JobNotificationSettings();

            // PARSE MAILADDRESSES
            if (c.OParUsed(JOB_NOTI_MAIL))
                _buffer.mailAddr = (MailAddress[])c.pars.GetPar(JOB_NOTI_MAIL).argValues;

            // PARSE PRIO
            if (c.OParUsed(JOB_NOTI_PRIO))
                _buffer.priority = ParsePrio((string)c.pars.GetPar(JOB_NOTI_PRIO).argValues[0]);

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

        // NOT USED
        /*
        public static JobRule ParseRule(List<OutputDescriptor> outDesc, string data)
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

            if (_operatorKnown == false)
                throw new Exception("Operation not known!");

            OutputDesc _desc = GetOutDesc(outDesc, _buffer[0]);
            if (_desc == null)
                throw new Exception("OutDescriptor not known!");

            _rule.obj = _desc.dataObject;

            if (!_rule.IsOperatorSupported())
                throw new Exception("Operator does not support this descriptor!");

            _rule.obj2 = _buffer[1];

            return _rule;
        }
        private static string[] SplitByOperator(string toSplit, string i)
        {
            return toSplit.Split(new string[] { i }, StringSplitOptions.RemoveEmptyEntries);
        }
        public static OutputDescriptor GetOutDesc(List<OutputDescriptor> outDesc, string name)
        {
            foreach (OutputDesc _temp in outDesc)
                if (_temp.name == name)
                    return _temp;
            return null;
        }
         * */
    }

    #endregion
}