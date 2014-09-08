using System;
using System.Net;

using MAD.Helper;
using MAD.Logging;
using MAD.JobSystemCore;
using SnmpSharpNet;

namespace MAD.CLICore
{
    public class SnmpInterfaceCommand : Command
    {
        #region fields
        public NetworkHelper.securityModel secModel; 
        
        private uint _version;
        private uint _expectedIfNr = 10;
        
        private IPAddress _targetIP;

        private string _output = "<color><blue>"; 
        private string _ifNr = "1.3.6.1.2.1.2.1.0";
        private string _ifEntryString = "1.3.6.1.2.1.2.2.1";

        private Pdu _index;
        private Pdu _descr;
        private Pdu _type;
        #endregion 

        #region methods
        #region inheritedFromCommand
        public SnmpInterfaceCommand()
        {
            rPar.Add(new ParOption("ver", "VERSION", "Version of SNMP to use.", false, false, new Type[] { typeof(uint) }));
            rPar.Add(new ParOption("ip", "Target-IP", "Target.", false, false, new Type[] { typeof(string) }));
            oPar.Add(new ParOption("p", "PRIV-PROTOCOL", "privPro", false, false, new Type[] { typeof(string) }));
            oPar.Add(new ParOption("a", "AUTH-PROTOCOL", "authProt", false, false, new Type[] { typeof(string) }));
            oPar.Add(new ParOption("s", "SECURITY-LEVEL", "security", false, false, new Type[] { typeof(string) }));
        }

        public override string Execute(int consoleWidth)
        {
            string _errorMessage = "<color><red>ERROR: ";

            try
            {
                ParseParameters();
            }
            catch (ArgumentException ex)
            {
                return _errorMessage + ex.Message;
            }

            
            UdpTarget _target = new UdpTarget(_targetIP, 161, 5000, 3);

            

            if (_version == 2)
            {
                AgentParameters _param = NetworkHelper.GetSNMPV2Param(NetworkHelper.SNMP_COMMUNITY_STRING);

                _expectedIfNr = ParameterCountRequest(_param, _target);

                if (_expectedIfNr == 0)
                    return "<color><red>ERROR: there are no Devices";

                PreparePackets();

                string _output = SendRequests(_target, _param);

                return _output; 
            }
            else if (_version == 3)
            {
                SecureAgentParameters _param = NetworkHelper.GetSNMPV3Param(_target, NetworkHelper.SNMP_COMMUNITY_STRING, secModel);

                _expectedIfNr = ParameterCountRequest(_param, _target);

                if (_expectedIfNr == 0)
                    return "<color><red>ERROR: there are no Devices";

                PreparePackets();

                string _output = SendRequests(_target, _param);

                return _output; 
            }
            else
            {
                Logger.Log("SNMP Request Error. Unsupported version of SNMP", Logger.MessageType.ERROR);
                return _errorMessage + "This is not a supported version of snmp";
            }
        }
        #endregion

        #region private 
        private uint ParameterCountRequest(AgentParameters param, UdpTarget target)
        {
            Oid deviceNr = new Oid(_ifNr);
            Pdu devices = new Pdu(PduType.Get);

            devices.VbList.Add(deviceNr);

            try
            {
                SnmpV2Packet devicesResult = (SnmpV2Packet)target.Request(devices, param);

                if (devicesResult.Pdu.ErrorStatus != 0)
                    return 0;
                else
                    return Convert.ToUInt32(devicesResult.Pdu.VbList[0].Value.ToString());
            }
            catch (Exception)
            {
                return 0;
            }

        }

        private uint ParameterCountRequest(SecureAgentParameters param, UdpTarget target)
        {
            Oid deviceNr = new Oid(_ifNr);
            Pdu devices = new Pdu(PduType.Get);

            devices.VbList.Add(deviceNr);

            try
            {
                SnmpV3Packet devicesResult = (SnmpV3Packet)target.Request(devices, param);

                if (devicesResult.ScopedPdu.ErrorStatus != 0)
                    return 0;
                else
                    return Convert.ToUInt32(devicesResult.ScopedPdu.VbList[0].Value.ToString());
            }
            catch (Exception)
            {
                return 0;
            }

        }

        private void ParseParameters()
        {
            ParseIP();
            ParseVersion();
            if (_version == 3)
            {
                ParseSecurityLevel();
                ParseAuthentificationProtocol();
                ParsePrivacyProtocol();
            }
        }

        private void ParseIP()
        {
            string _buffer = (string)pars.GetPar("ip").argValues[0];
            if (!IpAddress.IsIP(_buffer))
            {
                Logger.Log("SNMP device Request has problems with Parsing IPAddress", Logger.MessageType.ERROR);
                throw new ArgumentException("Can't parse IP Address");
            }
            else
                _targetIP = IPAddress.Parse(_buffer);
        }

        private void ParseVersion()
        {
            _version = (uint)pars.GetPar("ver").argValues[0];
        }

        private void ParseSecurityLevel()
        {
            string _buffer = (string)pars.GetPar("s").argValues[0];
            switch (_buffer)
            {
                case "authNoPriv":
                    secModel.securityLevel = NetworkHelper.securityLvl.authNoPriv;
                    if (!(OParUsed("a") && !OParUsed("p")))
                    {
                        Logger.Log("SNMP Request for devices failt because off wrong parameters.", Logger.MessageType.ERROR);
                        throw new ArgumentException("Wrong Parameters Used");
                    }
                    break;
                case "authPriv":
                    secModel.securityLevel = NetworkHelper.securityLvl.authPriv;
                    if (!(OParUsed("a") && OParUsed("p")))
                    {
                        Logger.Log("SNMP Request for devices failt because off wrong parameters.", Logger.MessageType.ERROR);
                        throw new ArgumentException("Wrong Parameters Used");
                    }
                    break;
                case "noAuthNoPriv":
                    secModel.securityLevel = NetworkHelper.securityLvl.noAuthNoPriv;
                    if (!(!OParUsed("a") && !OParUsed("p")))
                    {
                        Logger.Log("SNMP Request for devices failt because off wrong parameters.", Logger.MessageType.ERROR);
                        throw new ArgumentException("Wrong Parameters Used");
                    }
                    break;
                default:
                    Logger.Log("Unknown Error with SNMP device request parameters", Logger.MessageType.ERROR);
                    throw new ArgumentException("Unknown Error with Parameters");
            }
        }

