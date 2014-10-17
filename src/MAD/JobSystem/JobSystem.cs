using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;

using MAD.MacFinders;

using Newtonsoft.Json;

namespace MAD.JobSystemCore
{
    public class JobSystem
    {
        #region members

        public const string VERSION = "v2.9.0.0";
        public const int MAXNODES = 100;

        private JobScedule _scedule { get; set; }
        private object _sceduleLock = new object();

        public object jsLock = new object();
        private List<JobNode> _nodes = new List<JobNode>();

        public event EventHandler OnNodeCountChanged = null;
        public event EventHandler OnJobCountChanged = null;
        public event EventHandler OnShutdown = null;
        
        #endregion

        #region constructor

        public JobSystem()
        {
            _scedule = new JobScedule(jsLock, _nodes);
        }

        #endregion

        #region methodes

        #region jobsystem handling

        /// <summary>
        /// Save all current nodes into a json-file.
        /// </summary>
        /// <param name="filepath">Path to file (Execption will be thrown if directory does not exist or if the file already exist!).</param>
        public void SaveTable(string filepath)
        {
            lock(jsLock)
                JSSerializer.SerializeTable(filepath, _nodes);
        }

        /// <summary>
        /// Load nodes from json-file.
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns>Path to file (Execption will be thrown if file does not exist!).</returns>
        public int LoadTable(string filepath)
        {
            List<JobNode> _loadNodes = JSSerializer.DeserializeTable(filepath);
            lock (jsLock)
            {
                if (JobSystem.MAXNODES > _nodes.Count + _loadNodes.Count)
                    _nodes.AddRange(_loadNodes);
            }
            return _nodes.Count;
        }

        /// <summary>
        /// This method will clean-up all resources used by the JobSystem.
        /// </summary>
        public void Shutdown()
        {
            _scedule.Stop();

            if (OnShutdown != null)
                OnShutdown.Invoke(null, null);
        }

        #endregion

        #region scedule handling

        public void StartScedule()
        {
            lock(_sceduleLock)
                _scedule.Start();
        }

        public void StopScedule()
        {
            lock(_sceduleLock)
                _scedule.Stop();
        }

        public bool IsSceduleActive()
        {
            lock (_sceduleLock)
            { 
                if(_scedule.state == JobScedule.State.Active)
                    return true;
                else
                    return false;
            }
        }

        #endregion

        #region nodes/jobs informations

        public int NodesActive()
        {
            int _count = 0;
            lock (jsLock)
            {
                for (int i = 0; i < _nodes.Count; i++)
                {
                    if (_nodes[i].state == JobNode.State.Active)
                        _count++;
                }
            }
            return _count;
        }

        public int NodesInactive()
        {
            int _count = 0;
            lock (jsLock)
            {
                for (int i = 0; i < _nodes.Count; i++)
                {
                    if (_nodes[i].state == JobNode.State.Inactive)
                        _count++;
                }
            }
            return _count;
        }

        public int NodesInitialized()
        {
            int _count = 0;
            lock(jsLock)
                _count = _nodes.Count;
            return _count;
        }

        public int JobsActive()
        {
            int _count = 0;
            lock (jsLock)
            {
                for (int i = 0; i < _nodes.Count; i++)
                {
                    foreach (Job _job in _nodes[i].jobs)
                    {
                        if (_job.state == Job.JobState.Waiting)
                            _count++;
                    }
                }
            }
            return _count;
        }

        public int JobsInactive()
        {
            int _count = 0;
            lock (jsLock)
            {
                for (int i = 0; i < _nodes.Count; i++)
                {
                    foreach (Job _job in _nodes[i].jobs)
                    {
                        if (_job.state == Job.JobState.Inactive)
                            _count++;
                    }
                }
            }
            return _count;
        }

        public int JobsInitialized()
        {
            int _count = 0;
            lock (jsLock)
            {
                for (int i = 0; i < _nodes.Count; i++)
                    _count = _count + _nodes[i].jobs.Count;
            }
            return _count;
        }

        #endregion

        #region nodes handling

        public List<JobNode> UnsafeGetNodes()
        {
            return _nodes;
        }

        public JobNode UnsafeGetNode(int id)
        {
            for (int i = 0; i < _nodes.Count; i++)
                if (_nodes[i].id == id)
                    return _nodes[i];
            return null;
        }

        public void StartNode(int id)
        {
            lock (jsLock)
            {
                JobNode _node = UnsafeGetNode(id);
                if (_node != null)
                {
                    if (_node.state == JobNode.State.Inactive)
                        _node.state = JobNode.State.Active;
                    else
                        throw new JobNodeException("Node already active or has an exception!", null);
                }
                else
                    throw new JobNodeException("Node does not exist!", null);
            }
        }

        public void StopNode(int id)
        {
            lock (jsLock)
            {
                JobNode _node = UnsafeGetNode(id);
                if (_node != null)
                {
                    if (_node.state == JobNode.State.Active)
                        _node.state = JobNode.State.Inactive;
                    else
                        throw new JobNodeException("Node already inactive or has an exception!", null);
                }
                else
                    throw new JobNodeException("Node does not exist!", null);
            }
        }

        public void AddNode(JobNode node)
        {
            lock (jsLock)
            {
                if (MAXNODES > _nodes.Count)
                {
                    _nodes.Add(node);

                    if (OnNodeCountChanged != null)
                        OnNodeCountChanged.Invoke(null, null);
                }
                else
                    throw new JobSystemException("Node limit reached!", null);
            }
        }

