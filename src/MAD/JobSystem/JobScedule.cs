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
        private ReaderWriterLockSlim _nodesLock;

        private ManualResetEvent _lockSwitch = new ManualResetEvent(false);

        #endregion

        #region constructor

        public JobScedule(ReaderWriterLockSlim nodesLock, List<JobNode> jobNodes)
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
                DateTime _nowTime = DateTime.Now;

                _nodesLock.EnterReadLock();

                foreach (JobNode _node in _jobNodes)
                {
                    _node.nodeLock.EnterReadLock();

                    if (_node.state == JobNode.State.Active)
                    {
                        foreach (Job _job in _node.jobs)
                        {
                            _job.jobLock.EnterWriteLock();

                            if (_job.state == Job.JobState.Waiting)
                            {
                                if (_job.type != Job.JobType.NULL)
                                {
                                    if (_job.time.type == JobTime.TimeMethod.Relative)
                                    {
                                        if (_job.time.jobDelay.CheckTime())
                                        {
                                            _job.time.jobDelay.Reset();

                                            // Job is ready to be executed.

                                            _job.state = Job.JobState.Working;

                                            _holder.node = _node;
                                            _holder.job = _job;
                                            _holder.targetAddress = _node.ipAddress;

                                            JobThreadStart(_holder);

                                            // wait for Job to lock necessary locks.
                                            _lockSwitch.WaitOne();
                                            _lockSwitch.Reset();

                                            _job.jobLock.ExitWriteLock();
                                        }
                                        else
                                        {
                                            _job.time.jobDelay.SubtractFromDelaytime(_cycleTime);
                                            _job.jobLock.ExitWriteLock();
                                        }
                                    }
                                    else if (_job.time.type == JobTime.TimeMethod.Absolute)
                                    {
                                        JobTimeHandler _timeHandler = _job.time.GetJobTimeHandler(_nowTime);

                                        if (_timeHandler != null)
                                        {
                                            if (!_timeHandler.IsBlocked(_nowTime))
                                            {
                                                if (_timeHandler.CheckTime(_nowTime))
                                                {
                                                    _timeHandler.minuteAtBlock = _nowTime.Minute;

                                                    // Job is ready to be executed.

                                                    _job.state = Job.JobState.Working;

                                                    _holder.node = _node;
                                                    _holder.job = _job;
                                                    _holder.targetAddress = _node.ipAddress;

                                                    JobThreadStart(_holder);

                                                    // wait for Job to lock necessary locks.
                                                    
                                                    _job.jobLock.ExitWriteLock();

                                                    _lockSwitch.WaitOne();
                                                    _lockSwitch.Reset();
                                                }
                                                else
                                                    _job.jobLock.ExitWriteLock();
                                            }
                                            else
                                                _job.jobLock.ExitWriteLock();
                                        }
                                        else
                                            _job.jobLock.ExitWriteLock();
                                    }
                                    else
                                        _job.jobLock.ExitWriteLock();
                                }
                                else
                                    _job.jobLock.ExitWriteLock();
                            }
                            else
                                _job.jobLock.ExitWriteLock();
                        }
                    }

                    _node.nodeLock.ExitReadLock();
                }

                _nodesLock.ExitReadLock();

                if (_state == State.StopRequest)
                    break;

                Thread.Sleep(_cycleTime);
            }
        }

        private void JobThreadStart(JobHolder holder)
        {
            _workerPool.QueueWorkItem(new WorkItemCallback(JobInvoke), holder);
        }

        private object JobInvoke(object holder)
        {
            JobHolder _holder = (JobHolder)holder;
            JobNode _node = _holder.node;
            Job _job = _holder.job;

            // lock nodes and node read-only
            _nodesLock.EnterReadLock();
            _node.nodeLock.EnterReadLock();

            _lockSwitch.Set();

            _job.jobLock.EnterWriteLock();

            _job.tStart = DateTime.Now;
            try 
            {
                _job.Execute(_holder.targetAddress);
            }
            catch (Exception)
            {
                _job.state = Job.JobState.Exception;
            }
            _job.tStop = DateTime.Now;
            _job.tSpan = _job.tStop.Subtract(_job.tStart);

            if (_job.noti.useNotification)
            {
                List<JobRule> _bRules = _job.noti.GetBrokenRules(_job.outp);

                // check if notification is neseccary.
                if (_job.outp.outState == JobOutput.OutState.Exception ||
                    _job.outp.outState == JobOutput.OutState.Failed ||
                    _bRules.Count != 0)
                {
                    string _mailSubject = GenMailSubject(_job, "Job (target='" + _holder.targetAddress.ToString()
                        + "') finished with a not expected result!");

                    string _mailContent = GenJobInfo(_job);
                    _mailContent += GenBrokenRulesText(_job.outp, _bRules);

                    if (_job.noti.settings != null)
                    {
                        // This is not the perfect solution. Need to create a class, which
                        // can stack notifications, so we do not lose precious time here ...
                        NotificationSystem.SendMail(_job.noti.settings.mailAddr, _mailSubject, _mailContent, 2,
                            _job.noti.settings.login.smtpAddr, _job.noti.settings.login.mail,
                            _job.noti.settings.login.password, _job.noti.settings.login.port);
                    }
                    else
                    {
                        // use global notifiaction settings
                        NotificationSystem.SendMail(new MailAddress[1] { new MailAddress(MadConf.conf.defaultMailAddr) },
                            _mailSubject, _mailContent, 2);
                    }
                }
            }

            _job.state = Job.JobState.Waiting;

            // Job execution finished.

            _job.jobLock.ExitWriteLock();
            _node.nodeLock.ExitReadLock();
            _nodesLock.ExitReadLock(); 
            return null;
        }

        #region generate mail text

        private string GenMailSubject(Job job, string message)
        { 
            return "[MAD][ERROR] - Job (JOB-ID: '" + job.id + "'): " + message;
        }

        private string GenBrokenRulesText(JobOutput outp, List<JobRule> bRules)
        {
            string _buffer = "";
            int _count = 0;
            foreach (JobRule _rule in bRules)
            {
                object _data = outp.GetOutputDesc(_rule.outDescName).dataObject;
                if (_data == null)
                    _data = (string)"NULL";

                _buffer += _count + ".) Rule\n";
                _buffer += "-> Rule Equation:    " + _rule.outDescName + " <" + _rule.oper.ToString() + "> '" + _rule.compareValue.ToString() + "' = TRUE\n";
                _buffer += "-> Current Equation: '" + _data.ToString() + "' <" + _rule.oper.ToString() + "> '" + _rule.compareValue.ToString() + "' = FALSE\n\n";

                _buffer += "-> OutDescriptor: " + _rule.outDescName + "\n";
                _buffer += "-> Operation:     " + _rule.oper.ToString() + "\n";
                _buffer += "-> CompareValue:  '" + _rule.compareValue.ToString() + "'\n";
                _buffer += "=> CurrentValue:  '" + _data.ToString() + "'\n\n";
                _count++;
            }
            return _buffer;
        }
            
        private string GenJobInfo(Job job)
        {
            string _buffer = "";
            _buffer += "Job-ID:       '" + job.id + "'\n";
            _buffer += "Job-Name:     '" + job.name + "'\n";
            _buffer += "Job-Type:     '" + job.type.ToString() + "'\n";
            _buffer += "Job-OutState: '" + job.outp.outState.ToString() + "'.\n";
            _buffer += "Job-TStart:   '" + job.tStart + "'\n";
            _buffer += "Job-TStop:    '" + job.tStop + "'\n";
            _buffer += "Job-TSpan:    '" + job.tSpan + "'\n\n";
            return _buffer;
        }

        #endregion

        #endregion
    }

    public class JobHolder
    {
        public ReaderWriterLock _te;

        public JobNode node;
        public Job job;
        public System.Net.IPAddress targetAddress;
    }
}
