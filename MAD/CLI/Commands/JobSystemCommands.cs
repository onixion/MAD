using System;
using System.Net;
using MAD.JobSystem;

namespace MAD.CLI
{
    public class JobSystemStatusCommand : Command
    {
        ConsoleTable jobTable;

        public JobSystemStatusCommand()
        {
            InitCommand();
            description = "This command prints a table with all jobs!";
        }

        public override string Execute()
        {
            string[] tableRow = new string[] { "Job-ID", "Job-Name", "Job-Type", "Time-Type", "Time-Value", "Job-State", "Output-State" };
            jobTable = new ConsoleTable(tableRow, Console.BufferWidth);
            tableRow = jobTable.FormatStringArray(tableRow);

            output += "<color><yellow>\n";
            output += "Jobs initialized: " + Handler.components.jobSystem.jobs.Count + "\n";
            output += "Jobs running:     " + Handler.components.jobSystem.JobsRunning() + "\n";
            output += "Jobs stopped:     " + Handler.components.jobSystem.JobsStopped() + "\n\n";

            output += jobTable.WriteColumnes(tableRow) + "\n";
            output += jobTable.splitline + "\n";
            output += "<color><white>";

            /* It improves the performance when you can work with jobs directly! */
            foreach (Job _temp in Handler.components.jobSystem.jobs)
            {
                tableRow[0] = _temp.jobID.ToString();
                tableRow[1] = _temp.jobOptions.jobName;
                tableRow[2] = _temp.jobOptions.jobType.ToString();
                tableRow[3] = _temp.jobOptions.jobTime.type.ToString();

                if (_temp.jobOptions.jobTime.type == JobTime.TimeType.Relativ)
                {
                    tableRow[4] = _temp.jobOptions.jobTime.jobDelay.ToString();
                }
                else
                {
                    tableRow[4] = _temp.jobOptions.jobTime.jobTimes[0].ToString("hh:mm");
                }

                tableRow[5] = _temp.jobState.ToString();
                tableRow[6] = _temp.jobOutput.jobState.ToString();

                tableRow = jobTable.FormatStringArray(tableRow);
                output += jobTable.WriteColumnes(tableRow) + "\n";
            }

            return output;
        }
    }

    public class JobStatusCommand : Command
    {
        public JobStatusCommand()
        {
            InitCommand();

            optionalParameter.Add(new ParameterOption("id", "COMMAND-ID", "ID of the job.", false, false, new Type[] { typeof(int) }));
        }

        public override string Execute()
        {
            if (OptionalParameterUsed("id"))
            {
                Job _job = Handler.components.jobSystem.GetJob((int)parameters.GetParameter("id").argumentValue[0]);

                if (_job != null)
                {
                    output = _job.Status();
                }
                else
                {
                    output = "<color><red>Job does not exist!\n";
                }

                return output;
            }

            else
            {
                output += "<color><yellow>Jobs initialized: " + Handler.components.jobSystem.jobs.Count + "\n\n";

                foreach (Job _job in Handler.components.jobSystem.jobs)
                {
                    output += _job.Status() + "\n";
                }
            }

            return output;
        }
    
    }

    public static class JobDefaultValues
    {
        public static int defaultDelay = 20000;
        public static int defaultPort = 80;
        public static int defaultTTL = 250;
    }

    public class JobSystemAddPingCommand : Command
    {
        public JobSystemAddPingCommand()
        {
            InitCommand();

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

            _job.jobOptions.jobName = jobName;
            _job.jobOptions.jobType = JobOptions.JobType.PingRequest;
            _job.targetAddress = targetAddress;

            if (OptionalParameterUsed("t"))
            {
                _job.jobOptions.jobTime.jobDelay = (int)parameters.GetParameter("t").argumentValue[0];
            }

            if (OptionalParameterUsed("ttl"))
            {
                _job.ttl = (int)parameters.GetParameter("ttl").argumentValue[0];
            }

            Handler.components.jobSystem.CreateJob(_job);

            return output;
        }
    }

