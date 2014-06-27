using System;
using System.Collections.Generic;

namespace MAD.jobSys
{
    public class JobSystem
    {
        #region member

        private string _dataPath;

        public List<Job> jobs = new List<Job>();
        public Object jobsLock = new Object();

        private int _maxJobs = 100;
        public int maxJobs { get { return _maxJobs; } }

        private JobScedule _scedule;
        public JobScedule.State sceduleState { get { return _scedule.state; } }

        #endregion

        #region constructor

        public JobSystem(string dataPath)
        {
            _dataPath = dataPath;
            _scedule = new JobScedule(jobs, jobsLock);
        }

        #endregion

        #region methodes

        public void StartScedule()
        {
            _scedule.Start();
        }

        public void StopScedule()
        {
            _scedule.Stop();
        }

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
            if (_maxJobs > jobs.Count)
            {
                jobs.Add(job);
            }
            else
            {
                throw new Exception("Job could not be added, because the jobsystem has reached its max numbers of jobs.");
            }
        }

        public bool DestroyJob(int jobID)
        {
            lock (jobsLock)
            {
                for(int i = 0; i < jobs.Count; i++)
                {
                    if (jobs[i].jobID == jobID)
                    {
                        jobs.RemoveAt(i);
                        return true;
                    }
                }

                return false;
            }
        }

        public void DestroyAllJobs()
        {
            int _jobsCount = jobs.Count;

            for (int i = 0; i < jobs.Count; i++)
            {
                DestroyJob(jobs[i].jobID);
            }
        }

        public void StartJob(int jobID)
        {
            Job _job = GetJob(jobID);

            if (_job != null)
            {
                if (!_job.Start())
                {
                    throw new Exception("Job is already running!");
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
                    throw new Exception("Job is already stopped!");
                }
            }
            else
            {
                throw new Exception("Job does not exist!");
            }
        }

        public int JobsRunning()
        {
            int _count = 0;

            lock (jobsLock)
            {
                for (int i = 0; i < jobs.Count; i++)
                {
                    if (jobs[i].jobState == Job.JobState.Running)
                    {
                        _count++;
                    }
                }
            }

            return _count;
        }

        public int JobsStopped()
        {
            int _count = 0;

            lock (jobsLock)
            {
                for (int i = 0; i < jobs.Count; i++)
                {
                    if (jobs[i].jobState == Job.JobState.Stopped)
                    {
                        _count++;
                    }
                }
            }

            return _count;
        }

        public void ClearJobTable()
        {
            lock (jobsLock)
            {
                jobs.Clear();
            }
        }

        public void SaveJobTable(string path)
        {
            // TODO
        }

        public void LoadJobTable(string path)
        { 
            // TODO
        }

        #endregion
    }
}
