﻿using System;
using System.Net;
using System.Collections.Generic;

namespace MAD.JobSystemCore
{
    public class JobNodeInfo
    {
        private int _id;
        public int id { get { return _id; } }

        private Guid _guid;
        public Guid guid { get { return _guid; } }

        private string _name;
        public string name { get { return _name; } }

        private IPAddress _ip;
        public IPAddress ip { get { return _ip; } }

        private string _mac;
        public string mac { get { return _mac; } }

        private int _state;
        public int state { get { return _state; } }

        private List<JobInfo> _jobs;
        public List<JobInfo> jobs { get { return _jobs; } }

        public JobNodeInfo(int id, Guid guid, string name, IPAddress ip, string mac, int state, List<JobInfo> jobs)
        {
            _id = id;
            _guid = guid;
            _name = name;
            _ip = ip;
            _mac = mac;
            _state = state;
            _jobs = jobs;
        }
    }
}
