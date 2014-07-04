using System;
using System.Threading;
using System.Collections.Generic;

using Amib.Threading;

namespace MAD.JobSystemCore
{
    public class JobScedule
    {
        #region members

        private List<JobNode> _jobNodes;
        private Object _nodesLock;

        private Thread _cycleThread;
        private object _cycleThreadLock = new object();
        private int _cycleTime = 100;

        private SmartThreadPool _workerPool;
        private int _maxThreads = 10;

        public enum State { Running, Stopped, StopRequest }
        private State _state = State.Stopped;
        public State state { get { return _state; } }

        #endregion

        #region constructor

        public JobScedule(List<JobNode> jobNodes, object nodesLock)
        {
            _jobNodes = jobNodes;
            _nodesLock = nodesLock;

            _workerPool = new SmartThreadPool(2000, _maxThreads);
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
            while(true)
            {
                Thread.Sleep(_cycleTime);
                DateTime _time = DateTime.Now;

                lock (_nodesLock)
                {
                    foreach (JobNode _node in _jobNodes)
                    {
                        if (_node.state == JobNode.State.Active)
                        {
                            foreach (Job _job in _node._jobs)
                            {
                                if (!_job.jobLocked)
                                {
                                    if (_job.jobState == Job.JobState.Waiting)
                                    {
                                        if (CheckJobTime(_job.jobTime, _time))
                                        {
                                            if (_job.jobTime.type == JobTime.TimeType.Relative)
                                            {
                                                _job.jobTime.jobDelay.Reset();
                                                JobThreadStart(_job);
                                            }
                                            else if (_job.jobTime.type == JobTime.TimeType.Absolute)
                                            {
                                                JobTimeHandler _handler = _job.jobTime.GetJobTimeHandler(_time);

                                                if (!_handler.IsBlocked(_time))
                                                {
                                                    _handler.minuteAtBlock = _time.Minute;
                                                    JobThreadStart(_job);
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

        private void JobThreadStart(Job job)
        {
            _workerPool.QueueWorkItem(new WorkItemCallback(JobInvoke), job);
        }

        private object JobInvoke(object job)
        {
            Job _job = (Job)job;

            // Execute job and set all output varibales.
            _job.LaunchJob();

            // Get Notificationrules which are validity and if necessary make a query to the NotificationSystem.
            List<JobNotificationRule> _rules = _job.jobNotification.GetValidityRules();

            if (_rules.Count != 0)
            {
                // Query to NotificationSystem.
            }
 
            return null;
        }

        #endregion
    }
}
