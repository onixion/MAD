﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;

using Newtonsoft.Json;

namespace MAD.JobSystemCore
{
    public class JobNode
    {
        #region members

        [JsonIgnore]
        private static object _idLock = new object();
        [JsonIgnore]
        private static int _nodesCount = 0;
        [JsonIgnore]
        private int _id;
        [JsonIgnore]
        public int id { get { return _id; } }

        [JsonIgnore]
        public const int MAXJOBS = 100;

        /* This bool represents the state of the node. The node can
         * be active (state = 1) or inactive (state = 0). */
        [JsonIgnore]
        public int state = 0;
        /* This counter shows how many jobs are working. */
        [JsonIgnore]
        public int uWorker = 0;

        public Guid guid { get; set; }
        public string name { get; set; }
        public PhysicalAddress mac { get; set; } // sns
        public IPAddress ip { get; set; } // sns
        public List<Job> jobs = new List<Job>();

        #endregion

        #region constructors

        public JobNode()
        {
            InitID();
            this.guid = new Guid();
        }

        public JobNode(string nodeName, PhysicalAddress macAddress, IPAddress ipAddress, List<Job> jobs)
        {
            InitID();

            this.guid = new Guid();
            this.name = nodeName;
            this.mac = macAddress;
            this.ip = ipAddress;
            this.jobs = jobs;
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
