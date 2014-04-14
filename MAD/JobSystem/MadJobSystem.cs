using System;
using System.Collections;
using System.Collections.Generic;

using System.Net;
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

        /// <summary>
        /// Check if a job has been created with a given ID.
        /// </summary>
        public bool JobExist(int jobID)
        {
            foreach (Job temp in jobs)
                if (temp.jobID == jobID)
                    return true;
            return false;
        }

        /// <summary>
        /// Get job with a specific ID (if job do not exist: return null)
        /// </summary>
        public Job GetJob(int jobID)
        {
            foreach (Job job in jobs)
                if (jobID == job.jobID)
                    return job;

            return null;
        }

        /// <summary>
        /// Create a job.
        /// </summary>
        public void CreateJob(JobOptions jobOptions)
        {
            int jobType = (int)jobOptions.jobType;

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

        /// <summary>
        /// Destroy a job.
        /// </summary>
        public void DestroyJob(int jobID)
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

        /// <summary>
        /// Destroy all jobs.
        /// </summary>
        public void DestroyAllJobs()
        {
            for(int i = 0; i < jobs.Count; i++)
            {
                if (!jobs[i].IsActive())
                    jobs.RemoveAt(i);
            }
        }

        /// <summary>
        /// Start a specific job.
        /// </summary>
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

        /// <summary>
        /// Stop a specific job.
        /// </summary>
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

        /// <summary>
        /// Get number of active jobs
        /// </summary>
        public int JobsActive()
        {
            int count = 0;
            for (int i = 0; i < jobs.Count; i++)
                if (jobs[0].IsActive() == true)
                    count++;
            return count;
        }

        /// <summary>
        /// Get number of inactive jobs.
        /// </summary>
        public int JobsInactive()
        {
            int count = 0;
            for (int i = 0; i < jobs.Count; i++)
                if (jobs[0].IsActive() != true)
                    count++;
            return count;
        }
    }
}
