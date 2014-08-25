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

        private bool working;
        private string auth = "MAD";
        private string priv = "MAD";
        private string communityString = "public";

        private JobSnmp _foo;

        public JobCheckSnmp()
            : base(JobType.ServiceCheck)
        { }

        protected override string JobStatus()
        {
            string _tmp = "";

            if (working)
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
            UdpTarget target = new UdpTarget(targetAddress, 161, 5000, 3);

            switch (version)
            {
                case 1:
                    //NotImplemented Fehlermeldung
                    break;
                case 2:
                    SnmpV2Handling(target);
                    break;
                case 3:
                    SnmpV3Handling(target);
                    break;
            }
        }

        public void SnmpV2Handling(UdpTarget target)
        {
            OctetString community = new OctetString(communityString);
            AgentParameters param = new AgentParameters(community);

            param.Version = SnmpVersion.Ver2;

            ExecuteRequest(target, param);
        }

        public void SnmpV3Handling(UdpTarget target)
        {
            SecureAgentParameters param = new SecureAgentParameters();

            if (!target.Discovery(param))
            {
                working = false;
                return; 
            }

            switch (security)
            {
                case NetworkHelper.securityLvl.noAuthNoPriv:
                    param.noAuthNoPriv(communityString);

                    break;
                case NetworkHelper.securityLvl.authNoPriv:
                    if (authProt == NetworkHelper.snmpProtokolls.MD5)
                        param.authNoPriv(communityString, AuthenticationDigests.MD5, auth);
                    else if (authProt == NetworkHelper.snmpProtokolls.SHA)
                        param.authNoPriv(communityString, AuthenticationDigests.SHA1, auth);

                    break;
                case NetworkHelper.securityLvl.authPriv:
                    if (authProt == NetworkHelper.snmpProtokolls.MD5 && privProt == NetworkHelper.snmpProtokolls.AES)
                        param.authPriv(communityString, AuthenticationDigests.MD5, auth, PrivacyProtocols.AES128, priv);
                    else if (authProt == NetworkHelper.snmpProtokolls.MD5 && privProt == NetworkHelper.snmpProtokolls.DES)
                        param.authPriv(communityString, AuthenticationDigests.MD5, auth, PrivacyProtocols.DES, priv);
                    else if (authProt == NetworkHelper.snmpProtokolls.SHA && privProt == NetworkHelper.snmpProtokolls.AES)
                        param.authPriv(communityString, AuthenticationDigests.SHA1, auth, PrivacyProtocols.AES128, priv);
                    else if (authProt == NetworkHelper.snmpProtokolls.SHA && privProt == NetworkHelper.snmpProtokolls.DES)
                        param.authPriv(communityString, AuthenticationDigests.SHA1, auth, PrivacyProtocols.DES, priv);

                    break;
            }

            ExecuteRequest(target, param);
        }

        private void ExecuteRequest(UdpTarget target, AgentParameters param)
        {
            Oid name = new Oid("1.3.6.1.2.1.1.5.0");

            Pdu namePacket = new Pdu(PduType.Get);
            namePacket.VbList.Add(name);

            try
            {
                SnmpV2Packet nameResult = (SnmpV2Packet)target.Request(namePacket, param);

                if (nameResult.Pdu.VbList[0].Value.ToString().ToLowerInvariant() == System.Environment.MachineName.ToLowerInvariant())
                    working = true;
                else
                    working = false;
            }
            catch (Exception)
            {
                working = false;
            }
        }

        private void ExecuteRequest(UdpTarget target, SecureAgentParameters param)
        {
            Oid name = new Oid("1.3.6.1.2.1.1.5.0");

            Pdu namePacket = new Pdu(PduType.Get);
            namePacket.VbList.Add(name);

            try
            {
                SnmpV3Packet nameResult = (SnmpV3Packet)target.Request(namePacket, param);

                if (nameResult.ScopedPdu.VbList[0].Value.ToString().ToLowerInvariant() == System.Environment.MachineName.ToLowerInvariant())
                    working = true;
                else
                    working = false;
            }
            catch (Exception)
            {
                working = false;
            }
        }
    }
}
