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

        public enum State { Active, Inactive, StopRequest }
        private State _state = State.Inactive;
        public State state { get { return _state; } }

        private List<JobNode> _jobNodes;

        #endregion

        #region constructor

        public JobScedule(List<JobNode> jobNodes)
        {
            _jobNodes = jobNodes;
            _workerPool = new SmartThreadPool(1000, _maxThreads);
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
    
                foreach (JobNode _node in _jobNodes)
                {
                    if (_node.state == JobNode.State.Active)
                    {
                        foreach (Job _job in _node.jobs)
                        {
                            /* Scedule execute only these jobs, which states are equal to 'Waiting'. */
                            if (_job.state == Job.JobState.Waiting)
                            {
                                // Set job-state to working.
                                _job.state = Job.JobState.Working;

                                if (CheckJobTime(_job.time, _time))
                                {
                                    if (_job.time.type == JobTime.TimeType.Relative)
                                    {
                                        _job.time.jobDelay.Reset();

                                        _holder.job = _job;
                                        _holder.targetAddress = _node.ipAddress;

                                        JobThreadStart(_holder);
                                    }
                                    else if (_job.time.type == JobTime.TimeType.Absolute)
                                    {
                                        JobTimeHandler _handler = _job.time.GetJobTimeHandler(_time);

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
                                    if (_job.time.type == JobTime.TimeType.Relative)
                                        _job.time.jobDelay.SubtractFromDelaytime(_cycleTime);
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
