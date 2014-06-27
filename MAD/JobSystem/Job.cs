using System;
using System.Collections.Generic;
using System.Threading;

namespace MAD.jobSys
{
    public abstract class Job
    {
        #region members

        private static int _jobsCount = 0;
        public int jobID;
        private static object _jobInitLock = new object();

        public enum JobState { Running, Stopped, Exception }
        public JobState jobState = JobState.Stopped;

        public enum OutState { NULL, Success, Failed, Exception }
        public OutState outState = OutState.NULL;
        
        public JobOptions jobOptions;

        public DateTime lastStarted;
        public DateTime lastFinished;
        //public int deltaTime

        public List<OutDescriptor> outDescriptors = new List<OutDescriptor>();

        #endregion

        #region constructor

        protected Job(JobOptions jobOptions)
        {
            this.jobOptions = jobOptions;

            lock (_jobInitLock)
            {
                jobID = _jobsCount;
                _jobsCount++;
            }
        }

        #endregion

        #region methodes

        public bool Start()
        {
            if (jobState == JobState.Stopped)
            {
                jobState = JobState.Running;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Stop()
        {
            if (jobState == JobState.Running)
            {
                jobState = JobState.Stopped;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void LaunchJob()
        {
            lastStarted = DateTime.Now;
            Execute();
            lastFinished = DateTime.Now;
        }

        public abstract void Execute();

        #region for CLI only

        public string Status()
        {
            string _temp = "\n";

            _temp += "<color><yellow>ID: <color><white>" + jobID + "\n";
            _temp += "<color><yellow>NAME: <color><white>" + jobOptions.jobName + "\n";
            _temp += "<color><yellow>TYPE: <color><white>" + jobOptions.jobType.ToString() + "\n";
            _temp += "<color><yellow>STATE: <color><white>" + jobState.ToString() + "\n";
            _temp += "<color><yellow>TIME-TYPE: <color><white>" + jobOptions.jobTime.type.ToString() + "\n";

            if (jobOptions.jobTime.type == JobTime.TimeType.Relativ)
            {
                _temp += "<color><yellow>DELAY-TIME: <color><white>" + jobOptions.jobTime.jobDelay.delayTime + "\n";
                _temp += "<color><yellow>DELAY-REMAIN-TIME: <color><white>" + jobOptions.jobTime.jobDelay.delayTimeRemaining + "\n";
            }
            else if (jobOptions.jobTime.type == JobTime.TimeType.Absolute)
            {
                _temp += "<color><yellow>TIMES: <color><white>";

                foreach (JobTimeHandler _buffer in jobOptions.jobTime.jobTimes)
                {
                    _temp += _buffer.JobTimeStatus() + " ";
                }

                _temp += "\n";
            }

            _temp += "<color><yellow>OUTPUT-TIME: <color><white>" + lastFinished.ToString("dd.MM.yyyy HH:mm:ss") + "\n";
            _temp += "<color><yellow>OUTPUT-STATE: <color><white>" + outState.ToString() +"\n";

            return _temp + JobStatus();
        }

        protected abstract string JobStatus();

        #endregion

        #endregion
    }
}
