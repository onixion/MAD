using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;

using MAD.Helper;

using Newtonsoft.Json;

namespace MAD.JobSystemCore
{
    public class JobSystem
    {
        #region members

        public const string VERSION = "v2.9.0.0";
        public const int MAXNODES = 100;

        private JobScedule _scedule { get; set; }
        public JobScedule.State sceduleState { get { return _scedule.state; } }

        private List<JobNode> _nodes = new List<JobNode>();
        public List<JobNode> nodes { get { return _nodes; } }

        public ReaderWriterLockSlim nodesLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);

        public event EventHandler OnNodeCountChanged = null;
        public event EventHandler OnNodeAdded = null;
        public event EventHandler OnNodeRemoved = null;
        
        #endregion

        #region constructor

        public JobSystem()
        {
            _scedule = new JobScedule(nodesLock, _nodes);
        }

        #endregion

        #region methodes

        #region jobsystem handling

        public void SaveTable(string filepath)
        {
            nodesLock.EnterReadLock();
            JSSerializer.SerializeTable(filepath, nodes);
            nodesLock.ExitReadLock();
        }

        public int LoadTable(string filepath)
        {
            List<JobNode> _nodes = JSSerializer.DeserializeTable(filepath);

            nodesLock.EnterWriteLock();
            nodes.AddRange(_nodes);
            nodesLock.ExitWriteLock();

            return _nodes.Count;
        }

        public void Shutdown()
        {
            _scedule.Stop();
        }

        #endregion

        #region scedule handling

        public void StartScedule()
        {
            _scedule.Start();
        }

        public void StopScedule()
        {
            _scedule.Stop();
        }

        public bool IsSceduleActive()
        {
            if (_scedule.state == JobScedule.State.Active)
                return true;
            else
                return false;
        }

        #endregion

        #region nodes/jobs informations

        public int NodesActive()
        {
            int _count = 0;

            nodesLock.EnterReadLock();
            for (int i = 0; i < _nodes.Count; i++)
            {
                _nodes[i].nodeLock.EnterReadLock();
                if (_nodes[i].state == JobNode.State.Active)
                    _count++;
                _nodes[i].nodeLock.ExitReadLock();
            }
            nodesLock.ExitReadLock();

            return _count;
        }

        public int NodesInactive()
        {
            int _count = 0;

            nodesLock.EnterReadLock();
            for (int i = 0; i < _nodes.Count; i++)
            {
                nodes[i].nodeLock.EnterReadLock();
                if (_nodes[i].state == JobNode.State.Inactive)
                    _count++;
                nodes[i].nodeLock.ExitReadLock();
            }
            nodesLock.ExitReadLock();

            return _count;
        }

        public int NodesInitialized()
        {
            nodesLock.EnterReadLock();
            int _count = _nodes.Count;
            nodesLock.ExitReadLock();

            return _count;
        }

        public int JobsActive()
        {
            int _count = 0;

            nodesLock.EnterReadLock();
            for (int i = 0; i < _nodes.Count; i++)
            {
                _nodes[i].nodeLock.EnterReadLock();
                foreach (Job _job in _nodes[i].jobs)
                {
                    _job.jobLock.EnterReadLock();
                    if (_job.state == Job.JobState.Waiting)
                        _count++;
                    _job.jobLock.ExitReadLock();
                }
                _nodes[i].nodeLock.ExitReadLock();
            }
            nodesLock.ExitReadLock();

            return _count;
        }

        public int JobsInactive()
        {
            int _count = 0;

            nodesLock.EnterReadLock();
            for (int i = 0; i < _nodes.Count; i++)
            {
                _nodes[i].nodeLock.EnterReadLock();
                foreach (Job _job in _nodes[i].jobs)
                {
                    _job.jobLock.EnterReadLock();
                    if (_job.state == Job.JobState.Inactive)
                        _count++;
                    _job.jobLock.ExitReadLock();
                }
                _nodes[i].nodeLock.ExitReadLock();
            }
            nodesLock.ExitReadLock();

            return _count;
        }

        public int JobsInitialized()
        {
            int _count = 0;

            nodesLock.EnterReadLock();
            for (int i = 0; i < _nodes.Count; i++)
            {
                _nodes[i].nodeLock.EnterReadLock();
                _count = _count + _nodes[i].jobs.Count;
                _nodes[i].nodeLock.ExitReadLock();
            }
            nodesLock.ExitReadLock();

            return _count;
        }

