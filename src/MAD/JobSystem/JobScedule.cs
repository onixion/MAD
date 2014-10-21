using System;
using System.Threading;
using System.Collections.Generic;

using System.Net;
using System.Net.Mail;

using MAD;
using MAD.Notification;
using MAD.Logging;

using Amib.Threading;

namespace MAD.JobSystemCore
{
    public class JobScedule
    {
        #region members

        private bool _debug = false;
        private bool _log = false;

        private Thread _cycleThread;
        private int _cycleTime = 100;

        private object _stateLock = new object();
        /* state = 0 | inactive
         * state = 1 | active
         * state = 2 | stop-request
         * state = 3 | execption */
        private int _state = 0;
        public int state { get { return _state; } }

        private SmartThreadPool _workerPool;
        private int _maxThreads = 10;
        private int _maxTimeToWaitForIdle = 5000;

        private List<JobNode> _jobNodes;
        private object _jsLock = new object();

        private bool _notiEnabled = false;

        #endregion

        #region constructor

        public JobScedule(object jsLock, List<JobNode> jobNodes)
        {
            LoadConf();
            _jsLock = jsLock;
            _jobNodes = jobNodes;
            _workerPool = new SmartThreadPool(_maxThreads);
        }

        private void LoadConf()
        {
            lock (MadConf.confLock)
            {
                _debug = MadConf.conf.DEBUG_MODE;
                _log = MadConf.conf.LOG_MODE;
                _notiEnabled = MadConf.conf.NOTI_ENABLE;
            }
        }

        #endregion

        #region methodes

        public void Start()
        {
            lock (_stateLock)
            {
                if (_state == 0)
                {
                    _state = 1;
                    _cycleThread = new Thread(CycleJobTracker);
                    _cycleThread.Start();
                }
            }
        }

        public void Stop()
        {
            lock (_stateLock)
            {
                if (_state == 1)
                {
                    _state = 2;

                    _cycleThread.Join();
                    _workerPool.WaitForIdle(_maxTimeToWaitForIdle);
                    _workerPool.Cancel();

                    _state = 0;
                }
            }
        }

