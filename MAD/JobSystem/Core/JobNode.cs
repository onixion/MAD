using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;

namespace MAD.JobSystemCore
{
    public class JobNode
    {
        private static int _idCount = 0;
        private int _id;
        private object _initIDLock = new object();

        private PhysicalAddress _macAddress;
        private IPAddress _ipAddress;

        public List<Job> _jobs;

        private void InitID()
        {
            lock (_initIDLock)
            {
                _id = _idCount;
                _idCount++;
            }
        }
    }
}
