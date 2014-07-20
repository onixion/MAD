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
        private static object _idLock = new object();
        private int _id;
        public int id { get { return _id; } }

        public enum JobType { NULL, Ping, PortScan, Http, HostDetect, ServiceCheck }
        public JobType type = JobType.NULL;

        public enum JobState { NULL, Inactive, Waiting, Working, Exception }
        public JobState state = JobState.Inactive;

        public enum OutState { NULL, Success, Failed, Exception }
        public OutState outState = OutState.NULL;

        public DateTime lastStarted;
        public DateTime lastFinished;
        public TimeSpan lastTimeSpan;

        public string name;
        public JobTime time = new JobTime();
        public List<OutputDesc> outDesc = new List<OutputDesc>(); // NOT SERIALIZED YET!
        public JobNotification notification = new JobNotification(); // NOT SERIALIZED YET!

        #endregion

        #region constructors

        protected Job(string name, JobType type, JobTime time)
        {
            InitJob();
            this.name = name;
            this.type = type;
            this.time = time;
        }

        // for serialization
        protected Job(SerializationInfo info, StreamingContext context)
        {
            InitJob();
            this.name = (string) info.GetValue("SER_JOB_NAME", typeof(string));
            this.type = (JobType)info.GetValue("SER_JOB_TYPE", typeof(JobType));
            this.time = (JobTime)info.GetValue("SER_JOB_TIME", typeof(JobTime));
        }

        #endregion

        #region methodes

        private void InitJob()
        {
            lock (_idLock)
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
            _temp += "<color><yellow>NAME: <color><white>" + name + "\n";
            _temp += "<color><yellow>TYPE: <color><white>" + type.ToString() + "\n";
            _temp += "<color><yellow>STATE: <color><white>" + state.ToString() + "\n";
            _temp += "<color><yellow>TIME-TYPE: <color><white>" + time.type.ToString() + "\n";

            if (time.type == JobTime.TimeType.Relative)
            {
                _temp += "<color><yellow>DELAY-TIME: <color><white>" + time.jobDelay.delayTime + "\n";
                _temp += "<color><yellow>DELAY-REMAIN-TIME: <color><white>" + time.jobDelay.delayTimeRemaining + "\n";
            }
            else if (time.type == JobTime.TimeType.Absolute)
            {
                _temp += "<color><yellow>TIMES: <color><white>";

                foreach (JobTimeHandler _buffer in time.jobTimes)
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
            info.AddValue("SER_JOB_NAME", name);
            info.AddValue("SER_JOB_TYPE", type);
            info.AddValue("SER_JOB_TIME", time);

            GetObjectDataJobSpecific(info, context);
        }

        // serialization for job-specific members
        public abstract void GetObjectDataJobSpecific(SerializationInfo info, StreamingContext context);

        #endregion

        #endregion
    }
}
