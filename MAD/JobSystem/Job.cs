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

        public object jobLock = new object();

        private static int _jobsCount = 0;
        private static object _idLock = new object();
        private int _id;
        public int id { get { return _id; } }

        public enum JobState { Inactive, Waiting, Working, Exception }
        public JobState state = JobState.Inactive;

        public enum OutState { NULL, Success, Failed, Exception }
        public OutState outState = OutState.NULL;

        public DateTime tStart;
        public DateTime tStop;
        public TimeSpan tSpan;

        public List<OutputDesc> outDesc = new List<OutputDesc>();

        public string name { get; set; }
        public enum JobType { NULL, Ping, PortScan, Http, HostDetect, ServiceCheck }
        public JobType type = JobType.NULL;
        public JobTime time = new JobTime();
        public JobNotification noti;

        #endregion

        #region constructors

        protected Job(string name, JobType type)
        {
            InitJob();
            this.name = name;
            this.type = type;
            this.time = new JobTime();
            this.noti = new JobNotification();
        }

        protected Job(string name, JobType type, JobTime time, JobNotification noti)
        {
            InitJob();
            this.name = name;
            this.type = type;
            this.time = time;
            this.noti = noti;
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

        protected void SetOutput(string outDescName, object value)
        {
            OutputDesc _desc = GetOutputDesc(outDescName);
            if (_desc != null)
                _desc.dataObject = value;
            else
                throw new Exception("OutDescriptor not known!");
        }

        protected OutputDesc GetOutputDesc(string outDescName)
        {
            foreach (OutputDesc _desc in outDesc)
                if (_desc.name == outDescName)
                    return _desc;
            return null;
        }

        public abstract void Execute(IPAddress targetAddress);

        #region for CLI only

        public string Status()
        {
            string _temp = "\n";

            _temp += "<color><yellow>ID:\t\t<color><white>" + _id + "\n";
            _temp += "<color><yellow>NAME:\t\t<color><white>" + name + "\n";
            _temp += "<color><yellow>TYPE:\t\t<color><white>" + type.ToString() + "\n";
            _temp += "<color><yellow>STATE:\t\t<color><white>" + state.ToString() + "\n";
            _temp += "<color><yellow>TIME-TYPE:\t\t<color><white>" + time.type.ToString() + "\n";

            if (time.type == JobTime.TimeType.Relative)
            {
                _temp += "<color><yellow>DELAY-TIME:\t\t<color><white>" + time.jobDelay.delayTime + "\n";
                _temp += "<color><yellow>DELAY-REMAIN-TIME:\t\t<color><white>" + time.jobDelay.delayTimeRemaining + "\n";
            }
            else if (time.type == JobTime.TimeType.Absolute)
            {
                _temp += "<color><yellow>TIMES:\t\t<color><white>";

                foreach (JobTimeHandler _buffer in time.jobTimes)
                {
                    _temp += _buffer.JobTimeStatus() + " ";
                }

                _temp += "\n";
            }

            _temp += "<color><yellow>LAST-STARTTIME:\t\t<color><white>" + tStart.ToString("dd.MM.yyyy HH:mm:ss") + "\n";
            _temp += "<color><yellow>LAST-STOPTIME:\t\t<color><white>" + tStop.ToString("dd.MM.yyyy HH:mm:ss") + "\n";
            _temp += "<color><yellow>LAST-TIMESPAN:\t\t<color><white>" + tSpan.Seconds + "s " + tSpan.Milliseconds + "ms (" + tSpan.Ticks + " ticks)\n";
            _temp += "<color><yellow>OUTPUT-STATE:\t\t<color><white>" + outState.ToString() + "\n";

            return _temp + JobStatus();
        }

        protected abstract string JobStatus();

        #endregion

        #region for serialization only

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
