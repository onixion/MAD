using System;
using System.Collections;
using System.Collections.Generic;

namespace MAD
{
    public class MadJobSystem
    {
        public string version = "0.0.1.8";
        public List<Job> jobs;

        public MadJobSystem()
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

        public bool JobExist(int jobID)
        {
            foreach (Job temp in jobs)
                if (temp.jobID == jobID)
                    return true;
            return false;
        }

        public Job GetJob(int jobID)
        {
            foreach (Job job in jobs)
                if (jobID == job.jobID)
                    return job;
            return null;
        }

        public void RemoveJob(int jobID)
        {
            for(int i = 0; i < jobs.Count; i++)
            {
                if (jobs[i].jobID == jobID)
                {
                    if (jobs[i].IsActive())
                    {
                        jobs[i].Stop();
                        jobs.RemoveAt(i);
                    }
                    else
                        jobs.RemoveAt(i);

                    break;
                }
            }
        }

        public void ClearJobs()
        {
            for(int i = 0; i < jobs.Count; i++)
            {
                if (!jobs[i].IsActive())
                    jobs.RemoveAt(i);
            }
        }

        public void StartJob(int jobID)
        {
            foreach(Job temp in jobs)
            {
                if (temp.jobID == jobID)
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

        public int JobsCountActive()
        {
            int count = 0;
            for (int i = 0; i < jobs.Count; i++)
                if (jobs[0].IsActive() == true)
                    count++;
            return count;
        }

        public int JobsCountInactive()
        {
            int count = 0;
            for (int i = 0; i < jobs.Count; i++)
                if (jobs[0].IsActive() != true)
                    count++;
            return count;
        }
    }
}
