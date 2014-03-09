using System;
using System.Collections;
using System.Collections.Generic;
namespace JobSystemTest
{
    class JobSystem
    {
        public List<Job> jobs; // list with all jobs

        public JobSystem()
        { 
            jobs = new List<Job>();
        }

        public void AddJob(JobOptions options)
        {
            switch (options.type.ToString())
            { 
                case "PingRequest":
                    jobs.Add(new JobPing(options));
                    break;

                case "HttpRequest":
                    jobs.Add(new JobHttp(options));
                    break;

                case "PortScan":
                    jobs.Add(new JobPort(options));
                    break;
            }
        }

        public void DeleteJob(int JobID)
        {
            foreach (Job temp in jobs)
            {
                if (temp.jobID == JobID)
                {
                    jobs.RemoveAt((int)JobID);
                    break;
                }
            }
        }

        public void StartJob(int JobID)
        {
            foreach(Job temp in jobs)
            {
                if (temp.jobID == JobID)
                {
                    temp.Start();
                    break;
                }
            }
        }

        public void StopJob(int JobID)
        {
            foreach (Job temp in jobs)
            {
                if (temp.jobID == JobID)
                {
                    temp.Stop();
                    break;
                }
            }
        }
    }
}
