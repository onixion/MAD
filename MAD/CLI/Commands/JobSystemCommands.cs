using System;
using System.Net;

namespace MAD.CLI
{
    public class JobSystemStatusCommand : Command
    {
        private ConsoleTable jobTable;
        private string[] tableTitle = new string[] { "ID", "Name", "Type", "Active", "IP-Address", "Delay", "Output" };

        public override string Execute()
        {
            jobTable = new ConsoleTable(tableTitle);
            tableTitle = jobTable.FormatStringArray(tableTitle);

            output += "MadJobSystem v" + MadComponents.components.jobSystem.version + "\n\n";
            output += "Jobs initialized: " + MadComponents.components.jobSystem.jobs.Count + "\n";
            output += "Jobs active:      " + MadComponents.components.jobSystem.JobsActive() + "\n";
            output += "Jobs inactive:    " + MadComponents.components.jobSystem.JobsInactive() + "\n\n";
            output += jobTable.WriteColumnes(tableTitle) + "\n";
            output += jobTable.splitline + "\n";

            foreach (Job job in MadComponents.components.jobSystem.jobs)
            {
                string[] array = new string[tableTitle.Length];
                array[0] = job.jobID.ToString();
                array[1] = job.jobOptions.jobName;
                array[2] = job.jobOptions.jobType.ToString();
                array[3] = job.threadStopRequest.ToString();
                array[4] = job.jobOptions.targetAddress.ToString();
                array[5] = job.jobOptions.delay.ToString();
                array[6] = job.jobOutput;

                // format the string array
                array = jobTable.FormatStringArray(array);

                // add the string array to output
                foreach (string temp in array)
                {
                    output += temp;
                    output += " ";
                }
            }

            return output;
        }
    }

    public class JobListCommand : Command
    {
        public JobListCommand()
        {
            optionalParameter.Add(new ParameterOption("id",false, typeof(Int32)));
        }

        public override string Execute()
        {
            if(OptionalParameterUsed("id"))
            {
                int id = (int)parameters.GetParameter("id").value;

                if (MadComponents.components.jobSystem.JobExist(id))
                {
                    MadComponents.components.jobSystem.GetJob(id).JobStatus();
                }
                else
                    output = "Job with ID '" + id + "' does not exist.";
            }
            else
                foreach (Job job in MadComponents.components.jobSystem.jobs)
                    output += job.JobStatus();

            return output;
        }
    }

    public class JobSystemAddPingCommand : Command
    {
        public JobSystemAddPingCommand()
        {
            requiredParameter.Add(new ParameterOption("n", false, typeof(String)));
            requiredParameter.Add(new ParameterOption("ip", false, typeof(IPAddress)));

            optionalParameter.Add(new ParameterOption("t", false, typeof(Int32)));
            optionalParameter.Add(new ParameterOption("ttl", false, typeof(Int32)));
        }

        public override string Execute()
        {
            string jobName = (string)parameters.GetParameter("n").value;
            IPAddress targetAddress = (IPAddress)parameters.GetParameter("ip").value;

            if (OptionalParameterUsed("t") && OptionalParameterUsed("ttl"))
            {
                // both optional parameter are used
                MadComponents.components.jobSystem.CreateJob(new JobPingOptions(jobName, JobOptions.JobType.PingRequest, (int)parameters.GetParameter("t").value, targetAddress, (int)parameters.GetParameter("ttl").value));
            }
            else if (!OptionalParameterUsed("t") && !OptionalParameterUsed("ttl"))
            {
                // no optional parameter are used
                MadComponents.components.jobSystem.CreateJob(new JobPingOptions(jobName, JobOptions.JobType.PingRequest, 10000, targetAddress, 300));
            }
            else if (OptionalParameterUsed("t"))
            {
                // optional parameter "t" used
                MadComponents.components.jobSystem.CreateJob(new JobPingOptions(jobName, JobOptions.JobType.PingRequest, (int)parameters.GetParameter("t").value, targetAddress, 300));
            }
            else
            {
                // optional parameter "ttl" used
                MadComponents.components.jobSystem.CreateJob(new JobPingOptions(jobName, JobOptions.JobType.PingRequest, 10000, targetAddress, (int)parameters.GetParameter("ttl").value));
            }

            return output;
        }
    }

    public class JobSystemAddHttpCommand : Command
    {
        public JobSystemAddHttpCommand()
        {
            requiredParameter.Add(new ParameterOption("n", false, typeof(String)));
            requiredParameter.Add(new ParameterOption("ip", false, typeof(IPAddress)));

            optionalParameter.Add(new ParameterOption("t", false, typeof(Int32)));
            optionalParameter.Add(new ParameterOption("p", false, typeof(Int32)));
        }

