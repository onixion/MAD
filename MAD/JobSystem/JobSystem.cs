using System;
using System.Collections;
using System.Collections.Generic;

namespace MAD
{
    public class JobSystem
    {
        #region members

        public Version version = new Version(1, 0);
        public List<Job> jobs = new List<Job>();

        public readonly string dataPath;

        #endregion

        public JobSystem(string dataPath)
        {
            this.dataPath = dataPath;
            // check dir exist and create ...
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
            switch ((int)jobOptions.jobType)
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
                    if (jobs[i].threadStopRequest)
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
                if (!jobs[i].threadStopRequest)
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

        public bool StartJob(Job job)
        {
            return job.Start();
        }

        public bool StopJob(Job job)
        {
            return job.Stop();
        }

        public int JobsActive()
        {
            int count = 0;

            for (int i = 0; i < jobs.Count; i++)
            {
                if (jobs[0].threadStopRequest)
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
                if (!jobs[0].threadStopRequest)
                {
                    count++;
                }
            }

            return count;
        }

        #endregion
    }
}
