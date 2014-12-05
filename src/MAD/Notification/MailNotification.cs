﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Net;
using System.Net.Mail;

using MAD.JobSystemCore;
using MAD.Logging;

namespace MAD.Notification
{
    /*
     * Mail Notification
     * This here is just for testing the function of the
     * JobSystem ...
     */

    public static class MailNotification
    {
        private static  MailLogin _defaultLogin = null;
        private static MailAddress[] _defaultMails = null;

        private static List<object[]> _priv = new List<object[]>();
        private static List<object[]> _publ = new List<object[]>();

        private static object _sendLock = new object();
        private static Thread _workerThread = null;
        private static object _startStopLock = new object();
        private static bool _stopRequest = false;
        private static ManualResetEvent _sendEvent = new ManualResetEvent(false);

        private static SmtpClient _client;
        private static MailAddress[] _to;
        private static string _sub;
        private static string _con;
        private static JobNotificationSettings _sett;

        public static void Start()
        {
            lock (_startStopLock)
            {
                if (_workerThread == null)
                {
                    _workerThread = new Thread(new ThreadStart(Working));
                    _workerThread.Start();
                }
                else
                    throw new Exception("Threa already running!");
            }
        }

        public static void Stop()
        {
            lock (_startStopLock)
            {
                if (_workerThread != null)
                {
                    _stopRequest = true;
                    _sendEvent.Set();

                    _workerThread.Join();
                    _workerThread = null;
                }
                else
                    throw new Exception("Thread already stopped!");
            }
        }

        public static void SetDefaultOptions(MailLogin login, MailAddress[] to)
        {
            _defaultLogin = login;
            _defaultMails = to;
        }

        public static void SendMail(string subject, string message)
        {
            SendMail(null, subject, message);
        }

        public static void SendMail(JobNotificationSettings _sett, string subject, string message)
        {
            lock (_sendLock)
            {
                object[] _obj = new object[3];
                _obj[0] = subject;
                _obj[1] = message;
                _obj[2] = _sett;

                _publ.Add(_obj);
                _sendEvent.Set();
            }
        }

        private static void Working()
        {
            while (true)
            {
                // wait for a new request
                _sendEvent.WaitOne();
                _sendEvent.Reset();

                // check for stop-request
                if (_stopRequest)
                {
                    _stopRequest = false;
                    break;
                }

                MoveMails();
                WorkOffMails();
            }
        }

        private static void MoveMails()
        {
            lock (_sendLock)
            {
                foreach (object[] _obj in _publ)
                    _priv.Add(_obj);
                _publ.Clear();
            }
        }

        private static void WorkOffMails()
        {
            foreach (object[] ob in _priv)
            {
                try
                {
                    _sub = (string)ob[0];
                    _con = (string)ob[1];
                    _sett = (JobNotificationSettings)ob[2];

                    if (_sett == null)
                    {
                        _client = new SmtpClient(MadConf.conf.SMTP_SERVER, MadConf.conf.SMTP_PORT);
                        _client.Credentials = new NetworkCredential(MadConf.conf.SMTP_USER, MadConf.conf.SMTP_PASS);

                        _to = MadConf.conf.MAIL_DEFAULT;
                    }
                    else
                    {
                        if (_sett.login == null)
                        {
                            _client = new SmtpClient(MadConf.conf.SMTP_SERVER, MadConf.conf.SMTP_PORT);
                            _client.Credentials = new NetworkCredential(MadConf.conf.SMTP_USER, MadConf.conf.SMTP_PASS);
                        }
                        else
                        {
                            if (_sett.login.smtpAddr == null || _sett.login.port == 0)
                            {
                                _client = new SmtpClient(MadConf.conf.SMTP_SERVER, MadConf.conf.SMTP_PORT);
                                _client.Credentials = new NetworkCredential(MadConf.conf.SMTP_USER, MadConf.conf.SMTP_PASS);
                            }
                            else
                            {
                                _client = new SmtpClient(_sett.login.smtpAddr, _sett.login.port);
                                _client.Credentials = new NetworkCredential(_sett.login.mail.ToString(), _sett.login.password);
                            }
                        }

                        if (_sett.mailAddr == null || _sett.mailAddr.Length == 0)
                            _to = MadConf.conf.MAIL_DEFAULT;
                        else
                            _to = _sett.mailAddr;
                    }

                    _client.EnableSsl = true;
                    _client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    _client.Timeout = 10000;

                    foreach (MailAddress _mail in _to)
                    {
                        SendMailInter(_client, _mail, _sub, _con);
                    }
                }
                catch (Exception e)
                {
                    Logger.Log("MailNotification: " + e.Message, Logger.MessageType.ERROR);
                }
                finally
                {
                    _client.Dispose();
                }
            }

            _priv.Clear();
        }

        private static void SendMailInter(SmtpClient client, MailAddress mail, string sub, string con)
        {
            _client.Send(new MailMessage(mail.ToString(), mail.ToString(), sub, con));
        }
    }
}