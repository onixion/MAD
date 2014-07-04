using System;
using System.Collections.Generic;

namespace MAD.JobSystemCore
{
    public abstract class Job
    {
        #region members

        private static int _jobsCount = 0;
        private static object _jobInitLock = new object();
        public int jobID;

        public string jobName;

        public enum JobType { NULL, Ping, PortScan, Http, HostDetect, ServiceCheck }
        public JobType jobType;

        public enum JobState { Waiting, Running, Stopped, Exception }
        public JobState jobState = JobState.Stopped;

        public enum OutState { NULL, Success, Failed, Exception }
        public OutState outState = OutState.NULL;

        public bool jobLocked = false;

        public DateTime lastStarted;
        public DateTime lastFinished;
        public TimeSpan lastTimeSpan;

        public JobTime jobTime = new JobTime();
        public List<OutDescriptor> outDescriptors = new List<OutDescriptor>();

        public JobNotification jobNotification = new JobNotification();

        #endregion

        #region constructor

        protected Job(string jobName, JobType jobType, JobTime jobTime)
        {
            this.jobName = jobName;
            this.jobType = jobType;
            this.jobTime = jobTime;

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
                jobState = JobState.Waiting;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Stop()
        {
            if (jobState != JobState.Stopped)
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

            lastTimeSpan = lastStarted.Subtract(lastFinished);
        }

        public abstract void Execute();

        #region for CLI only

        public string Status()
        {
            string _temp = "\n";

            _temp += "<color><yellow>ID: <color><white>" + jobID + "\n";
            _temp += "<color><yellow>NAME: <color><white>" + jobName + "\n";
            _temp += "<color><yellow>TYPE: <color><white>" + jobType.ToString() + "\n";
            _temp += "<color><yellow>STATE: <color><white>" + jobState.ToString() + "\n";
            _temp += "<color><yellow>TIME-TYPE: <color><white>" + jobTime.type.ToString() + "\n";

            if (jobTime.type == JobTime.TimeType.Relative)
            {
                _temp += "<color><yellow>DELAY-TIME: <color><white>" + jobTime.jobDelay.delayTime + "\n";
                _temp += "<color><yellow>DELAY-REMAIN-TIME: <color><white>" + jobTime.jobDelay.delayTimeRemaining + "\n";
            }
            else if (jobTime.type == JobTime.TimeType.Absolute)
            {
                _temp += "<color><yellow>TIMES: <color><white>";

                foreach (JobTimeHandler _buffer in jobTime.jobTimes)
                {
                    _temp += _buffer.JobTimeStatus() + " ";
                }

                _temp += "\n";
            }

            _temp += "<color><yellow>LAST-STARTTIME: <color><white>" + lastStarted.ToString("dd.MM.yyyy HH:mm:ss") + "\n";
            _temp += "<color><yellow>LAST-STOPTIME: <color><white>" + lastFinished.ToString("dd.MM.yyyy HH:mm:ss") + "\n";
            _temp += "<color><yellow>LAST-TIMESPAN: <color><white>" + lastTimeSpan.Seconds + "s " + lastTimeSpan.Milliseconds + "ms (" + lastTimeSpan.Ticks + " ticks)\n";
            _temp += "<color><yellow>OUTPUT-STATE: <color><white>" + outState.ToString() +"\n";

            return _temp + JobStatus();
        }

        protected abstract string JobStatus();

        #endregion

        #endregion
    }
}
