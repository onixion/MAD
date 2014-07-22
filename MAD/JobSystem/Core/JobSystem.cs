using System;
using System.Collections.Generic;

namespace MAD.JobSystemCore
{
    public class JobSystem
    {
        #region members

        private Version _version = new Version(2, 3);
        public Version version { get { return _version; } }

        private JobScedule _scedule;
        public JobScedule.State sceduleState { get { return _scedule.state; } }

        private List<JobNode> _nodes = new List<JobNode>();
        public const int maxNodes = 100;

        public object jsLock = new object();

        #endregion

        #region constructor

        public JobSystem()
        {
            _scedule = new JobScedule(_nodes, jsLock);
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
            lock (jsLock)
                for (int i = 0; i < _nodes.Count; i++)
                    if (_nodes[i].state == JobNode.State.Active)
                        _count++;
            return _count;
        }

        public int NodesInactive()
        {
            int _count = 0;
            lock (jsLock)
                for (int i = 0; i < _nodes.Count; i++)
                    if (_nodes[i].state == JobNode.State.Inactive)
                        _count++;
            return _count;
        }

        public int JobsInitialized()
        {
            int _count = 0;
            lock (jsLock)
                for (int i = 0; i < _nodes.Count; i++)
                    _count = _count + _nodes[i].jobs.Count;
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
                return false;
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
                return false;
        }

        public bool AddNode(JobNode node)
        {
            if (maxNodes > _nodes.Count)
            {
                _nodes.Add(node);
                return true;
            }
            else
                return false;
        }

        public bool RemoveNode(int nodeID)
        {
            lock (jsLock)
            {
                for (int i = 0; i < _nodes.Count; i++)
                    if (_nodes[i].id == nodeID)
                    {
                        _nodes.RemoveAt(i);
                        return true;
                    }
            }
            return false;
        }

        public void RemoveAllNodes()
        {
            lock (jsLock)
                _nodes.Clear();
        }

        public JobNode GetNode(int nodeID)
        {
            foreach (JobNode _node in _nodes)
                if (_node.id == nodeID)
                    return _node;
            return null;
        }

        public JobNodeInfo GetNodeInfo(int nodeID)
        {
            // WORKIN ON THIS!
            return null;
        }

        #endregion

        #region nodes serialization

        public void SaveNode(string fileName)
        {
            lock (jsLock)
                JSSerializer.Serialize(fileName, _nodes);
        }

        public void LoadNode(string fileName)
        {
            List<JobNode> _buffer = (List<JobNode>)JSSerializer.Deserialize(fileName);
            lock (jsLock)
                _nodes.AddRange(_buffer);
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
                    return false;
            }
            else
                return false;
        }

        public bool StopJob(int jobID)
        {
            Job _job = GetJob(jobID);
            if (_job != null)
                if (_job.state == Job.JobState.Waiting)
                {
                    _job.state = Job.JobState.Inactive;
                    return true;
                }
                else
                    return false;
            else
                return false;
        }

        public bool AddJobToNode(int nodeID, Job jobToAdd)
        {
            JobNode _node = GetNode(nodeID);
            if (_node != null)
            {
                lock (jsLock)
                    _node.jobs.Add(jobToAdd);
                return true;
            }
            else
                return false;
        }

        public bool RemoveJob(int jobID)
        {
            lock (jsLock)
                foreach (JobNode _node in _nodes)
                    lock (_node.jobsLock)
                        for(int i = 0; i < _node.jobs.Count; i++)
                            if (_node.jobs[i].id == jobID)
                            {
                                _node.jobs.RemoveAt(i);
                                return true;
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
            foreach (JobNode _node in _nodes)
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

        public JobInfo GetJobInfo(int jobID)
        {
            lock (jsLock)
            {
                Job _temp = GetJob(jobID);

                if (_temp == null)
                    return null;

                JobInfo _info = new JobInfo();
                _info.id = _temp.id;
                _info.name = _temp.name;
                _info.state = _temp.state;
                _info.outState = _temp.outState;

                return _info;
            }
        }

        #endregion

        #endregion
    }
}
