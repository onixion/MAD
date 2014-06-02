using System;
using System.Collections;
using System.Collections.Generic;

namespace MAD.JobSystem
{
    public class JobSystem
    {
        #region member

        public Version version = new Version(1, 2, 6000);
        public List<Job> jobs = new List<Job>();

        #endregion

        #region methodes

        public bool JobExist(int jobID)
        {
            foreach (Job _temp in jobs)
            {
                if (_temp.jobID == jobID)
                {
                    return true;
                }
            }

            return false;
        }

        public Job GetJob(int jobID)
        {
            foreach (Job _job in jobs)
            {
                if (jobID == _job.jobID)
                {
                    return _job;
                }
            }

            return null;
        }

        public void CreateJob(Job job)
        {
            jobs.Add(job);
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
                if (!jobs[i].Active())
                {
                    jobs.RemoveAt(i);
                }
                else
                {
                    jobs[i].Stop();
                    jobs.RemoveAt(i);
                }
            }
        }

        public void StartJob(int jobID)
        {
            Job _job = GetJob(jobID);

            if (_job != null)
            {
                if (!_job.Start())
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
            Job _job = GetJob(jobID);

            if (_job != null)
            {
                if (!_job.Stop())
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
            int _count = 0;

            for (int i = 0; i < jobs.Count; i++)
            {
                if (jobs[i].Active())
                {
                    _count++;
                }
            }

            return _count;
        }

        public int JobsInactive()
        {
            return jobs.Count - JobsActive(); 
        }

        public void SaveTable(string path)
        { 
            // TODO
        }

        public void LoadTable(string path)
        { 
            // TODO
        }

        #endregion
    }
}
