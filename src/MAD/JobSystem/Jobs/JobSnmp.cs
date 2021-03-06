﻿using System;
using System.Net;

using MAD.Helper;
using SnmpSharpNet;

namespace MAD.JobSystemCore
{
    public class JobSnmp : Job
    {
        #region fields
        public uint version;                                                            //required Parameter möglichkeiten 2 oder 3

		public string ifEntryNr = MadConf.conf.snmpInterface;

        public NetworkHelper.securityModel secModel;                                    //required Parameter if version == 3

        private const string _ifEntryString = "1.3.6.1.2.1.2.2.1";
        private static uint _lastRecord = 0;
        private bool firstRun = true; 
        #endregion

        #region methods
        #region Constructors
        public JobSnmp()
            : base(JobType.SnmpCheck, Protocol.UDP)
        { 
            outp.outputs.Add(new OutputDescriptor("TimeIntervall", typeof(int), this.time.jobDelay.delayTime));
            outp.outputs.Add(new OutputDescriptor("Bytes", typeof(uint), false));
        }

        public JobSnmp(uint interfaceNr)
            : base(JobType.SnmpCheck, Protocol.UDP)
        {
            outp.outputs.Add(new OutputDescriptor("TimeIntervall", typeof(int), this.time.jobDelay.delayTime));
            outp.outputs.Add(new OutputDescriptor("Bytes", typeof(uint), false));
            ifEntryNr = interfaceNr.ToString();
        }

        #endregion

        #region FromJob
        public override void Execute(IPAddress _targetAddress)
        {
            UdpTarget _target = new UdpTarget(_targetAddress, 161, 5000, 3);
            switch (version)
            {
                case 1:
                    outp.outState = JobOutput.OutState.Failed;
                    Logging.Logger.Log("No Support for Version 1", Logging.Logger.MessageType.ERROR);
                    break;
                case 2:
                    ExecuteRequest(_target, NetworkHelper.GetSNMPV2Param(NetworkHelper.SNMP_COMMUNITY_STRING));
                    break;
                case 3:
                    if (NetworkHelper.GetSNMPV3Param(_target, NetworkHelper.SNMP_COMMUNITY_STRING, secModel) != null)
                        ExecuteRequest(_target, NetworkHelper.GetSNMPV3Param(_target, NetworkHelper.SNMP_COMMUNITY_STRING, secModel));
                    else
                        Logging.Logger.Log("Wasn't possible to execute snmp Request because of parameter fails", Logging.Logger.MessageType.ERROR);
                    break;
            }
            _target.Close();
        }
        #endregion

        #region Private

        private void ExecuteRequest(UdpTarget _target, AgentParameters _param)
        {
            const string _outOctetsString = "1.3.6.1.2.1.2.2.1.16";

            Oid _outOctets = new Oid(_outOctetsString + "." + ifEntryNr);

            Pdu _octets = new Pdu(PduType.Get);
            _octets.VbList.Add(_outOctets);

            try
            {
                SnmpV2Packet _octetsResult = (SnmpV2Packet)_target.Request(_octets, _param);

                if (_octetsResult.Pdu.ErrorStatus != 0)
                {
                    outp.outState = JobOutput.OutState.Failed;
                    Logging.Logger.Log("Something went wrong with reading the traffic. The Error Status of SNMP was not equal 0, but " + _octetsResult.Pdu.ErrorStatus.ToString(), Logging.Logger.MessageType.ERROR);
                }
                else
                {
                    uint _result = (uint)Convert.ToUInt64(_octetsResult.Pdu.VbList[0].Value.ToString()) - _lastRecord;
                    _lastRecord = (uint)Convert.ToUInt64(_octetsResult.Pdu.VbList[0].Value.ToString());
                    
                    if (firstRun)
                    {
                        outp.GetOutputDesc("Bytes").dataObject = 0;
                    }
                    else
                    {
                        outp.GetOutputDesc("Bytes").dataObject = _result;
                    }

                    outp.outState = JobOutput.OutState.Success;
                    Logging.Logger.Log("Traffic Read reports Success", Logging.Logger.MessageType.DEBUG);
                }
            }
            catch (Exception ex)
            {
                outp.outState = JobOutput.OutState.Failed;
                Logging.Logger.Log("Something went wrong with reading the traffic. The message is: " + ex.Message, Logging.Logger.MessageType.ERROR);
            }

        }

        private void ExecuteRequest(UdpTarget target, SecureAgentParameters param)
        {
            string outOctetsString = "1.3.6.1.2.1.2.2.1.16";

            Oid outOctets = new Oid(outOctetsString + "." + ifEntryNr);

            Pdu octets = new Pdu(PduType.Get);
            octets.VbList.Add(outOctets);

            try
            {
                SnmpV3Packet _octetsResult = (SnmpV3Packet)target.Request(octets, param);

                if (_octetsResult.ScopedPdu.ErrorStatus != 0)
                {
                    outp.outState = JobOutput.OutState.Failed;
                    Logging.Logger.Log("Something went wrong with reading the traffic. The Error Status of SNMP was not equal 0, but " + _octetsResult.ScopedPdu.ErrorStatus.ToString(), Logging.Logger.MessageType.ERROR);
                }
                else
                {
                    uint _result = (uint)Convert.ToUInt64(_octetsResult.Pdu.VbList[0].Value.ToString()) - _lastRecord;
                    _lastRecord = (uint)Convert.ToUInt64(_octetsResult.Pdu.VbList[0].Value.ToString());

                    if (firstRun)
                    {
                        outp.GetOutputDesc("Bytes").dataObject = 0;
                    }
                    else
                    {
                        outp.GetOutputDesc("Bytes").dataObject = _result;
                    }

                    outp.outState = JobOutput.OutState.Success;
                    Logging.Logger.Log("Traffic Read reports Success", Logging.Logger.MessageType.DEBUG);
                }
            }
            catch (Exception ex)
            {
                outp.outState = JobOutput.OutState.Failed;
                Logging.Logger.Log("Something went wrong with reading the traffic. The message is: " + ex.Message, Logging.Logger.MessageType.ERROR);
            }

        }
        #endregion
        #endregion
    }
}
