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
			rPar.Add(new ParOption("i", "INTENSITY", "A number between 1 and 5, defining how intense the test is", false, false, new Type[] { typeof(uint) }));
			oPar.Add (new ParOption ("ip", "Target-IP", "Needet for a intensity 5 scan, target address of the custom ping", false, false, new Type[] { typeof(string) }));
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
					_success++;

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
					_success++;
				if(_reply3.Status == IPStatus.Success)
					_success++;

				if(_success == 3)
					return "<color><blue> (3/3) worked! You seem to be connected!"; 
				else
					return "<color><red> (" + _success.ToString() + "/3) worked.. You seem to have a problem.. " +
						"\n Ping 1: " + _reply1.Status.ToString() +
						"\n Ping 2: " + _reply2.Status.ToString() +
						"\n Ping 3: " + _reply3.Status.ToString();
			}
			catch (Exception ex)
			{ 
				return "<color><red> A Error occured! \n ErrorStatus: " + ex.Data.ToString(); 
			}
		}

		private static string Intensity4Check()
		{
			string _httpExceptionMessage = "";
			PingReply _reply1;
			PingReply _reply2;
			PingReply _reply3;
			ushort _success = 0;

			try
			{
				WebRequest _request = WebRequest.Create("http://www.google.com:80");
				WebResponse _response = _request.GetResponse();
				_success++;
				_httpExceptionMessage += "Successful";
			}
			catch(Exception ex) 
			{
				_success = 0;
				_httpExceptionMessage += ex.ToString ();
			}

			try
			{
				_reply1 = _ping.Send(IPAddress.Parse("8.8.8.8"));				
				_reply2 = _ping.Send(IPAddress.Parse("8.8.4.4"));				
				_reply3 = _ping.Send(IPAddress.Parse("23.0.160.32"));			

				if(_reply1.Status == IPStatus.Success)
					_success++;
				if(_reply2.Status == IPStatus.Success)
					_success++;
				if(_reply3.Status == IPStatus.Success)
					_success++;
					
				if(_success == 4)
					return "<color><blue> (4/4) worked! You seem to be connected!"; 
				else
					return "<color><red> (" + _success.ToString() + "/4) worked.. You seem to have a problem.. " +
						"\n Ping 1: " + _reply1.Status.ToString() +
						"\n Ping 2: " + _reply2.Status.ToString() +
						"\n Ping 3: " + _reply3.Status.ToString() +
						"\n HTTP: " + _httpExceptionMessage;
			}
			catch (Exception ex)
			{ 
				return "<color><red> A Error with the Ping occured! \n ErrorStatus: " + ex.Data.ToString(); 
			}
		}

		private static string Intensity5Check()
		{
			string _httpExceptionMessage = "";
			IPAddress _ipAddr = IPAddress.Parse((string) pars.GetPar("ip").argValues[0]);

			PingReply _reply1;
			PingReply _reply2;
			PingReply _reply3; 
			PingReply _reply4;

			ushort _success = 0;

			try
			{
				WebRequest _request = WebRequest.Create("http://www.google.com:80");
				WebResponse _response = _request.GetResponse();
				_success++;
				_httpExceptionMessage += "Successful";
			}
			catch(Exception ex) 
			{
				_success = 0;
				_httpExceptionMessage += ex.ToString ();
			}

			try
			{
				_reply1 = _ping.Send(IPAddress.Parse("8.8.8.8"));				//google primary dns
				_reply2 = _ping.Send(IPAddress.Parse("8.8.4.4"));				//google secondary dns
				_reply3 = _ping.Send(IPAddress.Parse("23.0.160.32"));			//usa.gov -> because .. american government
				_reply4 = _ping.Send(_ipAddr);


				if(_reply1.Status == IPStatus.Success)
					_success++;
				if(_reply2.Status == IPStatus.Success)
					_success++;
				if(_reply3.Status == IPStatus.Success)
					_success++;
				if(_reply4.Status == IPStatus.Success)
					_success++;

				if(_success == 5)
					return "<color><blue> (5/5) worked! You seem to be connected!"; 
				else
					return "<color><red> (" + _success.ToString() + "/4) worked.. You seem to have a problem.. " +
						"\n Ping 1: " + _reply1.Status.ToString() +
						"\n Ping 2: " + _reply2.Status.ToString() +
						"\n Ping 3: " + _reply3.Status.ToString() +
						"\n Ping 4: " + _reply4.Status.ToString() +
						"\n HTTP: " + _httpExceptionMessage;
			}
			catch (Exception ex)
			{ 
				return "<color><red> A Error with the Ping occured! \n ErrorStatus: " + ex.Data.ToString(); 
			}
		}
        #endregion
    }
}
	