        private void ParseAuthentificationProtocol()
        {
            string _buffer = (string)pars.GetPar("a").argValues[0];
            switch (_buffer)
            {
                case "md5":
                    secModel.authentificationProtocol = NetworkHelper.snmpProtocols.MD5;
                    break;
                case "sha":
                    secModel.authentificationProtocol = NetworkHelper.snmpProtocols.SHA;
                    break;
                default:
                    Logger.Log("Wrong algorithm used for snmp device request authentification", Logger.MessageType.ERROR);
                    throw new ArgumentException("Wrong algorithm used");
            }

        }

        private void ParsePrivacyProtocol()
        {
            string _buffer = (string)pars.GetPar("p").argValues[0];
            switch (_buffer)
            {
                case "aes":
                    secModel.privacyProtocol = NetworkHelper.snmpProtocols.AES;
                    break;
                case "des":
                    secModel.privacyProtocol = NetworkHelper.snmpProtocols.DES;
                    break;
                default:
                    Logger.Log("Wrong algrorithm used for snmp device request privacy", Logger.MessageType.ERROR);
                    throw new ArgumentException("Wrong algorithm used");
            }
        }

        private void PreparePackets()
        {
            Oid _ifIndex = new Oid(_ifEntryString + ".1");
            Oid _ifDescr = new Oid(_ifEntryString + ".2");
            Oid _ifType = new Oid(_ifEntryString + ".3");

            _index = new Pdu(PduType.GetBulk);
            _index.VbList.Add(_ifIndex);
            _index.MaxRepetitions = (int)_expectedIfNr;

            _descr = new Pdu(PduType.GetBulk);
            _descr.VbList.Add(_ifDescr);
            _descr.MaxRepetitions = (int)_expectedIfNr;

            _type = new Pdu(PduType.GetBulk);
            _type.VbList.Add(_ifType);
            _type.MaxRepetitions = (int)_expectedIfNr;
        }

        private string SendRequests(UdpTarget target, AgentParameters param)
        {
            try
            {
                SnmpV2Packet _indexResult = (SnmpV2Packet)target.Request(_index, param);
                SnmpV2Packet _descrResult = (SnmpV2Packet)target.Request(_descr, param);
                SnmpV2Packet _typeResult = (SnmpV2Packet)target.Request(_type, param);

                if (_indexResult.Pdu.ErrorStatus != 0 || _descrResult.Pdu.ErrorStatus != 0 || _typeResult.Pdu.ErrorStatus != 0)
                {
                    Logger.Log("SNMP Request Error", Logger.MessageType.ERROR);
                    return "<color><red>ERROR: packet error status = " + _indexResult.Pdu.ErrorStatus.ToString() + " and not ZERO";
                }
                else
                {
                    for (int i = 0; i < _indexResult.Pdu.VbCount; i++)
                    {
                        _output += "Interface " + _indexResult.Pdu.VbList[i].Value.ToString() + ": Type " + _typeResult.Pdu.VbList[i].Value.ToString() + " -> " + _descrResult.Pdu.VbList[i].Value.ToString() + "\n";
                    }
                    Logger.Log("Successfully performed SNMP Device Request", Logger.MessageType.INFORM);
                    return _output;
                }
            }
            catch (Exception)
            {
                Logger.Log("SNMP Request Error", Logger.MessageType.ERROR);
                return "<color><red>ERROR: sending requests failed";
            }
        }

        private string SendRequests(UdpTarget target, SecureAgentParameters param)
        {
            try
            {
                SnmpV3Packet indexResult = (SnmpV3Packet)target.Request(_index, param);
                SnmpV3Packet descrResult = (SnmpV3Packet)target.Request(_descr, param);
                SnmpV3Packet typeResult = (SnmpV3Packet)target.Request(_type, param);

                if (indexResult.ScopedPdu.ErrorStatus != 0 || descrResult.ScopedPdu.ErrorStatus != 0 || typeResult.ScopedPdu.ErrorStatus != 0)
                {
                    Logger.Log("SNMP Request Error", Logger.MessageType.ERROR);
                    return "<color><red>ERROR: packet error status = " + indexResult.ScopedPdu.ErrorStatus.ToString() + " and not ZERO";
                }
                else
                {
                    for (int i = 0; i < indexResult.ScopedPdu.VbCount; i++)
                    {
                        _output += "Interface " + indexResult.ScopedPdu.VbList[i].Value.ToString() + ": Type " + typeResult.ScopedPdu.VbList[i].Value.ToString() + " -> " + descrResult.ScopedPdu.VbList[i].Value.ToString() + "\n";
                    }
                    Logger.Log("Successfully performed SNMP Device Request", Logger.MessageType.INFORM);
                    return _output;
                }
            }
            catch (Exception)
            {
                Logger.Log("SNMP Request Error", Logger.MessageType.ERROR);
                return "<color><red>ERROR: sending requests failed";
            }
        }
        #endregion 
        #endregion
    }
}
