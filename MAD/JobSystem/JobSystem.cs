using System;
using System.Collections.Generic;

namespace MAD.JobSystemCore
{
    public class JobSystem
    {
        #region members

        public readonly Version VERSION = new Version(2, 9);

        private JobScedule _scedule;
        public JobScedule.State sceduleState { get { return _scedule.state; } }

        private List<JobNode> _nodes = new List<JobNode>();
        public const int MAX_NODES = 100;
        public int nodesInitialized { get { return _nodes.Count; } }
        private object _nodesLock = new object();

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

        public bool SceduleActive()
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

        public int JobsInitialized()
        {
            int _count = 0;
            for (int i = 0; i < _nodes.Count; i++)
                _count = _count + _nodes[i].jobs.Count;
            return _count;
        }

        /// <returns>(IMPORANT: only use this reference as READ-ONLY)</returns>
        public List<JobNode> GetNodes()
        {
            return _nodes;
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
            if (MAX_NODES > _nodes.Count)
                lock (_nodesLock)
                    _nodes.Add(node);
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
                    _success = true;
                }
            if(!_success)
                throw new JobNodeException("Node does not exist!", null);
        }

        // TODO
        /// <returns>Count of removed nodes.</returns>
        public int RemoveNodeFromIDToID(int fromID, int toID)
        {
            return 0;
        }

        public void RemoveAllNodes()
        {
            lock (_nodesLock)
                _nodes.Clear();
        }

        /// <returns>(IMPORANT: only use this reference as READ-ONLY)</returns>
        public JobNode GetNode(int id)
        {
            foreach (JobNode _node in _nodes)
                if (_node.id == id)
                    return _node;
            return null;
        }

        public void UpdateNodeIP(int nodeID, System.Net.IPAddress ip)
        {
            JobNode _node = GetNode(nodeID);
            if (_node != null)
                lock (_node.nodeLock)
                    _node.ipAddress = ip;
            else
                throw new JobNodeException("Node does not exist!", null);
        }

        public bool NodeExist(int id)
        {
            JobNode _node = GetNode(id);
            if (_node != null)
                return true;
            else
                return false;
        }

        #endregion

        #region nodes serialization

        public void SaveNode(string fileName, int nodeId)
        {
            JobNode _node = GetNode(nodeId);
            if(_node != null)
                JSSerializer.Serialize(fileName, _node);
            else
                throw new JobNodeException("Node does not exist!", null);
        }

        public JobNode LoadNode(string fileName)
        {
            JobNode _buffer = (JobNode)JSSerializer.Deserialize(fileName, typeof(JobNode));
            return _buffer;
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
