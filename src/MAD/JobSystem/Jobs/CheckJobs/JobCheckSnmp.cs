using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

using SnmpSharpNet;

using MAD.Helper;
using MAD.Logging;

namespace MAD.JobSystemCore
{
    class JobCheckSnmp : Job
    {
        #region fields
        public uint version; //required parameter
        public NetworkHelper.securityModel secModel; //required Parameter if version == 3
        #endregion

        #region constructor
        public JobCheckSnmp()
            : base(JobType.ServiceCheck, Protocol.UDP)
        {
            outp.outputs.Add(new OutputDescriptor("Additional Information", typeof(string), false));
        }
        #endregion

        #region functions
        public override void Execute(System.Net.IPAddress targetAddress)
        {
            Logger.Log("Executing SNMP Check", Logger.MessageType.INFORM);

            UdpTarget _target = new UdpTarget(targetAddress, 161, 5000, 3);

            switch (version)
            {
                case 1:
                    outp.outState = JobOutput.OutState.Exception;
                    outp.GetOutputDesc("Additional Information").dataObject = "Version 1 is not Supported!";
                    Logger.Log("SNMPv1 is NOT Supportet!", Logger.MessageType.ERROR);
                    break;
                case 2:
                    ExecuteRequest(_target, NetworkHelper.GetSNMPV2Param(NetworkHelper.SNMP_COMMUNITY_STRING));
                    break;
                case 3:
                    if (NetworkHelper.GetSNMPV3Param(_target, NetworkHelper.SNMP_COMMUNITY_STRING, secModel) != null)
                        ExecuteRequest(_target, NetworkHelper.GetSNMPV3Param(_target, NetworkHelper.SNMP_COMMUNITY_STRING, secModel));
                    else
                        outp.outState = JobOutput.OutState.Failed;
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

                outp.outState = JobOutput.OutState.Success;
            }
            catch (Exception)
            {
                Logger.Log("SNMP Service seems to be dead", Logger.MessageType.ERROR);
                outp.outState = JobOutput.OutState.Failed;
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
                {
                    Logger.Log("SNMP Service seems to work", Logger.MessageType.INFORM);
                    outp.outState = JobOutput.OutState.Success;
                }
                else
                {
                    Logger.Log("SNMP Service seems to be dead", Logger.MessageType.ERROR);
                    outp.outState = JobOutput.OutState.Failed;
                }
            }
            catch (Exception)
            {
                Logger.Log("SNMP Service seems to be dead", Logger.MessageType.ERROR);
                outp.outState = JobOutput.OutState.Exception;
            }
        }
        #endregion
    }
}