        #endregion

        #region nodes handling

        public void StartNode(int id)
        {
            nodesLock.EnterWriteLock();
            JobNode _node = UnsafeGetNode(id);
            if (_node != null)
            {
                _node.nodeLock.EnterWriteLock();
                _node.state = JobNode.State.Active;
                _node.nodeLock.ExitWriteLock();
                nodesLock.ExitWriteLock();
            }
            else
            {
                nodesLock.ExitWriteLock();
                throw new JobNodeException("Node already active!", null);
            }
        }

        public void StopNode(int id)
        {
            nodesLock.EnterWriteLock();
            JobNode _node = UnsafeGetNode(id);
            if (_node != null)
            {
                _node.nodeLock.EnterWriteLock();
                _node.state = JobNode.State.Inactive;
                _node.nodeLock.ExitWriteLock();
                nodesLock.ExitWriteLock();
            }
            else
            {
                nodesLock.EnterWriteLock();
                throw new JobNodeException("Node already inactive!", null);
            }
        }

        public void AddNode(JobNode node)
        {
            nodesLock.EnterWriteLock();
            if (MAXNODES > _nodes.Count)
            {
                _nodes.Add(node);
                nodesLock.ExitWriteLock();

                //if (OnNodeCountChanged != null)
                //    OnNodeCountChanged.Invoke(null, null);
            }
            else
            {
                nodesLock.ExitUpgradeableReadLock();
                throw new JobSystemException("Node limit reached!", null);
            }
        }

        public void RemoveNode(int id)
        {
            bool _success = false;

            nodesLock.EnterWriteLock();
            for (int i = 0; i < _nodes.Count; i++)
            {
                if (_nodes[i].id == id)
                {
                    _nodes.RemoveAt(i);
                    nodesLock.ExitWriteLock();

                    //if (OnNodeCountChanged != null)
                    //    OnNodeCountChanged.Invoke(null, null);

                    _success = true;
                    break;
                }
            }

            if (!_success)
            {
                nodesLock.ExitWriteLock();
                throw new JobNodeException("Node does not exist!", null);
            }
        }

        public void RemoveAllNodes()
        {
            nodesLock.EnterWriteLock();
            _nodes.Clear();
            nodesLock.ExitWriteLock();

            //if(OnNodeCountChanged != null)
            //    OnNodeCountChanged.Invoke(null, null);
        }

        public bool NodeExist(int id)
        {
            nodesLock.EnterReadLock();
            JobNode _node = UnsafeGetNode(id);
            if (_node != null)
            {
                nodesLock.ExitReadLock();
                return true;
            }
            else
            {
                nodesLock.ExitReadLock();
                return false;
            }
        }

        public void UpdateNodeName(int nodeID, string name)
        {
            nodesLock.EnterReadLock();
            JobNode _node = UnsafeGetNode(nodeID);
            if (_node != null)
            {
                _node.nodeLock.EnterWriteLock();
                _node.name = name;
                _node.nodeLock.ExitWriteLock();
                nodesLock.ExitReadLock();
            }
            else
            {
                nodesLock.ExitReadLock();
                throw new JobNodeException("Node does not exist!", null);
            }
        }

        public void UpdateNodeMac(int nodeID, PhysicalAddress mac)
        {
            nodesLock.EnterReadLock();
            JobNode _node = UnsafeGetNode(nodeID);
            if (_node != null)
            {
                _node.nodeLock.EnterWriteLock();
                _node.macAddress = mac;
                _node.nodeLock.ExitWriteLock();
                nodesLock.ExitReadLock();
            }
            else
            {
                nodesLock.ExitReadLock();
                throw new JobNodeException("Node does not exist!", null);
            }
        }

        public void UpdateNodeIP(int nodeID, IPAddress ip)
        {
            nodesLock.EnterReadLock();
            JobNode _node = UnsafeGetNode(nodeID);
            if (_node != null)
            {
                _node.nodeLock.EnterWriteLock();
                _node.ipAddress = ip;
                _node.nodeLock.ExitWriteLock();
                nodesLock.ExitReadLock();
            }
            else
            {
                nodesLock.ExitReadLock();
                throw new JobNodeException("Node does not exist!", null);
            }
        }

