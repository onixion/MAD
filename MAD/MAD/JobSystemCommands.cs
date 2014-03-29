using System;
using System.Net;
using System.Collections.Generic;

namespace MAD
{
    public class JobSystemListCommand : Command
    {
        private JobSystem system;

        public JobSystemListCommand(JobSystem system)
        {
            this.system = system;
            optionalIndicators.Add(new object[]{"id", typeof(Int32)});
        }

        public override int Execute()
        {
            if (OptionalArgumentUsed("id"))
            {
                if (!ArgumentEmpty("id"))
                {
                    Job job = system.GetJob(Int32.Parse(GetArgument("id")));

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
                foreach(Job job in system.jobs)
                    job.JobStatus();

                if (system.jobs.Count == 0)
                    Console.WriteLine("(no jobs)");
            }

            return 0;

        }
    }

    public class JobSystemAddCommand : Command
    {
        private JobSystem system;

        public JobSystemAddCommand(JobSystem system)
        {
            this.system = system;

            requiredIndicators.Add(new object[]{"t",typeof(string)});
            requiredIndicators.Add(new object[] { "n", typeof(string) });
            requiredIndicators.Add(new object[] { "ip", typeof(string) });
            requiredIndicators.Add(new object[] { "d", typeof(Int32) });

            optionalIndicators.Add(new object[] { "p", typeof(Int32) });
            optionalIndicators.Add(new object[] { "ttl", typeof(Int32) });
        }

        public override int Execute()
        {
            switch(GetArgument("t"))
            {
                case "http":
                    if (ArgumentExists("p"))
                        system.jobs.Add(new JobHttp(new JobHttpOptions(GetArgument("n"),JobOptions.JobType.HttpRequest, Int32.Parse(GetArgument("d")), IPAddress.Parse(GetArgument("ip")), Int32.Parse(GetArgument("p")))));
                    else
                        return 1;
                    break;
                case "ping":
                    if (ArgumentExists("ttl"))
                        system.jobs.Add(new JobPing(new JobPingOptions(GetArgument("n"),JobOptions.JobType.PingRequest, Int32.Parse(GetArgument("d")), IPAddress.Parse(GetArgument("ip")), Int32.Parse(GetArgument("ttl")))));
                    else
                        return 1;
                    break;
                case "port":
                    if (ArgumentExists("p"))
                        system.jobs.Add(new JobPort(new JobPortOptions(GetArgument("n"),JobOptions.JobType.PortRequest, Int32.Parse(GetArgument("d")), IPAddress.Parse(GetArgument("ip")), Int32.Parse(GetArgument("p")))));
                    else
                        return 1;
                    break;
                default:
                    return 1;
            }
            return 0;
        }
    }

    public class JobSystemRemoveCommand : Command
    { 
        private JobSystem system;

        public JobSystemRemoveCommand(JobSystem system)
        {
            this.system = system;

            requiredIndicators.Add(new object[] { "id", typeof(Int32) });
        }

        public override int Execute()
        {
            if (!ArgumentEmpty("id"))
            {
                system.RemoveJob(Int32.Parse(GetArgument("id")));
                return 0;
            }
            else
                return 1;
        }
    }

    public class JobSystemStartCommand : Command
    {
        private JobSystem system;

        public JobSystemStartCommand(JobSystem system)
        {
            this.system = system;

            requiredIndicators.Add(new object[]{"id", typeof(Int32)});
        }

        public override int Execute()
        {
            if (OptionalArgumentUsed("id"))
            {
                system.StartJob(Int32.Parse(GetArgument("id")));
                return 0;
            }
            else
                return 1;
        }
    }

    public class JobSystemStopCommand : Command
    {
        private JobSystem system;

        public JobSystemStopCommand(JobSystem system)
        {
            this.system = system;

            requiredIndicators.Add(new object[] {"id",typeof(Int32)});
        }

        public override int Execute()
        {
            if (!ArgumentEmpty("id"))
            {
                system.StopJob(Int32.Parse(GetArgument("id")));
                return 0;
            }
            else
                return 1;
        }
    }
}
