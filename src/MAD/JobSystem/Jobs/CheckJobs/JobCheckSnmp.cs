using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using SnmpSharpNet;
using MAD.Helper;

namespace MAD.JobSystemCore
{
    class JobCheckSnmp : Job
    {
        public uint version; //required parameter
        public NetworkHelper.securityModel secModel; //required Parameter if version == 3

        private bool _working;

        private JobSnmp _foo;

        public JobCheckSnmp()
            : base(JobType.ServiceCheck)
        { }

        protected override string JobStatus()
        {
            string _tmp = "";

            if (_working)
            {
                _tmp += "SNMP is working";
                outp.outState = JobOutput.OutState.Success;
            }
            else
            {
                _tmp += "SNMP seems to be dead";
                outp.outState = JobOutput.OutState.Failed;
            }

            return (_tmp);
        }

        public override void Execute(System.Net.IPAddress targetAddress)
        {
            UdpTarget _target = new UdpTarget(targetAddress, 161, 5000, 3);

            switch (version)
            {
                case 1:
                    //NotImplemented Fehlermeldung
                    break;
                case 2:
                    ExecuteRequest(_target, NetworkHelper.GetSNMPV2Param(NetworkHelper.SNMP_COMMUNITY_STRING));
                    break;
                case 3:
                    if (NetworkHelper.GetSNMPV3Param(_target, NetworkHelper.SNMP_COMMUNITY_STRING, secModel) != null)
                        ExecuteRequest(_target, NetworkHelper.GetSNMPV3Param(_target, NetworkHelper.SNMP_COMMUNITY_STRING, secModel));
                    else
                        _working = false; 
                    break;
            }
        }

        private void ExecuteRequest(UdpTarget target, AgentParameters param)
        {
            Oid _name = new Oid("1.3.6.1.2.1.1.5.0");

            Pdu _namePacket = new Pdu(PduType.Get);
            _namePacket.VbList.Add(_name);

            try
            {
                SnmpV2Packet _nameResult = (SnmpV2Packet)target.Request(_namePacket, param);

                if (_nameResult.Pdu.VbList[0].Value.ToString().ToLowerInvariant() == System.Environment.MachineName.ToLowerInvariant())
                    _working = true;
                else
                    _working = false;
            }
            catch (Exception)
            {
                _working = false;
            }
        }

        private void ExecuteRequest(UdpTarget target, SecureAgentParameters param)
        {
            Oid _name = new Oid("1.3.6.1.2.1.1.5.0");

            Pdu _namePacket = new Pdu(PduType.Get);
            _namePacket.VbList.Add(_name);

            try
            {
                SnmpV3Packet nameResult = (SnmpV3Packet)target.Request(_namePacket, param);

                if (nameResult.ScopedPdu.VbList[0].Value.ToString().ToLowerInvariant() == System.Environment.MachineName.ToLowerInvariant())
                    _working = true;
                else
                    _working = false;
            }
            catch (Exception)
            {
                _working = false;
            }
        }
    }
}
