using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

using System.Net;
using System.Net.NetworkInformation;

using MAD.Helper;

namespace MAD.JobSystemCore
{
    [Serializable]
    public class JobHostDetect : Job                                                                           //This job is for detecting existing hosts in a network
    {                                                                                                   //Further commenting will happen if someone can't read my code.. i am to lazy to do it now..
        #region members
                                                                        
        public IPAddress Subnetmask;                                                                    

        private List<IPAddress> _hostAddresses = new List<IPAddress>();
        private NetworkHelper _helper = new NetworkHelper();
        private string _tmp = "";

        #endregion

        #region constructors

        public JobHostDetect()
            : base("NULL", JobType.HostDetect, new JobTime())
        {
            //this.Net = IPAddress.Parse("192.168.0.0"); <- This need to be implemented in the execute-method.
            this.Subnetmask = IPAddress.Parse("255.255.255.0");
        }

        public JobHostDetect(string jobName, JobType jobType, JobTime jobTime, IPAddress Subnetmask)
            : base(jobName, jobType, jobTime)
        {
            this.Subnetmask = Subnetmask;
        }

        public JobHostDetect(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            throw new NotImplementedException(); // this job is not ready for this
        }

        #endregion

        #region methodes

        public override void Execute(IPAddress targetAddress)
        {
            byte[] _hostBytes = _helper.GetHosts(Subnetmask);
            //byte[] _netBytes = Net.GetAddressBytes();
            byte[] _netBytes = targetAddress.GetAddressBytes();

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(_hostBytes);
                Array.Reverse(_netBytes);
            }

            uint _netPart = BitConverter.ToUInt32(_netBytes, 0);
            uint _broadcastHost = BitConverter.ToUInt32(_hostBytes, 0);

            for (uint _cnt = 1; _cnt < _broadcastHost; _cnt++)
            {
                Ping _ping = new Ping();

                uint _target = _netPart + _cnt;
                byte[] _targetByte = BitConverter.GetBytes(_target);

                Array.Reverse(_targetByte);

                IPAddress _targetIP = new IPAddress(_targetByte);
                try
                {
                    PingReply _reply = _ping.Send(_targetIP, 50);

                    if (_reply.Status == IPStatus.Success)
                    {
                        if (!_hostAddresses.Contains(_reply.Address))
                        {
                            _hostAddresses.Add(_reply.Address);
                        }
                    }

                    outState = OutState.Success;
                }
                catch (Exception)
                {
                    outState = OutState.Exception;
                }

                _ping.Dispose();
            }

        }

        protected override string JobStatus()
        {
            _tmp = "";
            _hostAddresses.ForEach(GiveAddresses);

            return (_tmp);
        }

        private void GiveAddresses(IPAddress _tmpAdr)
        {

            _tmp += "IP: " + _tmpAdr.ToString() + "\n";
            //jobOutput.jobOutputDescriptors.Add(new JobOutputDescriptor("Host", typeof(IPAddress), _tmpAdr));
        }

        #region for serialization

        public override void GetObjectDataJobSpecific(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }

        #endregion

        #endregion
    }
}
