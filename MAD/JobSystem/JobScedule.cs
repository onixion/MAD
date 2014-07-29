using System;
using System.Threading;
using System.Collections.Generic;

using Amib.Threading;

namespace MAD.JobSystemCore
{
    public class JobScedule
    {
        #region members

        private Thread _cycleThread;
        private object _cycleThreadLock = new object();
        private int _cycleTime = 100;

        private SmartThreadPool _workerPool;
        private int _maxThreads = 10;
        private int _maxTimeToWaitForIdle = 5000;

        public enum State { Active, Inactive, StopRequest }
        private State _state = State.Inactive;
        public State state { get { return _state; } }

        private List<JobNode> _jobNodes;
        private object _nodesLock;

        #endregion

        #region constructor

        public JobScedule(object nodesLock, List<JobNode> jobNodes)
        {
            _nodesLock = nodesLock;
            _jobNodes = jobNodes;
            _workerPool = new SmartThreadPool(_maxThreads);
        }

        #endregion

        #region methodes

        public void Start()
        {
            lock (_cycleThreadLock)
            {
                if (_state == State.Inactive)
                {
                    _state = State.Active;
                    _cycleThread = new Thread(CycleJobTracker);
                    _cycleThread.Start();
                }
            }
        }

        public void Stop()
        {
            lock (_cycleThreadLock)
            {
                if (_state == State.Active)
                {
                    _state = State.StopRequest;
                    
                    _cycleThread.Join();
                    _workerPool.WaitForIdle(_maxTimeToWaitForIdle);
                    _workerPool.Cancel();

                    _state = State.Inactive;
                }
            }
        }

        private void CycleJobTracker()
        {
            JobHolder _holder = new JobHolder(); // not very nice, but works for now

            while(true)
            {
                Thread.Sleep(_cycleTime);
                DateTime _time = DateTime.Now;
  
                // RESERVE NODES
                lock(_nodesLock)
                {
                    foreach (JobNode _node in _jobNodes)
                    {
                        // RESERV NODE
                        lock (_node.nodeLock)
                        {
                            if (_node.state == JobNode.State.Active)
                            {
                                foreach (Job _job in _node.jobs)
                                {
                                    // RESERVE JOB OF NODE
                                    lock (_job.jobLock)
                                    {
                                        if (_job.state == Job.JobState.Waiting)
                                        {
                                            if (CheckJobTime(_job.time, _time))
                                            {
                                                if (_job.time.type == JobTime.TimeType.Relative)
                                                {
                                                    _job.state = Job.JobState.Working;
                                                    _job.time.jobDelay.Reset();

                                                    _holder.job = _job; // SEARCHING FOR BETTER SOLUTION!
                                                    _holder.targetAddress = _node.ipAddress; // SEARCHING FOR BETTER SOLUTION!

                                                    JobThreadStart(_holder);
                                                }
                                                else if (_job.time.type == JobTime.TimeType.Absolute)
                                                {
                                                    JobTimeHandler _handler = _job.time.GetJobTimeHandler(_time);

                                                    if (!_handler.IsBlocked(_time))
                                                    {
                                                        _job.state = Job.JobState.Working;
                                                        _handler.minuteAtBlock = _time.Minute;

                                                        _holder.job = _job; // SEARCHING FOR BETTER SOLUTION!
                                                        _holder.targetAddress = _node.ipAddress; // SEARCHING FOR BETTER SOLUTION!

                                                        JobThreadStart(_holder);
                                                    }
                                                }
                                            }
                                            else
                                                if (_job.time.type == JobTime.TimeType.Relative)
                                                    _job.time.jobDelay.SubtractFromDelaytime(_cycleTime);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (_state == State.StopRequest)
                    break;
            }
        }

        private bool CheckJobTime(JobTime jobTime, DateTime time)
        {
            if (jobTime.type == JobTime.TimeType.Relative)
                if (jobTime.jobDelay.CheckTime())
                    return true;
                else
                    return false;
            else if (jobTime.type == JobTime.TimeType.Absolute)
                foreach (JobTimeHandler _handler in jobTime.jobTimes)
                    if (_handler.CheckTime(time))
                        return true;
                    else
                        return false;
            return false;
        }

        private void JobThreadStart(JobHolder holder)
        {
            _workerPool.QueueWorkItem(new WorkItemCallback(JobInvoke), holder);
        }

        private object JobInvoke(object holder)
        {
            JobHolder _holder = (JobHolder)holder;
            Job _job = _holder.job;

            _job.tStart = DateTime.Now;
            _job.Execute(_holder.targetAddress);
            _job.tStop = DateTime.Now;
            _job.tSpan = _job.tStart.Subtract(_job.tStop);

            // Check JobNotification.
            // Make query to NotificationSystem if necessary.

            _job.state = Job.JobState.Waiting;

            return null;
        }

        #endregion
    }

    /* Need to find a better solution for this problem ... */
    public struct JobHolder
    { 
        public Job job;
        public System.Net.IPAddress targetAddress;

        public JobHolder(Job job, System.Net.IPAddress targetAddress)
        {
            this.job = job;
            this.targetAddress = targetAddress;
        }
    }
}
