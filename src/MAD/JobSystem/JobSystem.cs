using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;

using MAD.Helper;

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

        private object _nodesLock = new object();

        public event EventHandler OnNodeCountChanged = null;
        public event EventHandler OnNodeAdded = null;
        public event EventHandler OnNodeRemoved = null;
        
        #endregion

        #region constructor

        public JobSystem()
        {
            _scedule = new JobScedule(_nodesLock, _nodes);
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
            for (int i = 0; i < _nodes.Count; i++)
                if (_nodes[i].state == JobNode.State.Active)
                    _count++;
            return _count;
        }

        public int NodesInactive()
        {
            int _count = 0;
            for (int i = 0; i < _nodes.Count; i++)
                if (_nodes[i].state == JobNode.State.Inactive)
                    _count++;
            return _count;
        }

        public int NodesInitialized()
        {
            return _nodes.Count;
        }

        public int JobsActive()
        {
            int _count = 0;
            for (int i = 0; i < _nodes.Count; i++)
                foreach (Job _job in _nodes[i].jobs)
                    if (_job.state == Job.JobState.Waiting)
                        _count++;
            return _count;
        }

        public int JobsInactive()
        {
            int _count = 0;
            for (int i = 0; i < _nodes.Count; i++)
                foreach (Job _job in _nodes[i].jobs)
                    if (_job.state == Job.JobState.Inactive)
                        _count++;
            return _count;
        }

        public int JobsInitialized()
        {
            int _count = 0;
            for (int i = 0; i < _nodes.Count; i++)
                _count = _count + _nodes[i].jobs.Count;
            return _count;
        }

        #endregion

        #region nodes handling

        public void StartNode(int id)
        {
            JobNode _node = GetNode(id);
            if (_node != null)
                lock (_node.nodeLock)
                    _node.state = JobNode.State.Active;
            else
                throw new JobNodeException("Node already active!", null);
        }

        public void StopNode(int id)
        {
            JobNode _node = GetNode(id);
            if (_node != null)
                lock (_node.nodeLock)
                    _node.state = JobNode.State.Inactive;
            else
                throw new JobNodeException("Node already inactive!", null);
        }

        public void AddNode(JobNode node)
        {
            if (MAXNODES > _nodes.Count)
            {
                lock (_nodesLock)
                    _nodes.Add(node);

                if(OnNodeCountChanged != null)
                    OnNodeCountChanged.Invoke(node, null);
                if(OnNodeAdded != null)
                    OnNodeAdded.Invoke(node, null);
            }
            else
                throw new JobSystemException("Node limit reached!", null);
        }

        public void RemoveNode(int id)
        {
            bool _success = false;
            for (int i = 0; i < _nodes.Count; i++)
                if (_nodes[i].id == id)
                {
                    lock (_nodesLock)
                        _nodes.RemoveAt(i);

                    if(OnNodeCountChanged != null)
                        OnNodeCountChanged.Invoke(null, null);
                    if(OnNodeRemoved != null)
                        OnNodeRemoved.Invoke(null, null);

                    _success = true;
                }
            if(!_success)
                throw new JobNodeException("Node does not exist!", null);
        }

        public void RemoveAllNodes()
        {
            lock (_nodesLock)
                _nodes.Clear();

            if(OnNodeCountChanged != null)
                OnNodeCountChanged.Invoke(null, null);
            if(OnNodeRemoved != null)
                OnNodeRemoved.Invoke(null, null);
        }

        /// <returns>Count of removed nodes.</returns>
        public int RemoveNodeFromIDToID(int fromID, int toID)
        {
            int _count = 0;
            JobNode _node;
            for (int i = fromID; i < toID; i++)
            {
                _node = GetNode(i);
                if (_node != null)
                {
                    _count++;
                    RemoveNode(i);
                }
            }
            return _count;
        }

        public void DuplicateNode(int id)
        {
        }

        public bool NodeExist(int id)
        {
            JobNode _node = GetNode(id);
            if (_node != null)
                return true;
            else
                return false;
        }

        public JobNode GetNode(int id)
        {
            foreach (JobNode _node in _nodes)
                if (_node.id == id)
                    return _node;
            return null;
        }

        public JobNode GetNode(string name)
        {
            foreach (JobNode _node in _nodes)
                if (_node.name == name)
                    return _node;
            return null;
        }

        public JobNode GetNode(PhysicalAddress mac)
        {
            foreach (JobNode _node in _nodes)
                if (_node.macAddress == mac)
                    return _node;
            return null;
        }

        public JobNode GetNode(IPAddress ip)
        {
            foreach (JobNode _node in _nodes)
                if (_node.ipAddress == ip)
                    return _node;
            return null;
        }

        public void UpdateNodeName(int nodeID, string name)
        {
            JobNode _node = GetNode(nodeID);
            if (_node != null)
                lock (_node.nodeLock)
                    _node.name = name;
            else
                throw new JobNodeException("Node does not exist!", null);
        }

        public void UpdateNodeMac(int nodeID, PhysicalAddress mac)
        {
            JobNode _node = GetNode(nodeID);
            if (_node != null)
                lock (_node.nodeLock)
                    _node.macAddress = mac;
            else
                throw new JobNodeException("Node does not exist!", null);
        }

        public void UpdateNodeIP(int nodeID, IPAddress ip)
        {
            JobNode _node = GetNode(nodeID);
            if (_node != null)
                lock (_node.nodeLock)
                    _node.ipAddress = ip;
            else
                throw new JobNodeException("Node does not exist!", null);
        }

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

        #endregion

        #region node serialization

        public void SaveNode(string fileName, int nodeId)
        {
            JobNode _node = GetNode(nodeId);
            if (_node != null)
                JSSerializer.SerializeNode(fileName, _node);
            else
                throw new JobNodeException("Node does not exist!", null);
        }

        public JobNode LoadNode(string fileName)
        {
            return JSSerializer.DeserializeNode(fileName);
        }

        #endregion

        #region jobs handling

        public void StartJob(int id)
        {
            Job _job = GetJob(id);
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
            JobNode _node = GetNode(nodeId);
            if (_node != null)
                lock(_node.nodeLock)
                    _node.jobs.Add(job);
            else
                throw new JobNodeException("Node does not exist!", null);
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

        public Job GetJob(int id)
        {
            foreach (JobNode _node in _nodes)
                foreach (Job _job in _node.jobs)
                    if (_job.id == id)
                        return _job;
            return null;
        }

        #endregion

        #endregion
    }
}
