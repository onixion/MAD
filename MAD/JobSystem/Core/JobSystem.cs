using System;
using System.Collections.Generic;

namespace MAD.JobSystemCore
{
    public class JobSystem
    {
        #region member

        private string _dataPath;

        public List<JobNode> nodes = new List<JobNode>();
        public Object nodesLock = new Object();

        private int _maxNodes = 100;
        public int maxNodes { get { return _maxNodes; } }

        private int _maxJobs = 100;
        public List<Job> cachedJobs = new List<Job>();

        private JobScedule _scedule;
        public JobScedule.State sceduleState { get { return _scedule.state; } }

        #endregion

        #region constructor

        public JobSystem(string dataPath)
        {
            _dataPath = dataPath;
            _scedule = new JobScedule(nodes, nodesLock);
        }

        #endregion

        #region methodes

        public void StartScedule()
        {
            _scedule.Start();
        }

        public void StopScedule()
        {
            _scedule.Stop();
        }

        public void StartNode(int nodeID)
        {
            JobNode _node = GetNode(nodeID);

            if (_node != null)
            {
                // Check if node is already active or not.

                // Set node state to 'running'.
            }
            else
            {
                throw new Exception("Node does not exist!");
            }
        }

        public void StopNode(int nodeID)
        {
            JobNode _node = GetNode(nodeID);

            if (_node != null)
            {
                // Same like start a node.
            }
            else
            {
                throw new Exception("Node does not exist!");
            }
        }

        public void AddNode(JobNode node)
        {
            if (_maxNodes > nodes.Count)
            {
                nodes.Add(node);
            }
            else
            {
                throw new Exception("Node could not be added, because the jobsystem has reached its max numbers of nodes.");
            }
        }

        public void RemoveNode(int nodeID)
        {
            lock (nodesLock)
            {
                JobNode _node = GetNode(nodeID);

                if (_node != null)
                {
                    // TODO:
                    //  Stop all jobs.
                    //  Remove node.
                }
                else
                {
                    throw new Exception("Node does not exist!");
                }
            }
        }

        public bool JobExist(int jobID)
        {
            foreach (JobNode _node in nodes)
            {
                foreach (Job _temp in _node._jobs)
                {
                    if (_temp.jobID == jobID)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public Job GetJob(int jobID)
        {
            foreach (JobNode _node in nodes)
            {
                foreach (Job _job in _node._jobs)
                {
                    if (jobID == _job.jobID)
                    {
                        return _job;
                    }
                }
            }

            return null;
        }

        public JobNode GetNode(int nodeID)
        {
            foreach (JobNode _node in nodes)
            {
                    if (_node.nodeID == nodeID)
                    {
                        return _node;
                    }
            }

            return null;
        }

        public void RemoveAllNodes()
        {
            // TODO
        }

        public int NodesActive()
        {
            int _count = 0;

            lock (nodesLock)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    // check if node state is on running and increase _count.
                }
            }

            return _count;
        }

        public int NodesInactive()
        {
            int _count = 0;

            lock (nodesLock)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    // same like above
                }
            }

            return _count;
        }

        public void AddToCache(Job job)
        {
            if (cachedJobs.Count <= _maxJobs)
            {
                cachedJobs.Add(job);
            }
            else
            {
                throw new Exception("Job cache limit reached!");
            }
        }
        // HERE I WAS
        public void RemoveFromCache(int jobID)
        {
            bool _removed = false;

            for (int i = 0; i < cachedJobs.Count; i++)
            {
                if (cachedJobs[i].jobID == jobID)
                {
                    cachedJobs.RemoveAt(i);
                    _removed = true;
                }
            }

            if (!_removed)
            {
                throw new Exception("Job does not exits!");
            }
        }

        public void SaveNodes(string path)
        {
            // TODO
        }

        public void LoadNodes(string path)
        { 
            // TODO
        }

        #endregion
    }
}
