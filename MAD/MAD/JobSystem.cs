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

        public void AddJob(JobOptions jobOptions)
        {
            int jobType = (int) jobOptions.jobType;

            switch (jobType)
            { 
                case 0: // PING REQUEST
                    jobs.Add(new JobPing((JobPingOptions)jobOptions));
                    break;
                case 1: // PORT SCAN
                    jobs.Add(new JobPort((JobPortOptions)jobOptions));
                    break;
                case 2: // HTTP REQUEST
                    jobs.Add(new JobHttp((JobHttpOptions)jobOptions));
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
