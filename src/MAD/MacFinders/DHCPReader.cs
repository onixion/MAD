using System;
using System.Collections.Generic;

using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Threading;

using MAD.Helper;
using MAD.Logging;
using MAD.MacFinders;
using MAD.JobSystemCore;

using Amib.Threading; 

namespace MAD.MacFinders
{
    public class DHCPReader                                                          
    {
        #region member
        
        private byte[] _data;

        private uint _sleepFor;

        private static bool _running = false;    

        private Thread _check;
        private Thread _start;
        private SmartThreadPool _pool;

        private UdpClient _listener = new UdpClient(67);

        private IPEndPoint _groupEP = new IPEndPoint(IPAddress.Any, 67);

        private JobSystem _js;

        #endregion 

        #region Constructor

        public DHCPReader(object arg)
        {
            _js = (JobSystem)arg;
            _sleepFor = 25000;                                                     
        }

        #endregion

        #region Methods

        #region public 

        public string Start()
        {
            if (!_running)
            {
                Logger.Log("MacFeeder started listening for Information", Logger.MessageType.INFORM);
                _running = true;
                _pool = new SmartThreadPool();
                _check = new Thread(UpdateLists);
                _start = new Thread(Prog);
                _check.Start();
                _start.Start();

                return null;
            }
            else
            {
                Logger.Log("Tried to start MacFeeder, but was already running", Logger.MessageType.INFORM);
                return "Is already running";
            }
        }

        public string Stop()                                                                                              //Strange bugs with exit.. update: still no idea

        {
            if (_running)
            {
                Logger.Log("MacFeeder stopped listening for Information", Logger.MessageType.INFORM);

                _running = false;

                _pool.Cancel();

                if (_check.ThreadState != ThreadState.Unstarted)
                    _check.Join(TimeSpan.FromSeconds(3));
                else
                    _check.Abort();

                if (_check.ThreadState == ThreadState.Running)
                    _check.Abort();

                if (_start.ThreadState != ThreadState.Unstarted)
                    _start.Join(TimeSpan.FromSeconds(3));
                else
                    _start.Abort();

                if (_start.ThreadState == ThreadState.Running)
                    _start.Abort();

                return null;
            }
            else
            {
                Logger.Log("Tried to stopp MacFeeder, but there wasn't any running", Logger.MessageType.INFORM);
                return "Is already stopped";
            }
       }

        public void ChangeCheckIntervall(uint time)
        {
            Logger.Log("Checkintervall for MacFeeder changed", Logger.MessageType.INFORM);
            _sleepFor = time; 
        }

        #endregion

        #region private

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
                if (NetworkHelper.IsDhcp(_data) && NetworkHelper.IsDhcpRequest(_data))
                {
                    ModelHost _tmpModel = new ModelHost();
                    _tmpModel.hostMac = NetworkHelper.getPhysicalAddressStringFromDhcp(_data);
                    _tmpModel.macGiven = true;

                    for (uint i = NetworkHelper.DHCP_COOKIE_POSITION; i < _data.Length; i++)
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
                                i = 1 + i + _data[i + 1];

                                continue;

                            case 12:

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
                                continue;

                            default:

                                break;
                        }
                    }

                    if (ModelHost.Exists(_tmpModel))
                    {
                        if (_tmpModel.nameGiven)
                            ModelHost.UpdateHost(_tmpModel, _tmpModel.hostIP, _tmpModel.hostName);
                        else
                            ModelHost.UpdateHost(_tmpModel, _tmpModel.hostIP);
                    }
                    else
                    {
                        ModelHost.AddToList(_tmpModel);
                    }

                    _js.SyncNodes(ModelHost.hostList);
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
                ModelHost.PingThroughList();
            }
        }
        #endregion
        #endregion
    }
}