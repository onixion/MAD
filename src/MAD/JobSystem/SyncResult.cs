using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;

namespace MAD.JobSystemCore
{
    public class SyncResult
    {
        public int nodesAdded;
        public int nodesUpdated;

        public SyncResult()
        {
            this.nodesAdded = 0;
            this.nodesUpdated = 0;
        }

        public SyncResult(int nodesAdded, int nodesUpdated)
        {
            this.nodesAdded = nodesAdded;
            this.nodesUpdated = nodesUpdated;
        }
    }
}
