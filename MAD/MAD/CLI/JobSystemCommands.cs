using System;
using System.Threading;
using System.Net;
using System.Collections.Generic;

namespace MAD
{
    public class JobSystemStatusCommand : Command
    {
        string jobSystemHeader = "MadJobSystem v" + MadComponents.components.jobSystem.version;
        int consoleWidth = Console.BufferWidth;

        string[] tableTitle = new string[] { "ID", "Name", "Type", "IP-Address", "Active", "Output" };
        ConsoleTable jobTable;

        public JobSystemStatusCommand() { }

        public override int Execute()
        {
            jobTable = new ConsoleTable(tableTitle);
            tableTitle = jobTable.FormatStringArray(tableTitle);

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(jobSystemHeader);
            Console.ForegroundColor = MadComponents.components.cli.textColor;
            Console.WriteLine();
            Console.WriteLine("Jobs initialized: " + MadComponents.components.jobSystem.jobs.Count);
            Console.WriteLine("Jobs active:      " + MadComponents.components.jobSystem.JobsCountActive());
            Console.WriteLine("Jobs inactive:    " + MadComponents.components.jobSystem.JobsCountInactive());
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            jobTable.WriteColumnes(tableTitle);
            Console.WriteLine("\n"+ jobTable.line);
            Console.ForegroundColor = MadComponents.components.cli.textColor;

            foreach (Job job in MadComponents.components.jobSystem.jobs)
            {
                string[] array = new string[tableTitle.Length];
                array[0] = job.jobID.ToString();
                array[1] = job.jobOptions.jobName;
                array[2] = job.jobOptions.jobType.ToString();
                array[3] = job.jobOptions.targetAddress.ToString();
                if (job.IsActive())
                    array[4] = "True";
                else
                    array[4] = "False";
                array[5] = job.jobOutput;
                array = jobTable.FormatStringArray(array);
                jobTable.WriteColumnes(array);
                Console.WriteLine();
            }

            Console.WriteLine();

            return 0;
        }
    }

    public class JobListCommand : Command
    {
        public JobListCommand()
        {
            optionalIndicators.Add(new object[] { "id", false });
        }

        public override int Execute()
        {
            if(OptionalArgumentUsed("id"))
            {
                try
                {
                    int id = Int32.Parse(GetArgument("id"));
                    Job job = MadComponents.components.jobSystem.GetJob(id);
                }
                catch (Exception e)
                {
                    ErrorMessage("Could not parse argument!");
                    return 0;
                }
            }
            else
            {
            foreach (Job job in MadComponents.components.jobSystem.jobs)
                job.JobStatus();
            }
            return 0;
        }
    }

    public class JobSystemAddPingCommand : Command
    {
        public JobSystemAddPingCommand()
        {
            requiredIndicators.Add(new object[] { "n", false });
            requiredIndicators.Add(new object[] { "ip", false });

            optionalIndicators.Add(new object[] { "d", false });
            optionalIndicators.Add(new object[] { "ttl", false });
        }

        public override int Execute()
        {
            try
            {
                IPAddress address = IPAddress.Parse(GetArgument("ip"));
                int delay = Int32.Parse(GetArgument("d"));
                int ttl = Int32.Parse(GetArgument("ttl"));

                MadComponents.components.jobSystem.AddJob(new JobPingOptions(GetArgument("n"),JobOptions.JobType.PingRequest, delay, address, ttl));
            }
            catch (Exception e)
            {
                ErrorMessage("Could not parse arguments.");
                return 0;
            }

            return 0;
        }
    }

    public class JobSystemAddHttpCommand : Command
    {
        public JobSystemAddHttpCommand()
        {
            requiredIndicators.Add(new object[] { "n", false });
            requiredIndicators.Add(new object[] { "ip", false });

            optionalIndicators.Add(new object[] { "d", false });
            optionalIndicators.Add(new object[] { "p", false });
        }

        public override int Execute()
        {
            try
            {
                IPAddress address = IPAddress.Parse(GetArgument("ip"));
                int delay = Int32.Parse(GetArgument("d"));
                int port = Int32.Parse(GetArgument("p"));

                MadComponents.components.jobSystem.AddJob(new JobHttpOptions(GetArgument("n"), JobOptions.JobType.HttpRequest, delay, address, port));
            }
            catch (Exception e)
            {
                ErrorMessage("Could not parse arguments.");
                return 0;
            }

            return 0;
        }
    }


    public class JobSystemAddPortCommand : Command
    {
        public JobSystemAddPortCommand()
        {
            requiredIndicators.Add(new object[] { "n", false });
            requiredIndicators.Add(new object[] { "ip", false });

            optionalIndicators.Add(new object[] { "d", false });
            optionalIndicators.Add(new object[] { "p", false });
        }

        public override int Execute()
        {
            try
            {
                IPAddress address = IPAddress.Parse(GetArgument("ip"));
                int delay = Int32.Parse(GetArgument("d"));
                int port = Int32.Parse(GetArgument("p"));

                MadComponents.components.jobSystem.AddJob(new JobPortOptions(GetArgument("n"), JobOptions.JobType.PortRequest, delay, address, port));
            }
            catch (Exception e)
            {
                ErrorMessage("Could not parse arguments.");
                return 0;
            }

            return 0;
        }
    }

    public class JobSystemRemoveCommand : Command // WORKING
    {
        public JobSystemRemoveCommand()
        {
            requiredIndicators.Add(new object[] { "id", false });
        }

        public override int Execute()
        {
            try
            {
                int id = Int32.Parse(GetArgument("id"));

                if (MadComponents.components.jobSystem.JobExist(id))
                {
                    MadComponents.components.jobSystem.RemoveJob(id);
                    return 0;
                }
                else
                {
                    ErrorMessage("Job does not exist!");
                    return 0;
                }
            }
            catch (Exception e)
            {
                ErrorMessage("Argument \"-id\" must be a number!");
                return 0;
            }
        }
    }

    public class JobSystemStartCommand : Command // WORKING
    {
        public JobSystemStartCommand()
        {
            requiredIndicators.Add(new object[] { "id", false });
        }

        public override int Execute()
        {
            try
            {
                int id = Int32.Parse(GetArgument("id"));

                if (MadComponents.components.jobSystem.JobExist(id))
                {
                    MadComponents.components.jobSystem.StartJob(id);
                    return 0;
                }
                else
                {
                    ErrorMessage("Job does not exist!");
                    return 0;
                }
            }
            catch (Exception e)
            {
                ErrorMessage("Argument \"-id\" must be a number!");
                return 0;
            }
        }
    }

    public class JobSystemStopCommand : Command // WORKING
    {
        public JobSystemStopCommand()
        {
            requiredIndicators.Add(new object[] { "id", false });
        }

        public override int Execute()
        {
            try
            {
                int id = Int32.Parse(GetArgument("id"));

                if (MadComponents.components.jobSystem.JobExist(id))
                {
                    MadComponents.components.jobSystem.StopJob(id);
                    return 0;
                }
                else
                {
                    ErrorMessage("Job does not exist!");
                    return 0;
                }
            }
            catch (Exception e)
            {
                ErrorMessage("Argument \"-id\" must be a number!");
                return 0;
            }
        }
    }
}
