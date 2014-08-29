using System;
using System.Net;

using MAD.Helper;
using SnmpSharpNet;

namespace MAD.JobSystemCore
{
    public class JobSnmp : Job
    {
        #region fields
        public uint version;                                                            //required Parameter möglichkeiten 2 oder 3
        public uint expectedIfNr = 10;                                                  //optional Parameter default (wie zu sehen) ist 10

        public bool interfaceParUsed;                                                   //required Parameter, irgendwie a option -i oder sowas.. wenn der Parameter vorkimmt wead a Auflistung von Interfaces ausgegeben und zwar in der Menge von expectedIfNr

        public string communityString = "public";
        public string auth = "MAD";
        public string priv = "MAD";
        public string ifEntryNr;                                                        //required Parameter sofern !interfaceParUsed

        public NetworkHelper.snmpProtokolls privProt;                                   //required Parameter sofern version == 3 && security != noAuthNoPriv            
        public NetworkHelper.snmpProtokolls authProt;                                   //required Parameter sofern version == 3 && security != noAuthNoPriv

        public NetworkHelper.securityLvl security;                                      //required Parameter sofern version == 3

        private const string _ifEntryString = "1.3.6.1.2.1.2.2.1";
        private static uint _lastRecord = 0;
        #endregion

        #region methods
        #region Constructors
        public JobSnmp()
            : base(JobType.SnmpCheck)
        { }

        public JobSnmp(string name, int version)
            : base(name, JobType.SnmpCheck)
        {
            this.version = (uint)version;
        }
        #endregion

        #region FromJob
        protected override string JobStatus()
        {
            throw new NotImplementedException();
        }

        public override void Execute(IPAddress _targetAddress)
        {
            UdpTarget _target = new UdpTarget(_targetAddress, 161, 5000, 3);
            switch (version)
            {
                case 1:
                    //Falls der zu blöd war zu verstehen dass lei 2 und 3 unterstützt werden dann a freundliche nachricht dass des nit geht
                    break;
                case 2:
                    SnmpV2Handling(_target);
                    break;
                case 3:
                    SnmpV3Handling(_target);
                    break;
            }
            _target.Close();
        }
        #endregion

        #region Private
        private void SnmpV2Handling(UdpTarget _target)
        {
            OctetString _community = new OctetString(communityString);
            AgentParameters _param = new AgentParameters(_community);

            _param.Version = SnmpVersion.Ver2;

            ExecuteRequest(_target, _param);
        }

        private void SnmpV3Handling(UdpTarget _target)
        {
            SecureAgentParameters _param = new SecureAgentParameters();

            if (!_target.Discovery(_param))
            {
                //a fehler meldung ausgeben nach der art "Das Programm konnte sich nicht mit dem Agenten Syncronisieren"
            }

            switch (security)
            {
                case NetworkHelper.securityLvl.noAuthNoPriv:
                    _param.noAuthNoPriv(communityString);

                    break;
                case NetworkHelper.securityLvl.authNoPriv:
                    if (authProt == NetworkHelper.snmpProtokolls.MD5)
                        _param.authNoPriv(communityString, AuthenticationDigests.MD5, auth);
                    else if (authProt == NetworkHelper.snmpProtokolls.SHA)
                        _param.authNoPriv(communityString, AuthenticationDigests.SHA1, auth);

                    break;
                case NetworkHelper.securityLvl.authPriv:
                    if (authProt == NetworkHelper.snmpProtokolls.MD5 && privProt == NetworkHelper.snmpProtokolls.AES)
                        _param.authPriv(communityString, AuthenticationDigests.MD5, auth, PrivacyProtocols.AES128, priv);
                    else if (authProt == NetworkHelper.snmpProtokolls.MD5 && privProt == NetworkHelper.snmpProtokolls.DES)
                        _param.authPriv(communityString, AuthenticationDigests.MD5, auth, PrivacyProtocols.DES, priv);
                    else if (authProt == NetworkHelper.snmpProtokolls.SHA && privProt == NetworkHelper.snmpProtokolls.AES)
                        _param.authPriv(communityString, AuthenticationDigests.SHA1, auth, PrivacyProtocols.AES128, priv);
                    else if (authProt == NetworkHelper.snmpProtokolls.SHA && privProt == NetworkHelper.snmpProtokolls.DES)
                        _param.authPriv(communityString, AuthenticationDigests.SHA1, auth, PrivacyProtocols.DES, priv);

                    break;
            }

            ExecuteRequest(_target, _param);
        }

        private void ExecuteRequest(UdpTarget _target, AgentParameters _param)
        {

            const string _outOctetsString = "1.3.6.1.2.1.2.2.1.16";

            Oid _outOctets = new Oid(_outOctetsString + "." + ifEntryNr);

            Pdu _octets = new Pdu(PduType.Get);
            _octets.VbList.Add(_outOctets);

            try
            {
                SnmpV2Packet _octetsResult = (SnmpV2Packet)_target.Request(_octets, _param);

                uint _result = (uint)Convert.ToUInt64(_octetsResult.Pdu.VbList[0].Value.ToString()) - _lastRecord;
                _lastRecord = (uint)Convert.ToUInt64(_octetsResult.Pdu.VbList[0].Value.ToString());

                if (_octetsResult.Pdu.ErrorStatus != 0)
                {
                    //Fehlermeldung 
                }
                else
                {
                    Console.WriteLine("Output since the last run: {0}", _result.ToString());
                    //Ausgabe nach dem Muster.. wie oben 
                }
            }
            catch (Exception)
            {
                //Fehlermeldung 
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

                uint _result = (uint)Convert.ToUInt64(_octetsResult.ScopedPdu.VbList[0].Value.ToString()) - _lastRecord;
                _lastRecord = (uint)Convert.ToUInt64(_octetsResult.ScopedPdu.VbList[0].Value.ToString());

                if (_octetsResult.ScopedPdu.ErrorStatus != 0)
                {
                    //Fehlermeldung
                }
                else
                {
                    Console.WriteLine("Output since the last run: {0}", _result.ToString());
                    //Ausgabe nach dem Muster.. wie oben 
                }
            }
            catch (Exception)
            {
                //Fehlermeldung
            }

        }
        #endregion
        #endregion
    }
}
