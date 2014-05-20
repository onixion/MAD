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
            Job job = GetJob(jobID);

            if (job != null)
            {
                job.Stop();
            }
            else
            {
                throw new Exception("Job does not exist!");
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

        public void StartJob(int jobID)
        {
            Job job = GetJob(jobID);

            if (job != null)
            {
                if (!job.Start())
                {
                    throw new Exception("Job is already active!");
                }
            }
            else
            {
                throw new Exception("Job does not exist!");
            }
        }

        public void StopJob(int jobID)
        {
            Job job = GetJob(jobID);

            if (job != null)
            {
                if (!job.Stop())
                {
                    throw new Exception("Job is already inactive!");
                }
            }
            else
            {
                throw new Exception("Job does not exist!");
            }
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
            return jobs.Count - JobsActive(); 
        }

        #endregion
    }
}
