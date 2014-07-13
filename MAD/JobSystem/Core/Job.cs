using System;
using System.Net;
using System.Collections.Generic;

using System.Runtime.Serialization;

namespace MAD.JobSystemCore
{
    [Serializable]
    public abstract class Job : ISerializable
    {
        #region members

        private static int _jobsCount = 0;
        private static object _jobInitLock = new object();
        private int _id;
        public int id { get { return _id; } }

        public enum JobType { NULL, Ping, PortScan, Http, HostDetect, ServiceCheck }
        public JobType jobType = JobType.NULL;

        public enum JobState { NULL, Inactive, Waiting, Working, Exception }
        public JobState state = JobState.Inactive;

        public enum OutState { NULL, Success, Failed, Exception }
        public OutState outState = OutState.NULL;

        public DateTime lastStarted;
        public DateTime lastFinished;
        public TimeSpan lastTimeSpan;

        public string jobName;

        public JobTime jobTime = new JobTime();
        public List<OutDescriptor> outDesc = new List<OutDescriptor>(); // NOT SERIALIZED YET!
        public JobNotification jobNoti = new JobNotification(); // NOT SERIALIZED YET!

        #endregion

        #region constructors

        protected Job(string jobName, JobType jobType, JobTime jobTime)
        {
            InitJob();
            this.jobName = jobName;
            this.jobType = jobType;
            this.jobTime = jobTime;
        }

        // for serialization
        protected Job(SerializationInfo info, StreamingContext context)
        {
            InitJob();
            this.jobName = (string) info.GetValue("SER_JOB_NAME", typeof(string));
            this.jobType = (JobType)info.GetValue("SER_JOB_TYPE", typeof(JobType));
            this.jobTime = (JobTime)info.GetValue("SER_JOB_TIME", typeof(JobTime));
        }

        #endregion

        #region methodes

        private void InitJob()
        {
            lock (_jobInitLock)
            {
                _id = _jobsCount;
                _jobsCount++;
            }
        }

        public abstract void Execute(IPAddress targetAddress);

        public string Status()
        {
            string _temp = "\n";

            _temp += "<color><yellow>ID: <color><white>" + _id + "\n";
            _temp += "<color><yellow>NAME: <color><white>" + jobName + "\n";
            _temp += "<color><yellow>TYPE: <color><white>" + jobType.ToString() + "\n";
            _temp += "<color><yellow>STATE: <color><white>" + state.ToString() + "\n";
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

        #region for serialization

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("SER_JOB_NAME", jobName);
            info.AddValue("SER_JOB_TYPE", jobType);
            info.AddValue("SER_JOB_TIME", jobTime);

            GetObjectDataJobSpecific(info, context);
        }

        // serialization for job-specific members
        public abstract void GetObjectDataJobSpecific(SerializationInfo info, StreamingContext context);

        #endregion

        #endregion
    }
}
