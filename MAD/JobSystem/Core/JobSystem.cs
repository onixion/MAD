using System;
using System.Collections.Generic;

namespace MAD.JobSystemCore
{
    public class JobSystem
    {
        #region members

        // Version
        private Version _version = new Version(2, 0);
        public Version version { get { return _version; } }

        // Scedule
        private JobScedule _scedule;
        public JobScedule.State sceduleState { get { return _scedule.state; } }

        // Nodes
        public List<JobNode> nodes = new List<JobNode>();
        public object jsNodesLock = new object();
        public const int maxNodes = 100;
        public int nodesCount { get { return nodes.Count; } }

        // Jobs
        public readonly int maxJobsPossible = JobSystem.maxNodes * JobNode.maxJobs;
        public int jobsCount { get { return JobsInitialized(); } }

        // Other
        private string _dataPath;

        #endregion

        #region constructor

        public JobSystem(string dataPath)
        {
            _dataPath = dataPath;
            _scedule = new JobScedule(nodes, jsNodesLock);
        }

        #endregion

        #region methodes

        #region scedule handling

        public void StartScedule()
        {
            _scedule.Start();
        }

        public void StopScedule()
        {
            _scedule.Stop();
        }

        #endregion

        #region nodes/jobs informations

        public int NodesActive()
        {
            int _count = 0;

            lock (jsNodesLock)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    if (nodes[i].state == JobNode.State.Active)
                    {
                        _count++;
                    }
                }
            }

            return _count;
        }

        public int NodesInactive()
        {
            int _count = 0;

            lock (jsNodesLock)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    if (nodes[i].state == JobNode.State.Inactive)
                    {
                        _count++;
                    }
                }
            }

            return _count;
        }

        public int JobsInitialized()
        {
            int _count = 0;

            lock (jsNodesLock)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    _count = _count + nodes[i].jobs.Count;
                }
            }

            return _count;
        }

        #endregion

        #region nodes handling

        public bool StartNode(int nodeID)
        {
            JobNode _node = GetNode(nodeID);

            if (_node != null)
            {
                _node.state = JobNode.State.Active;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool StopNode(int nodeID)
        {
            JobNode _node = GetNode(nodeID);

            if (_node != null)
            {
                _node.state = JobNode.State.Inactive;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool AddNode(JobNode node)
        {
            if (maxNodes > nodes.Count)
            {
                nodes.Add(node);
                return true;
            }
            else
            {
                //throw new Exception("Node could not be added, because the jobsystem has reached its max numbers of nodes.");
                return false;
            }
        }

        public bool RemoveNode(int nodeID)
        {
            lock (jsNodesLock)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    if (nodes[i].id == nodeID)
                    {
                        nodes.RemoveAt(i);
                        return true;
                    }
                }

                return false;
            }
        }

        public void RemoveAllNodes()
        {
            lock (jsNodesLock)
            {
                nodes.Clear();
            }
        }

        public JobNode GetNode(int nodeID)
        {
            lock (jsNodesLock)
            {
                foreach (JobNode _node in nodes)
                {
                    if (_node.id == nodeID)
                    {
                        return _node;
                    }
                }
            }

            return null;
        }

        #endregion

        #region jobs handling

        public bool StartJob(int jobID)
        {
            Job _job = GetJob(jobID);

            if (_job != null)
            {
                if (_job.state == Job.JobState.Inactive)
                {
                    _job.state = Job.JobState.Waiting;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool StopJob(int jobID)
        {
            Job _job = GetJob(jobID);

            if (_job != null)
            {
                if (_job.state == Job.JobState.Waiting)
                {
                    _job.state = Job.JobState.Inactive;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool AddJobToNode(int nodeID, Job jobToAdd)
        {
            JobNode _node = GetNode(nodeID);

            if (_node != null)
            {
                lock (jsNodesLock)
                {
                    _node.jobs.Add(jobToAdd);
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool RemoveJob(int jobID)
        {
            lock (jsNodesLock)
            {
                foreach (JobNode _node in nodes)
                {
                    lock (_node.jobsLock)
                    {
                        for(int i = 0; i < _node.jobs.Count; i++)
                        {
                            if (_node.jobs[i].id == jobID)
                            {
                                _node.jobs.RemoveAt(i);
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        public bool UpdateJob(int jobID, Job newJob)
        {
            Job _job = GetJob(jobID);

            if (_job != null)
            {
                // TODO
                throw new NotImplementedException();
                //return true;
            }
            else
            {
                return false;
            }
        }

        public bool JobExist(int jobID)
        {
            foreach (JobNode _node in nodes)
            {
                foreach (Job _temp in _node.jobs)
                {
                    if (_temp.id == jobID)
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
                foreach (Job _job in _node.jobs)
                {
                    if (jobID == _job.id)
                    {
                        return _job;
                    }
                }
            }

            return null;
        }

        #endregion

        #region save/load nodes from file

        public void SaveNodes(string path)
        {
            // TODO
        }

        public void LoadNodes(string path)
        { 
            // TODO
        }

        #endregion

        #endregion
    }
}
