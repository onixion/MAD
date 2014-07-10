using System;
using System.Collections.Generic;

using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Threading;

using MAD.Helper;

namespace MAD.DHCPReader
{
    public class DHCPReader
    {
        #region member

        private static NetworkHelper _helper = new NetworkHelper();
        private IPEndPoint _groupEP = new IPEndPoint(IPAddress.Any, 67);

        private byte[] _data;

        private bool _acknowledge; 
        private bool _addressGiven;
        private bool _nameGiven = false;

        private string _hostName = "";
        private IPAddress _requestedIP;

        public List<ModelHost> _dummyList;

        #endregion 

        #region Methods

        public void Execute()
        {
            while (true)
            {
                CatchDHCP();
                StartThread();
            }
        }

        private void CatchDHCP()
        {
            UdpClient _listener = new UdpClient(67);
            _data = _listener.Receive(ref _groupEP);
            _listener.Close();
        }

        private void StartThread()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessData));
        }

        private void ProcessData(Object stateInfo)
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
        }
        #endregion
    }
}