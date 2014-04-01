using System;
using System.Threading;
using System.Net;
using System.Collections.Generic;

namespace MAD
{
    public class JobSystemStatusCommand : Command
    {
        public JobSystemStatusCommand() { }

        public override int Execute()
        {
            Console.WriteLine();
            Console.WriteLine("Jobs initialized: " + MadComponents.components.jobSystem.jobs.Count);
            Console.WriteLine("Jobs active:      " + MadComponents.components.jobSystem.JobsCountActive());
            Console.WriteLine("Jobs inactive:    " + MadComponents.components.jobSystem.JobsCountInactive());
            Console.WriteLine();

            return 0;
        }
    }

    public class JobSystemListCommand : Command
    {
        public JobSystemListCommand()
        {
            optionalIndicators.Add(new object[]{"id", false});
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
            requiredIndicators.Add(new object[]{"n", false});
            requiredIndicators.Add(new object[]{ "ip", false});

            optionalIndicators.Add(new object[]{ "d", false});
            optionalIndicators.Add(new object[]{ "ttl", false});
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
            requiredIndicators.Add(new object[] { "id", false});
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
                    return 30;
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
            requiredIndicators.Add(new object[]{"id", false});
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
            requiredIndicators.Add(new object[] {"id",false});
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

    public class JobSystemTableCommand : Command
    {
        public JobSystemTableCommand()
        { }

        public override int Execute()
        {
            while (true)
            {
                WriteTable();
                Thread.Sleep(10);
            }

            return 0;
        }

        private void WriteTable()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("ID\t\tName\t\tType\t\tOutput");
            Console.ForegroundColor = ConsoleColor.Black;

            foreach (Job temp in MadComponents.components.jobSystem.jobs)
                Console.WriteLine(temp.jobID + "\t\t" + temp.jobOptions.jobName + "\t\t" + temp.jobOptions.jobType.ToString() + "\t\t" + temp.jobOutput);
        }
    }
}