        private void CycleJobTracker()
        {
            while(true)
            {
                DateTime _nowTime = DateTime.Now;
                try
                {
                    lock (_jsLock)
                    {
                        foreach (JobNode _node in _jobNodes)
                        {
                            if (_node.state == 1)
                            {
                                foreach (Job _job in _node.jobs)
                                {
                                    if (_job.state == 1)
                                    {
                                        if (_job.type != Job.JobType.NULL)
                                        {
                                            if (JobTimeCheck(_job, _nowTime))
                                            {
                                                if (_log)
                                                    Logger.Log("(SCHEDULE) JOB (ID:" + _job.id + ")(GUID:" + _job.guid + ") started execution.", Logger.MessageType.INFORM);

                                                // Change job-state.
                                                _node.uWorker++;
                                                _job.state = 2;

                                                JobHolder _holder = new JobHolder();
                                                _holder.node = _node;
                                                _holder.job = _job;

                                                JobThreadStart(_holder);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                { 
                    // This try-catch is just for debugging reasons here. If the program works
                    // as planed, it will never get into this catch.
                    throw new SystemException("SCEDULE: INTERNAL-EXECPTION: " + e.Message);
                }

                if (_state == 2)
                    break;

                //Thread.Sleep(_cycleTime);
            }
        }

        private bool JobTimeCheck(Job job, DateTime time)
        {
            if (job.time.type == JobTime.TimeMethod.Relative)
            {
                if (job.time.jobDelay.CheckTime())
                {
                    job.time.jobDelay.Reset();
                    return true;
                }
                else
                {
                    job.time.jobDelay.SubtractFromDelaytime(_cycleTime);
                    return false;
                }
            }
            else if (job.time.type == JobTime.TimeMethod.Absolute)
            {
                JobTimeHandler _timeHandler = job.time.GetJobTimeHandler(time);

                if (_timeHandler != null)
                {
                    if (!_timeHandler.IsBlocked(time))
                    {
                        if (_timeHandler.CheckTime(time))
                        {
                            _timeHandler.minuteAtBlock = time.Minute;
                            return true;
                        }
                    }
                    
                    return false;
                }
                else
                    return false;
            }
            else
                return false;
        }

        private void JobThreadStart(JobHolder holder)
        {
            _workerPool.QueueWorkItem(new WorkItemCallback(JobInvoke), holder);
        }

        private object JobInvoke(object holder)
        {
            /* In the worst case scenario the node can be deleted at this point 
             * and the only variable that prevents it from doing this, is the 
             * 'uCounter'. Everytime the job starts, it increases the 'uCounter' by 1.
             * After execution it decreases the counter by 1. Only if the uCounter is
             * equal to 0 the node can be deleted (or else). */
            JobHolder _holder = (JobHolder)holder;
            JobNode _node = _holder.node;
            Job _job = _holder.job;

            _job.tStart = DateTime.Now;
            try 
            {
                _job.Execute(_node.ip); 
            }
            catch (Exception)
            { 
                _job.state = 3; 
            }
            _job.tStop = DateTime.Now;
            _job.tSpan = _job.tStop.Subtract(_job.tStart);

            _job.outp.outputs[0].dataObject = _job.outp.outState.ToString();

            if (_job.notiFlag)
            {
                if (_notiEnabled)
                {
                    List<JobRule> _bRules = GetBrokenRules(_job);
                    if (_bRules.Count != 0)
                    {
                        string _mailSubject = GenMailSubject(_job, "Job (target='" + _holder.node.ip.ToString()
                            + "') finished with a not expected result!");

                        string _mailContent = "";
                        _mailContent += "JobNode-ID:  '" + _node.id + "'\n";
                        _mailContent += "JobNode-IP:  '" + _node.ip.ToString() + "'\n";
                        _mailContent += "JobNode-MAC: '" + _node.mac.ToString() + "'\n\n";
                        _mailContent += GenJobInfo(_job);
                        _mailContent += GenBrokenRulesText(_job.outp, _bRules);

                        if (_job.settings != null)
                        {
                            // This is not the perfect solution. Need to create a class, which
                            // can stack notifications, so we do not lose precious time here ...
                            NotificationSystem.SendMail(_job.settings.mailAddr, _mailSubject, _mailContent, 2,
                                _job.settings.login.smtpAddr, _job.settings.login.mail,
                                _job.settings.login.password, _job.settings.login.port);
                        }
                        else
                        {
                            // WORKIN ON THIS!!!
                            /*
                            if (_conf.MAIL_DEFAULT != null || _conf.MAIL_DEFAULT.Length != 0)
                            {
                                // Use global notification-settings.

                                /* The JobScedule does not know if the SMTP-Login works. */
                            /*
                                NotificationSystem.SendMail(_conf.MAIL_DEFAULT,
                                    _mailSubject, _mailContent, 2);
                            }
                            else
                            {
                                // No notification is possible, because the Job has no settings and the default
                                // mail-addresses are not set.
                                Logger.Log("No notification possible! Job has no settings and no global settings set!", Logger.MessageType.ERROR);
                            }*/
                        }
                    }
                }
            }

            if (_log)
                Logger.Log("(SCHEDULE) JOB (ID:" + _job.id + ")(GUID:" + _job.guid + ") stopped execution.", Logger.MessageType.INFORM);

            // Change job-state.
            _job.state = 1;
            _node.uWorker--;
            return null;
        }

        #region generate mail text

        private string GenMailSubject(Job job, string message)
        { 
            return "[MAD][ERROR] - Job (JOB-ID: '" + job.id + "'): " + message;
        }

        private List<JobRule> GetBrokenRules(Job job)
        {
            List<JobRule> _bRules = new List<JobRule>();

            foreach (JobRule _rule in job.rules)
                if (!_rule.CheckRuleValidity(job.outp))
                    _bRules.Add(_rule);

            return _bRules;
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

                _buffer += "#" + _count + ".) Broken-Rule\n";
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
            _buffer += "Job-TStart:   '" + job.tStart.ToString("dd.MM.yyyy HH:mm:ss") + "'\n";
            _buffer += "Job-TStop:    '" + job.tStop.ToString("dd.MM.yyyy HH:mm:ss") + "'\n";
            _buffer += "Job-TSpan:    '" + job.tSpan.Seconds + "s" + job.tSpan.Milliseconds + "ms'\n\n";
            return _buffer;
        }

        #endregion

        #endregion
    }

    public class JobHolder
    {
        public JobNode node;
        public Job job;
    }
}
