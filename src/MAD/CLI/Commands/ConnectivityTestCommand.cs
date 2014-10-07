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
					return "<color><red> (0/1) worked.. You seem to have a problem.. \n ErrorStatus: " + _reply.Status.ToString();
            }
            catch (Exception ex)
            {
				return "<color><red> A Error occured! \n ErrorStatus: " + ex.Data.ToString();
            }

            
        }

        private static string Intensity2Check()
        {
			PingReply _reply1;
			PingReply _reply2;
			ushort _success = 0;

            try
            {
				_reply1 = _ping.Send(IPAddress.Parse("8.8.8.8"));
				_reply2 = _ping.Send(IPAddress.Parse("8.8.4.4"));
				if(_reply1.Status == IPStatus.Success)
					_success++;
				if(_reply2.Status == IPStatus.Success)
					_success++);

				if(_success == 2)
					return "<color><blue> (2/2) worked! You seem to be connected!"; 
				else
					return "<color><red> (" + _success.ToString() + "/2) worked.. You seem to have a problem.. " +
						"\n Ping 1: " + _reply1.Status.ToString() +
						"\n Ping 2: " + _reply2.Status.ToString();
            }
			catch (Exception ex)
            { 
				return "<color><red> A Error occured! \n ErrorStatus: " + ex.Data.ToString(); 
			}
        }

		private static string Intensity3Check()
		{
			PingReply _reply1;
			PingReply _reply2;
			PingReply _reply3; 
			ushort _success = 0;

			try
			{
				_reply1 = _ping.Send(IPAddress.Parse("8.8.8.8"));
				_reply2 = _ping.Send(IPAddress.Parse("8.8.4.4"));
				_reply3 = _ping.Send(IPAddress.Parse("23.0.160.32"));

				if(_reply1.Status == IPStatus.Success)
					_success++;
				if(_reply2.Status == IPStatus.Success)
					_success++);

				if(_success == 2)
					return "<color><blue> (2/2) worked! You seem to be connected!"; 
				else
					return "<color><red> (" + _success.ToString() + "/2) worked.. You seem to have a problem.. " +
						"\n Ping 1: " + _reply1.Status.ToString() +
						"\n Ping 2: " + _reply2.Status.ToString();
			}
			catch (Exception ex)
			{ 
				return "<color><red> A Error occured! \n ErrorStatus: " + ex.Data.ToString(); 
			}
		}
        #endregion
    }
}
	