using System;
using System.Collections.Generic;

namespace MAD.jobSys
{
    public class JobSystem
    {
        #region member

        private Version _version = new Version(1, 6);
        public string version { get { return _version.ToString(); } }

        private string _dataPath;
        public string dataPath { get { return _dataPath; } }

        public List<Job> jobs = new List<Job>();

        private int _maxJobs = 100;
        public int maxJobs { get { return _maxJobs; } }

        #endregion

        #region constructor

        public JobSystem(string dataPath)
        {
            _dataPath = dataPath;
        }

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
            if (_maxJobs >= jobs.Count)
            {
                jobs.Add(job);
            }
            else
            {
                throw new Exception("Job could not be added, becauce the jobsystem has reached the max numbers of jobs.");
            }
        }

        public void DestroyJob(int jobID)
        {
            Job _job = GetJob(jobID);

            if (_job == null)
            {
                throw new Exception("Job does not exist!");
            }

            for (int i = 0; i < jobs.Count; i++)
            {
                if (jobs[i].jobID == jobID)
                {
                    jobs[i].Stop();
                    jobs.RemoveAt(i);

                    break;
                }
            }
        }

        public void DestroyAllJobs()
        {
            for (int i = 0; i < jobs.Count; i++)
            {
                jobs[i].Stop();
                jobs.RemoveAt(i);
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

            for (int i = 0; i < jobs.Count; i++)
            {
                if (jobs[i].jobState == Job.State.Running)
                {
                    _count++;
                }
            }

            return _count;
        }

        public int JobsStopped()
        {
            int _count = 0;

            for (int i = 0; i < jobs.Count; i++)
            {
                if (jobs[i].jobState == Job.State.Stopped)
                {
                    _count++;
                }
            }

            return _count;
        }

        public void ClearJobTable()
        {
            jobs.Clear();
        }

        public void SaveJobTable(string path)
        {
            foreach (Job _temp in jobs)
            { 
                // TODO
            }
        }

        public void LoadJobTable(string path)
        { 
            // TODO
        }

        #endregion
    }
}
