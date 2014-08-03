using System;
using System.Net;
using System.Text;
using System.IO;

using MadNet;

namespace CLIClient
{
    public class Program
    {
        static int Main(string[] args)
        {
            MadClient _client = new MadClient();

            _client.CLISetupAssisten();
            Console.WriteLine("");
            _client.CLIConnect();

            Console.WriteLine("\nPress any key to exit program ...");
            Console.ReadKey();

            return 0;
        }
    }

    public class MadClient
    {
        private IPAddress _server { get; set; }
        private int _port { get; set; }

        private string _user { get; set; }
        private string _passMD5 { get; set; }

        public MadClient()
        { }

        public void SetUserPass(string username, string password)
        {
            _user = username;
            _passMD5 = MD5Hashing.GetHash(password);
        }

        public void SetConnection(IPAddress server, int port)
        {
            _server = server;
            _port = port;
        }

        public void CLISetupAssisten()
        {
            string _cliInput;

            IPAddress _server;
            int _port;
            string _username;
            string _password = "";

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Connection-Setup:");

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
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Connect to '" + _server + ":" + _port + "' with the username '" + _user + "'?");
                Console.WriteLine("Sure about that? Y/N");
                Console.ForegroundColor = ConsoleColor.White;

                ConsoleKeyInfo _key = Console.ReadKey(true);
                if (_key.Key == ConsoleKey.Y)
                {
                    CLIClient _client = new CLIClient(new IPEndPoint(_server, _port));

                    try
                    {
                        _client.Connect();
                        _client.LoginToRemoteCLI(_user, _passMD5);
                    }
                    catch (Exception e)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(e.Message);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                else if (_key.Key == ConsoleKey.N)
                {
                    break;
                }
            }
        }
    }
}
