using System;
using System.Net;
using System.Text;
using System.IO;

using MadNet;
using CLIIO;

namespace CLIClient
{
    public class Program
    {
        static int Main(string[] args)
        {
            /* ARGS => <server-ip> <server-port> <username> <password> */

            MadClient _client = new MadClient();
            
            if (args.Length == 4)
            {
                try
                {
                    IPAddress _serverIP;
                    try { _serverIP = IPAddress.Parse(args[0]); }
                    catch (Exception)
                    {
                        CLIOutput.WriteToConsole("<color><red>Could not parse '" + args[0] + "' into an ip-address!");
                        throw new Exception();
                    }

                    int _serverPort = 999;
                    try { _serverPort = Int32.Parse(args[1]); }
                    catch (Exception)
                    {
                        CLIOutput.WriteToConsole("<color><red>Could not parse '" + args[1] + "' into an interger!");
                        throw new Exception();
                    }

                    _client.SetConnection(_serverIP, _serverPort);

                    string _username = args[2];
                    string _password = args[3];

                    _client.SetUserPass(_username, _password);
                    _client.CLIConnect();
                }
                catch (Exception) { }

                Console.ReadKey();
                return 0;
            }
            else if( args.Length == 0)
            {
                // do nothing
            }
            else
            {
                CLIOutput.WriteToConsole("To many args! .. switching to CLI-Setup-Assistent ..");
            }
            
            ConsoleKeyInfo _key;
            while (true)
            {
                _client.CLISetupAssisten();

                while (true)
                {
                    CLIOutput.WriteToConsole("\n<color><yellow>Connect to '" + _client.server + ":" + _client.port + "' with the username '" + _client.user + "'?");
                    CLIOutput.WriteToConsole("<color><yellow>Sure about that? Y(es) / N(o)");

                    _key = Console.ReadKey(true);
                    if (_key.Key == ConsoleKey.Y)
                    {
                        _client.CLIConnect();
                    }
                    else
                    {
                        break;
                    }
                }

                CLIOutput.WriteToConsole("\n<color><yellow>Re-configure or exit? R(e-Configure) / E(xit)");

                _key = Console.ReadKey(true);
                if (_key.Key != ConsoleKey.R)
                    break;
            }

            return 0;
        }
    }

    public class MadClient
    {
        public IPAddress server { get; set; }
        public int port { get; set; }

        public string user { get; set; }
        private string _passMD5 { get; set; }

        public MadClient()
        { }

        public void SetUserPass(string username, string password)
        {
            user = username;
            _passMD5 = MD5Hashing.GetHash(password);
        }

        public void SetConnection(IPAddress server, int port)
        {
            this.server = server;
            this.port = port;
        }

        public void CLISetupAssisten()
        {
            string _cliInput;

            IPAddress _server;
            int _port;
            string _username;
            string _password = "";

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nConnection-Setup:");

            Console.ForegroundColor = ConsoleColor.White;
            while (true)
            {
                Console.Write("Server-IP: ");
                _cliInput = Console.ReadLine();

                try
                {
                    _server = IPAddress.Parse(_cliInput);
                    break;
                }
                catch (Exception)
                {
                    if (_cliInput != "")
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Could not parse '" + _cliInput + "' into an ip-address!");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
            }

            while (true)
            {
                Console.Write("Server-PORT: ");
                _cliInput = Console.ReadLine();

                try
                {
                    _port = Int32.Parse(_cliInput);
                    break;
                }
                catch (Exception)
                {
                    if (_cliInput != "")
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Could not parse '" + _cliInput + "' into a integer!");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
            }

            SetConnection(_server, _port);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nLogin-Setup:");

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Username: ");
            _username = Console.ReadLine();

            Console.Write("Password: ");

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);

                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    _password += key.KeyChar;
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && _password.Length > 0)
                    {
                        _password = _password.Substring(0, _password.Length - 1);
                    }
                    else
                    {
                        if (key.Key == ConsoleKey.Enter)
                        {
                            Console.WriteLine("");
                            break;
                        }
                    }
                }
            }

            SetUserPass(_username, _password);
        }

        public void CLIConnect()
        {
            CLIClient _client = new CLIClient(new IPEndPoint(server, port));

            try
            {
                _client.Connect();
                _client.LoginToRemoteCLI(user, _passMD5);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}
