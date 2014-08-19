using System;
using System.Net;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace MAD.JobSystemCore
{
    [Serializable]
    public abstract class Job : IXmlSerializable
    {
        #region members

        public object jobLock = new object();

        private static int _jobsCount = 0;
        private static object _idLock = new object();
        private int _id;
        public int id { get { return _id; } }

        [XmlIgnoreAttribute]
        public enum JobState { Inactive, Waiting, Working, Exception }
        [XmlIgnoreAttribute]
        public enum OutState { NULL, Success, Failed, Exception }
        [XmlIgnoreAttribute]
        public DateTime tStart;
        [XmlIgnoreAttribute]
        public DateTime tStop;
        [XmlIgnoreAttribute]
        public TimeSpan tSpan;

        public List<OutputDesc> outDesc = new List<OutputDesc>();

        public string name { get; set; }
        public enum JobType { NULL, Ping, PortScan, Http, HostDetect, ServiceCheck, SnmpCheck }
        public JobType type = JobType.NULL;
        public JobTime time = new JobTime();
        public JobNotification noti;

        public JobState state = JobState.Inactive;
        public OutState outState = OutState.NULL;

        #endregion

        #region constructors

        public Job()
        {
            InitJob();
        }

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

            if (time.type == JobTime.TimeMethod.Relative)
            {
                _temp += "<color><yellow>DELAY-TIME:\t\t<color><white>" + time.jobDelay.delayTime + "\n";
                _temp += "<color><yellow>DELAY-REMAIN-TIME:\t\t<color><white>" + time.jobDelay.delayTimeRemaining + "\n";
            }
            else if (time.type == JobTime.TimeMethod.Absolute)
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

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }

        public void ReadXml(XmlReader reader)
        {
            ReadXmlJobSpec(reader);
        }

        public void WriteXml(XmlWriter writer)
        {
            WriteXmlJobSpec(writer);
        }

        public virtual void ReadXmlJobSpec(XmlReader reader) { }

        public virtual void WriteXmlJobSpec(XmlWriter writer) { }

        #endregion

        #endregion
    }
}
