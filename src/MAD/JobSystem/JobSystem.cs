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
        private object _sceduleLock = new object();

        private ReaderWriterLockSlim _nodesLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
        private List<JobNode> _nodes = new List<JobNode>();

        public event EventHandler OnNodeCountChanged = null;
        public event EventHandler OnJobCountChanged = null;
        public event EventHandler OnShutdown = null;
        
        #endregion

        #region constructor

        public JobSystem()
        {
            _scedule = new JobScedule(_nodesLock, _nodes);
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
            _nodesLock.EnterWriteLock();
            try
            {
                JSSerializer.SerializeTable(filepath, _nodes);
            }
            catch (Exception e)
            {
                _nodesLock.ExitWriteLock();
                throw e;
            }
            _nodesLock.ExitWriteLock();
        }

        /// <summary>
        /// Load nodes from json-file.
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns>Path to file (Execption will be thrown if file does not exist!).</returns>
        public int LoadTable(string filepath)
        {
            List<JobNode> _loadNodes = JSSerializer.DeserializeTable(filepath);
            _nodesLock.EnterWriteLock();
            if (JobSystem.MAXNODES > _nodes.Count + _loadNodes.Count)
                _nodes.AddRange(_loadNodes);
            _nodesLock.ExitWriteLock();
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

            _nodesLock.EnterReadLock();
            for (int i = 0; i < _nodes.Count; i++)
            {
                _nodes[i].nodeLock.EnterReadLock();
                if (_nodes[i].state == JobNode.State.Active)
                    _count++;
                _nodes[i].nodeLock.ExitReadLock();
            }
            _nodesLock.ExitReadLock();

            return _count;
        }

        public int NodesInactive()
        {
            int _count = 0;

            _nodesLock.EnterReadLock();
            for (int i = 0; i < _nodes.Count; i++)
            {
                _nodes[i].nodeLock.EnterReadLock();
                if (_nodes[i].state == JobNode.State.Inactive)
                    _count++;
                _nodes[i].nodeLock.ExitReadLock();
            }
            _nodesLock.ExitReadLock();

            return _count;
        }

        public int NodesInitialized()
        {
            _nodesLock.EnterReadLock();
            int _count = _nodes.Count;
            _nodesLock.ExitReadLock();

            return _count;
        }

        public int JobsActive()
        {
            int _count = 0;

            _nodesLock.EnterReadLock();
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
            _nodesLock.ExitReadLock();

            return _count;
        }

        public int JobsInactive()
        {
            int _count = 0;

            _nodesLock.EnterReadLock();
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
            _nodesLock.ExitReadLock();

            return _count;
        }

        public int JobsInitialized()
        {
            int _count = 0;

            _nodesLock.EnterReadLock();
            for (int i = 0; i < _nodes.Count; i++)
            {
                _nodes[i].nodeLock.EnterReadLock();
                _count = _count + _nodes[i].jobs.Count;
                _nodes[i].nodeLock.ExitReadLock();
            }
            _nodesLock.ExitReadLock();

            return _count;
        }

        #endregion

        #region nodes handling

        public void StartNode(int id)
        {
            JobNode _node = GetNodeLocked(id);
            if (_node != null)
            {
                _node.nodeLock.EnterReadLock();
                if (_node.state == JobNode.State.Inactive)
                {
                    _node.nodeLock.ExitReadLock();
                    _node.nodeLock.EnterWriteLock();
                    _node.state = JobNode.State.Active;
                    _node.nodeLock.ExitWriteLock();
                    NodeUnlock();
                }
                else
                {
                    _node.nodeLock.ExitReadLock();
                    NodeUnlock();
                    throw new JobNodeException("Node already active or has an exception!", null);
                }
            }
            else
                throw new JobNodeException("Node does not exist!", null);
        }

        public void StopNode(int id)
        {
            JobNode _node = GetNodeLocked(id);
            if (_node != null)
            {
                _node.nodeLock.EnterReadLock();
                if (_node.state == JobNode.State.Active)
                {
                    _node.nodeLock.ExitReadLock();
                    _node.nodeLock.EnterWriteLock();
                    _node.state = JobNode.State.Inactive;
                    _node.nodeLock.ExitWriteLock();
                    NodeUnlock();
                }
                else
                {
                    _node.nodeLock.ExitReadLock();
                    NodeUnlock();
                    throw new JobNodeException("Node already inactive or has an exception!", null);
                }
            }
            else
                throw new JobNodeException("Node does not exist!", null);
        }

        public void AddNode(JobNode node)
        {
            _nodesLock.EnterWriteLock();
            if (MAXNODES > _nodes.Count)
            {
                _nodes.Add(node);
                _nodesLock.ExitWriteLock();

                if (OnNodeCountChanged != null)
                    OnNodeCountChanged.Invoke(null, null);
            }
            else
            {
                _nodesLock.ExitWriteLock();
                throw new JobSystemException("Node limit reached!", null);
            }
        }

        public void RemoveNode(int id)
        {
            bool _success = false;

            _nodesLock.EnterWriteLock();
            for (int i = 0; i < _nodes.Count; i++)
            {
                if (_nodes[i].id == id)
                {
                    _nodes.RemoveAt(i);
                    _nodesLock.ExitWriteLock();

                    if (OnNodeCountChanged != null)
                        OnNodeCountChanged.Invoke(null, null);

                    _success = true;
                    break;
                }
            }

            if (!_success)
            {
                _nodesLock.ExitWriteLock();
                throw new JobNodeException("Node does not exist!", null);
            }
        }

        public void RemoveAllNodes()
        {
            _nodesLock.EnterWriteLock();
            _nodes.Clear();
            _nodesLock.ExitWriteLock();

            if(OnNodeCountChanged != null)
                OnNodeCountChanged.Invoke(null, null);
        }

        public bool NodeExist(int id)
        {
            JobNode _node = GetNodeLocked(id);
            if (_node != null)
            {
                NodeUnlock();
                return true;
            }
            return false;
        }

        public bool UpdateNodeName(int nodeID, string name)
        {
            JobNode _node = GetNodeLocked(nodeID);
            if (_node != null)
            {
                _node.nodeLock.EnterWriteLock();
                _node.name = name;
                _node.nodeLock.ExitWriteLock();
                _nodesLock.ExitReadLock();
                return true;
            }
            else
                return false;
        }

        public bool UpdateNodeMac(int nodeID, PhysicalAddress mac)
        {
            JobNode _node = GetNodeLocked(nodeID);
            if (_node != null)
            {
                _node.nodeLock.EnterWriteLock();
                _node.macAddress = mac;
                _node.nodeLock.ExitWriteLock();
                _nodesLock.ExitReadLock();
                return true;
            }
            else
                return false;
        }

        public bool UpdateNodeIP(int nodeID, IPAddress ip)
        {
            JobNode _node = GetNodeLocked(nodeID);
            if (_node != null)
            {
                _node.nodeLock.EnterWriteLock();
                _node.ipAddress = ip;
                _node.nodeLock.ExitWriteLock();
                _nodesLock.ExitReadLock();
                return true;
            }
            else
                return false;
        }

        // not finished yet
        public SyncResult SyncNodes(List<ModelHost> currentHosts)
        {
            SyncResult _result = new SyncResult();

            _nodesLock.EnterWriteLock();
            for (int i = 0; i < currentHosts.Count; i++)
            { 
                
            
            
            }
            _nodesLock.ExitWriteLock();

            return _result;
        }

        #endregion

        #region node serialization

        public void SaveNode(string fileName, int id)
        {
            JobNode _node = GetNodeLocked(id);
            if (_node != null)
            {
                NodeLockRead(_node);
                JSSerializer.SerializeNode(fileName, _node);
                NodeUnlockRead(_node);
                NodeUnlock();
            }
            else
                throw new JobNodeException("Node does not exist!", null);
        }

        public void LoadNode(string fileName)
        {
            JobNode _node = JSSerializer.DeserializeNode(fileName);

            _nodesLock.EnterWriteLock();
            _nodes.Add(_node);
            _nodesLock.ExitWriteLock();
        }

        #endregion

        #region jobs handling

        public void StartJob(int id)
        {
            JobNode node;
            Job _job = GetJobLockedGlobal(id, out node);
            if (_job != null)
            {
                JobLockWrite(_job);
                if (_job.state == Job.JobState.Inactive)
                {
                    _job.state = Job.JobState.Waiting;
                    JobUnlockWrite(_job);
                    JobUnlockedGlobal(node);
                }
                else
                {
                    JobUnlockWrite(_job);
                    JobUnlockedGlobal(node);
                    throw new JobException("Job already active!", null);
                }
            }
            else
                throw new JobException("Job does not exist!", null);
        }

        public void StopJob(int id)
        {
            JobNode node;
            Job _job = GetJobLockedGlobal(id, out node);
            if (_job != null)
            {
                JobLockWrite(_job);
                if (_job.state == Job.JobState.Waiting)
                {
                    _job.state = Job.JobState.Inactive;
                    JobUnlockWrite(_job);
                    JobUnlockedGlobal(node);
                }
                else
                {
                    JobUnlockWrite(_job);
                    JobUnlockedGlobal(node);
                    throw new JobException("Job already inactive!", null);
                }
            }
            else
                throw new JobException("Job does not exist!", null);
        }

        public void AddJobToNode(int nodeId, Job job)
        {
            JobNode _node = GetNodeLocked(nodeId);
            if (_node != null)
            {
                _node.nodeLock.EnterWriteLock();

                if (JobNode.MAXJOBS > _node.jobs.Count)
                {
                    _node.jobs.Add(job);

                    _node.nodeLock.ExitWriteLock();
                    NodeUnlock();
                }
                else
                {
                    _node.nodeLock.ExitWriteLock();
                    NodeUnlock();

                    throw new JobSystemException("Jobs limit reached!", null);
                }
            }
            else
            {
                throw new JobNodeException("Node does not exist!", null);
            }
        }

        public void RemoveJob(int id)
        {
            _nodesLock.EnterReadLock();
            foreach (JobNode _node in _nodes)
            {
                _node.nodeLock.EnterWriteLock();
                for(int i = 0; i < _node.jobs.Count; i++)
                {
                    if (_node.jobs[i].id == id)
                    {
                        _node.jobs.RemoveAt(i);
                        break;
                    }
                }
                _node.nodeLock.ExitWriteLock();
            }
            _nodesLock.ExitReadLock();
        }

        public bool JobExist(int id)
        {
            JobNode _node;
            Job _job = GetJobLockedGlobal(id, out _node);
            if (_job != null)
            {
                JobUnlockedGlobal(_node);
                return true;
            }
            else
                return false;
        }

        #endregion

        #region write/read or read-only access to JobNodes and Jobs

        #region Nodes reference handling (with locks)

        /// <summary>
        /// Returns a locked read-only reference of all nodes.
        /// </summary>
        /// <returns>A list of nodes (DO NOT ADD OR REMOVE ANY ITEMS OF THIS LIST -> READ-ONLY).</returns>
        public List<JobNode> GetNodesLockedRead()
        {
            _nodesLock.EnterReadLock();
            return _nodes;
        }

        /// <summary>
        /// Unlocks a read-only list of nodes.
        /// </summary>
        public void UnlockNodesRead()
        {
            _nodesLock.ExitReadLock();
        }

        /// <summary>
        /// Retruns a locked read/write reference of all nodes (BEWARE: After the execution of this method the whole JobSystem is blocked!
        /// No jobs can be executed, no information can be read, nothing! Use this method only if you really need it!).
        /// </summary>
        /// <returns>A list of nodes.</returns>
        public List<JobNode> GetNodesLockedWrite()
        {
            _nodesLock.EnterWriteLock();
            return _nodes;
        }

        /// <summary>
        /// Unlocks a read/write list of nodes.
        /// </summary>
        public void UnlockNodesWrite()
        {
            _nodesLock.ExitWriteLock();
        }

        #endregion

        #region Node reference handling (with locks)

        /// <summary>
        /// Returns a locked node reference.
        /// </summary>
        /// <param name="id">Id of the node.</param>
        /// <returns>If it returns null, the node does not exist (no locks need to be unlocked).
        /// If it returns a reference, it is locked and needs to get unlocked after usage (NodeUnlock(..)).</returns>
        public JobNode GetNodeLocked(int id)
        {
            _nodesLock.EnterReadLock();
            foreach (JobNode node in _nodes)
            {
                node.nodeLock.EnterReadLock();
                if (node.id == id)
                {
                    node.nodeLock.ExitReadLock();
                    return node;
                }
                else
                    node.nodeLock.ExitReadLock();
            }
            _nodesLock.ExitReadLock();
            return null;
        }

        /// <summary>
        /// Unlocks a locked node reference.
        /// </summary>
        public void NodeUnlock()
        {
            _nodesLock.ExitReadLock();
        }

        /// <summary>
        /// Locks a node reference for read-only.
        /// </summary>
        /// <param name="node">Reference of the node.</param>
        public void NodeLockRead(JobNode node)
        {
            node.nodeLock.EnterReadLock();
        }

        /// <summary>
        /// Unlocks a node reference for read-only.
        /// </summary>
        /// <param name="node">Reference of the node.</param>
        public void NodeUnlockRead(JobNode node)
        {
            node.nodeLock.ExitReadLock();
        }

        /// <summary>
        /// Locks a node reference for read/write.
        /// </summary>
        /// <param name="node">Reference of the node.</param>
        public void NodeLockWrite(JobNode node)
        {
            node.nodeLock.EnterWriteLock();
        }

        /// <summary>
        /// Unlocks a node reference for read/write.
        /// </summary>
        /// <param name="node">Reference of the node.</param>
        public void NodeUnlockWrite(JobNode node)
        {
            node.nodeLock.ExitWriteLock();
        }

        #endregion

        #region Job reference handling (with locks)

        /// <summary>
        /// Returns a locked job reference.
        /// </summary>
        /// <param name="node">Reference of the node.</param>
        /// <param name="id">Id of the job.</param>
        /// <returns>If it returns null, the job does not exits (no locks need to be unlocked).
        /// If it returns a reference, it is locked and need to be unlocked after usage (JobUnlock(..)).</returns>
        public Job GetJobLocked(JobNode node, int id)
        {
            node.nodeLock.EnterReadLock();
            foreach (Job job in node.jobs)
            {
                job.jobLock.EnterReadLock();
                if (job.id == id)
                {
                    job.jobLock.ExitReadLock();
                    return job; // lock remains open!
                }
                else
                    job.jobLock.ExitReadLock();
            }
            node.nodeLock.ExitReadLock();
            return null;
        }

        /// <summary>
        /// Unlocks a job reference.
        /// </summary>
        /// <param name="node">Reference of the node.</param>
        public void JobUnlock(JobNode node)
        {
            node.nodeLock.ExitReadLock();
        }

        /// <summary>
        /// Returns a job reference.
        /// </summary>
        /// <param name="id">Id of the job.</param>
        /// <param name="node">Reference to a node.</param>
        /// <returns>If it returns null and the node reference is null, job does not exits (no locks need to be unlocked).
        /// If it returns a reference, the job needs to be unlock after usage (JobUnlockGlobal(..)).</returns>
        public Job GetJobLockedGlobal(int id, out JobNode node)
        {
            _nodesLock.EnterReadLock();
            foreach (JobNode _node in _nodes)
            {
                _node.nodeLock.EnterReadLock();
                foreach (Job _job in _node.jobs)
                {
                    _job.jobLock.EnterReadLock();
                    if (_job.id == id)
                    {
                        _job.jobLock.ExitReadLock();
                        node = _node;
                        return _job;
                    }
                    _job.jobLock.ExitReadLock();
                }
                _node.nodeLock.ExitReadLock();
            }
            _nodesLock.ExitReadLock();
            node = null;
            return null;
        }

        /// <summary>
        /// Unlocks a job reference.
        /// </summary>
        /// <param name="node">Reference of the node.</param>
        public void JobUnlockedGlobal(JobNode node)
        {
            node.nodeLock.ExitReadLock();
            _nodesLock.ExitReadLock();
        }

        /// <summary>
        /// Locks a job for read-only.
        /// </summary>
        /// <param name="job">Reference of the job.</param>
        public void JobLockRead(Job job)
        {
            job.jobLock.EnterReadLock();
        }

        /// <summary>
        /// Unlocks a job for read-only.
        /// </summary>
        /// <param name="job">Reference of the job.</param>
        public void JobUnlockRead(Job job)
        {
            job.jobLock.ExitReadLock();
        }

        /// <summary>
        /// Locks a job for read/write.
        /// </summary>
        /// <param name="job">Reference of the job.</param>
        public void JobLockWrite(Job job)
        {
            job.jobLock.EnterWriteLock();
        }

        /// <summary>
        /// Unlocks a job for read/write.
        /// </summary>
        /// <param name="job">Reference of the job.</param>
        public void JobUnlockWrite(Job job)
        {
            job.jobLock.ExitWriteLock();
        }

        #endregion

        #endregion

        #endregion
    }
}
