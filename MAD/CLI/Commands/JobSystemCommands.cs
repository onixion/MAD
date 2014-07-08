using System;
using System.Net;
using System.Net.NetworkInformation;

using MAD.JobSystemCore;

namespace MAD.CLICore
{
    // TODO: string Execute(int consoleWidth)

    #region commands for scedule

    public class JobSceduleStartCommand : Command
    {
        JobSystem _js;

        public JobSceduleStartCommand(object[] args)
            : base()
        {
            _js = (JobSystem)args[0];
        }

        public override string Execute()
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

        public override string Execute()
        {
            _js.StopScedule();
            return "<color><green>Scedule stopped.";
        }
    }

    #endregion

    #region commands for jobsystem

    public class JobSystemStatusCommand : Command
    {
        private JobSystem _js;

        public JobSystemStatusCommand(object[] args)
            : base()
        {
            _js = (JobSystem)args[0];
            description = "This command shows informations about the jobsystem.";
        }

        public override string Execute()
        {
            output += "<color><yellow>Scedule-State: ";

            if (_js.sceduleState == JobScedule.State.Running)
            {
                output += "<color><green>" + _js.sceduleState.ToString() + "<color><yellow>";
            }
            else
            {
                output += "<color><red>" + _js.sceduleState.ToString() + "<color><yellow>";
            }

            return output;
        }
    
    }

    public class JobSystemStatusNodesCommand : Command
    {
        JobSystem _js;
        ConsoleTable _jobTable;

        public JobSystemStatusNodesCommand(object[] args)
            : base()
        {
            _js = (JobSystem)args[0];
            description = "This command prints a table with all nodes.";
        }

        public override string Execute()
        {
            string[] tableRow = new string[] { "Node-ID", "Node-Name", "Node-State", "MAC-Address", "IP-Address", "Jobs Init." };
            _jobTable = new ConsoleTable(tableRow, Console.BufferWidth);

            output += "<color><yellow>Nodes max:         " + JobSystem.maxNodes + "\n";
            output += "Nodes initialized: " + _js.nodes.Count + "\n";
            output += "Nodes active:      " + _js.NodesActive() + "\n";
            output += "Nodes inactive:    " + _js.NodesInactive() + "\n\n";

            output += _jobTable.splitline + "\n";
            output += _jobTable.FormatStringArray(tableRow) + "\n";
            output += _jobTable.splitline + "\n";
            output += "<color><white>";

            lock (_js.jsNodesLock)
            {
                foreach (JobNode _temp in _js.nodes)
                {
                    tableRow[0] = _temp.nodeID.ToString();
                    tableRow[1] = _temp.nodeName;
                    tableRow[2] = _temp.state.ToString();
                    tableRow[3] = _temp.macAddress.ToString();
                    tableRow[4] = _temp.ipAddress.ToString();
                    tableRow[5] = _temp.jobs.Count.ToString();

                    output += _jobTable.FormatStringArray(tableRow) + "\n";
                }
            }

            return output;
        }
    }

    public class JobSystemStatusJobsCommand : Command
    {
        JobSystem _js;
        ConsoleTable _jobTable;

        public JobSystemStatusJobsCommand(object[] args)
            : base()
        {
            _js = (JobSystem)args[0];
            description = "This command prints a table with all jobs.";
        }