        public override string Execute()
        {
            string jobName = (string)parameters.GetParameter("n").value;
            IPAddress targetAddress = (IPAddress)parameters.GetParameter("ip").value;

            if (OptionalParameterUsed("t") && OptionalParameterUsed("p"))
            {
                // both optional parameter are used
                MadComponents.components.jobSystem.CreateJob(new JobHttpOptions(jobName, JobOptions.JobType.HttpRequest, (int)parameters.GetParameter("t").value, targetAddress, (int)parameters.GetParameter("p").value));
            }
            else if (!OptionalParameterUsed("t") && !OptionalParameterUsed("p"))
            {
                // no optional parameter are used
                MadComponents.components.jobSystem.CreateJob(new JobHttpOptions(jobName, JobOptions.JobType.HttpRequest, 10000, targetAddress, 80));
            }
            else if (OptionalParameterUsed("t"))
            {
                // optional parameter "t" used
                MadComponents.components.jobSystem.CreateJob(new JobHttpOptions(jobName, JobOptions.JobType.HttpRequest, (int)parameters.GetParameter("t").value, targetAddress, 80));
            }
            else
            {
                // optional parameter "p" used
                MadComponents.components.jobSystem.CreateJob(new JobHttpOptions(jobName, JobOptions.JobType.HttpRequest, 10000, targetAddress, (int)parameters.GetParameter("p").value));
            }

            return output;
        }
    }

    public class JobSystemAddPortCommand : Command
    {
        public JobSystemAddPortCommand()
        {
            requiredParameter.Add(new ParameterOption("n", false, typeof(String)));
            requiredParameter.Add(new ParameterOption("ip", false, typeof(IPAddress)));

            optionalParameter.Add(new ParameterOption("t", false, typeof(Int32)));
            optionalParameter.Add(new ParameterOption("p", false, typeof(Int32)));
        }

        public override string Execute()
        {
            string jobName = (string)parameters.GetParameter("n").value;
            IPAddress targetAddress = (IPAddress)parameters.GetParameter("ip").value;

            if (OptionalParameterUsed("t") && OptionalParameterUsed("p"))
            {
                // both optional parameter are used
                MadComponents.components.jobSystem.CreateJob(new JobPortOptions(jobName, JobOptions.JobType.PortRequest, (int)parameters.GetParameter("t").value, targetAddress, (int)parameters.GetParameter("p").value));
            }
            else if (!OptionalParameterUsed("t") && !OptionalParameterUsed("p"))
            {
                // no optional parameter are used
                MadComponents.components.jobSystem.CreateJob(new JobPortOptions(jobName, JobOptions.JobType.PortRequest, 10000, targetAddress, 80));
            }
            else if (OptionalParameterUsed("t"))
            {
                // optional parameter "t" used
                MadComponents.components.jobSystem.CreateJob(new JobPortOptions(jobName, JobOptions.JobType.PortRequest, (int)parameters.GetParameter("t").value, targetAddress, 80));
            }
            else
            {
                // optional parameter "p" used
                MadComponents.components.jobSystem.CreateJob(new JobPortOptions(jobName, JobOptions.JobType.PortRequest, 10000, targetAddress, (int)parameters.GetParameter("p").value));
            }

            return output;
        }
    }

    public class JobSystemRemoveCommand : Command
    {
        public JobSystemRemoveCommand()
        {
            requiredParameter.Add(new ParameterOption("id", false, typeof(Int32)));
        }

        public override string Execute()
        {
            int id = (int)parameters.GetParameter("id").value;

            if (MadComponents.components.jobSystem.JobExist(id))
            {
                MadComponents.components.jobSystem.StopJob(id);
                MadComponents.components.jobSystem.DestroyJob(id);
            }
            else
                output = "[FAIL] Job does not exist.";

            return output;
        }
    }

    public class JobSystemStartCommand : Command
    {
        public JobSystemStartCommand()
        {
            requiredParameter.Add(new ParameterOption("id", false, typeof(Int32)));
        }

        public override string Execute()
        {
            int id = (int)parameters.GetParameter("id").value;

            if (MadComponents.components.jobSystem.JobExist(id))
            {
                MadComponents.components.jobSystem.StartJob(id);
            }
            else
                output = "[FAIL] Job does not exist.";

            return output;
        }
    }

    public class JobSystemStopCommand : Command
    {
        public JobSystemStopCommand()
        {
            requiredParameter.Add(new ParameterOption("id", false, typeof(Int32)));
        }

        public override string Execute()
        {
            int id = (int)parameters.GetParameter("id").value;

            if (MadComponents.components.jobSystem.JobExist(id))
            {
                MadComponents.components.jobSystem.StopJob(id);
            }
            else
                output = "[FAIL] Job does not exist.";

            return output;
        }
    }
}
