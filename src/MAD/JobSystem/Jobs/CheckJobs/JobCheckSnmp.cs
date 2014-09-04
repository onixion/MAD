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
        public NetworkHelper.snmpProtokolls privProt;  //required parameter if version == 3 && security == AuthPriv
        public NetworkHelper.snmpProtokolls authProt;  //required parameter if version == 3 && security != noAuthNoPriv
        public NetworkHelper.securityLvl security;     //required parameter if version == 3

        private bool _working;
        private string _auth = "MAD";
        private string _priv = "MAD";
        private string _communityString = "public";

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
                    SnmpV2Handling(_target);
                    break;
                case 3:
                    SnmpV3Handling(_target);
                    break;
            }
        }

        public void SnmpV2Handling(UdpTarget target)
        {
            OctetString _community = new OctetString(_communityString);
            AgentParameters _param = new AgentParameters(_community);

            _param.Version = SnmpVersion.Ver2;

            ExecuteRequest(target, _param);
        }

        public void SnmpV3Handling(UdpTarget target)
        {
            SecureAgentParameters _param = new SecureAgentParameters();

            if (!target.Discovery(_param))
            {
                _working = false;
                return; 
            }

            switch (security)
            {
                case NetworkHelper.securityLvl.noAuthNoPriv:
                    _param.noAuthNoPriv(_communityString);

                    break;
                case NetworkHelper.securityLvl.authNoPriv:
                    if (authProt == NetworkHelper.snmpProtokolls.MD5)
                        _param.authNoPriv(_communityString, AuthenticationDigests.MD5, _auth);
                    else if (authProt == NetworkHelper.snmpProtokolls.SHA)
                        _param.authNoPriv(_communityString, AuthenticationDigests.SHA1, _auth);

                    break;
                case NetworkHelper.securityLvl.authPriv:
                    if (authProt == NetworkHelper.snmpProtokolls.MD5 && privProt == NetworkHelper.snmpProtokolls.AES)
                        _param.authPriv(_communityString, AuthenticationDigests.MD5, _auth, PrivacyProtocols.AES128, _priv);
                    else if (authProt == NetworkHelper.snmpProtokolls.MD5 && privProt == NetworkHelper.snmpProtokolls.DES)
                        _param.authPriv(_communityString, AuthenticationDigests.MD5, _auth, PrivacyProtocols.DES, _priv);
                    else if (authProt == NetworkHelper.snmpProtokolls.SHA && privProt == NetworkHelper.snmpProtokolls.AES)
                        _param.authPriv(_communityString, AuthenticationDigests.SHA1, _auth, PrivacyProtocols.AES128, _priv);
                    else if (authProt == NetworkHelper.snmpProtokolls.SHA && privProt == NetworkHelper.snmpProtokolls.DES)
                        _param.authPriv(_communityString, AuthenticationDigests.SHA1, _auth, PrivacyProtocols.DES, _priv);

                    break;
            }

            ExecuteRequest(target, _param);
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
