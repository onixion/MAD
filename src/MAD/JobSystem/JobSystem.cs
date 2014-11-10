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

        public const string VERSION = "v2.9.0.3";
        public const int MAXNODES = 100;

        private JobSchedule _Schedule { get; set; }
        private object _ScheduleLock = new object();

        public object jsLock = new object();
        private List<JobNode> _nodes = new List<JobNode>();

        public event EventHandler OnScheduleStateChange = null;
        public event EventHandler OnNodeCountChange = null;
        public event EventHandler OnNodeStatusChange = null;
        public event EventHandler OnJobCountChange = null;
        public event EventHandler OnJobStatusChange = null;
        public event EventHandler OnShutdown = null;
        
        #endregion

        #region constructor

        public JobSystem()
        {
            _Schedule = new JobSchedule(jsLock, _nodes);
        }

        #endregion

        #region methods

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
                else
                    throw new JobSystemException("Nodes limit reached!", null);
            }
            return _nodes.Count;
        }

        /// <summary>
        /// This method will clean-up all resources used by the JobSystem.
        /// </summary>
        public void Shutdown()
        {
            _Schedule.Stop();

            if (OnShutdown != null)
                OnShutdown.Invoke(null, null);
        }

        #endregion

        #region Schedule handling

        public void StartSchedule()
        {
            lock(_ScheduleLock)
                _Schedule.Start();

            if (OnScheduleStateChange != null)
                OnScheduleStateChange.Invoke(null, null);
        }

        public void StopSchedule()
        {
            lock(_ScheduleLock)
                _Schedule.Stop();

            if (OnScheduleStateChange != null)
                OnScheduleStateChange.Invoke(null, null);
        }

        public bool IsScheduleActive()
        {
            lock (_ScheduleLock)
            { 
                if(_Schedule.state == 1)
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
                for (int i = 0; i < _nodes.Count; i++)
                    if (_nodes[i].state == 1)
                        _count++;
            return _count;
        }

        public int NodesInactive()
        {
            int _count = 0;
            lock (jsLock)
                for (int i = 0; i < _nodes.Count; i++)
                    if (_nodes[i].state == 0)
                        _count++;
            return _count;
        }

        public int NodesInitialized()
        {
            int _count = 0;
            lock(jsLock)
                _count = _nodes.Count;
            return _count;
        }

        public string NodeState(int state)
        {
            switch (state)
            {
                case 0:
                    return "Inactive";
                case 1:
                    return "Active";
                default:
                    throw new Exception("NOT-VALID-NODESTATE");
            }
        }

        public int JobsStopped()
        {
            int _count = 0;
            lock (jsLock)
                for (int i = 0; i < _nodes.Count; i++)
                    foreach (Job _job in _nodes[i].jobs)
                        if (_job.state == 0)
                            _count++;
            return _count;
        }

        public int JobsWaiting()
        {
            int _count = 0;
            lock (jsLock)
                for (int i = 0; i < _nodes.Count; i++)
                    foreach (Job _job in _nodes[i].jobs)
                        if (_job.state == 1)
                            _count++;
            return _count;
        }

        public int JobsWorking()
        {
            int _count = 0;
            lock (jsLock)
                for (int i = 0; i < _nodes.Count; i++)
                    foreach (Job _job in _nodes[i].jobs)
                        if (_job.state == 2)
                            _count++;
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

        public string JobState(int state)
        {
            switch (state)
            { 
                case 0:
                    return "Stopped";
                case 1:
                    return "Waiting";
                case 2:
                    return "Working";
                case 3:
                    return "Exception";
                default:
                    throw new Exception("NOT-VALID-JOBSTATE");
            }
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

        public JobNode UnsafeGetNode(string mac)
        {
            for (int i = 0; i < _nodes.Count; i++)
                if (_nodes[i].mac.ToString() == mac)
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
                    if (_node.state == 0)
                        _node.state = 1;
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
                    if (_node.state == 1)
                        _node.state = 0;
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
                    _nodes.Add(node);
                else
                    throw new JobSystemException("Nodes limit reached!", null);
            }

            if (OnNodeCountChange != null)
                OnNodeCountChange.Invoke(null, null);
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
                        if (_nodes[i].uWorker == 0)
                            _nodes.RemoveAt(i);
                        else
                            throw new JobNodeException("Node busy (" + _nodes[i].uWorker + " Jobs working).", null);
                        _success = true;
                        break;
                    }
                }
            }
            if (!_success)
                throw new JobNodeException("Node does not exist!", null);

            if (OnNodeCountChange != null)
                OnNodeCountChange.Invoke(null, null);
        }

        public int RemoveAllNodes()
        {
            int _removedNodes = 0;
            lock (jsLock)
            {
                for (int i = 0; i < _nodes.Count; i++)
                    if (_nodes[i].uWorker == 0)
                    {
                        _nodes.RemoveAt(i);
                        _removedNodes++;
                    }
            }

            if (OnNodeCountChange != null)
                OnNodeCountChange.Invoke(null, null);

            return _removedNodes;
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
                    _node.mac = mac;
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
                    _node.ip = ip;
                    return true;
                }
                else
                    return false;
            }
        }

        public SyncResult SyncNodes(List<ModelHost> currentHosts)
        {
            SyncResult _result = new SyncResult();
            lock (jsLock)
            {
                foreach (ModelHost _host in currentHosts)
                {
                    JobNode _node = null;
                    try
                    {
                        _node = UnsafeGetNode(_host.hostMac);
                        if (_node != null)
                        {
                            // Node does exist with this mac -> update IP and HOST
                            _node.name = _host.hostName;
                            _node.ip = _host.hostIP;
                            _result.nodesUpdated++;
                        }
                        else
                        {
                            // Node with this mac does not exist -> make new node.
                            JobNode _newNode = new JobNode(_host.hostName, PhysicalAddress.Parse(_host.hostMac), _host.hostIP, new List<Job>());
                            _nodes.Add(_newNode);
                            _result.nodesAdded++;
                        }
                    }
                    catch(Exception)
                    { }
                }
            }
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
                        if (_job.state == 0)
                            _job.state = 1;
                        else
                            throw new JobException("Job already active!", null);
                }
                else
                    throw new JobException("Job does not exist!", null);
            }

            if (OnJobStatusChange != null)
                OnJobStatusChange.Invoke(null, null);
        }

        public void StopJob(int id)
        {
            lock (jsLock)
            {
                JobNode _node;
                Job _job = UnsafeGetJob(id, out _node);
                if (_job != null)
                {
                    if (_job.state == 1)
                        _job.state = 0;
                    else
                        throw new JobException("Job already inactive!", null);
                }
                else
                    throw new JobException("Job does not exist!", null);
            }

            if (OnJobStatusChange != null)
                OnJobStatusChange.Invoke(null, null);
        }

        public void AddJobToNode(int nodeId, Job job)
        {
            lock (jsLock)
            {
                JobNode _node = UnsafeGetNode(nodeId);
                if (_node != null)
                {
                    if (JobNode.MAXJOBS > _node.jobs.Count)
                        _node.jobs.Add(job);
                    else
                        throw new JobSystemException("Jobs limit reached!", null);
                }
                else
                    throw new JobNodeException("Node does not exist!", null);
            }

            if (OnJobCountChange != null)
                OnJobCountChange.Invoke(null, null);
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
                            if (_node.jobs[i].state == 0 || _node.jobs[i].state == 1)
                            {
                                _node.jobs.RemoveAt(i);
                                _success = true;
                                break;
                            }
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

        #region CLI-only

        public string GetJSStats()
        {
            string _output = "";
            _output += "<color><yellow> JobSystem (VER " + JobSystem.VERSION + ")\n";
            _output += "<color><yellow> ├─> MAX-NODES: <color><white>" + JobSystem.MAXNODES + "\n";
            _output += "<color><yellow> ├─> MAX-JOBS:  <color><white>" + JobNode.MAXJOBS + "\n";
            _output += "<color><yellow> ├─> NODES:     <color><white>" + NodesInitialized() + "\n";
            _output += "<color><yellow> └─> JOBS:      <color><white>" + JobsInitialized() + "\n";
            return _output;
        }

        public string GetJSScheduleStats()
        {
            string _output = "";
            _output += "<color><yellow> JobSchedule\n";
            _output += "<color><yellow> └─> STATE: ";
            
            if(IsScheduleActive())
                _output += "<color><green>Active";
            else
                _output += "<color><red>Inactive";

            _output += "\n";
            return _output;
        }

        public string GetNodesStats()
        {
            string _output = "";
            _output += "<color><yellow> Nodes\n";
            _output += "<color><yellow> ├─> INACTIVE: <color><red>" + NodesInactive() + "\n";
            _output += "<color><yellow> └─> ACTIVE:   <color><green>" + NodesActive() + "\n";
            return _output;
        }

        public string GetJobsStats()
        {
            string _output = "";
            _output += "<color><yellow> Jobs\n";
            _output += "<color><yellow> ├─> STOPPED: <color><red>" + JobsStopped() + "\n";
            _output += "<color><yellow> ├─> WAITING: <color><green>" + JobsWaiting() + "\n";
            _output += "<color><yellow> └─> WORKING: <color><green>" + JobsWorking() + "\n";
            return _output;
        }

        #endregion

        #endregion
    }
}
