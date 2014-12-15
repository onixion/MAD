using System;

namespace MAD.JobSystemCore
{
    public class NodeEventArgs : EventArgs
    {
        public JobNode node;

        public NodeEventArgs(JobNode node)
            : base()
        {
            this.node = node;
        }
    }

    public class JobAndNodeEventArgs : EventArgs
    {
        public JobNode node;
        public Job job;

        public JobAndNodeEventArgs(JobNode node, Job job)
            : base()
        {
            this.node = node;
            this.job = job;
        }
    }
}
