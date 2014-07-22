using System;
using System.Collections.Generic;

namespace MAD.JobSystemCore
{
    public class JobSystem
    {
        #region members

        private Version _version = new Version(2, 6);
        public Version version { get { return _version; } }

        private JobScedule _scedule;
        public JobScedule.State sceduleState { get { return _scedule.state; } }

        private List<JobNode> _nodes = new List<JobNode>();
        public const int maxNodes = 100;
        public int nodesInitialized { get { return _nodes.Count; } }

        private object _jsLock = new object();

        #endregion

        #region constructor

        public JobSystem()
        {
            _scedule = new JobScedule(_nodes, _jsLock);
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
            lock (_jsLock)
                for (int i = 0; i < _nodes.Count; i++)
                    if (_nodes[i].state == JobNode.State.Active)
                        _count++;
            return _count;
        }

        public int NodesInactive()
        {
            int _count = 0;
            lock (_jsLock)
                for (int i = 0; i < _nodes.Count; i++)
                    if (_nodes[i].state == JobNode.State.Inactive)
                        _count++;
            return _count;
        }

        public int JobsInitialized()
        {
            int _count = 0;
            lock (_jsLock)
                for (int i = 0; i < _nodes.Count; i++)
                    _count = _count + _nodes[i].jobs.Count;
            return _count;
        }

        /// <summary>
        /// This method is ONLY used to get all informations of the nodes
        /// and jobs. DO NOT USE IT TO CHANGE NODES OR JOBS!
        /// </summary>
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
                _node.state = JobNode.State.Active;
            else
                throw new JobNodeException("Node already active!", null);
        }

        public void StopNode(int id)
        {
            JobNode _node = GetNode(id);
            if (_node != null)
                _node.state = JobNode.State.Inactive;
            else
                throw new JobNodeException("Node already inactive!", null);
        }

        public void AddNode(JobNode node)
        {
            if (maxNodes > _nodes.Count)
                _nodes.Add(node);
            else
                throw new JobSystemException("Node limit reached!", null);
        }

        public void RemoveNode(int id)
        {
            if (!RemoveJobIntern(id))
                throw new JobNodeException("Node does not exist!", null);
        }

        private bool RemoveNodeIntern(int id)
        {
            lock (_jsLock)
                for (int i = 0; i < _nodes.Count; i++)
                    if (_nodes[i].id == id)
                    {
                        _nodes.RemoveAt(i);
                        return true;
                    }
            return false;
        }

        public void RemoveAllNodes()
        {
            lock (_jsLock)
                _nodes.Clear();
        }

        public JobNode GetNode(int id)
        {
            lock(_jsLock)
                foreach (JobNode _node in _nodes)
                    if (_node.id == id)
                        return _node;
            return null;
        }

        #endregion

        #region nodes serialization

        public void SaveNodes(string fileName)
        {
            lock (_jsLock)
                JSSerializer.Serialize(fileName, _nodes);
        }

        public void LoadNodes(string fileName)
        {
            List<JobNode> _buffer = (List<JobNode>)JSSerializer.Deserialize(fileName);
            lock (_jsLock)
                _nodes.AddRange(_buffer);
        }

        #endregion

        #region jobs handling

        public void StartJob(int id)
        {
            // This does only work, if nobody executes the RemoveNode/RemoveJob ...
            Job _job = GetJob(id);
            if (_job != null)
                if (_job.state == Job.JobState.Inactive)
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
                    _job.state = Job.JobState.Inactive;
                else
                    throw new JobException("Job already active!", null);
            else
                throw new JobException("Job does not exist!", null);
        }

        /// <param name="id">The id of the node to add the job to.</param>
        public void AddJobToNode(int id, Job job)
        {
            JobNode _node = GetNode(id);
            if (_node != null)
                lock (_jsLock)
                    _node.jobs.Add(job);
            else
                throw new JobNodeException("Node does not exist!", null);
        }

        public void RemoveJob(int id)
        {
            if (!RemoveJobIntern(id))
                throw new JobException("Job does not exist!", null);
        }

        private bool RemoveJobIntern(int id)
        {
            lock (_jsLock)
                foreach (JobNode _node in _nodes)
                    lock (_node.jobsLock)
                        for (int i = 0; i < _node.jobs.Count; i++)
                            if (_node.jobs[i].id == id)
                            {
                                _node.jobs.RemoveAt(i);
                                return true;
                            }
            return false;
        }

        public void UpdateJob(int id, Job job)
        {
            if (!UpdateJobIntern(id, job))
                throw new JobException("Job does not exist!", null);
        }

        public bool UpdateJobIntern(int id, Job job)
        {
            Job _job = GetJob(id);
            if (_job == null)
                return false;
            lock (_jsLock)
                _job = job;
            return true;
        }

        public bool JobExist(int id)
        {
            foreach (JobNode _node in _nodes)
                foreach (Job _temp in _node.jobs)
                    if (_temp.id == id)
                        return true;
            return false;
        }

        /// <summary>
        /// Get a reference of a job. (IMPORTANT: After you executed
        /// this method make sure no other threads work with this reference!)
        /// This reference will also update, since the scedule is working with
        /// it.
        /// </summary>
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