        public override string Execute()
        {
            string[] tableRow = new string[] {"Node-ID","Job-ID", "Job-Name", "Job-Type", "Job-State", "Time-Type", "Time-Value(s)", "Output-State"};
            _jobTable = new ConsoleTable(tableRow, Console.BufferWidth);

            output += "<color><yellow><< JOBS IN NODES >>\n\n";

            output += "Jobs max:             " + JobSystem.maxJobsOverall + "\n";
            output += "Jobs initialized:     " + _js.NodesJobsInitialized() + "\n";
            output += "Jobs waiting/running: " + _js.NodesActive() + "\n";
            output += "Jobs stopped:         " + _js.NodesInactive() + "\n\n";

            output += _jobTable.splitline + "\n";
            output += _jobTable.FormatStringArray(tableRow) + "\n";
            output += _jobTable.splitline + "\n";
            output += "<color><white>";

            lock (_js.jsNodesLock)
            {
                foreach (JobNode _temp in _js.nodes)
                {
                    foreach (Job _temp2 in _temp.jobs)
                    {
                        tableRow[0] = _temp.nodeID.ToString();
                        tableRow[1] = _temp2.jobID.ToString();
                        tableRow[2] = _temp2.jobName;
                        tableRow[3] = _temp2.jobType.ToString();
                        tableRow[4] = _temp2.jobState.ToString();
                        tableRow[5] = _temp2.jobTime.type.ToString();
                        tableRow[6] = _temp2.jobTime.Values();
                        tableRow[6] = _temp2.jobState.ToString();

                        output += _jobTable.FormatStringArray(tableRow) + "\n";
                    }
                }
            }

            output += "\n\n<color><yellow><< JOBS IN CACHE >>\n\n";

            output += "Jobs max:             " + JobSystem.maxCachedJobs + "\n";
            output += "Jobs initialized:     " + _js.cachedJobs.Count + "\n";

            output += _jobTable.splitline + "\n";
            output += _jobTable.FormatStringArray(tableRow) + "\n";
            output += _jobTable.splitline + "\n";
            output += "<color><white>";

            foreach (Job _temp2 in _js.cachedJobs)
            {
                tableRow[0] = "CACHED";
                tableRow[1] = _temp2.jobID.ToString();
                tableRow[2] = _temp2.jobName;
                tableRow[3] = _temp2.jobType.ToString();
                tableRow[4] = _temp2.jobState.ToString();
                tableRow[5] = _temp2.jobTime.type.ToString();
                tableRow[6] = _temp2.jobTime.Values();
                tableRow[6] = _temp2.jobState.ToString();

                output += _jobTable.FormatStringArray(tableRow) + "\n";
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

        public override string Execute()
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

        public override string Execute()
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

    public class JobSystemStartJobCommand : Command
    { 
        private JobSystem _js;

        public JobSystemStartJobCommand(object[] args)
        {
            _js = (JobSystem)args[0];
            requiredParameter.Add(new ParameterOption("id", "<JOB-ID>", "Id of the job.", false, false, new Type[] { typeof(int) }));
            description = "This command sets the job to active.";
        }

        public override string Execute()
        {
            Job _job = _js.GetJob((int)parameters.GetParameter("id").argumentValues[0]);

            if (_job != null)
            {
                _job.jobState = Job.JobState.Waiting; // not sure if this works

                return "<color><green>Node is inactive.";
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

        public override string Execute()
        {
            Job _job = _js.GetJob((int)parameters.GetParameter("id").argumentValues[0]);

            if (_job != null)
            {
                _job.jobState = Job.JobState.Stopped; // not sure if this works

                return "<color><green>Job is inactive.";
            }
            else
            {
                return "<color><red>Job does not exist!";
            }
        }
    }

    public class JobSystemCreateNodeCommand : Command
    { 
        private JobSystem _js;

        public JobSystemCreateNodeCommand(object[] args)
        {
            _js = (JobSystem)args[0];
            requiredParameter.Add(new ParameterOption("n", "<NODE-NAME>", "Name of the node.", false, false, new Type[] { typeof(string) }));
            requiredParameter.Add(new ParameterOption("mac", "<MAC-ADDRESS>", "MAC-Address of the target.", false, false, new Type[] { typeof(PhysicalAddress) }));
            requiredParameter.Add(new ParameterOption("ip", "<IP-ADDRESS>", "IP-Address of the target.", false, false, new Type[] { typeof(IPAddress) }));
            optionalParameter.Add(new ParameterOption("clear", "", "Flag to clear the cache.", true, false, null));
            description = "This command creates a node for the cached jobs.";
        }

        public override string Execute()
        {
            JobNode _node = new JobNode(_js.jsNodesLock);

            _node.nodeName = (string)parameters.GetParameter("n").argumentValues[0];
            _node.macAddress = (PhysicalAddress)parameters.GetParameter("mac").argumentValues[0]; // MAC ADDRESS
            _node.ipAddress = (IPAddress)parameters.GetParameter("ip").argumentValues[0];

            _node.jobs = _js.cachedJobs;

            // Add node to jobsystem.
            _js.AddNode(_node);
            // Clear cache.
            _js.ClearCache();

            return "<color><green>Node created (ID '" + _node.nodeID + "').";
        }
    }

    #endregion

    #region commands for jobs (mainly used for cached jobs)

    public class JobStatusCommand : Command
    {
        private JobSystem _js;

        public JobStatusCommand(object[] args)
            : base()
        {
            _js = (JobSystem)args[0];
            optionalParameter.Add(new ParameterOption("id", "JOB-ID", "ID of the job.", false, false, new Type[] { typeof(int) }));
        }

        public override string Execute()
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
            requiredParameter.Add(new ParameterOption("n", "JOB-NAME", "Name of the job", false, false, new Type[] { typeof(string) }));
            requiredParameter.Add(new ParameterOption("s", "SERVICE", serviceDescript, false, false, new Type[] { typeof(string) }));
            //there will be more, still working
            optionalParameter.Add(new ParameterOption("a", "IPADDRESS", "IP-Address of the server", false, false, new Type[] { typeof(IPAddress) }));
            optionalParameter.Add(new ParameterOption("u", "USERNAME", "Username on the server, e.g. ftp", false, false, new Type[] { typeof(string) }));
            optionalParameter.Add(new ParameterOption("p", "PASSWORD", "Password on the server, e.g. ftp", false, false, new Type[] { typeof(string) }));
            optionalParameter.Add(new ParameterOption("t", "TIME", "Delaytime or time on with the job should be executed", false, true, new Type[] { typeof(Int32), typeof(string) }));
            description = "Checks the given Service for availibility. See in 'job serviceCheck help' for a list of available jobs"; //empty promises yet
        }

        public override string Execute()
        {
            JobServiceCheck _job = new JobServiceCheck();

            _job.jobName = (string)parameters.GetParameter("n").argumentValues[0];
            _job.jobType = Job.JobType.ServiceCheck;

            _job.argument = (string)parameters.GetParameter("s").argumentValues[0];

            if (OptionalParameterUsed("a"))
                _job.targetIP = (IPAddress)parameters.GetParameter("a").argumentValues[0];
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

            if (_js.AddToCache(_job))
            {
                return "<color><green>Job added to cache.";
            }
            else
            {
                return "<color><red>Cache limit reached!";
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
            requiredParameter.Add(new ParameterOption("a", "NET-ADDRESS", "Netaddress of the target Net", false, false, new Type[] { typeof(IPAddress) }));
            requiredParameter.Add(new ParameterOption("m", "SUBNETMASK", "Subnetmask of the target Net", false, false, new Type[] { typeof(IPAddress) }));
            optionalParameter.Add(new ParameterOption("t", "TIME", "Delaytime or time on with the job should be executed.", false, true, new Type[] { typeof(Int32), typeof(string) }));
            description = "Checks the given Network for all IPAddresses. Mind that it won't work if Ping is blocked.";
        }

        public override string Execute()
        {
            JobHostDetect _job = new JobHostDetect();

            _job.jobName = (string)parameters.GetParameter("n").argumentValues[0];
            _job.jobType = Job.JobType.HostDetect;

            _job.Net = (IPAddress)parameters.GetParameter("a").argumentValues[0];
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

            if (_js.AddToCache(_job))
            {
                return "<color><green>Job added to cache.";
            }
            else
            {
                return "<color><red>Cache limit reached!";
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
            requiredParameter.Add(new ParameterOption("ip", "IP-ADDRESS", "IPAddress of the target-machine.", false, false, new Type[] { typeof(IPAddress) }));
            optionalParameter.Add(new ParameterOption("t", "TIME", "Delaytime or time on which th job should be executed.", false, true, new Type[] { typeof(Int32), typeof(string) }));
            optionalParameter.Add(new ParameterOption("ttl", "TTL", "TTL of the ping.", false, false, new Type[] { typeof(int) })); 

            description = "This command adds a job with the jobtype 'PingRequest' to the cache of the jobsystem.";
        }

        public override string Execute()
        {
            string jobName = (string)parameters.GetParameter("n").argumentValues[0];
            IPAddress targetAddress = (IPAddress)parameters.GetParameter("ip").argumentValues[0];

            JobPing _job = new JobPing();
            _job.jobName = jobName;
            _job.jobType = Job.JobType.Ping;
            _job.targetAddress = targetAddress;

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

            if (_js.AddToCache(_job))
            {
                return "<color><green>Job added to cache.";
            }
            else
            {
                return "<color><red>Cache limit reached!";
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
            requiredParameter.Add(new ParameterOption("ip", "IP-ADDRESS", "IpAddres of the target.", false, true, new Type[] { typeof(IPAddress) }));
            optionalParameter.Add(new ParameterOption("t", "JOB-TIME", "Delaytime or time on which the job schould be executed", false, true, new Type[] { typeof(string), typeof(int) }));
            optionalParameter.Add(new ParameterOption("p", "PORT", "Port-Address of the target.", false, false, new Type[] { typeof(int) }));
        }

        public override string Execute()
        {
            string jobName = (string)parameters.GetParameter("n").argumentValues[0];
            IPAddress targetAddress = (IPAddress)parameters.GetParameter("ip").argumentValues[0];

            JobHttp _job = new JobHttp();

            _job.jobName = jobName;
            _job.jobType = Job.JobType.Http;
            _job.targetAddress = targetAddress;

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

            if (_js.AddToCache(_job))
            {
                return "<color><green>Job added to cache.";
            }
            else
            {
                return "<color><red>Cache limit reached!";
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
            requiredParameter.Add(new ParameterOption("ip", "IP-ADDRESS", "IpAddres of the target.", false, true, new Type[] { typeof(IPAddress) }));
            requiredParameter.Add(new ParameterOption("p", "PORT", "Port-Address of the target.", false, false, new Type[] { typeof(int) }));
            optionalParameter.Add(new ParameterOption("t", "JOB-TIME", "Delaytime or time on which the job should be executed", false, true, new Type[] { typeof(string), typeof(int) }));
        }

        public override string Execute()
        {
            string jobName = (string)parameters.GetParameter("n").argumentValues[0];
            IPAddress targetAddress = (IPAddress)parameters.GetParameter("ip").argumentValues[0];
            int port = (int)parameters.GetParameter("p").argumentValues[0];

            JobPort _job = new JobPort();

            _job.jobName = jobName;
            _job.jobType = Job.JobType.PortScan;
            _job.targetAddress = targetAddress;
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

            if (_js.AddToCache(_job))
            {
                return "<color><green>Job added to cache.";
            }
            else
            {
                return "<color><red>Cache limit reached!";
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

        public override string Execute()
        {
            int id = (int)parameters.GetParameter("id").argumentValues[0];

            try
            {
                //_js.DestroyJob(id);
                output += "<color><green>Job destroyed.";
            }
            catch (Exception e)
            {
                output += "<color><red>" + e.Message;
            }

            return output;
        }
    }

    #endregion
}
