using System;
using System.Collections;
using System.Collections.Generic;

namespace MAD
{
    public class JobSystem
    {
        #region members

        public Version version = new Version(1, 0);
        public List<Job> jobs;

        #endregion

        public JobSystem()
        { 
            jobs = new List<Job>();
        }

        #region methodes

        public bool JobExist(int jobID)
        {
            foreach (Job temp in jobs)
            {
                if (temp.jobID == jobID)
                {
                    return true;
                }
            }

            return false;
        }

        public Job GetJob(int jobID)
        {
            foreach (Job job in jobs)
            {
                if (jobID == job.jobID)
                {
                    return job;
                }
            }

            return null;
        }

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

        public void DestroyJob(int jobID)
        {
            for(int i = 0; i < jobs.Count; i++)
            {
                if (jobs[i].jobID == jobID)
                {
                    if (jobs[i].threadRunning)
                    {
                        jobs[i].Stop();
                        jobs.RemoveAt(i);
                    }
                    else
                    {
                        jobs.RemoveAt(i);
                    }

                    break;
                }
            }
        }

        public void DestroyAllJobs()
        {
            for (int i = 0; i < jobs.Count; i++)
            {
                if (!jobs[i].threadRunning)
                {
                    jobs.RemoveAt(i);
                }
                else
                {
                    jobs[0].Stop();
                    jobs.RemoveAt(i);
                }
            }
        }

        public void StartJob(int jobID)
        {
            foreach (Job temp in jobs)
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

        public int JobsActive()
        {
            int count = 0;

            for (int i = 0; i < jobs.Count; i++)
            {
                if (jobs[0].threadRunning)
                {
                    count++;
                }
            }

            return count;
        }

        public int JobsInactive()
        {
            int count = 0;

            for (int i = 0; i < jobs.Count; i++)
            {
                if (!jobs[0].threadRunning)
                {
                    count++;
                }
            }

            return count;
        }

        #endregion
    }
}
