﻿using System;
using System.Collections.Generic;

namespace MAD.JobSystemCore
{
    public class JobSystem
    {
        #region member

        private string _dataPath;

        public List<JobNode> nodes = new List<JobNode>();
        public object jsNodesLock = new object();

        public const int maxNodes = 100;
        public static int maxJobsOverall { get { return maxNodes * JobNode.maxJobs; } }

        public List<Job> cachedJobs = new List<Job>();
        public static int maxCachedJobs { get { return JobNode.maxJobs; } }

        private JobScedule _scedule;
        public JobScedule.State sceduleState { get { return _scedule.state; } }

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

        // What is if the reference disappears?
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

        public void RemoveAllNodes()
        {
            lock(jsNodesLock)
            {
                nodes.Clear();
            }
        }

        public int NodesActive()
        {
            int _count = 0;

            lock (jsNodesLock)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    if(nodes[i].state == JobNode.State.Active)
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
                    if(nodes[i].state == JobNode.State.Inactive)
                    {
                        _count++;
                    }
                }
            }

            return _count;
        }

        public int NodesJobsInitialized()
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

        #region job handling

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

        public bool AddToCache(Job job)
        {
            if (cachedJobs.Count <= maxCachedJobs)
            {
                cachedJobs.Add(job);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool RemoveFromCache(int jobID)
        {
            for (int i = 0; i < cachedJobs.Count; i++)
            {
                if (cachedJobs[i].id == jobID)
                {
                    cachedJobs.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        public void ClearCache()
        {
            cachedJobs.Clear();
        }

        #endregion

        // TODO______________________________

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
