using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.IO; 

using MAD.JobSystemCore;
using MAD.MacFinders;
using MAD.Notification;
using MAD.Helper;
using MadNet;

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
            output += _js.GetJSStats() + "\n";
            output += _js.GetJSScheduleStats() + "\n";
            output += _js.GetNodesStats() + "\n";
            output += _js.GetJobsStats();
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
            return "<color><green>Table saved.";
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

    public class JobScheduleStartCommand : Command
    {
        JobSystem _js;

        public JobScheduleStartCommand(object[] args)
            : base()
        {
            _js = (JobSystem)args[0];
        }

        public override string Execute(int consoleWidth)
        {
            _js.StartSchedule();
            return "<color><green>Schedule started.";
        }
    }

    public class JobScheduleStopCommand : Command
    {
        JobSystem _js;

        public JobScheduleStopCommand(object[] args)
            : base()
        {
            _js = (JobSystem)args[0];
        }

        public override string Execute(int consoleWidth)
        {
            _js.StopSchedule();
            return "<color><green>Schedule stopped.";
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
            output += _js.GetNodesStats();
            output += "<color><yellow>" + ConsoleTable.GetSplitline(consoleWidth);

            string[] _tableRow = _tableRow = new string[] { "Node-ID", "Node-Name", "Node-State", "MAC-Address", "IP-Address", "Jobs Init." };
            output += ConsoleTable.FormatStringArray(consoleWidth, _tableRow);
            output += ConsoleTable.GetSplitline(consoleWidth);
            output += "<color><white>";

            lock (_js.jsLock)
            {
                List<JobNode> _nodes = _js.LGetNodes();
                foreach (JobNode _temp in _nodes)
                {
                    _tableRow[0] = _temp.id.ToString();
                    _tableRow[1] = _temp.name;
                    _tableRow[2] = _js.NodeState(_temp.state);
                    _tableRow[3] = _temp.mac.ToString();
                    _tableRow[4] = _temp.ip.ToString();
                    _tableRow[5] = _temp.jobs.Count.ToString();
                    output += ConsoleTable.FormatStringArray(consoleWidth, _tableRow);
                }
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
            _js.StopNode((int)pars.GetPar("id").argValues[0]);
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
            oPar.Add(new ParOption("i", "IP-RENEW", "Try to renew IP-Addresses over ARP-Request.", true, false, null));
            description = "This command creates a node.";
        }

        public override string Execute(int consoleWidth)
        {
            JobNode _node = new JobNode();

            _node.name = (string)pars.GetPar("n").argValues[0];
            _node.ip = (IPAddress)pars.GetPar("ip").argValues[0];
            _node.mac = ((PhysicalAddress)pars.GetPar("mac").argValues[0]).ToString();

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

    public class JobSystemEditNodeCommand : Command
    {
        private JobSystem _js;

        public JobSystemEditNodeCommand(object[] args)
        {
            _js = (JobSystem)args[0];
            rPar.Add(new ParOption("id", "NODE-ID", "ID of the node to edit.", false, false, new Type[] { typeof(Int32) }));
            oPar.Add(new ParOption("n", "NODE-NAME", "Name of the node.", false, false, new Type[] { typeof(string) }));
            oPar.Add(new ParOption("mac", "MAC-ADDRESS", "MAC-Address of the target.", false, false, new Type[] { typeof(PhysicalAddress) }));
            oPar.Add(new ParOption("ip", "IP-ADDRESS", "IP-Address of the target.", false, false, new Type[] { typeof(IPAddress) }));
            oPar.Add(new ParOption("i", "IP-RENEW", "Try to renew IP-Addresses over ARP-Request.", true, false, null));
            description = "This command edits a node. ";
        }

        public override string Execute(int consoleWidth)
        {
            lock (_js.jsLock)
            {
                JobNode _node = _js.LGetNode((Int32)pars.GetPar("id").argValues[0]);
                if (_node != null)
                {
                    if (OParUsed("n"))
                        _node.name = (string)pars.GetPar("n").argValues[0];

                    if (OParUsed("mac"))
                        _node.mac = ((PhysicalAddress)pars.GetPar("mac").argValues[0]).ToString();

                    if (OParUsed("ip"))
                        _node.ip = (IPAddress)pars.GetPar("ip").argValues[0];

                    return "<color><green>Node edited.";
                }
                else
                    return "<color><red>Node does not exist!";
            }
        }
    }

    /*                                                                                                                          Auskommentiert für die Implementierung durch Alin
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
    */
      
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
                _js.LoadNode(_fileName);
                return "<color><green>Node loaded.";
            }
            catch(Exception e)
            {
                return "<color><red>EX: " + e.Message;
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

            oPar.Add(new ParOption("more", "", "Show more infos.", true, false, null));
            description = "This command prints a table with all initialized jobs.";
        }

        public override string Execute(int consoleWidth)
        {
            output += _js.GetJobsStats();
            output += "<color><yellow>" + ConsoleTable.GetSplitline(consoleWidth);

            string[] _tableRow = null;

            if (!OParUsed("more"))
                _tableRow = new string[] { "Node-ID", "Job-ID", "Job-Name", "Job-Type", "Job-State", "Time-Type", "Time-Value(s)", "Output-State" };
            else
                _tableRow = new string[] { "Node-ID", "Job-ID", "Job-Name", "Job-Type", "Job-State", "Time-Type", "Time-Value(s)", "Last-Started", "Last-Stopped", "Last-Delta", "Output-State" };

            output += ConsoleTable.FormatStringArray(consoleWidth, _tableRow);
            output += ConsoleTable.GetSplitline(consoleWidth);
            output += "<color><white>";

            lock (_js.jsLock)
            {
                List<JobNode> _nodes = _js.LGetNodes();
                foreach (JobNode _temp in _nodes)
                {
                    foreach (Job _temp2 in _temp.jobs)
                    {
                        if (_temp2.state == 0 || _temp2.state == 1)
                        {
                            _tableRow[0] = _temp.id.ToString();
                            _tableRow[1] = _temp2.id.ToString();
                            _tableRow[2] = _temp2.name;
                            _tableRow[3] = _temp2.type.ToString();
                            _tableRow[4] = _js.JobState(_temp2.state);
                            _tableRow[5] = _temp2.time.type.ToString();
                            _tableRow[6] = _temp2.time.GetValues();

                            if (!OParUsed("more"))
                                _tableRow[7] = _temp2.outp.outState.ToString();
                            else
                            {
                                _tableRow[7] = _temp2.tStart.ToString("HH:mm:ss");
                                _tableRow[8] = _temp2.tStop.ToString("HH:mm:ss");
                                _tableRow[9] = "+" + _temp2.tSpan.Seconds + "s" + _temp2.tSpan.Milliseconds + "ms";
                                _tableRow[10] = _temp2.outp.outState.ToString();
                            }
                        }
                        else
                        {
                            _tableRow[0] = _temp.id.ToString();
                            _tableRow[1] = _temp2.id.ToString();
                            _tableRow[2] = "-";
                            _tableRow[3] = "-";
                            _tableRow[4] = _js.JobState(_temp2.state);
                            _tableRow[5] = "-";
                            _tableRow[6] = "-";

                            if (!OParUsed("more"))
                                _tableRow[7] = "-";
                            else
                            {
                                _tableRow[7] = "-";
                                _tableRow[8] = "-";
                                _tableRow[9] = "-";
                                _tableRow[10] = "-";
                            }
                        }

                        output += ConsoleTable.FormatStringArray(consoleWidth, _tableRow);
                    }
                }
                return output;
            }
        }
    }

    public class JobInfoCommand : Command
    {
        private JobSystem _js;

        public JobInfoCommand(object[] args)
        {
            _js = (JobSystem)args[0];
            oPar.Add(new ParOption("id", "JOB-ID", "ID of the job.", false, false, new Type[] { typeof(int) }));
            description = "This command shows useful informations about jobs.";
        }

        public override string Execute(int consoleWidth)
        {
            if (OParUsed("id"))
            {
                lock (_js.jsLock)
                {
                    JobNode _node;
                    Job _job = _js.LGetJob((int)pars.GetPar("id").argValues[0], out _node);
                    if (_job != null)
                    {
                        output = GetJobInfo(_job);
                        return output;
                    }
                    else
                        return "<color><red>Job does not exist!";
                }
            }
            else
            {
                lock (_js.jsLock)
                {
                    List<JobNode> _nodes = _js.LGetNodes();
                    foreach (JobNode _node in _nodes)
                    {
                        foreach (Job _job in _node.jobs)
                        {
                            output += GetJobInfo(_job);
                            output += GetJobSpecificInfo(_job);
                            output += "\n";
                        }
                    }
                    return output;
                }
            }
        }

        private string GetJobInfo(Job job)
        {
            string output = "";

            output += "<color><yellow>[<color><white>JOB ID='" + job.id + "']\n\n";
            output += "<color><yellow>Name:          <color><white>" + job.name + "\n";
            output += "<color><yellow>Type:          <color><white>" + job.type.ToString() + "\n";
            output += "<color><yellow>Time-Type:     <color><white>" + job.time.type.ToString() + "\n";
            output += "<color><yellow>Time-Value(s): <color><white>" + job.time.GetValues() + "\n\n";

            output += "<color><yellow>Last-Started:  <color><white>" + job.tStart.ToString("dd.MM.yyyy HH:mm:ss") + "\n";
            output += "<color><yellow>Last-Stopped:  <color><white>" + job.tStop.ToString("dd.MM.yyyy HH:mm:ss") + "\n";
            output += "<color><yellow>Last-Span:     <color><white>" + "+" + job.tSpan.Seconds + "s " + job.tSpan.Milliseconds + "ms" + "\n\n";

            output += "\n\n<color><yellow>Notification-Rules:";

            if (job.rules == null)
                output += " <color><white>NULL";
            else
            {
                if (job.rules.Count != 0)
                    foreach (JobRule _rule in job.rules)
                    {
                        output += "\n  <color><yellow>>Target-OutDesc.: <color><white>" + _rule.outDescName;
                        output += "\n  <color><yellow> Operation:       <color><white>" + _rule.oper.ToString();
                        output += "\n  <color><yellow> Compare value:   <color><white>" + _rule.compareValue.ToString() + "\n";
                    }
                else
                    output += "<color><white>EMPTY";
            }

            output += "\n\n<color><yellow>Notification-Settings: <color><white>";
            if (job.settings != null)
            {
                output += "\n  <color><yellow>Smtp-Address:      <color><white>" + job.settings.login.smtpAddr;
                output += "\n  <color><yellow>Smtp-Port:         <color><white>" + job.settings.login.port;
                output += "\n  <color><yellow>Username (E-Mail): <color><white>" + job.settings.login.mail;
                output += "\n  <color><yellow>Password:          <color><white>" + job.settings.login.password;
                output += "\n  <color><yellow>Mails to send to:  <color><white>";

                foreach (MailAddress _addr in job.settings.mailAddr)
                    output += _addr.ToString() + " ";
            }
            else
                output += "NULL";

            return output;
        }

        private string GetJobSpecificInfo(Job job)
        {
            switch (job.type)
            { 
                    // TO DO
                default:
                    return "";
            }
        }
    }

    public class JobOutDescInfoCommand : Command
    { 
        private JobSystem _js;

        public JobOutDescInfoCommand(object[] args)
        {
            _js = (JobSystem)args[0];
            rPar.Add(new ParOption("id", "JOB-ID", "ID of the job.", false, false, new Type[] { typeof(int) }));
            description = "This command shows the out-Descriptors of a job.";
        }

        public override string Execute(int consoleWidth)
        {
            lock(_js.jsLock)
            {
                JobNode _node = null;
                Job _job = _js.LGetJob((int)pars.GetPar("id").argValues[0], out _node);
                if(_job != null)
                {
                    foreach(OutputDescriptor _desc in _job.outp.outputs)
                    {
                        output += "<color><yellow>>OutDesc.: <color><white>" + _desc.name + "\n";
                        output += "<color><yellow> Type:     <color><white>" + _desc.dataType.ToString() + "\n\n";
                    }
                    return output;
                }
                else
                    throw new JobException("Job does not exist!", null);
            }
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

    public class JobSystemEditJobCommand : Command
    {
        private JobSystem _js;

        public JobSystemEditJobCommand(object[] args)
        {
            _js = (JobSystem)args[0];

            rPar.Add(new ParOption("id", "JOB-ID", "ID of the job to edit.", false, false, new Type[] { typeof(Int32) }));
            oPar.Add(new ParOption("n", "JOB-NAME", "Name of the job.", false, false, new Type[] { typeof(string) }));
            oPar.Add(new ParOption("t", "TIME", "Delaytime or time on which th job should be executed.", false, true, new Type[] { typeof(Int32), typeof(string) }));
            oPar.Add(new ParOption("rule", "NOT.-RULE", "Define Rule(s).", false, true, new Type[] { typeof(string) }));
            oPar.Add(new ParOption("ne", "", "Enables notification over E-Mail.", true, false, null));
        }

        public override string Execute(int consoleWidth)
        {
            lock (_js.jsLock)
            {
                JobNode _node = null;
                Job _job = _js.LGetJob((Int32)pars.GetPar("id").argValues[0], out _node);

                if (OParUsed("n"))
                    _job.name = (string)pars.GetPar("n").argValues[0];

                if (OParUsed("t"))
                    _job.time = ParseJobTime(this);

                if (OParUsed("rule"))
                {
                    List<JobRule> _rules = ParseJobNotificationRules(pars, _job.outp);
                    _job.rules = _rules;
                }

                if (OParUsed("ne"))
                    _job.notiFlag = true;
                else
                    _job.notiFlag = false;

                return "<color><green>Job updated.";
            }
        }

        public static JobTime ParseJobTime(Command c)
        {
            JobTime _buffer = new JobTime();
            if (c.OParUsed("t"))
            {
                Type _argType = c.GetArgType("t");

                if (_argType == typeof(int))
                {
                    _buffer.type = JobTime.TimeMethod.Relative;





                    _buffer.jobDelay = new JobDelayHandler((int)c.pars.GetPar("t").argValues[0]);
                }
                else if (_argType == typeof(string))
                {
                    _buffer.type = JobTime.TimeMethod.Absolute;
                    _buffer.jobTimes = JobTime.ParseStringArray(c.pars.GetPar("t").argValues);
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

        public static List<JobRule> ParseJobNotificationRules(ParInput pars, JobOutput outp)
        {
            List<JobRule> _rules = new List<JobRule>();

            object[] _args = pars.GetPar("rule").argValues;
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
                _buffer = SplitByOperator(data, "!=");
                if (_buffer.Length == 2)
                {
                    _rule.oper = JobRule.Operation.NotEqual;
                    _operatorKnown = true;
                    break;
                }

                _buffer = SplitByOperator(data, "=");
                if (_buffer.Length == 2)
                {
                    _rule.oper = JobRule.Operation.Equal;
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

    public class JobSystemSetJobMailSettingsCommand : NotificationConCommand
    {
        private JobSystem _js;

        public JobSystemSetJobMailSettingsCommand(object[] args)
        {
            _js = (JobSystem)args[0];
            rPar.Add(new ParOption("id", "JOB-ID", "ID of the job.", false, false, new Type[] { typeof(int) }));
        }

        public override string Execute(int consoleWidth)
        {
            lock (_js.jsLock)
            {
                JobNode _node;
                Job _job = _js.LGetJob((int)pars.GetPar("id").argValues[0], out _node);
                if (_job != null)
                {
                    _job.settings = ParseJobNotificationSettings(pars);
                    return "<color><green>Rules set.";
                }
                else
                    return "<color><red>Job does not exist!";
            }
        }
    }

    #region commands for adding jobs

    public class JobSystemAddPingCommand : JobAddCommand
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
            JobPing _job = new JobPing();
            _job.name = (string)pars.GetPar("n").argValues[0];
            _job.time = ParseJobTime(this);

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

    public class JobSystemAddHttpCommand : JobAddCommand
    {
        private JobSystem _js;

        public JobSystemAddHttpCommand(object[] args)
            : base()
        {
            _js = (JobSystem)args[0];
            oPar.Add(new ParOption("p", "PORT", "Port-Address of the target.", false, false, new Type[] { typeof(int) }));
            oPar.Add(new ParOption("tout", "TIMEOUT", "Timeout of the ping.", false, false, new Type[] { typeof(int) }));
            description = "This command adds a job with the jobtype 'HTTPRequest' to the node with the given ID.";
        }

        public override string Execute(int consoleWidth)
        {
            JobHttp _job = new JobHttp();
            _job.name = (string)pars.GetPar("n").argValues[0];
            _job.time = ParseJobTime(this);
            
            if (OParUsed("p"))
                _job.port = (int)pars.GetPar("p").argValues[0];

            if (OParUsed("tout"))
                _job.timeout = (int)pars.GetPar("tout").argValues[0];

            int _nodeID = (int)pars.GetPar("id").argValues[0];
            _js.AddJobToNode(_nodeID, _job);
            return "<color><green>Job (ID " + _job.id + ") added to node (ID " + _nodeID + ").";
        }
    }

    public class JobSystemAddPortCommand : JobAddCommand
    {
        private JobSystem _js;

        public JobSystemAddPortCommand(object[] args)
            : base()
        {
            _js = (JobSystem)args[0];
            rPar.Add(new ParOption("p", "PORT", "Port of the target.", false, false, new Type[] { typeof(int) }));
            oPar.Add(new ParOption("tout", "TIMEOUT", "Timeout of the ping.", false, false, new Type[] { typeof(int) }));
            description = "This command adds a job with the jobtype 'PortScan' to the node with the given ID.";
        }

        public override string Execute(int consoleWidth)
        {
            JobPort _job = new JobPort();

            _job.name = (string)pars.GetPar("n").argValues[0];
            _job.time = ParseJobTime(this);

            _job.port = (int)pars.GetPar("p").argValues[0];

            if (OParUsed("tout"))
                _job.timeout = (int)pars.GetPar("tout").argValues[0];

            int _nodeID = (int)pars.GetPar("id").argValues[0];
            _js.AddJobToNode(_nodeID, _job);
            return "<color><green>Job (ID " + _job.id + ") added to node (ID " + _nodeID + ").";
        }
    }

    public class JobSystemAddHostDetectCommand : JobAddCommand
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

    public class JobSystemAddCheckSnmpCommand : JobAddCommand
    {
        private JobSystem _js;

        public JobSystemAddCheckSnmpCommand(object[] args)
            : base()
        {
            _js = (JobSystem)args[0];
            rPar.Add(new ParOption("ver", "VERSION", "Version of SNMP to use.", false, false, new Type[] { typeof(uint) }));
            oPar.Add(new ParOption("l", "SECURITY-LEVEL", "Level of used security", false, false, new Type[] { typeof(string) }));
            oPar.Add(new ParOption("a", "AUTH-PROTOCOL", "Protocol for authentification", false, false, new Type[] { typeof(string) }));
            oPar.Add(new ParOption("p", "PRIV-PROTCOL", "Protocol for privacy", false, false, new Type[] { typeof(string) }));
        }

        public override string Execute(int consoleWidth)
        {
            JobCheckSnmp _job = new JobCheckSnmp();

            _job.name = (string)pars.GetPar("n").argValues[0];
            _job.time = ParseJobTime(this);

            _job.version = (uint)pars.GetPar("ver").argValues[0];

            if (_job.version == 3)
            {
                string _buffer = (string)pars.GetPar("s").argValues[0];
                switch (_buffer)
                {
                    case "authNoPriv":
                        _job.secModel.securityLevel = NetworkHelper.securityLvl.authNoPriv;
                        if (!(OParUsed("a") && !OParUsed("p")))
                            return "<color><red>ERROR: Wrong Parameters Used";
                        break;
                    case "authPriv":
                        _job.secModel.securityLevel = NetworkHelper.securityLvl.authPriv;
                        if (!(OParUsed("a") && OParUsed("p")))
                            return "<color><red>ERROR: Wrong Parameters Used";
                        break;
                    case "noAuthNoPriv":
                        _job.secModel.securityLevel = NetworkHelper.securityLvl.noAuthNoPriv;
                        if (!(!OParUsed("a") && !OParUsed("p")))
                            return "<color><red>ERROR: Wrong Parameters Used";
                        break;
                    default:
                        return "<color><red>ERROR: Wrong Security Level! choose between authNoPriv, authPriv and noAuthNoPriv";
                }

                _buffer = (string)pars.GetPar("a").argValues[0];
                switch (_buffer)
                {
                    case "md5":
                        _job.secModel.authentificationProtocol = NetworkHelper.snmpProtocols.MD5;
                        break;
                    case "sha":
                        _job.secModel.authentificationProtocol = NetworkHelper.snmpProtocols.SHA;
                        break;
                    default:
                        return "<color><red>ERROR: Wrong authentification Protocol! choose between md5, sha";
                }

                _buffer = (string)pars.GetPar("p").argValues[0];
                switch (_buffer)
                {
                    case "aes":
                        _job.secModel.privacyProtocol = NetworkHelper.snmpProtocols.AES;
                        break;
                    case "des":
                        _job.secModel.privacyProtocol = NetworkHelper.snmpProtocols.DES;
                        break;
                    default:
                        return "<color><red>ERROR: Wrong privacy Protocol! choose between aes, des";
                }
            }

            int _nodeID = (int)pars.GetPar("id").argValues[0];
            _js.AddJobToNode(_nodeID, _job);
            return "<color><green>Job (ID " + _job.id + ") added to node (ID " + _nodeID + ").";
        }
    }

    public class JobSystemAddCheckFtpCommand : JobAddCommand
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

    public class JobSystemAddCheckDnsCommand : JobAddCommand
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

    #region parsing job parameters

    public class JobAddCommand : Command
    {
        public const string JOB_NAME = "n";
        public const string JOB_ID = "id";
        public const string JOB_TIME_PAR = "t";
 
        public JobAddCommand()
            : base()
        {
            rPar.Add(new ParOption(JOB_NAME, "<JOB-NAME>", "Name of the job.", false, false, new Type[] { typeof(string) }));
            rPar.Add(new ParOption(JOB_ID, "<NODE-ID>", "ID of the node to add the job to.", false, false, new Type[] { typeof(int) }));
            oPar.Add(new ParOption(JOB_TIME_PAR, "<TIME>", "Delaytime or time on which th job should be executed.", false, true, new Type[] { typeof(Int32), typeof(string) }));
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

    // for NotificationSettings
    public class NotificationConCommand : Command
    {
        public const string NOTI_SMTP_ENDPOINT = "smtp";
        public const string NOTI_SMTP_LOGIN = "login";
        public const string NOTI_SMTP_MAILS = "mailTo";

        public NotificationConCommand()
            :base()
        {
            rPar.Add(new ParOption(NOTI_SMTP_ENDPOINT, "<SMTP-ADDR>:<SMTP-PORT>", "Serveraddress and Serverport.", false, false, new Type[] { typeof(string) }));
            rPar.Add(new ParOption(NOTI_SMTP_LOGIN, "<SMTP-MAIL>:<SMTP-PASS>", "From Mailaddress.", false, false, new Type[] { typeof(string) }));
            rPar.Add(new ParOption(NOTI_SMTP_MAILS, "<TO-MAIL-ADDR>", "Mailaddresses to send notifications to.", false, true, new Type[] { typeof(MailAddress) }));
        }

        public override string Execute(int consoleWidth)
        { 
            return null; 
        }

        public JobNotificationSettings ParseJobNotificationSettings(ParInput pars)
        {
            JobNotificationSettings _settings = new JobNotificationSettings();

            string[] _temp = pars.GetPar(NOTI_SMTP_ENDPOINT).argValues[0].ToString().Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
            if (_temp.Length == 2)
            {
                _settings.login.smtpAddr = _temp[0];

                try
                {
                    _settings.login.port = Int32.Parse(_temp[1]);
                }
                catch (Exception)
                {
                    throw new Exception(CLIError.Error(CLIError.ErrorType.ArgumentTypeError, "Could not parse SMTP-Port!", true));
                }
            }
            else
                throw new Exception(CLIError.Error(CLIError.ErrorType.SyntaxError, "SMTP-Endpoint could not be parsed correctly!", true));

            string[] _temp2 = pars.GetPar(NOTI_SMTP_LOGIN).argValues[0].ToString().Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
            if (_temp2.Length == 2)
            {
                try
                {
                    _settings.login.mail = new MailAddress(_temp2[0]);
                }
                catch (Exception)
                {
                    throw new Exception(CLIError.Error(CLIError.ErrorType.ArgumentTypeError, "Could not parse MailAddress!", true));
                }

                _settings.login.password = _temp2[1];
            }
            else
                throw new Exception(CLIError.Error(CLIError.ErrorType.SyntaxError, "SMTP-Login could not be parsed correctly!", true));

            _settings.mailAddr = new MailAddress[1] { (MailAddress) pars.GetPar(NOTI_SMTP_MAILS).argValues[0] };

            return _settings;
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
    }

    #endregion

    #endregion
}
