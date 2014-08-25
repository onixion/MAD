﻿using System;
using System.Net;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

using Newtonsoft.Json;

namespace MAD.JobSystemCore
{
    public abstract class Job
    {
        #region members

        [JsonIgnore]
        public object jobLock = new object();
        [JsonIgnore]
        private static object _idLock = new object();
        [JsonIgnore]
        private static int _jobsCount = 0;
        [JsonIgnore]
        private int _id;
        [JsonIgnore]
        public int id { get { return _id; } }

        public enum JobType { NULL, Ping, PortScan, Http, HostDetect, ServiceCheck, SnmpCheck }
        public enum JobState { Inactive, Waiting, Working, Exception }
        
        public string name { get; set; }
        public JobTime time { get; set; }


        public JobNotification noti { get; set; }
        public JobNotificationSettings settings { get; set; }

        [JsonIgnore]
        public JobOutput outp { get; set; }

        // Explanation for leaving those one out, can be found in 'JobScedule.cs'.
        //public List<JobRule> rules { get; set; }

        public DateTime tStart { get; set; }
        public DateTime tStop { get; set; }
        public TimeSpan tSpan { get; set; }

        public JobType type = JobType.NULL;
        [JsonIgnore]
        public JobState state = JobState.Inactive;

        #endregion

        #region constructors

        protected Job(JobType type)
        {
            InitJob();
            this.type = type;
            this.time = new JobTime();
            this.outp = new JobOutput();
            this.noti = new JobNotification();
            this.settings = new JobNotificationSettings();
        }

        protected Job(string name, JobType type)
        {
            InitJob();
            this.name = name;
            this.type = type;
            this.time = new JobTime();
            this.outp = new JobOutput();
            this.noti = new JobNotification();
            this.settings = new JobNotificationSettings();
        }

        protected Job(string name, JobType type, JobTime time, JobOutput outp, JobNotification noti, JobNotificationSettings settings)
        {
            InitJob();
            this.name = name;
            this.type = type;
            this.time = time;
            this.outp = outp;
            this.noti = noti;
            this.settings = settings;
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