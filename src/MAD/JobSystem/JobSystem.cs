using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;

using MAD.MacFinders;
using MAD.Logging;
using MAD.Database;

using Newtonsoft.Json;

namespace MAD.JobSystemCore
{
    public class JobSystem
    {
        #region members

        public const string VERSION = "v3.0.4.0";
        public const int MAXNODES = 100;

        private JobSchedule _Schedule { get; set; }
        private object _ScheduleLock = new object();

        public object jsLock = new object();
        private List<JobNode> _nodes = new List<JobNode>();

        public event EventHandler OnScheduleStateChange = null;
        public event EventHandler OnNodeCountChange = null;
        public event EventHandler OnJobCountChange = null;
        public event EventHandler OnJobStatusChange = null;
        public event EventHandler OnShutdown = null;

        //private DBHandler _handler;

        private DB _db;

        #endregion

        #region constructor

        public JobSystem(DB db)
        {
            _Schedule = new JobSchedule(jsLock, _nodes, db);
            _db = db;
        }

        #endregion

        #region methods

        #region jobsystem handling

        public void SaveTable(string filepath)
        {
            lock (jsLock)
            {
                try
                {
                    JSSerializer.SerializeTable(filepath, _nodes);
                }
                catch (Exception e)
                {
                    Logger.Log("(JS) Error while saving table: " + e.Message, Logger.MessageType.ERROR);
                    throw new Exception("Error while saving table.", e);
                }
            }
        }

        public int LoadTable(string filepath)
        {
            List<JobNode> _loadNodes;
            try
            {
                _loadNodes = JSSerializer.DeserializeTable(filepath);
            }
            catch (Exception e)
            {
                Logger.Log("(JS) Error while loading table: " + e.Message, Logger.MessageType.ERROR);
                throw new Exception("Error while loading table.", e);
            }

            lock (jsLock)
            {
                if (JobSystem.MAXNODES > _nodes.Count + _loadNodes.Count)
                {
                    _nodes.AddRange(_loadNodes);
                }
                else
                    throw new JobSystemException("Nodes limit reached!", null);
            }
            return _nodes.Count;
        }

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
                    return "";
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
                    return "";
            }
        }

        public string ResultMessage(int result)
        {
            switch (result)
            { 
                case 1:
                    return "JobSystem not available.";
                case 2:
                    return "JobSystem intern error.";
                case 3:
                    return "Nodes limit reached.";
                case 4:
                    return "Jobs limit reached.";

                case 50:
                    return "Node does not exist.";
                case 51:
                    return "Node already running.";

                case 70:
                    return "Job does not exist.";
                case 71:
                    return "Job already running.";
                case 72:
                    return "Job already stopped.";


                default:
                    return "";
            }
        }

        public List<JobNodeInfo> GetNodesAndJobs()
        {
            List<JobNodeInfo> nodes = new List<JobNodeInfo>();

            lock (jsLock)
            {
                string[] buffer;
                foreach (JobNode node in _nodes)
                {
                    List<JobInfo> jobs = new List<JobInfo>();
                    
                    foreach(Job job in node.jobs)
                    {
                        jobs.Add(new JobInfo(job.id, job.guid, job.name, job.type.ToString(), job.outp.outState.ToString()));
                    }

                    buffer = _db.GetMemoFromNode(node.guid.ToString());

                    nodes.Add(new JobNodeInfo(node.id, node.guid, node.name, node.ip, node.mac.ToString(), node.state, buffer[0], buffer[1], jobs));
                }
            }
            
            return nodes;
        }

        #endregion

        #region nodes handling

        public List<JobNode> LGetNodes()
        {
            return _nodes;
        }

        public JobNode LGetNode(int id)
        {
            for (int i = 0; i < _nodes.Count; i++)
                if (_nodes[i].id == id)
                    return _nodes[i];
            return null;
        }

        public JobNode LGetNode(string mac)
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
                JobNode _node = LGetNode(id);
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
                JobNode _node = LGetNode(id);
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
                {
                    _nodes.Add(node);
                }
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
                JobNode _node = LGetNode(id);
                if (_node != null)
                    return true;
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
                        _node = LGetNode(_host.hostMac);
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
                            JobNode _newNode = new JobNode(_host.hostName, _host.hostMac, _host.hostIP, new List<Job>());
                            _nodes.Add(_newNode);
                            _result.nodesAdded++;
                        }
                    }
                    catch(Exception e)
                    {
                        Logger.Log("(JS) Sync-error: " + e.Message, Logger.MessageType.ERROR);
                    }
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
                JobNode _node = LGetNode(id);
                if (_node != null)
                    JSSerializer.SerializeNode(fileName, _node);
                else
                    throw new JobNodeException("Node does not exist!", null);
            }
        }

        public void LoadNode(string fileName)
        {
            JobNode _node = JSSerializer.DeserializeNode(fileName);

            lock (jsLock)
            {
                if (MAXNODES > _nodes.Count)
                {
                    _nodes.Add(_node);
                }
                else
                    throw new JobSystemException("Nodes limit reached!", null);
            }
        }

        #endregion

        #region jobs handling

        public Job LGetJob(int id, out JobNode node)
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
                JobNode _node = null;
                Job _job = LGetJob(id, out _node);
                if (_job != null)
                {
                    if (_job.state == 0)
                        _job.state = 1;
                    else
                        throw new JobException("Job (ID='" + _job.id + "') already active.",null);
                }
                else
                    throw new JobException("Job (ID='" + _job.id + "') does not exist.", null);
            }

            if (OnJobStatusChange != null)
                OnJobStatusChange.Invoke(null, null);
        }

        public void StopJob(int id)
        {
            lock (jsLock)
            {
                JobNode _node;
                Job _job = LGetJob(id, out _node);
                if (_job != null)
                {
                    if (_job.state == 1)
                        _job.state = 0;
                    else
                        throw new JobException("Job (ID='" + _job.id + "') already inactive.", null);
                }
                else
                    throw new JobException("Job (ID='" + _job.id + "') does not exist.", null);
            }

            if (OnJobStatusChange != null)
                OnJobStatusChange.Invoke(null, null);
        }

        public void AddJobToNode(int nodeId, Job job)
        {
            lock (jsLock)
            {
                JobNode _node = LGetNode(nodeId);
                if (_node != null)
                {
                    if (JobNode.MAXJOBS > _node.jobs.Count)
                        _node.jobs.Add(job);
                    else
                        throw new JobSystemException("Job limit reached.", null);
                }
                else
                    throw new JobNodeException("Node (ID='" + _node.id + "') does not exist.", null);
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
                if (_success != false)
                    throw new JobException("Job (ID='" + id + "') does not exist.", null);
            }
        }

        public bool JobExist(int id)
        {
            lock (jsLock)
            {
                JobNode _node;
                Job _job = LGetJob(id, out  _node);
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
