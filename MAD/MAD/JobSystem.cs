using System;
using System.Collections;
using System.Collections.Generic;

namespace MAD
{
    public class JobSystem
    {
        public List<Job> jobs;

        public JobSystem()
        { 
            jobs = new List<Job>();
        }

        public void AddJob(JobOptions jobOptions, object jobTypeOptions)
        {
            switch (jobOptions.jobType)
            { 
                case "HttpRequest":
                    jobs.Add(new JobHttp(jobOptions, (JobHttpOptions) jobTypeOptions));
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
