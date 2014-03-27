using System;
using System.Net;
using System.Collections.Generic;

namespace MAD
{
    class JobSystemListCommand : Command
    {
        private JobSystem system;

        public JobSystemListCommand(JobSystem system)
        {
            this.system = system;
            optionalIndicators.Add("id");
        }

        public override int Execute()
        {
            if (OptionalArgumentExists("id"))
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

            requiredIndicators.Add("t");
            requiredIndicators.Add("n");
            requiredIndicators.Add("ip");
            requiredIndicators.Add("d");

            optionalIndicators.Add("p");

        }

        public override int Execute()
        {
            switch(GetArgument("t"))
            {
                case "http":

                    if (ArgumentSupported("p"))
                        system.jobs.Add(new JobHttp(new JobHttpOptions(GetArgument("n"),JobOptions.JobType.HttpRequest, Int32.Parse(GetArgument("d")), IPAddress.Parse(GetArgument("ip")), Int32.Parse(GetArgument("p")))));
                    else
                        return 1;

                    break;

                case "ping":
                    break;

                case "port":
                    break;
                default:
                    return 1;
            }

            return 0;
        }
    }
}
