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
        private uint _version;
        private uint _expectedIfNr = 10;

        public NetworkHelper.securityModel secModel; 

        private IPAddress _targetIP;

        private string _output = "<color><blue>"; 

        private string _ifEntryString = "1.3.6.1.2.1.2.2.1";

        public SnmpInterfaceCommand()
        {
            rPar.Add(new ParOption("ver", "VERSION", "Version of SNMP to use.", false, false, new Type[] { typeof(uint) }));
            rPar.Add(new ParOption("ip", "Target-IP", "Target.", false, false, new Type[] { typeof(string) }));
            oPar.Add(new ParOption("p", "", "privPro", false, false, new Type[] { typeof(string) }));
            oPar.Add(new ParOption("a", "", "authProt", false, false, new Type[] { typeof(string) }));
            oPar.Add(new ParOption("s", "", "security", false, false, new Type[] { typeof(string) }));
        }

        public override string Execute(int consoleWidth)
        {
            
            string _buffer; 

            if (_version == 3)
            {
               

                


                // PARSE authProt
                
                // PARSE security
                
            }

            // PARSE ip
            

            UdpTarget target = new UdpTarget(_targetIP, 161, 5000, 3);

            // if optional par is used.

            // PARSE privProt
            

            if (_version == 2)
            { 
                // VER 2
                OctetString community = new OctetString(_communityString);
                AgentParameters param = new AgentParameters(community);

                param.Version = SnmpVersion.Ver2;

                Oid ifIndex = new Oid(_ifEntryString + ".1");
                Oid ifDescr = new Oid(_ifEntryString + ".2");
                Oid ifType = new Oid(_ifEntryString + ".3");

                _expectedIfNr = ParameterCountRequest(param, target);

                if (_expectedIfNr == 0)
                    return "<color><red>ERROR: there are no Devices";

                Pdu index = new Pdu(PduType.GetBulk);
                index.VbList.Add(ifIndex);
                index.MaxRepetitions = (int)_expectedIfNr;

                Pdu descr = new Pdu(PduType.GetBulk);
                descr.VbList.Add(ifDescr);
                descr.MaxRepetitions = (int)_expectedIfNr;

                Pdu type = new Pdu(PduType.GetBulk);
                type.VbList.Add(ifType);
                type.MaxRepetitions = (int)_expectedIfNr;

                try
                {
                    SnmpV2Packet indexResult = (SnmpV2Packet)target.Request(index, param);
                    SnmpV2Packet descrResult = (SnmpV2Packet)target.Request(descr, param);
                    SnmpV2Packet typeResult = (SnmpV2Packet)target.Request(type, param);

                    if (indexResult.Pdu.ErrorStatus != 0 || descrResult.Pdu.ErrorStatus != 0 || typeResult.Pdu.ErrorStatus != 0)
                    {
                        Logger.Log("SNMP Request Error", Logger.MessageType.ERROR);
                        return "<color><red>ERROR: packet error status = " + indexResult.Pdu.ErrorStatus.ToString() + " and not ZERO";
                    }
                    else
                    {
                        for (int i = 0; i < indexResult.Pdu.VbCount; i++)
                        {
                            _output += "Interface " + indexResult.Pdu.VbList[i].Value.ToString() + ": Type " + typeResult.Pdu.VbList[i].Value.ToString() + " -> " + descrResult.Pdu.VbList[i].Value.ToString() + "\n";
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
            else if (_version == 3)
            {
                // VER 3
                SecureAgentParameters param = new SecureAgentParameters();

                if (!target.Discovery(param))
                {
                    Logger.Log("SNMP Request was not able to syncronize", Logger.MessageType.ERROR);
                    return "<color><red>ERROR: Client was not able to syncronize with Agend";
                }

                switch (security)
                {
                    case NetworkHelper.securityLvl.noAuthNoPriv:
                        param.noAuthNoPriv(_communityString);

                        break;
                    case NetworkHelper.securityLvl.authNoPriv:
                        if (authProt == NetworkHelper.snmpProtocols.MD5)
                            param.authNoPriv(_communityString, AuthenticationDigests.MD5, _auth);
                        else if (authProt == NetworkHelper.snmpProtocols.SHA)
                            param.authNoPriv(_communityString, AuthenticationDigests.SHA1, _auth);

                        break;
                    case NetworkHelper.securityLvl.authPriv:
                        if (authProt == NetworkHelper.snmpProtocols.MD5 && privProt == NetworkHelper.snmpProtocols.AES)
                            param.authPriv(_communityString, AuthenticationDigests.MD5, _auth, PrivacyProtocols.AES128, _priv);
                        else if (authProt == NetworkHelper.snmpProtocols.MD5 && privProt == NetworkHelper.snmpProtocols.DES)
                            param.authPriv(_communityString, AuthenticationDigests.MD5, _auth, PrivacyProtocols.DES, _priv);
                        else if (authProt == NetworkHelper.snmpProtocols.SHA && privProt == NetworkHelper.snmpProtocols.AES)
                            param.authPriv(_communityString, AuthenticationDigests.SHA1, _auth, PrivacyProtocols.AES128, _priv);
                        else if (authProt == NetworkHelper.snmpProtocols.SHA && privProt == NetworkHelper.snmpProtocols.DES)
                            param.authPriv(_communityString, AuthenticationDigests.SHA1, _auth, PrivacyProtocols.DES, _priv);

                        break;
                }

                _expectedIfNr = ParameterCountRequest(param, target);

                if (_expectedIfNr == 0)
                    return "<color><red>ERROR: there are no Devices";

                Oid ifIndex = new Oid(_ifEntryString + ".1");
                Oid ifDescr = new Oid(_ifEntryString + ".2");
                Oid ifType = new Oid(_ifEntryString + ".3");

                Pdu index = new Pdu(PduType.GetBulk);
                index.VbList.Add(ifIndex);
                index.MaxRepetitions = (int)_expectedIfNr;

                Pdu descr = new Pdu(PduType.GetBulk);
                descr.VbList.Add(ifDescr);
                descr.MaxRepetitions = (int)_expectedIfNr;

                Pdu type = new Pdu(PduType.GetBulk);
                type.VbList.Add(ifType);
                type.MaxRepetitions = (int)_expectedIfNr;

                try
                {
                    SnmpV3Packet indexResult = (SnmpV3Packet)target.Request(index, param);
                    SnmpV3Packet descrResult = (SnmpV3Packet)target.Request(descr, param);
                    SnmpV3Packet typeResult = (SnmpV3Packet)target.Request(type, param);

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
            else
            {
                Logger.Log("SNMP Request Error. Unsupported version of SNMP", Logger.MessageType.ERROR);
                return "ERROR: This is not a supported version of snmp";
            }
        }

        private uint ParameterCountRequest(AgentParameters param, UdpTarget target)
        {
            Oid deviceNr = new Oid(_ifEntryString);
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
            Oid deviceNr = new Oid(_ifEntryString);
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
                return "<color><red>Could not parse ip-address!";
            }
            else
                _targetIP = IPAddress.Parse(_buffer);
        }

        private void ParseVersion()
        {
            _version = (uint)pars.GetPar("v").argValues[0];
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
                        return "<color><red>ERROR: Wrong Parameters";
                    }
                    break;
                case "authPriv":
                    secModel.securityLevel = NetworkHelper.securityLvl.authPriv;
                    if (!(OParUsed("a") && OParUsed("p")))
                    {
                        Logger.Log("SNMP Request for devices failt because off wrong parameters.", Logger.MessageType.ERROR);
                        return "<color><red>ERROR: Wrong Parameters";
                    }
                    break;
                case "noAuthNoPriv":
                    secModel.securityLevel = NetworkHelper.securityLvl.noAuthNoPriv;
                    if (!(!OParUsed("a") && !OParUsed("p")))
                    {
                        Logger.Log("SNMP Request for devices failt because off wrong parameters.", Logger.MessageType.ERROR);
                        return "<color><red>ERROR: Wrong Parameters";
                    }
                    break;
                default:
                    Logger.Log("Unknown Error with SNMP device request parameters", Logger.MessageType.ERROR);
                    return "<color><red>ERROR";
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
                    Logger.Log("Unknown Error with SNMP device request parameters", Logger.MessageType.ERROR);
                    return "<color><red>ERROR:";
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
                    Logger.Log("Unknown Error with SNMP device request parameters", Logger.MessageType.ERROR);
                    return "<color><red>ERROR:";
            }
        }
    }
}
