using System;
using System.Net;
using MAD.jobSys;

namespace MAD.cli
{
    public class JobSystemStatusCommand : Command
    {
        JobSystem _js;
        ConsoleTable _jobTable;

        public JobSystemStatusCommand(object[] args)
            : base()
        {
            _js = (JobSystem)args[0];
            description = "This command prints a table with all jobs.";
        }

        public override string Execute()
        {
            string[] tableRow = new string[] { "Job-ID", "Job-Name", "Job-Type", "Time-Type", "Time-Value(s)", "Job-State", "Out-State" };
            _jobTable = new ConsoleTable(tableRow, Console.BufferWidth);

            output += "<color><yellow>\n";
            output += "Scedule-State: ";

            if (_js.sceduleState == JobScedule.State.Running)
            {
                output += "<color><green>" + _js.sceduleState.ToString() + "<color><yellow>";
            }
            else
            {
                output += "<color><red>" + _js.sceduleState.ToString() + "<color><yellow>";
            }

            output += "\n\n";

            output += "Jobs max:         " + _js.maxJobs + "\n";
            output += "Jobs initialized: " + _js.jobs.Count + "\n";
            output += "Jobs running:     " + _js.JobsRunning() + "\n";
            output += "Jobs stopped:     " + _js.JobsStopped() + "\n\n";

            output += _jobTable.splitline + "\n";
            output += _jobTable.FormatStringArray(tableRow) + "\n";
            output += _jobTable.splitline + "\n";
            output += "<color><white>";

            lock (_js.jobsLock)
            {
                foreach (Job _temp in _js.jobs)
                {
                    tableRow[0] = _temp.jobID.ToString();
                    tableRow[1] = _temp.jobName;
                    tableRow[2] = _temp.jobType.ToString();
                    tableRow[3] = _temp.jobTime.type.ToString();

                    if (_temp.jobTime.type == JobTime.TimeType.Relativ)
                    {
                        tableRow[4] = _temp.jobTime.jobDelay.delayTime.ToString();
                    }
                    else if (_temp.jobTime.type == JobTime.TimeType.Absolute)
                    {
                        tableRow[4] = "";

                        foreach (JobTimeHandler _temp2 in _temp.jobTime.jobTimes)
                        {
                            tableRow[4] += _temp2.JobTimeStatus() + " ";
                        }
                    }
                    else
                    {
                        tableRow[4] = "NULL";
                    }

                    tableRow[5] = _temp.jobState.ToString();
                    tableRow[6] = _temp.outState.ToString();

                    output += _jobTable.FormatStringArray(tableRow) + "\n";
                }
            }

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

    public class JobStatusCommand : Command
    {
        private JobSystem _js;

        public JobStatusCommand(object[] args)
            : base()
        {
            _js = (JobSystem)args[0];
            optionalParameter.Add(new ParameterOption("id", "COMMAND-ID", "ID of the job.", false, false, new Type[] { typeof(int) }));
        }

        public override string Execute()
        {
            if (OptionalParameterUsed("id"))
            {
                Job _job = _js.GetJob((int)parameters.GetParameter("id").argumentValue[0]);

                if (_job != null)
                {
                    output = _job.Status();
                }
                else
                {
                    output = "<color><red>Job does not exist!";
                }

                return output;
            }

            else
            {
                output += "<color><yellow>Jobs initialized: " + _js.jobs.Count + "\n\n";

                foreach (Job _job in _js.jobs)
                {
                    output += _job.Status() + "\n";
                }
            }

            return output;
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
            description = "This command adds a job with the jobtype 'HostDetect' to the jobsystem.";
        }

        public override string Execute()
        {
            JobHostDetect _job = new JobHostDetect();

            _job.jobName = (string)parameters.GetParameter("n").argumentValue[0];
            _job.jobType = Job.JobType.HostDetect;

            _job.Net = (IPAddress)parameters.GetParameter("a").argumentValue[0];
            _job.Net = (IPAddress)parameters.GetParameter("m").argumentValue[0];

            if (OptionalParameterUsed("t"))
            {
                Type _argumentType = GetArgumentType("t");

                if (_argumentType == typeof(int))
                {
                    _job.jobTime.type = JobTime.TimeType.Relativ;
                    _job.jobTime.jobDelay = new JobDelayHandler((int)parameters.GetParameter("t").argumentValue[0]);
                }
                else if (_argumentType == typeof(string))
                {
                    _job.jobTime.type = JobTime.TimeType.Absolute;

                    try
                    {
                        _job.jobTime.jobTimes = JobTime.ParseStringArray(parameters.GetParameter("t").argumentValue);
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
                _job.jobTime.type = JobTime.TimeType.Relativ;
            }

            _js.CreateJob(_job);

            return output;
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

            description = "This command adds a job with the jobtype 'PingRequest' to the jobsystem.";
        }

        public override string Execute()
        {
            string jobName = (string)parameters.GetParameter("n").argumentValue[0];
            IPAddress targetAddress = (IPAddress)parameters.GetParameter("ip").argumentValue[0];

            JobPing _job = new JobPing();
            _job.jobName = jobName;
            _job.jobType = Job.JobType.Ping;
            _job.targetAddress = targetAddress;

            if (OptionalParameterUsed("t"))
            {
                Type _argumentType = GetArgumentType("t");

                if (_argumentType == typeof(int))
                {
                    _job.jobTime.type = JobTime.TimeType.Relativ;
                    _job.jobTime.jobDelay = new JobDelayHandler((int)parameters.GetParameter("t").argumentValue[0]);
                }
                else if (_argumentType == typeof(string))
                {
                    _job.jobTime.type = JobTime.TimeType.Absolute;

                    try
                    {
                        _job.jobTime.jobTimes = JobTime.ParseStringArray(parameters.GetParameter("t").argumentValue);
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
                _job.jobTime.type = JobTime.TimeType.Relativ;
            }

            if (OptionalParameterUsed("ttl"))
            {
                _job.ttl = (int)parameters.GetParameter("ttl").argumentValue[0];
            }

            try
            {
                _js.CreateJob(_job);
            }
            catch (Exception e)
            {
                output = "<color><red>" + e.Message;
            }

            return output;
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
            string jobName = (string)parameters.GetParameter("n").argumentValue[0];
            IPAddress targetAddress = (IPAddress)parameters.GetParameter("ip").argumentValue[0];

            JobHttp _job = new JobHttp();

            _job.jobName = jobName;
            _job.jobType = Job.JobType.Http;
            _job.targetAddress = targetAddress;

            if (OptionalParameterUsed("t"))
            {
                Type _argumentType = GetArgumentType("t");

                if (_argumentType == typeof(int))
                {
                    _job.jobTime.type = JobTime.TimeType.Relativ;
                    _job.jobTime.jobDelay = new JobDelayHandler((int)parameters.GetParameter("t").argumentValue[0]);
                }
                else if (_argumentType == typeof(string))
                {
                    _job.jobTime.type = JobTime.TimeType.Absolute;

                    try
                    {
                        _job.jobTime.jobTimes = JobTime.ParseStringArray(parameters.GetParameter("t").argumentValue);
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
                _job.jobTime.type = JobTime.TimeType.Relativ;
            }

            if (OptionalParameterUsed("p"))
            {
                _job.port = (int)parameters.GetParameter("p").argumentValue[0];
            }

            try
            {
                _js.CreateJob(_job);
            }
            catch (Exception e)
            {
                output = "<color><red>" + e.Message;
            }

            return output;
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
            string jobName = (string)parameters.GetParameter("n").argumentValue[0];
            IPAddress targetAddress = (IPAddress)parameters.GetParameter("ip").argumentValue[0];
            int port = (int)parameters.GetParameter("p").argumentValue[0];

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
                    _job.jobTime.type = JobTime.TimeType.Relativ;
                    _job.jobTime.jobDelay = new JobDelayHandler((int)parameters.GetParameter("t").argumentValue[0]);
                }
                else if (_argumentType == typeof(string))
                {
                    _job.jobTime.type = JobTime.TimeType.Absolute;

                    try
                    {
                        _job.jobTime.jobTimes = JobTime.ParseStringArray(parameters.GetParameter("t").argumentValue);
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
                _job.jobTime.type = JobTime.TimeType.Relativ;
            }

            try
            {
                _js.CreateJob(_job);
            }
            catch (Exception e)
            {
                output = "<color><red>" + e.Message;
            }

            return output;
        }
    }

    public class JobSystemRemoveCommand : Command
    {
        private JobSystem _js;

        public JobSystemRemoveCommand(object[] args)
            : base()
        {
            _js = (JobSystem)args[0];
            requiredParameter.Add(new ParameterOption("id", "JOB-ID", "ID of the job.", false, true, new Type[] { typeof(int) }));
        }

        public override string Execute()
        {
            int id = (int)parameters.GetParameter("id").argumentValue[0];

            try
            {
                _js.DestroyJob(id);
                output += "<color><green>Job destroyed.";
            }
            catch (Exception e)
            {
                output += "<color><red>" + e.Message;
            }

            return output;
        }
    }

    public class JobSystemStartCommand : Command
    {
        private JobSystem _js;

        public JobSystemStartCommand(object[] args)
            : base()
        {
            _js = (JobSystem)args[0];
            requiredParameter.Add(new ParameterOption("id", "JOB-ID", "ID of the job.", false, true, new Type[] { typeof(int) }));
        }

        public override string Execute()
        {
            int id = (int)parameters.GetParameter("id").argumentValue[0];

            try
            {
                _js.StartJob(id);
                output = "<color><green>Job started.";
            }
            catch (Exception e)
            {
                output = "<color><red>" + e.Message;
            }      

            return output;
        }
    }

    public class JobSystemStopCommand : Command
    {
        private JobSystem _js;

        public JobSystemStopCommand(object[] args)
            : base()
        {
            _js = (JobSystem)args[0];
            requiredParameter.Add(new ParameterOption("id", "JOB-ID", "ID of the job.", false, true, new Type[] { typeof(int) }));
        }

        public override string Execute()
        {
            int id = (int)parameters.GetParameter("id").argumentValue[0];

            try
            {
                _js.StopJob(id);
                output = "<color><green>Job stopped.";
            }
            catch (Exception e)
            {
                output = "<color><red>FAIL: " + e.Message;
            }      

            return output;
        }
    }
}