        public void RemoveNode(int id)
        {
            bool _success = false;

            lock (jsLock)
            {
                for (int i = 0; i < _nodes.Count; i++)
                {
                    if (_nodes[i].id == id)
                    {
                        _nodes.RemoveAt(i);

                        if (OnNodeCountChanged != null)
                            OnNodeCountChanged.Invoke(null, null);

                        _success = true;
                        break;
                    }
                }
            }

            if (!_success)
                throw new JobNodeException("Node does not exist!", null);
        }

        public void RemoveAllNodes()
        {
            lock(jsLock)
                _nodes.Clear();

            if(OnNodeCountChanged != null)
                OnNodeCountChanged.Invoke(null, null);
        }

        public bool NodeExist(int id)
        {
            lock (jsLock)
            {
                JobNode _node = UnsafeGetNode(id);
                if (_node != null)
                    return true;
                else
                    return false;
            }
        }

        public bool UpdateNodeName(int nodeID, string name)
        {
            lock (jsLock)
            {
                JobNode _node = UnsafeGetNode(nodeID);
                if (_node != null)
                {
                    _node.name = name;
                    return true;
                }
                else
                    return false;
            }
        }

        public bool UpdateNodeMac(int nodeID, PhysicalAddress mac)
        {
            lock (jsLock)
            {
                JobNode _node = UnsafeGetNode(nodeID);
                if (_node != null)
                {
                    _node.macAddress = mac;
                    return true;
                }
                else
                    return false;
            }
        }

        public bool UpdateNodeIP(int nodeID, IPAddress ip)
        {
            lock (jsLock)
            {
                JobNode _node = UnsafeGetNode(nodeID);
                if (_node != null)
                {
                    _node.ipAddress = ip;
                    return true;
                }
                else
                    return false;
            }
        }

        // not finished yet
        public SyncResult SyncNodes(List<ModelHost> currentHosts)
        {
            SyncResult _result = new SyncResult();

            return _result;
        }

        #endregion

        #region node serialization

        public void SaveNode(string fileName, int id)
        {
            lock (jsLock)
            {
                JobNode _node = UnsafeGetNode(id);
                if (_node != null)
                    JSSerializer.SerializeNode(fileName, _node);
                else
                    throw new JobNodeException("Node does not exist!", null);
            }
        }

        public void LoadNode(string fileName)
        {
            JobNode _node = JSSerializer.DeserializeNode(fileName);

            lock(jsLock)
                _nodes.Add(_node);
        }

        #endregion

        #region jobs handling

        public Job UnsafeGetJob(int id, out JobNode node)
        {
            for (int i = 0; i < _nodes.Count; i++)
                for (int i2 = 0; i2 < _nodes[i].jobs.Count; i2++)
                    if (_nodes[i].jobs[i2].id == id)
                    {
                        node = _nodes[i];
                        return _nodes[i].jobs[i2];
                    }
            node = null;
            return null;
        }

        public void StartJob(int id)
        {
            lock (jsLock)
            {
                JobNode _node;
                Job _job = UnsafeGetJob(id, out _node);
                if (_job != null)
                {
                    if (!_job.uFlag)
                    {
                        if (_job.state == Job.JobState.Inactive)
                            _job.state = Job.JobState.Waiting;
                        else
                            throw new JobException("Job already active!", null);
                    }
                    else
                        throw new JobException("Job busy.", null);
                }
                else
                    throw new JobException("Job does not exist!", null);
            }
        }

        public void StopJob(int id)
        {
            lock (jsLock)
            {
                JobNode _node;
                Job _job = UnsafeGetJob(id, out _node);
                if (_job != null)
                {
                    if (!_job.uFlag)
                    {
                        if (_job.state == Job.JobState.Waiting)
                            _job.state = Job.JobState.Inactive;
                        else
                            throw new JobException("Job already inactive!", null);
                    }
                    else
                        throw new JobException("Job busy.", null);
                }
                else
                    throw new JobException("Job does not exist!", null);
            }
        }

        public void AddJobToNode(int nodeId, Job job)
        {
            lock (jsLock)
            {
                JobNode _node = UnsafeGetNode(nodeId);
                if (_node != null)
                {
                    if (JobNode.MAXJOBS > _node.jobs.Count)
                    {
                        if (!_node.uFlag)
                            _node.jobs.Add(job);
                        else
                            throw new JobNodeException("Node busy.", null);
                    }
                    else
                        throw new JobSystemException("Jobs limit reached!", null);
                }
                else
                    throw new JobNodeException("Node does not exist!", null);
            }
        }

        public void RemoveJob(int id)
        {
            lock (jsLock)
            {
                bool _success = false;
                foreach (JobNode _node in _nodes)
                {
                    for (int i = 0; i < _node.jobs.Count; i++)
                    {
                        if (_node.jobs[i].id == id)
                        {
                            if (!_node.jobs[i].uFlag)
                            {
                                _node.jobs.RemoveAt(i);
                                _success = true;
                                break;
                            }
                            else
                                throw new JobException("Job busy.", null);
                        }
                    }
                }
                if(_success != false)
                    throw new JobException("Job does not exist!", null);
            }
        }

        public bool JobExist(int id)
        {
            lock (jsLock)
            {
                JobNode _node;
                Job _job = UnsafeGetJob(id, out _node);
                if (_job != null)
                    return true;
                else
                    return false;
            }
        }

        #endregion

        #endregion
    }
}
