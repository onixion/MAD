using System;
using System.Net;

namespace MAD.CLI
{
    public class JobSystemStatusCommand : Command
    {
        public JobSystemStatusCommand()
        {
            optionalParameter.Add(new ParameterOption("id", false, typeof(int)));
        }

        public override string Execute()
        {
            if (!OptionalParameterUsed("id"))
            {
                ConsoleTable jobTable;
                string[] tableTitle = new string[] { "ID", "Name", "Type", "Delay", "Active", "Output" };
                jobTable = new ConsoleTable(tableTitle);
                tableTitle = jobTable.FormatStringArray(tableTitle);

                output += "<color><yellow>\n";
                output += "Jobs initialized: " + MadComponents.components.jobSystem.jobs.Count + "\n";
                output += "Jobs active:      " + MadComponents.components.jobSystem.JobsActive() + "\n";
                output += "Jobs inactive:    " + MadComponents.components.jobSystem.JobsInactive() + "\n\n";
                output += "<color><red>";

                output += jobTable.WriteColumnes(tableTitle) + "\n";
                output += jobTable.splitline + "\n";

                output += "<color><white>";
                foreach (Job job in MadComponents.components.jobSystem.jobs)
                {
                    string[] array = new string[tableTitle.Length];
                    array[0] = job.jobID.ToString();
                    array[1] = job.jobOptions.jobName;
                    array[2] = job.jobOptions.jobType.ToString();
                    array[3] = job.jobOptions.jobDelay.ToString();
                    array[4] = job.Active().ToString();
                    array[5] = job.jobOutput;

                    // format the string array
                    array = jobTable.FormatStringArray(array);

                    // add the string array to output
                    output += jobTable.WriteColumnes(array);
                }

                output += "\n";
            }
            else
            { 
                int id = (int)parameters.GetParameter("id").value;

                if (!MadComponents.components.jobSystem.JobExist(id))
                {
                    Job job = MadComponents.components.jobSystem.GetJob(id);
                    ConsoleTable table;
                    string[] columes;

                    switch (job.jobOptions.jobType.ToString())
                    { 
                        case "PingRequest":
                            columes = new string[] {"ID", "Name", "Type", "Delay", "IP", "TTL", "Avtive", "Output" };
                            table = new ConsoleTable(columes);
                            columes = table.FormatStringArray(columes);
                            output += "<color><red>" + table.WriteColumnes(columes) + "\n";
                            output += table.splitline + "\n";

                            string[] array = new string[7];
                            array[0] = job.jobID.ToString();
                            array[1] = job.jobOptions.jobName;
                            array[2] = job.jobOptions.jobType.ToString();
                            array[3] = job.jobOptions.jobDelay.ToString();

                            // TODO

                            break;
                    }
                }
                else
                {
                    output += "<color><red>Job does not exist!";
                }
            }

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
                MadComponents.components.jobSystem.CreateJob(new JobOptions(jobName, (int)parameters.GetParameter("t").value, JobOptions.JobType.PingRequest), new JobPingOptions(targetAddress, (int)parameters.GetParameter("ttl").value));
            }
            else if (!OptionalParameterUsed("t") && !OptionalParameterUsed("ttl"))
            {
                // no optional parameter are used
                MadComponents.components.jobSystem.CreateJob(new JobOptions(jobName, 20000, JobOptions.JobType.PingRequest), new JobPingOptions(targetAddress, 250));
            }
            else if (OptionalParameterUsed("t"))
            {
                // optional parameter "t" used
                MadComponents.components.jobSystem.CreateJob(new JobOptions(jobName, (int)parameters.GetParameter("t").value, JobOptions.JobType.PingRequest), new JobPingOptions(targetAddress, 250));
            }
            else
            {
                // optional parameter "ttl" used
                MadComponents.components.jobSystem.CreateJob(new JobOptions(jobName, 20000, JobOptions.JobType.PingRequest), new JobPingOptions(targetAddress, (int)parameters.GetParameter("ttl").value));
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
                MadComponents.components.jobSystem.CreateJob(new JobOptions(jobName, (int)parameters.GetParameter("t").value, JobOptions.JobType.HttpRequest), new JobHttpOptions(targetAddress, (int)parameters.GetParameter("p").value));
            }
            else if (!OptionalParameterUsed("t") && !OptionalParameterUsed("p"))
            {
                // no optional parameter are used
                MadComponents.components.jobSystem.CreateJob(new JobOptions(jobName, 20000, JobOptions.JobType.HttpRequest), new JobHttpOptions(targetAddress, 80));
            }
            else if (OptionalParameterUsed("t"))
            {
                // optional parameter "t" used
                MadComponents.components.jobSystem.CreateJob(new JobOptions(jobName, (int)parameters.GetParameter("t").value, JobOptions.JobType.HttpRequest), new JobHttpOptions(targetAddress, 80));
            }
            else
            {
                // optional parameter "p" used
                MadComponents.components.jobSystem.CreateJob(new JobOptions(jobName, 20000, JobOptions.JobType.HttpRequest), new JobHttpOptions(targetAddress, (int)parameters.GetParameter("p").value));
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
            requiredParameter.Add(new ParameterOption("p", false, typeof(Int32)));

            optionalParameter.Add(new ParameterOption("t", false, typeof(Int32)));
        }

        public override string Execute()
        {
            string jobName = (string)parameters.GetParameter("n").value;
            IPAddress targetAddress = (IPAddress)parameters.GetParameter("ip").value;
            int port = (int)parameters.GetParameter("p").value;

            if (OptionalParameterUsed("t"))
            {
                // optional parameter "t" used
                MadComponents.components.jobSystem.CreateJob(new JobOptions(jobName, (int)parameters.GetParameter("t").value, JobOptions.JobType.PortRequest), new JobPortOptions(targetAddress, port));
            }
            else
            {
                // optional parameter "p" used
                MadComponents.components.jobSystem.CreateJob(new JobOptions(jobName, 20000, JobOptions.JobType.PortRequest), new JobPortOptions(targetAddress, port));
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

            try
            {
                MadComponents.components.jobSystem.DestroyJob(id);
                output += "<color><green>Job destroyed.";
            }
            catch (Exception e)
            {
                output += "<color><red>FAIL: " + e.Message;
            }

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

            try
            {
                MadComponents.components.jobSystem.StartJob(id);
                output = "<color><green>Job started.";
            }
            catch (Exception e)
            {
                output = "<color><red>FAIL: " + e.Message;
            }      

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

            try
            {
                MadComponents.components.jobSystem.StopJob(id);
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
