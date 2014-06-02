using System;
using System.Net;
using MAD.JobSystem;

namespace MAD.CLI
{
    public class JobSystemStatusCommand : Command
    {
        ConsoleTable jobTable;

        public override string Execute()
        {
            string[] tableRow = new string[] { "ID", "Name", "Type", "Delay", "Active", "Output-State" };
            jobTable = new ConsoleTable(tableRow, Console.BufferWidth);
            tableRow = jobTable.FormatStringArray(tableRow);

            output += "<color><yellow>\n";
            output += "Jobs initialized: " + MadComponents.components.jobSystem.jobs.Count + "\n";
            output += "Jobs active:      " + MadComponents.components.jobSystem.JobsActive() + "\n";
            output += "Jobs inactive:    " + MadComponents.components.jobSystem.JobsInactive() + "\n\n";

            output += jobTable.WriteColumnes(tableRow) + "\n";
            output += jobTable.splitline + "\n";
            output += "<color><white>";

            /* It improves the performance when you can work with jobs directly! */
            foreach (Job _temp in MadComponents.components.jobSystem.jobs)
            {
                tableRow[0] = _temp.jobID.ToString();
                tableRow[1] = _temp.jobOptions.jobName;
                tableRow[2] = _temp.jobOptions.jobType.ToString();
                tableRow[3] = _temp.jobOptions.jobDelay.ToString();
                tableRow[4] = _temp.Active().ToString();
                tableRow[5] = _temp.jobOutput.jobState.ToString();

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
            optionalParameter.Add(new ParameterOption("id", false, typeof(int)));
        }

        public override string Execute()
        {
            if (OptionalParameterUsed("id"))
            {
                Job _job = MadComponents.components.jobSystem.GetJob((int)parameters.GetParameter("id").value);

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

            }
            else if (!OptionalParameterUsed("t") && !OptionalParameterUsed("ttl"))
            {
                // no optional parameter are used

            }
            else if (OptionalParameterUsed("t"))
            {
                // optional parameter "t" used
            }
            else
            {
                // optional parameter "ttl" used

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

            }
            else if (!OptionalParameterUsed("t") && !OptionalParameterUsed("p"))
            {
                // no optional parameter are used
 
            }
            else if (OptionalParameterUsed("t"))
            {
                // optional parameter "t" used
 
            }
            else
            {
                // optional parameter "p" used
               
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

            JobPort _job; 

            if (OptionalParameterUsed("t"))
            {
                // optional parameter "t" used
                _job = new JobPort(new JobOptions(jobName, (int)parameters.GetParameter("t").value, JobOptions.JobType.PortRequest), targetAddress, port);
            }
            else
            {
                // optional parameter "t"  not used
                _job = new JobPort(new JobOptions(jobName, 20000, JobOptions.JobType.PortRequest), targetAddress, port);
            }

            // add job to jobsystem
            MadComponents.components.jobSystem.CreateJob(_job);

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

    public class JobSystemOutputCommand : Command
    {
        public JobSystemOutputCommand()
        {
            requiredParameter.Add(new ParameterOption("id", false, typeof(int)));
        }

        public override string Execute()
        {
            int id = (int)parameters.GetParameter("id").value;

            Job job = MadComponents.components.jobSystem.GetJob(id);

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
