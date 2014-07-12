using System;
using System.Net;
using System.Net.NetworkInformation;

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
            output += "<color><yellow>\nJOBSYSTEM version " + _js.version + "\n\n";

            output += " Nodes stored in RAM: <color><white>" + _js.nodesCount + "<color><yellow>\t\t(MAX=" + JobSystem.maxNodes + ")\n";
            output += " Jobs  stored in RAM: <color><white>" + _js.JobsInitialized() + "<color><yellow>\t\t(MAX=" + JobSystem.maxJobsPossible + ")\n";

            output += "\n\n Scedule-State: ";

            if (_js.sceduleState == JobScedule.State.Running)
                output += "<color><green>" + _js.sceduleState.ToString() + "<color><yellow>";
            else
                output += "<color><red>" + _js.sceduleState.ToString() + "<color><yellow>";

            output += "\n\n";

            return output;
        }

    }

    #endregion

    #region commands for SCEUDLE

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

            output += " <color><yellow>Nodes max:         <color><white>" + JobSystem.maxNodes + "\n";
            output += " <color><yellow>Nodes initialized: <color><white>" + _js.nodes.Count + "\n";
            output += " <color><yellow>Nodes active:      <color><white>" + _js.NodesActive() + "\n";
            output += " <color><yellow>Nodes inactive:    <color><white>" + _js.NodesInactive() + "\n\n";

            output += "<color><yellow>" + ConsoleTable.splitline; 
            output += ConsoleTable.FormatStringArray(Console.BufferWidth, _tableRow);
            output += ConsoleTable.splitline;

            output += "<color><white>";
            lock (_js.jsNodesLock)
            {
                foreach (JobNode _temp in _js.nodes)
                {
                    _tableRow[0] = _temp.id.ToString();
                    _tableRow[1] = _temp.nodeName;
                    _tableRow[2] = _temp.state.ToString();
                    _tableRow[3] = _temp.macAddress.ToString();
                    _tableRow[4] = _temp.ipAddress.ToString();
                    _tableRow[5] = _temp.jobs.Count.ToString();

                    output += ConsoleTable.FormatStringArray(Console.BufferWidth, _tableRow);
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
            requiredParameter.Add(new ParameterOption("id", "<NODE-ID>", "Id of the node.", false, false, new Type[] { typeof(int) }));
            description = "This command sets the node-state to active.";
        }

        public override string Execute(int consoleWidth)
        {
            if (_js.StartNode((int)parameters.GetParameter("id").argumentValues[0]))
            {
                return "<color><green>Node is active.";
            }
            else
            {
                return "<color><red>Node does not exist!";
            }
        }
    }

    public class JobSystemStopNodeCommand : Command
    { 
        private JobSystem _js;

        public JobSystemStopNodeCommand(object[] args)
        {
            _js = (JobSystem)args[0];
            requiredParameter.Add(new ParameterOption("id", "<NODE-ID>", "Id of the node.", false, false, new Type[] { typeof(int) }));
            description = "This command sets the node-state to inactive.";
        }

        public override string Execute(int consoleWidth)
        {
            if (_js.StartNode((int)parameters.GetParameter("id").argumentValues[0]))
            {
                return "<color><green>Node is inactive.";
            }
            else
            {
                return "<color><red>Node does not exist!";
            }
        }
    }

    public class JobSystemAddNodeCommand : Command
    { 
        private JobSystem _js;

        public JobSystemAddNodeCommand(object[] args)
        {
            _js = (JobSystem)args[0];
            requiredParameter.Add(new ParameterOption("n", "NODE-NAME", "Name of the node.", false, false, new Type[] { typeof(string) }));
            requiredParameter.Add(new ParameterOption("mac", "MAC-ADDRESS", "MAC-Address of the target.", false, false, new Type[] { typeof(PhysicalAddress) }));
            requiredParameter.Add(new ParameterOption("ip", "IP-ADDRESS", "IP-Address of the target.", false, false, new Type[] { typeof(IPAddress) }));

            // If there is enough time, this can be added. So jobs can be parsed directly from command.
            //requiredParameter.Add(new ParameterOption("jobs", "<JOB-OBJECTS>", "", false, false, new Type[] { typeof(IPAddress) }));

            description = "This command creates a node for the cached jobs.";
        }

        public override string Execute(int consoleWidth)
        {
            JobNode _node = new JobNode();

            _node.nodeName = (string)parameters.GetParameter("n").argumentValues[0];
            _node.macAddress = (PhysicalAddress)parameters.GetParameter("mac").argumentValues[0]; // MAC ADDRESS
            _node.ipAddress = (IPAddress)parameters.GetParameter("ip").argumentValues[0];

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
            requiredParameter.Add(new ParameterOption("id", "NODE-ID", "ID of the node.", false, false, new Type[] { typeof(int) }));
            description = "This command removes a node.";
        }

        public override string Execute(int consoleWidth)
        {
            if (_js.RemoveNode((int)parameters.GetParameter("id").argumentValues[0]))
            {
                return "<color><green>Node removed.";
            }
            else
            {
                return JSError.Error(JSError.Type.NodeNotExist, null, true);
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

            output += "\n";

            output += " <color><yellow>Jobs max:             <color><white>" + JobSystem.maxJobsPossible + "\n";
            output += " <color><yellow>Jobs initialized:     <color><white>" + _js.JobsInitialized() + "\n";
            output += " <color><yellow>Jobs waiting/running: <color><white>" + _js.NodesActive() + "\n";
            output += " <color><yellow>Jobs stopped:         <color><white>" + _js.NodesInactive() + "\n\n";

            output += "<color><yellow>" + ConsoleTable.splitline;
            output += ConsoleTable.FormatStringArray(Console.BufferWidth, _tableRow);
            output += ConsoleTable.splitline;

            output += "<color><white>";
            lock (_js.jsNodesLock)
            {
                foreach (JobNode _temp in _js.nodes)
                {
                    foreach (Job _temp2 in _temp.jobs)
                    {
                        _tableRow[0] = _temp.id.ToString();
                        _tableRow[1] = _temp2.id.ToString();
                        _tableRow[2] = _temp2.jobName;
                        _tableRow[3] = _temp2.jobType.ToString();
                        _tableRow[4] = _temp2.state.ToString();
                        _tableRow[5] = _temp2.jobTime.type.ToString();
                        _tableRow[6] = _temp2.jobTime.Values();
                        _tableRow[7] = _temp2.outState.ToString();

                        output += ConsoleTable.FormatStringArray(Console.BufferWidth, _tableRow);
                    }
                }
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
            optionalParameter.Add(new ParameterOption("id", "JOB-ID", "ID of the job.", false, false, new Type[] { typeof(int) }));
        }

        public override string Execute(int consoleWidth)
        {
            if (OptionalParameterUsed("id"))
            {
                lock (_js.jsNodesLock)
                {
                    Job _job = _js.GetJob((int)parameters.GetParameter("id").argumentValues[0]);

                    if (_job != null)
                    {
                        output = _job.Status();
                    }
                    else
                    {
                        output = "<color><red>Job does not exist!";
                    }
                }

                return output;
            }
            else
            {
                lock (_js.jsNodesLock)
                {
                    foreach (JobNode _node in _js.nodes)
                    {
                        foreach (Job _job in _node.jobs)
                        {
                            output += _job.Status() + "\n";
                        }
                    }
                }
            }

            return output;
        }
    }

    public class JobSystemStartJobCommand : Command
    {
        private JobSystem _js;

        public JobSystemStartJobCommand(object[] args)
        {
            _js = (JobSystem)args[0];
            requiredParameter.Add(new ParameterOption("id", "<JOB-ID>", "Id of the job.", false, false, new Type[] { typeof(int) }));
            description = "This command sets the job to active.";
        }

        public override string Execute(int consoleWidth)
        {
            int _jobID = (int)parameters.GetParameter("id").argumentValues[0];

            if (_js.StartJob(_jobID))
            {
                return "<color><green>Job is active.";
            }
            else
            {
                return "<color><red>Job does not exist!";
            }
        }
    }

    public class JobSystemStopJobCommand : Command
    {
        private JobSystem _js;

        public JobSystemStopJobCommand(object[] args)
        {
            _js = (JobSystem)args[0];
            requiredParameter.Add(new ParameterOption("id", "<JOB-ID>", "Id of the job.", false, false, new Type[] { typeof(int) }));
            description = "This command sets the job to inactive.";
        }

        public override string Execute(int consoleWidth)
        {
            int _jobID = (int)parameters.GetParameter("id").argumentValues[0];

            if (_js.StopJob(_jobID))
            {
                return "<color><green>Job is inactive.";
            }
            else
            {
                return "<color><red>Job does not exist!";
            }
        }
    }

    public class JobSystemRemoveJobCommand : Command
    {
        private JobSystem _js;

        public JobSystemRemoveJobCommand(object[] args)
            : base()
        {
            _js = (JobSystem)args[0];
            requiredParameter.Add(new ParameterOption("id", "JOB-ID", "ID of the job.", false, true, new Type[] { typeof(int) }));
        }

        public override string Execute(int consoleWidth)
        {
            int _jobID = (int)parameters.GetParameter("id").argumentValues[0];

            if (_js.RemoveJob(_jobID))
                return "<color><green>Job removed.";
            else
                return JSError.Error(JSError.Type.JobNotExist, null, true);
        }
    }

    #region commands for adding jobs

    public class JobSystemAddServiceCheckCommand : Command
    {
        private JobSystem _js;
        string serviceDescript = "Choose the service to check \n" +
            "        Choose between following: \n" +
            "           dns -> doesn't need any more arguments\n" +
            "           ftp -> needs -a, -u, -p; leave username and password empty if there is no\n" +
            "           NYI\n";

        public JobSystemAddServiceCheckCommand(object[] args)
            : base()
        {
            _js = (JobSystem)args[0];
            requiredParameter.Add(new ParameterOption("id", "NODE-ID", "ID of the node to add the job to.", false, false, new Type[] { typeof(int) }));
            requiredParameter.Add(new ParameterOption("n", "JOB-NAME", "Name of the job", false, false, new Type[] { typeof(string) }));
            requiredParameter.Add(new ParameterOption("s", "SERVICE", serviceDescript, false, false, new Type[] { typeof(string) }));
            //there will be more, still working
            optionalParameter.Add(new ParameterOption("u", "USERNAME", "Username on the server, e.g. ftp", false, false, new Type[] { typeof(string) }));
            optionalParameter.Add(new ParameterOption("p", "PASSWORD", "Password on the server, e.g. ftp", false, false, new Type[] { typeof(string) }));
            optionalParameter.Add(new ParameterOption("t", "TIME", "Delaytime or time on with the job should be executed", false, true, new Type[] { typeof(Int32), typeof(string) }));
            description = "Checks the given Service for availibility. See in 'job serviceCheck help' for a list of available jobs"; //empty promises yet
        }

        public override string Execute(int consoleWidth)
        {
            JobServiceCheck _job = new JobServiceCheck();

            _job.jobName = (string)parameters.GetParameter("n").argumentValues[0];
            _job.jobType = Job.JobType.ServiceCheck;

            _job.argument = (string)parameters.GetParameter("s").argumentValues[0];

            if (OptionalParameterUsed("u"))
                _job.username = (string)parameters.GetParameter("u").argumentValues[0];
            if (OptionalParameterUsed("p"))
                _job.password = (string)parameters.GetParameter("p").argumentValues[0];

            if (OptionalParameterUsed("t"))
            {
                Type _argumentType = GetArgumentType("t");

                if (_argumentType == typeof(int))
                {
                    _job.jobTime.type = JobTime.TimeType.Relative;
                    _job.jobTime.jobDelay = new JobDelayHandler((int)parameters.GetParameter("t").argumentValues[0]);
                }
                else if (_argumentType == typeof(string))
                {
                    _job.jobTime.type = JobTime.TimeType.Absolute;

                    try
                    {
                        _job.jobTime.jobTimes = JobTime.ParseStringArray(parameters.GetParameter("t").argumentValues);
                    }
                    catch (Exception e)
                    {
                        return e.Message;
                    }
                }
            }
            else
            {
                _job.jobTime.jobDelay = new JobDelayHandler(20000);
                _job.jobTime.type = JobTime.TimeType.Relative;
            }

            JobNode _node = _js.GetNode((int)parameters.GetParameter("id").argumentValues[0]);

            if (_node != null)
            {
                lock (_node.jobsLock)
                {
                    _node.jobs.Add(_job);
                }

                return "<color><green>Job (ID " + _job.id + ") added to node (ID " + _node.id + ").";
            }
            else
            {
                return JSError.Error(JSError.Type.NodeNotExist, "(ID " + _node.id + ")", true);
            }
        }
    }

    public class JobSystemAddHostDetectCommand : Command
    {
        private JobSystem _js;

        public JobSystemAddHostDetectCommand(object[] args)
            : base()
        {
            _js = (JobSystem)args[0];
            requiredParameter.Add(new ParameterOption("n", "JOB-NAME", "Name of the job", false, false, new Type[] { typeof(string) }));
            requiredParameter.Add(new ParameterOption("id", "NODE-ID", "ID of the node to add the job to.", false, false, new Type[] { typeof(int) }));
            //requiredParameter.Add(new ParameterOption("a", "NET-ADDRESS", "Netaddress of the target Net", false, false, new Type[] { typeof(IPAddress) }));
            requiredParameter.Add(new ParameterOption("m", "SUBNETMASK", "Subnetmask of the target Net", false, false, new Type[] { typeof(IPAddress) }));
            optionalParameter.Add(new ParameterOption("t", "TIME", "Delaytime or time on with the job should be executed.", false, true, new Type[] { typeof(Int32), typeof(string) }));
            description = "Checks the given Network for all IPAddresses. Mind that it won't work if Ping is blocked.";
        }

        public override string Execute(int consoleWidth)
        {
            JobHostDetect _job = new JobHostDetect();

            _job.jobName = (string)parameters.GetParameter("n").argumentValues[0];
            _job.jobType = Job.JobType.HostDetect;

            //_job. = (IPAddress)parameters.GetParameter("a").argumentValues[0]; Not necessary anymore
            _job.Subnetmask = (IPAddress)parameters.GetParameter("m").argumentValues[0];

            if (OptionalParameterUsed("t"))
            {
                Type _argumentType = GetArgumentType("t");

                if (_argumentType == typeof(int))
                {
                    _job.jobTime.type = JobTime.TimeType.Relative;
                    _job.jobTime.jobDelay = new JobDelayHandler((int)parameters.GetParameter("t").argumentValues[0]);
                }
                else if (_argumentType == typeof(string))
                {
                    _job.jobTime.type = JobTime.TimeType.Absolute;

                    try
                    {
                        _job.jobTime.jobTimes = JobTime.ParseStringArray(parameters.GetParameter("t").argumentValues);
                    }
                    catch (Exception e)
                    {
                        return e.Message;
                    }
                }
            }
            else
            {
                _job.jobTime.jobDelay = new JobDelayHandler(20000);
                _job.jobTime.type = JobTime.TimeType.Relative;
            }

            JobNode _node = _js.GetNode((int)parameters.GetParameter("id").argumentValues[0]);

            if (_node != null)
            {
                lock (_node.jobsLock)
                {
                    _node.jobs.Add(_job);
                }

                return "<color><green>Job (ID " + _job.id + ") added to node (ID " + _node.id + ").";
            }
            else
            {
                return JSError.Error(JSError.Type.NodeNotExist, "(ID " + _node.id + ")", true);
            }
        }
    }

    public class JobSystemAddPingCommand : Command
    {
        private JobSystem _js;

        public JobSystemAddPingCommand(object[] args)
            : base()
        {
            _js = (JobSystem)args[0];

            requiredParameter.Add(new ParameterOption("n", "JOB-NAME", "Name of the job.", false, false, new Type[] { typeof(string) }));
            requiredParameter.Add(new ParameterOption("id", "NODE-ID", "ID of the node to add the job to.", false, false, new Type[] { typeof(int) }));
            optionalParameter.Add(new ParameterOption("t", "TIME", "Delaytime or time on which th job should be executed.", false, true, new Type[] { typeof(Int32), typeof(string) }));
            optionalParameter.Add(new ParameterOption("ttl", "TTL", "TTL of the ping.", false, false, new Type[] { typeof(int) })); 

            description = "This command adds a job with the jobtype 'PingRequest' to the node with the given ID.";
        }

        public override string Execute(int consoleWidth)
        {
            string jobName = (string)parameters.GetParameter("n").argumentValues[0];

            JobPing _job = new JobPing();
            _job.jobName = jobName;
            _job.jobType = Job.JobType.Ping;

            if (OptionalParameterUsed("t"))
            {
                Type _argumentType = GetArgumentType("t");

                if (_argumentType == typeof(int))
                {
                    _job.jobTime.type = JobTime.TimeType.Relative;
                    _job.jobTime.jobDelay = new JobDelayHandler((int)parameters.GetParameter("t").argumentValues[0]);
                }
                else if (_argumentType == typeof(string))
                {
                    _job.jobTime.type = JobTime.TimeType.Absolute;

                    try
                    {
                        _job.jobTime.jobTimes = JobTime.ParseStringArray(parameters.GetParameter("t").argumentValues);
                    }
                    catch (Exception e)
                    {
                        return "<color><red>" + e.Message;
                    }
                }
            }
            else
            {
                _job.jobTime.jobDelay = new JobDelayHandler(20000);
                _job.jobTime.type = JobTime.TimeType.Relative;
            }

            if (OptionalParameterUsed("ttl"))
            {
                _job.ttl = (int)parameters.GetParameter("ttl").argumentValues[0];
            }

            // So now the JobPing is finished and set properly. 
            
            JobNode _node = _js.GetNode((int)parameters.GetParameter("id").argumentValues[0]);

            if(_node != null)
            {
                // Use lock. Because if the scedule is working on those jobs, it will fail.
                lock(_node.jobsLock)
                {
                    _node.jobs.Add(_job);
                }

                return "<color><green>Job (ID " + _job.id + ") added to node (ID " + _node.id + ").";
            }
            else
            {
                // JobSystemError.
                return JSError.Error(JSError.Type.NodeNotExist, null, true);
            }
        }
    }

    public class JobSystemAddHttpCommand : Command
    {
        private JobSystem _js;

        public JobSystemAddHttpCommand(object[] args)
            : base()
        {
            _js = (JobSystem)args[0];
            requiredParameter.Add(new ParameterOption("n", "JOB-NAME", "Name of the job.", false, false, new Type[] { typeof(string) }));
            requiredParameter.Add(new ParameterOption("id", "NODE-ID", "ID of the node to add the job to.", false, false, new Type[] { typeof(int) }));
            optionalParameter.Add(new ParameterOption("t", "JOB-TIME", "Delaytime or time on which the job schould be executed", false, true, new Type[] { typeof(string), typeof(int) }));
            optionalParameter.Add(new ParameterOption("p", "PORT", "Port-Address of the target.", false, false, new Type[] { typeof(int) }));
        }

        public override string Execute(int consoleWidth)
        {
            string jobName = (string)parameters.GetParameter("n").argumentValues[0];

            JobHttp _job = new JobHttp();

            _job.jobName = jobName;
            _job.jobType = Job.JobType.Http;

            if (OptionalParameterUsed("t"))
            {
                Type _argumentType = GetArgumentType("t");

                if (_argumentType == typeof(int))
                {
                    _job.jobTime.type = JobTime.TimeType.Relative;
                    _job.jobTime.jobDelay = new JobDelayHandler((int)parameters.GetParameter("t").argumentValues[0]);
                }
                else if (_argumentType == typeof(string))
                {
                    _job.jobTime.type = JobTime.TimeType.Absolute;

                    try
                    {
                        _job.jobTime.jobTimes = JobTime.ParseStringArray(parameters.GetParameter("t").argumentValues);
                    }
                    catch (Exception e)
                    {
                        return e.Message;
                    }
                }
            }
            else
            {
                _job.jobTime.jobDelay = new JobDelayHandler(20000);
                _job.jobTime.type = JobTime.TimeType.Relative;
            }

            if (OptionalParameterUsed("p"))
            {
                _job.port = (int)parameters.GetParameter("p").argumentValues[0];
            }

            JobNode _node = _js.GetNode((int)parameters.GetParameter("id").argumentValues[0]);

            if(_node != null)
            {
                // Use lock. Because if the scedule is working on those jobs, it will fail.
                lock(_node.jobsLock)
                {
                    _node.jobs.Add(_job);
                }

                return "<color><green>Job (ID " + _job.id + ") added to node (ID " + _node.id + ").";
            }
            else
            {
                // JobSystemError.
                return JSError.Error(JSError.Type.NodeNotExist, "(ID " + _node.id + ")", true);
            }
        }
    }

    public class JobSystemAddPortCommand : Command
    {
        private JobSystem _js;

        public JobSystemAddPortCommand(object[] args)
            : base()
        {
            _js = (JobSystem)args[0];
            requiredParameter.Add(new ParameterOption("n", "JOB-NAME", "Name of the job.", false, false, new Type[] { typeof(string) }));
            requiredParameter.Add(new ParameterOption("id", "NODE-ID", "ID of the node to add the job to.", false, false, new Type[] { typeof(int) }));
            requiredParameter.Add(new ParameterOption("p", "PORT", "Port-Address of the target.", false, false, new Type[] { typeof(int) }));
            optionalParameter.Add(new ParameterOption("t", "JOB-TIME", "Delaytime or time on which the job should be executed", false, true, new Type[] { typeof(string), typeof(int) }));
        }

        public override string Execute(int consoleWidth)
        {
            string jobName = (string)parameters.GetParameter("n").argumentValues[0];
            int port = (int)parameters.GetParameter("p").argumentValues[0];

            JobPort _job = new JobPort();

            _job.jobName = jobName;
            _job.jobType = Job.JobType.PortScan;
            _job.port = port;

            if (OptionalParameterUsed("t"))
            {
                Type _argumentType = GetArgumentType("t");

                if (_argumentType == typeof(int))
                {
                    _job.jobTime.type = JobTime.TimeType.Relative;
                    _job.jobTime.jobDelay = new JobDelayHandler((int)parameters.GetParameter("t").argumentValues[0]);
                }
                else if (_argumentType == typeof(string))
                {
                    _job.jobTime.type = JobTime.TimeType.Absolute;

                    try
                    {
                        _job.jobTime.jobTimes = JobTime.ParseStringArray(parameters.GetParameter("t").argumentValues);
                    }
                    catch (Exception e)
                    {
                        return e.Message;
                    }
                }
            }
            else
            {
                _job.jobTime.jobDelay = new JobDelayHandler(20000);
                _job.jobTime.type = JobTime.TimeType.Relative;
            }

            JobNode _node = _js.GetNode((int)parameters.GetParameter("id").argumentValues[0]);

            if (_node != null)
            {
                lock (_node.jobsLock)
                {
                    _node.jobs.Add(_job);
                }

                return "<color><green>Job (ID " + _job.id + ") added to node (ID " + _node.id + ").";
            }
            else
            {
                return JSError.Error(JSError.Type.NodeNotExist, "(ID " + _node.id + ")", true);
            }
        }
    }

    #endregion

    #endregion
}