    public class JobSystemAddHttpCommand : Command
    {
        public JobSystemAddHttpCommand()
        {
            InitCommand();

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

            _job.jobOptions.jobName = jobName;
            _job.jobOptions.jobType = JobOptions.JobType.HttpRequest;
            _job.targetAddress = targetAddress;

            if (OptionalParameterUsed("t"))
            {
                _job.jobOptions.jobTime.jobDelay = (int)parameters.GetParameter("t").argumentValue[0];
            }

            if (OptionalParameterUsed("p"))
            {
                _job.port = (int)parameters.GetParameter("p").argumentValue[0];
            }

            Handler.components.jobSystem.CreateJob(_job);

            return output;
        }
    }

    public class JobSystemAddPortCommand : Command
    {
        public JobSystemAddPortCommand()
        {
            InitCommand();

            requiredParameter.Add(new ParameterOption("n", "JOB-NAME", "Name of the job.", false, false, new Type[] { typeof(string) }));
            requiredParameter.Add(new ParameterOption("ip", "IP-ADDRESS", "IpAddres of the target.", false, true, new Type[] { typeof(IPAddress) }));
            requiredParameter.Add(new ParameterOption("t", "JOB-TIME", "Delaytime or time on which the job schould be executed", false, true, new Type[] { typeof(string), typeof(int) }));
            optionalParameter.Add(new ParameterOption("p", "PORT", "Port-Address of the target.", false, false, new Type[] { typeof(int) }));
        }

        public override string Execute()
        {
            string jobName = (string)parameters.GetParameter("n").argumentValue[0];
            IPAddress targetAddress = (IPAddress)parameters.GetParameter("ip").argumentValue[0];
            int port = (int)parameters.GetParameter("p").argumentValue[0];

            JobPort _job = new JobPort();

            _job.jobOptions.jobName = jobName;
            _job.jobOptions.jobType = JobOptions.JobType.PortRequest;
            _job.targetAddress = targetAddress;
            _job.port = port;

            if (OptionalParameterUsed("t"))
            {
                _job.jobOptions.jobTime.jobDelay = (int)parameters.GetParameter("t").argumentValue[0];
            }

            // add job to jobsystem
            Handler.components.jobSystem.CreateJob(_job);

            return output;
        }
    }

    public class JobSystemRemoveCommand : Command
    {
        public JobSystemRemoveCommand()
        {
            InitCommand();

            requiredParameter.Add(new ParameterOption("id", "JOB-ID", "ID of the job.", false, true, new Type[] { typeof(int) }));
        }

        public override string Execute()
        {
            int id = (int)parameters.GetParameter("id").argumentValue[0];

            try
            {
                Handler.components.jobSystem.DestroyJob(id);
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
        public JobSystemStartCommand()
        {
            InitCommand();

            requiredParameter.Add(new ParameterOption("id", "JOB-ID", "ID of the job.", false, true, new Type[] { typeof(int) }));
        }

        public override string Execute()
        {
            int id = (int)parameters.GetParameter("id").argumentValue[0];

            try
            {
                Handler.components.jobSystem.StartJob(id);
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
        public JobSystemStopCommand()
        {
            InitCommand();

            requiredParameter.Add(new ParameterOption("id", "JOB-ID", "ID of the job.", false, true, new Type[] { typeof(int) }));
        }

        public override string Execute()
        {
            int id = (int)parameters.GetParameter("id").argumentValue[0];

            try
            {
                Handler.components.jobSystem.StopJob(id);
                output = "<color><green>Job stopped.";
            }
            catch (Exception e)
            {
                output = "<color><red>FAIL: " + e.Message;
            }      

            return output;
        }
    }

    public class JobSystemOutputCommand : Command
    {
        public JobSystemOutputCommand()
        {
            InitCommand();

            requiredParameter.Add(new ParameterOption("id", "JOB-ID", "ID of the job.", false, true, new Type[] { typeof(int) }));
        }

        public override string Execute()
        {
            int id = (int)parameters.GetParameter("id").argumentValue[0];

            Job job = Handler.components.jobSystem.GetJob(id);

            if (job != null)
            {
                output += "<color><yellow>" + job.jobOutput;
                return output;
            }
            else
            {
                output += "<color><red>Job does not exist!";
                return output;
            }
        }
    }
}
