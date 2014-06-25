using System;
using System.Threading;
using System.Collections.Generic;

namespace MAD.jobSys
{
    public class JobScedule
    {
        #region members

        private List<Job> _jobs;
        private Object _jobsLock;

        private Thread _cycleThread;
        private object _cycleThreadLock = new object();
        private int _cycleTime = 100;

        private Thread[] _workerThreads;
        private int _workerThreadsCount = 10;

        public enum State { Running, Stopped, StopRequest }
        private State _state = State.Stopped;
        public State state { get { return _state; } }

        #endregion

        #region constructor

        public JobScedule(List<Job> jobs, Object jobsLock)
        {
            _jobs = jobs;
            _jobsLock = jobsLock;

            InitWorkerThreads();
        }

        private void InitWorkerThreads()
        {
            _workerThreads = new Thread[_workerThreadsCount];
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

                lock (_jobsLock)
                {
                    foreach (Job _job in _jobs)
                    {
                        if (_job.jobState == Job.State.Running)
                        {
                            if (CheckJobTime(_job.jobOptions.jobTime, _time))
                            {
                                UpdateJobTime(_job.jobOptions.jobTime);
                                JobThreadStart(_job);
                            }
                            else
                            {
                                UpdateJobTime(_job.jobOptions.jobTime);
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

        private bool CheckJobTime( JobTime jobTime, DateTime time)
        {
            if (jobTime.type == JobTime.TimeType.Relativ)
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

        private void UpdateJobTime(JobTime jobTime)
        {
            if (jobTime.type == JobTime.TimeType.Relativ)
            {
                if (jobTime.jobDelay.delayTimeRemaining <= 0)
                {
                    jobTime.jobDelay.ResetRemainTime();
                }
                else
                {
                    jobTime.jobDelay.WorkDelayTime(_cycleTime);
                }
            }
            else if (jobTime.type == JobTime.TimeType.Absolute)
            {

            }
            else
            {
                throw new Exception("JOBTIME-TYPE NULL!");
            }
        }

        private void JobThreadStart(Job job)
        {
            bool _workerThreadStarted = false;

            while (true)
            {
                for (int i = 0; i < _workerThreads.Length; i++)
                {/*
                    if (_workerThreads[i])
                    {
                        _workerThreads[i] = new Thread(JobInvoke);

                        _workerThreads[i].Start(job);
                        _workerThreadStarted = true;

                        break;
                    }*/
                }

                if (_workerThreadStarted)
                {
                    break;
                }
            }
        }

        private void JobInvoke(object job)
        {
            Job _job = (Job)job;
            _job.LaunchJob();
        }

        #endregion
    }
}
