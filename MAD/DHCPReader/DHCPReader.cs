﻿using System;
using System.Collections.Generic;

using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Threading;

using MAD.Helper;
using MAD.JobSystemCore;

using Amib.Threading; 

namespace MAD.DHCPReader
{
    public class MACFeeder                                                          //need to do something about that name.. 
    {
        #region member

        private static NetworkHelper _helper = new NetworkHelper();
        

        private byte[] _data;

        uint _sleepFor;

        private bool _running = false;
        private bool _acknowledge; 
        private bool _addressGiven;
        private bool _nameGiven = false;

        private string _hostName = "";
        private IPAddress _requestedIP;

        public List<ModelHost> _dummyList;

        private Thread _check;
        private Thread _start;
        private SmartThreadPool _pool;

        #endregion 

        #region Constructor

        public MACFeeder()
        {
            _sleepFor = 25000;                                                      //default value will be changeable
        }

        #endregion

        #region Methods

        public void Start()
        {
            _running = true;
            _pool = new SmartThreadPool();
            _check = new Thread(UpdateLists);
            _start = new Thread(Prog);
            _check.Start();
            _start.Start();
        }

        public void Stop()
        {
            _running = false;
            _pool.Join();
            _check.Join();
        }

        public void ChangeCheckIntervall(uint time)
        {
            _sleepFor = time; 
        }

        private void Prog()
        {

            while (_running)
            {
                CatchDHCP();
                StartThread();
            }
        }

        private void CatchDHCP()
        {
            
        }

        private void StartThread()
        {
            _pool.QueueWorkItem(ProcessData);
        }

        private object ProcessData(Object stateInfo)
        {

            if (_helper.IsDhcp(_data) && _helper.IsDhcpRequest(_data))
            {
                string _macAddress = _helper.getMacString(_data);
                
                for (uint i = NetworkHelper._magicCookiePosition; i < _data.Length; i++)
                {                   
                    switch (Convert.ToUInt16(_data[i]))
                    {
                        case 50:
                            byte[] _ipBytes = new byte[4];
                            
                            for (uint ii = 0; ii < 4; ii++)
                            {
                                _ipBytes[ii] = _data[i + 2 + ii];
                            }
                            
                            _addressGiven = true;
                            _requestedIP = new IPAddress(_ipBytes);
                            
                            continue;
                        
                        case 12:                           
                            byte _nameLength = _data[i + 1];
                            
                            if (!_nameGiven)                                                                                     //Awaiting instuctions by Instructor about frequency of not use of hostname
                            {
                                try
                                {
                                    for (uint iii = 1; iii <= _nameLength; iii++)
                                    {
                                        _hostName += (char)_data[i + 1 + iii];
                                        _nameGiven = true; 
                                    }
                                }
                                catch
                                {
                                    _nameGiven = false; 
                                }
                            }
                            
                            continue;
                        
                        default: 
                            
                            break;
                    }

                    if (_addressGiven)
                    {
                        Thread.Sleep(3000);
                        try
                        {
                            Ping _ping = new Ping();
                            PingReply _reply = _ping.Send(_requestedIP);

                            if (_reply.Status == IPStatus.Success)
                                _acknowledge = true;
                            else
                                _acknowledge = false;
                        }
                        catch
                        {
                            _acknowledge = false;
                        }
                    }
                }

                if (_addressGiven)
                {
                    Thread.Sleep(3000);
                    try
                    {
                        Ping _ping = new Ping();
                        PingReply _reply = _ping.Send(_requestedIP);

                        if (_reply.Status == IPStatus.Success)
                            _acknowledge = true;
                        else
                            _acknowledge = false;
                    }
                    catch
                    {
                        _acknowledge = false;
                    }
                }

                if (_addressGiven && _acknowledge && _nameGiven)
                {
                    _dummyList.Remove(_dummyList.Find(x => x.hostMac.Contains(_macAddress)));
                    _dummyList.Add(new ModelHost(_macAddress, _requestedIP, _hostName));
                }
                else if (_addressGiven && _acknowledge && !_nameGiven)
                {
                    _dummyList.Remove(_dummyList.Find(x => x.hostMac.Contains(_macAddress)));
                    _dummyList.Add(new ModelHost(_macAddress, _requestedIP));
                }
                else if (!_addressGiven || !_acknowledge && _nameGiven)
                {
                    _dummyList.Remove(_dummyList.Find(x => x.hostMac.Contains(_macAddress)));
                    _dummyList.Add(new ModelHost(_macAddress, _hostName));
                }
                else if (!_addressGiven || !_acknowledge && !_nameGiven)
                {
                    _dummyList.Remove(_dummyList.Find(x => x.hostMac.Contains(_macAddress)));
                    _dummyList.Add(new ModelHost(_macAddress));
                }
            }
            return null;
        }

        private void UpdateLists()
        {
            while (true)
            {
                Thread.Sleep((int)_sleepFor);
                bool _active;

                if (_dummyList != null)
                {
                    foreach (ModelHost _dummy in _dummyList)
                    {
                        Ping _ping = new Ping();

                        try
                        {
                            PingReply _reply = _ping.Send(_dummy.hostIP);

                            if (_reply.Status == IPStatus.Success)
                            {
                                _active = true;
                            }
                            else
                            {
                                _active = false;
                            }
                        }
                        catch
                        {
                            _active = false;
                        }

                        if (!_active)
                        {
                            _dummyList.Remove(_dummy);
                        }
                    }
                }
            }
        }      
        #endregion
    }
}