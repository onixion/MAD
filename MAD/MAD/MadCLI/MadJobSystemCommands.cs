using System;
using System.Net;
using System.Collections.Generic;

namespace MAD
{
    public class JobSystemListCommand : Command
    {
        public JobSystemListCommand()
        {
            optionalIndicators.Add(new object[]{"id", typeof(Int32)});
        }

        public override int Execute()
        {
            if (OptionalArgumentUsed("id"))
            {
                if (!ArgumentEmpty("id"))
                {
                    Job job = MadComponents.components.jobSystem.GetJob(Int32.Parse(GetArgument("id")));

                    if (job != null)
                        job.JobStatus();
                    else
                        return 30;
                }
                else
                    return 3;
            }
            else
            {
                foreach (Job job in MadComponents.components.jobSystem.jobs)
                    job.JobStatus();

                if (MadComponents.components.jobSystem.jobs.Count == 0)
                    Console.WriteLine("(no jobs)");
            }

            return 0;
        }
    }

    public class JobSystemAddCommand : Command
    {
        public JobSystemAddCommand()
        {
            requiredIndicators.Add(new object[]{"t",typeof(string)});
            requiredIndicators.Add(new object[] { "n", typeof(string) });
            requiredIndicators.Add(new object[] { "ip", typeof(string) });
            requiredIndicators.Add(new object[] { "d", typeof(Int32) });

            optionalIndicators.Add(new object[] { "p", typeof(Int32) });
            optionalIndicators.Add(new object[] { "ttl", typeof(Int32) });
        }

        public override int Execute()
        {
            if (!ArgumentEmpty("t") && !ArgumentEmpty("n") && !ArgumentEmpty("ip") && !ArgumentEmpty("d"))
            {
                switch (GetArgument("t"))
                {
                    case "http":
                        if (ArgumentExists("p"))
                            MadComponents.components.jobSystem.jobs.Add(new JobHttp(new JobHttpOptions(GetArgument("n"), JobOptions.JobType.HttpRequest, Int32.Parse(GetArgument("d")), IPAddress.Parse(GetArgument("ip")), Int32.Parse(GetArgument("p")))));
                        else
                            return 1;
                        break;
                    case "ping":
                        if (ArgumentExists("ttl"))
                            MadComponents.components.jobSystem.jobs.Add(new JobPing(new JobPingOptions(GetArgument("n"), JobOptions.JobType.PingRequest, Int32.Parse(GetArgument("d")), IPAddress.Parse(GetArgument("ip")), Int32.Parse(GetArgument("ttl")))));
                        else
                            return 1;
                        break;
                    case "port":
                        if (ArgumentExists("p"))
                            MadComponents.components.jobSystem.jobs.Add(new JobPort(new JobPortOptions(GetArgument("n"), JobOptions.JobType.PortRequest, Int32.Parse(GetArgument("d")), IPAddress.Parse(GetArgument("ip")), Int32.Parse(GetArgument("p")))));
                        else
                            return 1;
                        break;
                    default:
                        return 1;
                }
                return 0;
            }
            else
                return 1;
        }
    }

    public class JobSystemRemoveCommand : Command
    {
        public JobSystemRemoveCommand(MadJobSystem system)
        {
            requiredIndicators.Add(new object[] { "id", typeof(Int32) });
        }

        public override int Execute()
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
    }

    public class JobSystemStartCommand : Command
    {
        public JobSystemStartCommand()
        {
            requiredIndicators.Add(new object[]{"id", typeof(Int32)});
        }

        public override int Execute()
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
    }

    public class JobSystemStopCommand : Command
    {
        public JobSystemStopCommand()
        {
            requiredIndicators.Add(new object[] {"id",typeof(Int32)});
        }

        public override int Execute()
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
    }
}
