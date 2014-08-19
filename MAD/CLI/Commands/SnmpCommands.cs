using System;
using System.Net;

using MAD.Helper;
using MAD.JobSystemCore;
using SnmpSharpNet;

namespace MAD.CLICore
{
    public class SnmpInterfaceCommand : Command
    {
        private uint version;
        private uint expectedIfNr = 10;

        public NetworkHelper.snmpProtokolls privProt;          
        public NetworkHelper.snmpProtokolls authProt;
        public NetworkHelper.securityLvl security;

        private IPAddress targetIP;

        private const string communityString = "public";
        private const string priv = "MAD";
        private const string auth = "MAD";
        private string _output = "<color><blue>"; 

        private string ifEntryString = "1.3.6.1.2.1.2.2.1";

        public SnmpInterfaceCommand()
        {
            rPar.Add(new ParOption("ver", "VERSION", "Version of SNMP to use.", false, false, new Type[] { typeof(uint) }));
            rPar.Add(new ParOption("ip", "Target-IP", "Target.", false, false, new Type[] { typeof(string) }));
            oPar.Add(new ParOption("e", "INTERFACE-COUNT", "Expected number of interfaces.", false, false, new Type[] { typeof(uint) }));
            oPar.Add(new ParOption("p", "", "privPro", false, false, new Type[] { typeof(string) }));
            oPar.Add(new ParOption("a", "", "authProt", false, false, new Type[] { typeof(string) }));
            oPar.Add(new ParOption("s", "", "security", false, false, new Type[] { typeof(string) }));
        }

