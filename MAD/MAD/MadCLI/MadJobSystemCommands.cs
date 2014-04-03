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

        string line = "".PadRight(Console.BufferWidth, '_');


        public JobSystemStatusCommand() { }

        public override int Execute()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(jobSystemHeader);
            Console.ForegroundColor = MadComponents.components.cli.textColor;
            Console.WriteLine();
            Console.WriteLine("Jobs initialized: " + MadComponents.components.jobSystem.jobs.Count);
            Console.WriteLine("Jobs active:      " + MadComponents.components.jobSystem.JobsCountActive());
            Console.WriteLine("Jobs inactive:    " + MadComponents.components.jobSystem.JobsCountInactive());
            Console.WriteLine();
            Console.WriteLine(line);

            string[] title = new string[] { "ID", "Name", "Type", "IpAddress", "Active", "Output" };
            Table jobTable = new Table(title);
            title = jobTable.FormatStringArray(title);

            Console.ForegroundColor = ConsoleColor.Yellow;
            jobTable.WriteColumnes(title);
            Console.ForegroundColor = MadComponents.components.cli.textColor;

            foreach (Job job in MadComponents.components.jobSystem.jobs)
            {
                string[] array = new string[title.Length];
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
                Console.WriteLine();
                jobTable.WriteColumnes(array);
            }

            Console.WriteLine();
            Console.WriteLine(line);

            return 0;
        }
    }

    public class JobSystemListCommand : Command
    {
        public JobSystemListCommand()
        {
            optionalIndicators.Add(new object[] { "id", false });
        }

        public override int Execute()
        {
            if (OptionalArgumentUsed("id"))
            {
                Job job = MadComponents.components.jobSystem.GetJob(Int32.Parse(GetArgument("id")));

                if (job != null)
                    job.JobStatus();
                else
                    return 30;
            }
            else
            {
                foreach (Job job in MadComponents.components.jobSystem.jobs)
                    job.JobStatus();
            }

            return 0;
        }
    } // WORKING

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
            }
            catch (Exception e)
            {
                ErrorMessage("Could not parse argument \"-ip\" to an ip-address!");
                return 0;
            }

            // HERE!!!

            return 0;
        }
    }

    public class JobSystemRemoveCommand : Command // WORKING
    {
        public JobSystemRemoveCommand(MadJobSystem system)
        {
            requiredIndicators.Add(new object[] { "id", false });
        }

        public override int Execute()
        {
            try
            {
                int id = Int32.Parse(GetArgument("id"));

                if (id != null)
                {
                    if (MadComponents.components.jobSystem.JobExist(id))
                    {
                        MadComponents.components.jobSystem.RemoveJob(id);
                        return 0;
                    }
                    else
                        return 30;
                }
                else
                {
                    ErrorMessage("Argument \"-id\" is null!");
                    return 0;
                }
            }
            catch (Exception e)
            {
                return 3;
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
                    return 30;
            }
            catch (Exception e)
            {
                return 3;
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
                    return 30;
            }
            catch (Exception e)
            {
                return 3;
            }
        }
    }
}
