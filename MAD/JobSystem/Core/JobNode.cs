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

        // id
        private static int _nodesCount = 0;
        private object _nodeInitLock = new object();
        private int _id;
        public int id { get { return _id; } }

        // state
        public enum State { Active, Inactive, Exception }
        public State state = State.Inactive;

        // jobs
        public List<Job> jobs = new List<Job>();
        public object jobsLock = new object();
        public const int maxJobs = 100;

        public string nodeName;
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
            this.nodeName = nodeName;
            this.macAddress = macAddress;
            this.ipAddress = ipAddress;
            this.jobs = jobs;
        }

        public JobNode(SerializationInfo info, StreamingContext context)
        {
            nodeName = (string)info.GetValue("SER_NODE_NAME", typeof(string));
            macAddress = PhysicalAddress.Parse((string)info.GetValue("SER_NODE_MAC", typeof(string)));
            ipAddress = IPAddress.Parse((string)info.GetValue("SER_NODE_IP", typeof(string)));

            jobs = (List<Job>)info.GetValue("SER_NODE_JOBS", typeof(List<Job>));
        }

        #endregion

        #region methodes

        private void InitID()
        {
            lock (_nodeInitLock)
            {
                _id = _nodesCount;
                _nodesCount++;
            }
        }

        // This is need for serialization.
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("SER_NODE_NAME", nodeName);
            info.AddValue("SER_NODE_MAC", macAddress.ToString());
            info.AddValue("SER_NODE_IP", ipAddress.ToString());
            info.AddValue("SER_NODE_JOBS", jobs);
        }

        #endregion
    }
}
