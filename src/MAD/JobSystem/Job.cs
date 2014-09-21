using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;

using Newtonsoft.Json;

namespace MAD.JobSystemCore
{
    public abstract class Job
    {
        #region members

        public enum JobType { NULL, Ping, PortScan, Http, HostDetect, ServiceCheck, SnmpCheck }
        public enum JobState { Inactive, Waiting, Working, Exception }

        [JsonIgnore]
        public ReaderWriterLockSlim jobLock = new ReaderWriterLockSlim();
        [JsonIgnore]
        private static object _idLock = new object();
        [JsonIgnore]
        private static int _jobsCount = 0;
        [JsonIgnore]
        private int _id;
        [JsonIgnore]
        public int id { get { return _id; } }
        [JsonIgnore]
        public JobOutput outp { get; set; }
        [JsonIgnore]
        public JobState state = JobState.Inactive;
        [JsonIgnore]
        public DateTime tStart { get; set; }
        [JsonIgnore]
        public DateTime tStop { get; set; }
        [JsonIgnore]
        public TimeSpan tSpan { get; set; }

        // general
        public string name { get; set; }
        public JobType type { get; set; }
        public JobTime time { get; set; }

        // notification
        public bool notiFlag = true;
        public JobNotificationSettings settings { get; set; }
        public List<JobRule> rules = new List<JobRule>();

        #endregion

        #region constructors

        protected Job(JobType type)
        {
            InitJob();
            this.type = type;
            this.outp = new JobOutput();
            this.time = new JobTime();
        }

        protected Job(string name, JobType type)
        {
            InitJob();
            this.name = name;
            this.type = type;
            this.outp = new JobOutput();
            this.time = new JobTime();
        }

        protected Job(string name, JobType type, JobTime time, JobOutput outp)
        {
            InitJob();
            this.name = name;
            this.type = type;
            this.outp = outp;
            this.time = time;
            this.settings = new JobNotificationSettings();
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

        #region for CLI only

        /* Is this really a good idea?
         * Maybe create a command, which determines JobTyes ...
         * Because this method is more complicated and takes more affort ...
         */
        public string Status()
        {
            string _temp = "\n";

            _temp += "<color><yellow>ID:\t\t<color><white>" + _id + "\n";
            _temp += "<color><yellow>NAME:\t\t<color><white>" + name + "\n";
            _temp += "<color><yellow>TYPE:\t\t<color><white>" + type.ToString() + "\n";
            _temp += "<color><yellow>STATE:\t\t<color><white>" + state.ToString() + "\n";
            _temp += "<color><yellow>TIME-TYPE:\t\t<color><white>" + time.type.ToString() + "\n";

            if (time.type == JobTime.TimeMethod.Relative)
            {
                _temp += "<color><yellow>DELAY-TIME:\t\t<color><white>" + time.jobDelay.delayTime + "\n";
                _temp += "<color><yellow>DELAY-REMAIN-TIME:\t\t<color><white>" + time.jobDelay.delayTimeRemaining + "\n";
            }
            else if (time.type == JobTime.TimeMethod.Absolute)
            {
                _temp += "<color><yellow>TIMES:\t\t<color><white>";
                foreach (JobTimeHandler _buffer in time.jobTimes)
                    _temp += _buffer.JobTimeStatus() + " ";
                _temp += "\n";
            }

            _temp += "<color><yellow>LAST-STARTTIME:\t\t<color><white>" + tStart.ToString("dd.MM.yyyy HH:mm:ss") + "\n";
            _temp += "<color><yellow>LAST-STOPTIME:\t\t<color><white>" + tStop.ToString("dd.MM.yyyy HH:mm:ss") + "\n";
            _temp += "<color><yellow>LAST-TIMESPAN:\t\t<color><white>" + tSpan.Seconds + "s " + tSpan.Milliseconds + "ms (" + tSpan.Ticks + " ticks)\n";
            _temp += "<color><yellow>OUTPUT-STATE:\t\t<color><white>" + outp.outState.ToString() + "\n";

            return _temp + JobStatus();
        }

        protected abstract string JobStatus();

        #endregion

        #endregion
    }
}
