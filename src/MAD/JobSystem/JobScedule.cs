using System;
using System.Threading;
using System.Collections.Generic;

using System.Net;
using System.Net.Mail;

using MAD;
using MAD.Notification;

using Amib.Threading;

namespace MAD.JobSystemCore
{
    public class JobScedule
    {
        #region members

        private Thread _cycleThread;
        private int _cycleTime = 100;
        private object _cycleThreadLock = new object();

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
                DateTime _time = DateTime.Now;

                // Lock node-list.
                lock (_nodesLock)
                {
                    foreach (JobNode _node in _jobNodes)
                    {
                        // Lock node.
                        lock (_node.nodeLock)
                        {
                            if (_node.state == JobNode.State.Active)
                            {
                                // Lock job-list of the node.
                                lock (_node.jobsLock)
                                {
                                    foreach (Job _job in _node.jobs)
                                    {
                                        // Lock the job.
                                        lock (_job.jobLock)
                                        {
                                            if (_job.state == Job.JobState.Waiting)
                                            {
                                                if (_job.type != Job.JobType.NULL)
                                                {
                                                    if (CheckJobTime(_job.time, _time))
                                                    {
                                                        if (_job.time.type == JobTime.TimeMethod.Relative)
                                                        {
                                                            _job.state = Job.JobState.Working;
                                                            _job.time.jobDelay.Reset();

                                                            _holder.job = _job; // SEARCHING FOR BETTER SOLUTION!
                                                            _holder.targetAddress = _node.ipAddress; // SEARCHING FOR BETTER SOLUTION!

                                                            JobThreadStart(_holder);
                                                        }
                                                        else if (_job.time.type == JobTime.TimeMethod.Absolute)
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
                                                    {
                                                        if (_job.time.type == JobTime.TimeMethod.Relative)
                                                            _job.time.jobDelay.SubtractFromDelaytime(_cycleTime);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (_state == State.StopRequest)
                    break;

                Thread.Sleep(_cycleTime);
            }
        }

        private bool CheckJobTime(JobTime jobTime, DateTime time)
        {
            if (jobTime.type == JobTime.TimeMethod.Relative)
                if (jobTime.jobDelay.CheckTime())
                    return true;
                else
                    return false;
            else if (jobTime.type == JobTime.TimeMethod.Absolute)
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
            try { _job.Execute(_holder.targetAddress); }
            catch (Exception)
            {
                _job.state = Job.JobState.Exception;
            }

            _job.tStop = DateTime.Now;
            _job.tSpan = _job.tStop.Subtract(_job.tStart);

            HandleNotification(_job);

            _job.state = Job.JobState.Waiting;

            return null;
        }

        private void HandleNotification(Job job)
        {
            List<JobRule> _brokenRules = job.noti.GetBrokenRules(job.outp);

            if (job.outp.outState == JobOutput.OutState.Failed || job.outp.outState == JobOutput.OutState.Exception || _brokenRules.Count != 0)
            {
                string _mailSubject = "[MAD][ERROR] - Job (JOB-ID: '" + job.id + "') finished with a not expected result.";
                string _mailContent = "";

                _mailContent += "Job-Name:     '" + job.name + "'\n";
                _mailContent += "Job-Type:     '" + job.type.ToString() + "'\n";
                _mailContent += "Job-OutState: '" + job.outp.outState.ToString() + "'.\n\n";

                _mailContent += "Job-TStart:   '" + job.tStart + "'\n";
                _mailContent += "Job-TStop:    '" + job.tStop + "'\n";
                _mailContent += "Job-TSpan:    '" + job.tSpan + "'\n\n";
                
                if (_brokenRules.Count != 0)
                {
                    _mailContent += "_________________________________________________\n";
                    _mailContent += "-> Rules broken: " + _brokenRules.Count + "\n\n";

                    int _count = 0;
                    foreach (JobRule _brokenRule in _brokenRules)
                    {
                        object _data = job.outp.GetOutputDesc(_brokenRule.outDescName).dataObject;
                        if (_data == null)
                            _data = (string)"NULL";

                        _mailContent += _count + ".) Rule\n";
                        _mailContent += "-> Rule Equation:    " + _brokenRule.outDescName + " <" + _brokenRule.oper.ToString() + "> " + _brokenRule.compareValue.ToString() + " = TRUE\n";
                        _mailContent += "-> Current Equation: " + _data.ToString() + " <" + _brokenRule.oper.ToString() + "> " + _brokenRule.compareValue.ToString() + " = FALSE\n\n";
                        
                        _mailContent += "-> OutDescriptor: " + _brokenRule.outDescName + "\n";
                        _mailContent += "-> Operation:     " + _brokenRule.oper.ToString() + "\n";
                        _mailContent += "-> CompareValue:  " + _brokenRule.compareValue.ToString() + "\n";
                        _mailContent += "=> CurrentValue:  " + _data.ToString() + "\n";
                        _mailContent += "\n";
                        _count++;
                    }

                    _mailContent += "_________________________________________________\n";
                }

                if (job.noti.settings != null)
                {
                    NotificationSystem.SendMail(job.noti.settings.mailAddr, _mailSubject, _mailContent, 3, job.noti.settings.login.smtpAddr,
                        job.noti.settings.login.mail, job.noti.settings.login.password, job.noti.settings.login.port);
                }
                else
                {
                    NotificationSystem.SendMail(new MailAddress[1] {new MailAddress("alin.porcic@gmail.com")}, _mailSubject, _mailContent, 3);
                }
            }
        }

        private void SendNotification(MailLogin login, MailAddress[] to, string subject, string content)
        {
            
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
