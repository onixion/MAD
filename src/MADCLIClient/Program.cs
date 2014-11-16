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
            /* ARGS => <server-ip>   <server-port> <aes-pass> */
            /* ARGS => <server-host> <server-port> <aes-pass> */

            IPAddress _serverIp = null;
            int _serverPort = 0;
            string _aesPass = null;

            if (args.Length == 3)
            {
                try
                {
                    if (!IPAddress.TryParse(args[0], out _serverIp))
                    {
                        CLIOutput.WriteToConsole("<color><yellow>Resolving hostname ...");

                        IPAddress[] _ips = Dns.GetHostAddresses(args[0]);
                        if (_ips.Length == 0)
                        {
                            CLIOutput.WriteToConsole("<color><red>Could not resolve '" + args[0] + "' ...");
                            CLIOutput.WriteToConsole("<color><red>Press any key to close ...");
                            return 0;
                        }
                        _serverIp = _ips[0];
                    }

                    try { _serverPort = Int32.Parse(args[1]); }
                    catch (Exception)
                    {
                        CLIOutput.WriteToConsole("<color><red>Could not parse '" + args[1] + "' into an interger!");
                        throw new Exception();
                    }

                    _aesPass = args[2];
                }
                catch (Exception)
                {
                    CLIOutput.WriteToConsole("<color><red>Press any key to close ...");
                    Console.ReadKey();
                    return 0;
                }
            }
            else
            {
                CLIOutput.WriteToConsole("To many args! .. switching to CLI-Setup-Assistent ..");

                string _consoleInput;

                CLIOutput.WriteToConsole("<color><yellow>1.) Server-IP (or hostname): ");
                _consoleInput = Console.ReadLine();

                if (!IPAddress.TryParse(_consoleInput, out _serverIp))
                {
                    CLIOutput.WriteToConsole("<color><yellow>Resolving hostname ...");

                    IPAddress[] _ips = Dns.GetHostAddresses(_consoleInput);
                    if (_ips.Length == 0)
                    {
                        CLIOutput.WriteToConsole("<color><red>Could not resolve this hostname ...");
                        CLIOutput.WriteToConsole("<color><red>Press any key to close ...");
                        Console.ReadKey();
                        return 0;
                    }
                    _serverIp = _ips[0];

                    CLIOutput.WriteToConsole("<color><yellow>" + _serverIp.ToString());
                }

                while (true)
                {
                    CLIOutput.WriteToConsole("<color><yellow>2.) Server-Port: ");

                    try
                    {
                        _serverPort = Int32.Parse(Console.ReadLine());
                        break;
                    }
                    catch (Exception)
                    {
                        CLIOutput.WriteToConsole("<color><red>No port-number!");
                    }
                }

                CLIOutput.WriteToConsole("<color><yellow>3.) AES-Key: ");
                _aesPass = Console.ReadLine();
            }

            CLIClient _client = new CLIClient(new IPEndPoint(_serverIp, _serverPort), _aesPass);

            try
            {
                _client.Connect();
                _client.GetServerInfo();

                CLIOutput.WriteToConsole("<color><yellow>SERVER-HEADER: " + _client.serverHeader + "\n");
                CLIOutput.WriteToConsole("<color><yellow>SERVER-VER.:   " + _client.serverVersion + "\n");

                _client.StartRemoteCLI();
            }
            catch (Exception e)
            {
                CLIOutput.WriteToConsole("<color><red>Execption: " + e.Message);
                CLIOutput.WriteToConsole("<color><red>Press any key to close ...");
                Console.ReadKey();
            }

            return 0;
        }
    }
}