        // not locked right yet
        public SyncResult SyncNodes(List<ModelHost> currentHosts)
        {
            SyncResult _result = new SyncResult();
            foreach (ModelHost _host in currentHosts)
            {
                // First check if a node exists with the same mac-address
                JobNode _node = GetNode(PhysicalAddress.Parse((string) _host.hostMac));
                if (_node == null)
                {
                    _node = new JobNode();
                    _node.name = _host.hostName;
                    _node.macAddress = PhysicalAddress.Parse(_host.hostMac);
                    _node.ipAddress = _host.hostIP;

                    AddNode(_node);
                    _result.nodesAdded++;
                }
                else
                {
                    // check if node needs ip-update.
                    if (_node.ipAddress != _host.hostIP)
                        UpdateNodeIP(_node.id, _host.hostIP);

                    // check if node needs hostname-update
                    //if (_node.name != _host.hostName)
                    //    UpdateNodeIP(_node.id, _host.hostName);

                    _result.nodesUpdated++;
                }
            }
            return _result;
        }

        #region NO LOCKS USED IN HERE

        public JobNode UnsafeGetNode(int id)
        {
            foreach (JobNode _node in _nodes)
                if (_node.id == id)
                    return _node;
            return null;
        }

        #endregion

        #endregion

        #region node serialization

        public void SaveNode(string fileName, int nodeId)
        {
            nodesLock.EnterReadLock();
            JobNode _node = UnsafeGetNode(nodeId);
            if (_node != null)
            {
                JSSerializer.SerializeNode(fileName, _node);
                nodesLock.ExitReadLock();
            }
            else
            {
                nodesLock.ExitReadLock();
                throw new JobNodeException("Node does not exist!", null);
            }
        }

        public void LoadNode(string fileName)
        {
            JobNode _node = JSSerializer.DeserializeNode(fileName);

            nodesLock.EnterWriteLock();
            nodes.Add(_node);
            nodesLock.ExitWriteLock();
        }

        #endregion

        #region jobs handling

        public void StartJob(int id)
        {
            nodesLock.EnterReadLock();
            Job _job = null;
            foreach (JobNode _node in nodes)
            {
                _node.nodeLock.EnterReadLock();
            }


            Job _job = UnsafeGetJob(id);
            if (_job != null)
                if (_job.state == Job.JobState.Inactive)
                    lock (_job.jobLock)
                        _job.state = Job.JobState.Waiting;
                else
                    throw new JobException("Job already active!", null);
            else
                throw new JobException("Job does not exist!", null);
        }

        public void StopJob(int id)
        {
            Job _job = GetJob(id);
            if (_job != null)
                if (_job.state == Job.JobState.Waiting)
                    lock(_job.jobLock)
                        _job.state = Job.JobState.Inactive;
                else
                    throw new JobException("Job already active!", null);
            else
                throw new JobException("Job does not exist!", null);
        }

        public void AddJobToNode(int nodeId, Job job)
        {
            nodesLock.EnterReadLock();
            JobNode _node = UnsafeGetNode(nodeId);
            if (_node != null)
            {
                _node.nodeLock.EnterWriteLock();
                _node.jobsLock.EnterWriteLock();
                _node.jobs.Add(job);
                _node.jobsLock.ExitWriteLock();
                _node.nodeLock.ExitWriteLock();
                nodesLock.ExitReadLock();
            }
            else
            {
                nodesLock.ExitReadLock();
                throw new JobNodeException("Node does not exist!", null);
            }
        }

        public void RemoveJob(int id)
        {
            bool _success = false;
            foreach (JobNode _node in _nodes)
                for (int i = 0; i < _node.jobs.Count; i++)
                    if (_node.jobs[i].id == id)
                        lock (_node.nodeLock)
                        {
                            _node.jobs.RemoveAt(i);
                            _success = true;
                        }
            if(!_success)
                throw new JobException("Job does not exist!", null);
        }

        public bool JobExist(int id)
        {
            foreach (JobNode _node in _nodes)
                foreach (Job _temp in _node.jobs)
                    if (_temp.id == id)
                        return true;
            return false;
        }

        public Job UnsafeGetJob(JobNode node, int id)
        {
            foreach (Job job in node.jobs)
                if (job.id == id)
                    return job;
            return null;
        }

        #endregion

        #endregion
    }
}
