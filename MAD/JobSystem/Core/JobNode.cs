using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.Serialization;

namespace MAD.JobSystemCore
{
    [Serializable]
    public class JobNode : ISerializable
    {
        #region member

        private static int _nodesCount = 0;
        private static object _idLock = new object();
        private int _id;
        public int id { get { return _id; } }

        public enum State { Active, Inactive, Exception }
        public State state = State.Inactive;

        public List<Job> jobs = new List<Job>();
        public object jobsLock = new object();
        public const int maxJobs = 100;

        public string name;
        public PhysicalAddress macAddress;
        public IPAddress ipAddress;

        #endregion

        #region constructors

        public JobNode()
        {
            InitID();
        }

        public JobNode(string nodeName, PhysicalAddress macAddress, IPAddress ipAddress, List<Job> jobs)
        {
            InitID();
            this.name = nodeName;
            this.macAddress = macAddress;
            this.ipAddress = ipAddress;
            this.jobs = jobs;
        }

        public JobNode(SerializationInfo info, StreamingContext context)
        {
            InitID();
            this.name = (string)info.GetValue("SER_NODE_NAME", typeof(string));
            this.macAddress = PhysicalAddress.Parse((string)info.GetValue("SER_NODE_MAC", typeof(string)));
            this.ipAddress = IPAddress.Parse((string)info.GetValue("SER_NODE_IP", typeof(string)));
            this.jobs = (List<Job>)info.GetValue("SER_NODE_JOBS", typeof(List<Job>));
        }

        #endregion

        #region methodes

        private void InitID()
        {
            lock (_idLock)
            {
                _id = _nodesCount;
                _nodesCount++;
            }
        }

        // This is need for serialization.
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("SER_NODE_NAME", name);
            info.AddValue("SER_NODE_MAC", macAddress.ToString());
            info.AddValue("SER_NODE_IP", ipAddress.ToString());
            info.AddValue("SER_NODE_JOBS", jobs);
        }

        #endregion
    }
}
