using System;
using System.Threading;
using System.Collections.Generic;

using Amib.Threading;

namespace MAD.JobSystemCore
{
    public class JobScedule
    {
        #region members

        // worker-thread
        private Thread _cycleThread;
        private object _cycleThreadLock = new object();
        private int _cycleTime = 100;

        // threadpool
        private SmartThreadPool _workerPool;
        private int _maxThreads = 10;

        // state
        public enum State { Running, Stopped, StopRequest }
        private State _state = State.Stopped;
        public State state { get { return _state; } }

        // nodes
        private List<JobNode> _jobNodes;
        private object _jsNodesLock;

        #endregion

        #region constructor

        public JobScedule(List<JobNode> jobNodes, object jsNodesLock)
        {
            _jobNodes = jobNodes;
            _jsNodesLock = jsNodesLock;

            _workerPool = new SmartThreadPool(2000, _maxThreads); // not sure about '2000'
        }

        #endregion

        #region methodes

        public void Start()
        {
            lock (_cycleThreadLock)
            {
                if (_state == State.Stopped)
                {
                    _state = State.Running;
                    _cycleThread = new Thread(CycleJobTracker);
                    _cycleThread.Start();
                }
            }
        }

        public void Stop()
        {
            lock (_cycleThreadLock)
            {
                if (_state == State.Running)
                {
                    _state = State.StopRequest;
                    _cycleThread.Join();
                    _state = State.Stopped;
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

                lock (_jsNodesLock)
                {
                    foreach (JobNode _node in _jobNodes)
                    {
                        if (_node.state == JobNode.State.Active)
                        {
                            foreach (Job _job in _node.jobs)
                            {
                                if (_job.jobState == Job.JobState.Waiting)
                                {
                                    if (CheckJobTime(_job.jobTime, _time))
                                    {
                                        if (_job.jobTime.type == JobTime.TimeType.Relative)
                                        {
                                            _job.jobTime.jobDelay.Reset();

                                            _holder.job = _job;
                                            _holder.targetAddress = _node.ipAddress;

                                            JobThreadStart(_holder);
                                        }
                                        else if (_job.jobTime.type == JobTime.TimeType.Absolute)
                                        {
                                            JobTimeHandler _handler = _job.jobTime.GetJobTimeHandler(_time);

                                            if (!_handler.IsBlocked(_time))
                                            {
                                                _handler.minuteAtBlock = _time.Minute;

                                                _holder.job = _job;
                                                _holder.targetAddress = _node.ipAddress;

                                                JobThreadStart(_holder);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (_job.jobTime.type == JobTime.TimeType.Relative)
                                        {
                                            _job.jobTime.jobDelay.SubtractFromDelaytime(_cycleTime);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (_state == State.StopRequest)
                {
                    break;
                }
            }
        }

        private bool CheckJobTime(JobTime jobTime, DateTime time)
        {
            if (jobTime.type == JobTime.TimeType.Relative)
            {
                if (jobTime.jobDelay.CheckTime())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (jobTime.type == JobTime.TimeType.Absolute)
            {
                foreach (JobTimeHandler _handler in jobTime.jobTimes)
                {
                    if (_handler.CheckTime(time))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

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

            // Set start-time.
            _job.lastStarted = DateTime.Now;

            // Execute job.
            _job.Execute(_holder.targetAddress);

            // Set job-state to wait.
            _job.jobState = Job.JobState.Waiting;

            // Set stop-time.
            _job.lastFinished = DateTime.Now;

            // Set time-span.
            _job.lastTimeSpan = _job.lastStarted.Subtract(_job.lastFinished);

            // Get Notificationrules which are validity.
            List<JobNotificationRule> _rules = _job.jobNotification.GetValidityRules();

            // If necessary make a query to the NotificationSystem.
            if (_rules.Count != 0)
            {
                // Query to NotificationSystem.
            }
 
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
