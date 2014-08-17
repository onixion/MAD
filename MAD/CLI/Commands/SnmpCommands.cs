using System;

using MAD.Helper;
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

        private IpAddress target;

        public SnmpInterfaceCommand()
        {
            oPar.Add(new ParOption("e", "INTERFACE-COUNT", "Expected number of interfaces.", false, false, new Type[] { typeof(uint) }));
            rPar.Add(new ParOption("ver", "VERSION", "Version of SNMP to use.", false, false, new Type[] { typeof(uint) }));
            rPar.Add(new ParOption("ip", "Target-IP", "Target.", false, false, new Type[] { typeof(string) }));
            rPar.Add(new ParOption("p", "", "privPro", false, false, new Type[] { typeof(string) }));
            rPar.Add(new ParOption("a", "", "authProt", false, false, new Type[] { typeof(string) }));
            rPar.Add(new ParOption("s", "", "security", false, false, new Type[] { typeof(string) }));
        }

        public override string Execute(int consoleWidth)
        {
            // PARSE ip
            string _buffer = (string)pars.GetPar("ip").argValues[0];
            if (!IpAddress.IsIP(_buffer))
                return "<color><red>Could not parse ip-address!";
            else
                target = new IpAddress(_buffer);
            
            // if optional par is used.
            if(OParUsed("e"))
            {
                expectedIfNr = (uint)pars.GetPar("e").argValues[0];
            }

            // PARSE privProt
            _buffer = (string)pars.GetPar("p").argValues[0];
            switch (_buffer)
            { 
                case "aes":
                    privProt = NetworkHelper.snmpProtokolls.AES;
                    break;
                case "des":
                    privProt = NetworkHelper.snmpProtokolls.DES;
                    break;
                case "md5":
                    privProt = NetworkHelper.snmpProtokolls.MD5;
                    break;
                case "sha":
                    privProt = NetworkHelper.snmpProtokolls.SHA;
                    break;
                default:
                    return "<color><red>ERROR:";
            }

            // PARSE authProt
            _buffer = (string)pars.GetPar("a").argValues[0];
            switch (_buffer)
            {
                case "aes":
                    authProt = NetworkHelper.snmpProtokolls.AES;
                    break;
                case "des":
                    authProt = NetworkHelper.snmpProtokolls.DES;
                    break;
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
            _buffer = (string)pars.GetPar("s").argValues[0];
            switch (_buffer)
            {
                case "authNoPriv":
                    security = NetworkHelper.securityLvl.authNoPriv;
                    break;
                case "authPriv":
                    security = NetworkHelper.securityLvl.authPriv;
                    break;
                case "noAuthNoPriv":
                    security = NetworkHelper.securityLvl.noAuthNoPriv;
                    break;
                default:
                    return "<color><red>ERROR:";
            }

            version = (uint)pars.GetPar("ver").argValues[0];
            if (version == 2)
            { 
                // VER 2
            }
            else if (version == 3)
            {
                // VER 3
            }
            else
            {
                return "ERROR: ...";
            }

            // 'NIX' wird in ROT auf die Console ausgeben.
            return "<color><red>NIX";
        }
    }
}