        public override string Execute(int consoleWidth)
        {
            version = (uint)pars.GetPar("ver").argValues[0];
            string _buffer = (string)pars.GetPar("ip").argValues[0];

            if (version == 3)
            {
                _buffer = (string)pars.GetPar("s").argValues[0];
                switch (_buffer)
                {
                    case "authNoPriv":
                        security = NetworkHelper.securityLvl.authNoPriv;
                        if(!(OParUsed("a") && !OParUsed("p")))
                            return "<color><red>ERROR: Wrong Parameters";
                        break;
                    case "authPriv":
                        security = NetworkHelper.securityLvl.authPriv;
                        if (!(OParUsed("a") && OParUsed("p")))
                            return "<color><red>ERROR: Wrong Parameters";
                        break;
                    case "noAuthNoPriv":
                        security = NetworkHelper.securityLvl.noAuthNoPriv;
                        if (!(!OParUsed("a") && !OParUsed("p")))
                            return "<color><red>ERROR: Wrong Parameters";
                        break;
                    default:
                        return "<color><red>ERROR";
                }

                _buffer = (string)pars.GetPar("p").argValues[0];
                switch (_buffer)
                {
                    case "aes":
                        privProt = NetworkHelper.snmpProtokolls.AES;
                        break;
                    case "des":
                        privProt = NetworkHelper.snmpProtokolls.DES;
                        break;
                    default:
                        return "<color><red>ERROR:";
                }


                // PARSE authProt
                _buffer = (string)pars.GetPar("a").argValues[0];
                switch (_buffer)
                {
                    case "md5":
                        authProt = NetworkHelper.snmpProtokolls.MD5;
                        break;
                    case "sha":
                        authProt = NetworkHelper.snmpProtokolls.SHA;
                        break;
                    default:
                        return "<color><red>ERROR:";
                }

                // PARSE security
                
            }

            // PARSE ip
            
            if (!IpAddress.IsIP(_buffer))
                return "<color><red>Could not parse ip-address!";
            else
                targetIP = IPAddress.Parse(_buffer);

            UdpTarget target = new UdpTarget(targetIP, 161, 5000, 3);

            // if optional par is used.
            if(OParUsed("e"))
            {
                expectedIfNr = (uint)pars.GetPar("e").argValues[0];
            }

            // PARSE privProt
            

            if (version == 2)
            { 
                // VER 2
                OctetString community = new OctetString(communityString);
                AgentParameters param = new AgentParameters(community);

                param.Version = SnmpVersion.Ver2;

                Oid ifIndex = new Oid(ifEntryString + ".1");
                Oid ifDescr = new Oid(ifEntryString + ".2");
                Oid ifType = new Oid(ifEntryString + ".3");

                Pdu index = new Pdu(PduType.GetBulk);
                index.VbList.Add(ifIndex);
                index.MaxRepetitions = (int)expectedIfNr;

                Pdu descr = new Pdu(PduType.GetBulk);
                descr.VbList.Add(ifDescr);
                descr.MaxRepetitions = (int)expectedIfNr;

                Pdu type = new Pdu(PduType.GetBulk);
                type.VbList.Add(ifType);
                type.MaxRepetitions = (int)expectedIfNr;

                try
                {
                    SnmpV2Packet indexResult = (SnmpV2Packet)target.Request(index, param);
                    SnmpV2Packet descrResult = (SnmpV2Packet)target.Request(descr, param);
                    SnmpV2Packet typeResult = (SnmpV2Packet)target.Request(type, param);

                    if (indexResult.Pdu.ErrorStatus != 0 || descrResult.Pdu.ErrorStatus != 0 || typeResult.Pdu.ErrorStatus != 0)
                    {
                        return "<color><red>ERROR: packet error status = " + indexResult.Pdu.ErrorStatus.ToString() + " and not ZERO";
                    }
                    else
                    {
                        for (int i = 0; i < indexResult.Pdu.VbCount; i++)
                        {
                            _output += "Interface " + indexResult.Pdu.VbList[i].Value.ToString() + ": Type " + typeResult.Pdu.VbList[i].Value.ToString() + " -> " + descrResult.Pdu.VbList[i].Value.ToString() + "\n";
                        }
                        return _output; 
                    }
                }
                catch (Exception)
                {
                    return "<color><red>ERROR: sending requests failed";
                }
            }
            else if (version == 3)
            {
                // VER 3
                SecureAgentParameters param = new SecureAgentParameters();

                if (!target.Discovery(param))
                {
                    return "<color><red>ERROR: Client was not able to syncronize with Agend";
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

                Oid ifIndex = new Oid(ifEntryString + ".1");
                Oid ifDescr = new Oid(ifEntryString + ".2");
                Oid ifType = new Oid(ifEntryString + ".3");

                Pdu index = new Pdu(PduType.GetBulk);
                index.VbList.Add(ifIndex);
                index.MaxRepetitions = (int)expectedIfNr;

                Pdu descr = new Pdu(PduType.GetBulk);
                descr.VbList.Add(ifDescr);
                descr.MaxRepetitions = (int)expectedIfNr;

                Pdu type = new Pdu(PduType.GetBulk);
                type.VbList.Add(ifType);
                type.MaxRepetitions = (int)expectedIfNr;

                try
                {
                    SnmpV3Packet indexResult = (SnmpV3Packet)target.Request(index, param);
                    SnmpV3Packet descrResult = (SnmpV3Packet)target.Request(descr, param);
                    SnmpV3Packet typeResult = (SnmpV3Packet)target.Request(type, param);

                    if (indexResult.ScopedPdu.ErrorStatus != 0 || descrResult.ScopedPdu.ErrorStatus != 0 || typeResult.ScopedPdu.ErrorStatus != 0)
                    {
                        return "<color><red>ERROR: packet error status = " + indexResult.ScopedPdu.ErrorStatus.ToString() + " and not ZERO";
                    }
                    else
                    {
                        for (int i = 0; i < indexResult.ScopedPdu.VbCount; i++)
                        {
                            _output += "Interface " + indexResult.ScopedPdu.VbList[i].Value.ToString() + ": Type " + typeResult.ScopedPdu.VbList[i].Value.ToString() + " -> " + descrResult.ScopedPdu.VbList[i].Value.ToString() + "\n";
                        }
                        return _output;
                    }
                }
                catch (Exception)
                {
                    return "<color><red>ERROR: sending requests failed";
                }
            }
            else
            {
                return "ERROR: This is not a supported version of snmp";
            }

            // 'NIX' wird in ROT auf die Console ausgeben.
            return "<color><red>NIX";
        }
    }
}
