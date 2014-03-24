using System;
using System.Collections;
using System.Collections.Generic;
namespace JobSystemTest
{
    class JobSystem
    {
        public List<Job> jobs;

        public JobSystem()
        { 
            jobs = new List<Job>();
        }

        public void AddJob(JobOptions options, params object[] args)
        {
            switch (options.jobType.ToString())
            { 
                case "PingRequest":
                    if (args.Length == 1)
                        jobs.Add(new JobPing(options, (int)args[0]));
                    else
                        jobs.Add(new JobPing(options, 200));
                    break;

                case "HttpRequest":
                    if (args.Length == 1)
                        jobs.Add(new JobHttp(options, (int)args[0]));
                    else
                        jobs.Add(new JobHttp(options, 80));
                    break;
            
            
            }
        }

        public void RemoveJob(int JobID)
        {
            foreach (Job temp in jobs)
            {
                if (temp.jobID == JobID)
                {
                    if(!temp.IsRunning())
                        jobs.RemoveAt((int)JobID);

                    break;
                }
            }
        }

        public void ClearJobs()
        {
            for(int i = 0; i < jobs.Count; i++)
            {
                if (!jobs[i].IsRunning())
                    jobs.RemoveAt(i);
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
