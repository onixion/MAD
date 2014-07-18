using System;
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

        public List<ModelHost> _dummyList = new List<ModelHost>();

        private Thread _check;
        private Thread _start;
        private SmartThreadPool _pool;

        private UdpClient _listener = new UdpClient(67);

        private IPEndPoint _groupEP = new IPEndPoint(IPAddress.Any, 67);

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

        public void Stop()                                                                                              //Strange bugs with exit.. 

        {
            _running = false;

            _pool.Cancel();

            if (_check.ThreadState != ThreadState.Unstarted)
                _check.Join(TimeSpan.FromSeconds(3));
            else
                _check.Abort();

            if (_start.ThreadState != ThreadState.Unstarted)
                _start.Join(TimeSpan.FromSeconds(3));
            else
                _start.Abort();   
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
            if (_running)
            {
                _data = _listener.Receive(ref _groupEP);                                                               
                //_listener.Close();
            }
            else
            {
                _listener.Close();
            }
        }

        private void StartThread()
        {
            _pool.QueueWorkItem(ProcessData);
            _pool.Start();
        }

        private void ProcessData()
        {
            try
            {
                if (_helper.IsDhcp(_data) && _helper.IsDhcpRequest(_data))
                {
                    ModelHost _tmpModel = new ModelHost();
                    _tmpModel.hostMac = _helper.getMacString(_data);
                    bool noName = false; 

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

                                _tmpModel.ipGiven = true;
                                _tmpModel.hostIP = new IPAddress(_ipBytes);

                                continue;

                            case 55:
                                noName = true;

                                continue;

                            case 12:
                                if (!noName)
                                {
                                    byte _nameLength = _data[i + 1];
                                    _tmpModel.hostName = "";
                                    try
                                    {
                                        for (uint iii = 1; iii <= _nameLength; iii++)
                                        {
                                            _tmpModel.hostName += (char)_data[i + 1 + iii];
                                            _tmpModel.nameGiven = true;
                                        }
                                    }
                                    catch
                                    {

                                    }
                                }

                                continue;

                            default:

                                break;
                        }
                    }

                    if (_tmpModel.ipGiven)
                    {
                        Thread.Sleep(3000);
                        try
                        {
                            Ping _ping = new Ping();
                            PingReply _reply = _ping.Send(_tmpModel.hostIP);

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

                    _dummyList.Remove(_dummyList.Find(x => x.hostMac.Contains(_tmpModel.hostMac)));
                    _dummyList.Add(_tmpModel);
                    _tmpModel.ManuallyIncreaseCount();

                    /*
                    if (_tmpModel.ipGiven && _acknowledge && _tmpModel.nameGiven)
                    {
                        if(_dummyList != null)
                            _dummyList.Remove(_dummyList.Find(x => x.hostMac.Contains(_tmpModel.hostMac)));
                        _dummyList.Add(new ModelHost(_macAddress, _requestedIP, _hostName));
                    }
                    else if (_tmpModel.ipGiven && _acknowledge && !_tmpModel.nameGiven)
                    {
                        if(_dummyList != null)
                            _dummyList.Remove(_dummyList.Find(x => x.hostMac.Contains(_tmpModel.hostMac)));
                        _dummyList.Add(new ModelHost(_macAddress, _requestedIP));
                    }
                    else if (!_tmpModel.ipGiven || !_acknowledge && _tmpModel.nameGiven)
                    {
                        if(_dummyList != null)
                            _dummyList.Remove(_dummyList.Find(x => x.hostMac.Contains(_tmpModel.hostMac)));
                        _dummyList.Add(new ModelHost(_macAddress, _hostName));
                    }
                    else if (!_tmpModel.ipGiven || !_acknowledge && !_tmpModel.nameGiven)
                    {
                        if(_dummyList != null)
                            _dummyList.Remove(_dummyList.Find(x => x.hostMac.Contains(_macAddress)));
                        _dummyList.Add(new ModelHost(_macAddress));
                    }
                    */
                }
            }
            catch
            {
                
            }
        }

        private void UpdateLists()
        {
            while (true)
            {
                Thread.Sleep((int)_sleepFor);
                bool _active;

                if (_dummyList != null)
                {
                    for (int i = 0; i < _dummyList.Count; i++)
                    {
                        ModelHost _dummy = _dummyList[i];
                        if (_dummy.hostIP != null)
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
                                _dummy.ManuallyDecreaseCount();
                            }
                        }
                    }
                }
            }
        }

        public string PrintLists()
        {
            string _output = "";

            if (_dummyList != null)
            {
                foreach (ModelHost _dummy in _dummyList)
                {
                    _output += "Host " + _dummy.ID.ToString();
                    _output += "\n MAC-Address: " + _dummy.hostMac;

                    if (_dummy.hostName != null)
                        _output += "\n Host Name: " + _dummy.hostName;
                    else
                        _output += "\n Host Name: NA..";

                    if (_dummy.hostIP != null)
                        _output += "\n IP-Address: " + _dummy.hostIP.ToString();
                    else
                        _output += "\n IP-Address: NA..";

                    _output += "\n \n";
                }
            }
            return _output;  
        }
        #endregion
    }
}