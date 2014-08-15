using System;
using System.Net;

using MAD.Helper;

using SnmpSharpNet;

namespace MAD.JobSystemCore
{
    [Serializable]
    public class JobSnmp : Job
    {
        public uint version;                                                            //required Parameter möglichkeiten 2 oder 3
        public uint expectedIfNr = 10;                                                  //optional Parameter default (wie zu sehen) ist 10

        public bool interfaceParUsed;                                                   //required Parameter, irgendwie a option -i oder sowas.. wenn der Parameter vorkimmt wead a Auflistung von Interfaces ausgegeben und zwar in der Menge von expectedIfNr

        public string communityString = "";                                             //required Parameter
        public string auth = "";                                                        //required Parameter sofern version == 3 && security != noAuthNoPriv
        public string priv = "";                                                        //required Parameter sofern version == 3 && security != noAuthNoPriv
        public string ifEntryNr;                                                        //required Parameter sofern !interfaceParUsed

        public NetworkHelper.snmpProtokolls privProt;                                   //required Parameter sofern version == 3 && security != noAuthNoPriv            
        public NetworkHelper.snmpProtokolls authProt;                                   //required Parameter sofern version == 3 && security != noAuthNoPriv

        public NetworkHelper.securityLvl security;                                      //required Parameter sofern version == 3

        private string ifEntryString = "1.3.6.1.2.1.2.2.1";
        private static uint lastRecord = 0; 

        public override void Execute(IPAddress targetAddress)
        {
            UdpTarget target = new UdpTarget(targetAddress, 161, 5000, 3);
            switch (version)
            {
                case 1:
                    //Falls der zu blöd war zu verstehen dass lei 2 und 3 unterstützt werden dann a freundliche nachricht dass des nit geht
                    break;
                case 2:
                    SnmpV2Handling(target);
                    break;
                case 3:
                    SnmpV3Handling(target);
                    break;
            }
            target.Close();
        }

        private void SnmpV2Handling(UdpTarget target)
        {
            OctetString community = new OctetString(communityString);
            AgentParameters param = new AgentParameters(community);

            param.Version = SnmpVersion.Ver2;

            ExecuteRequest(target, param);
        }

        private void SnmpV3Handling(UdpTarget target)
        {
            SecureAgentParameters param = new SecureAgentParameters();

            if (!target.Discovery(param))
            {										
                //a fehler meldung ausgeben nach der art "Das Programm konnte sich nicht mit dem Agenten Syncronisieren"
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
            if (interfaceParUsed)
            {
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
                        //Fehlermeldung 
                    }
                    else
                    {
                        for (int i = 0; i < indexResult.Pdu.VbCount; i++)
                        {
                            Console.WriteLine("Interface {0}: Type {1} -> {2}", indexResult.Pdu.VbList[i].Value.ToString(), typeResult.Pdu.VbList[i].Value.ToString(), descrResult.Pdu.VbList[i].Value.ToString());
                            //a Ausgabe nach dem Shema.. bis ma die Datenbank haben 
                        }
                    }
                }
                catch (Exception)
                {
                    //Fehlereldung
                }
            }
            else
            {
                string outOctetsString = "1.3.6.1.2.1.2.2.1.16";

                Oid outOctets = new Oid(outOctetsString + "." + ifEntryNr);

                Pdu octets = new Pdu(PduType.Get);
                octets.VbList.Add(outOctets);

                try
                {
                    SnmpV2Packet octetsResult = (SnmpV2Packet)target.Request(octets, param);

                    uint result = (uint)Convert.ToUInt64(octetsResult.Pdu.VbList[0].Value.ToString()) - lastRecord;
                    lastRecord = (uint)Convert.ToUInt64(octetsResult.Pdu.VbList[0].Value.ToString());

                    if (octetsResult.Pdu.ErrorStatus != 0)
                    {
                        //Fehlermeldung 
                    }
                    else
                    {
                        Console.WriteLine("Output since the last run: {0}", result.ToString());
                        //Ausgabe nach dem Muster.. wie oben 
                    }
                }
                catch (Exception)
                {
                    //Fehlermeldung 
                }
            }
        }

        private void ExecuteRequest(UdpTarget target, SecureAgentParameters param)
        {
            if (interfaceParUsed)
            {
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
                        //Fehlermeldung
                    }
                    else
                    {
                        for (int i = 0; i < indexResult.ScopedPdu.VbCount; i++)
                        {
                            Console.WriteLine("Interface {0}: Type {1} -> {2}", indexResult.ScopedPdu.VbList[i].Value.ToString(), typeResult.ScopedPdu.VbList[i].Value.ToString(), descrResult.ScopedPdu.VbList[i].Value.ToString());
                            //Ausgabe nach dem Muster.. wie oben 
                        }
                    }
                }
                catch (Exception)
                { 
                    //Fehlermeldung
                }
            }
            else
            {
                string outOctetsString = "1.3.6.1.2.1.2.2.1.16";

                Oid outOctets = new Oid(outOctetsString + "." + ifEntryNr);

                Pdu octets = new Pdu(PduType.Get);
                octets.VbList.Add(outOctets);

                try
                {
                    SnmpV3Packet octetsResult = (SnmpV3Packet)target.Request(octets, param);

                    uint result = (uint)Convert.ToUInt64(octetsResult.ScopedPdu.VbList[0].Value.ToString()) - lastRecord;
                    lastRecord = (uint)Convert.ToUInt64(octetsResult.ScopedPdu.VbList[0].Value.ToString());

                    if (octetsResult.ScopedPdu.ErrorStatus != 0)
                    {
                        //Fehlermeldung
                    }
                    else
                    {
                        Console.WriteLine("Output since the last run: {0}", result.ToString());
                        //Ausgabe nach dem Muster.. wie oben 
                    }
                }
                catch (Exception)
                {
                    //Fehlermeldung
                }
            }
        }
    }
}
