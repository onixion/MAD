﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;

using Newtonsoft.Json;

namespace MAD.JobSystemCore
{
    public class JobNode
    {
        #region members

        public enum State { Active, Inactive, Exception }

        [JsonIgnore]
        public object nodeLock = new object();
        [JsonIgnore]
        private static object _idLock = new object();
        [JsonIgnore]
        private static int _nodesCount = 0;
        [JsonIgnore]
        private int _id;
        [JsonIgnore]
        public int id { get { return _id; } }
        [JsonIgnore]
        public State state = State.Inactive;
        [JsonIgnore]
        public const int MAX_JOBS = 100;
        
        public string name { get; set; }
        public PhysicalAddress macAddress { get; set; } // sns
        public IPAddress ipAddress { get; set; } // sns
        public JobNotification defaultNoti { get; set; }

        public List<Job> jobs = new List<Job>();

        #endregion

        #region constructors

        public JobNode()
        {
            InitID();
        }

        public JobNode(string nodeName, PhysicalAddress macAddress, IPAddress ipAddress, List<Job> jobs, JobNotification defaultNoti)
        {
            InitID();
            this.name = nodeName;
            this.macAddress = macAddress;
            this.ipAddress = ipAddress;
            this.jobs = jobs;
            this.defaultNoti = defaultNoti;
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

        #endregion
    }
}