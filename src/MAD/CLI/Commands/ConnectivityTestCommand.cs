using System;
using System.Net;
using System.Net.NetworkInformation;

namespace MAD.CLICore
{
    class ConnectivityTestCommand : Command 
    {
        #region fields

        private static Ping _ping = new Ping();

        #endregion

        #region methods
        public ConnectivityTestCommand()
        {
            oPar.Add(new ParOption("i", "INTENSITY", "A number between 1 and 5, defining how intense the test is", false, false, new Type[] { typeof(uint) }));
        }

        public override string Execute(int consoleWidth)
        {
            uint _intensity = (uint) pars.GetPar("i").argValues[0];

            switch (_intensity)
            {
                case 1:
                    return Intensity1Check();
                    break;
                case 2:
                    Intensity2Check();
                    break;
                case 3:
                    Intensity3Check();
                    break;
                case 4:
                    Intensity4Check();
                    break;
                case 5:
                    Intensity5Check();
                    break;
                default:
                    return "<color><red>Please choose a number between 1 and 5!";
            }
        }

        private static string Intensity1Check()
        {
            PingReply _reply;
            try
            {
               _reply = _ping.Send(IPAddress.Parse("8.8.8.8"));
               if (_reply.Status == IPStatus.Success)
                return "<color><blue> (1/1) worked! You seem to be connected!";
               else
                return "<color><red> (0/1) worked.. You seem to have a problem.. ErrorStatus: " + _reply.Status.ToString();
            }
            catch (Exception ex)
            {
                return "<color><red> (0/1) worked.. You seem to have a problem.. ErrorStatus: " + ex.Data.ToString();
            }

            
        }

        private static string Intensity2Check()
        {
            PingReply _reply;
            try
            {
                _reply = _ping.Send(IPAddress.Parse("8.8.8.8"));
            }
            catch (Exception)
            { }

            if (_reply.Status == IPStatus.Success)
                return "<color><blue> (1/1) worked! You seem to be connected!";
            else
                return "<color><red> (0/1) worked.. You seem to have a problem.. ErrorStatus: " + _reply.Status.ToString();
        }
        #endregion
    }
}
