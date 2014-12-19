using System;
using System.Net;
using System.Text;
using System.IO;
using System.Xml;

using MadNet;
using CLIIO;

namespace CLIClient
{
    public class Program
    {
        private static string CONFIG_FILE = "madclient.conf";

        static int Main(string[] args)
        {
            IPAddress _serverIp = null;
            int _serverPort = 0;
            string _aesPass = null;

            if (args.Length != 0)
                Program.CONFIG_FILE = args[0];

            try
            {
                if (File.Exists(Program.CONFIG_FILE))
                {
                    ConfigReader.ReadConfig(Program.CONFIG_FILE, out _serverIp, out _serverPort, out _aesPass);
                }
                else
                {
                    ConfigReader.CreateConfig(Program.CONFIG_FILE);
                    ConfigReader.ReadConfig(Program.CONFIG_FILE, out _serverIp, out _serverPort, out _aesPass);
                }
            }
            catch (Exception e)
            {
                CLIOutput.WriteToConsole("<color><red>Exception: " + e.Message);
                return 0;
            }

            CLIOutput.WriteToConsole("<color><yellow>IPAddress: <color><white>" + _serverIp.ToString() + "\n");
            CLIOutput.WriteToConsole("<color><yellow>Port:      <color><white>" + _serverPort + "\n");

            CLIClient _client = new CLIClient(new IPEndPoint(_serverIp, _serverPort), _aesPass);

            try
            {
                _client.Connect();
                _client.GetServerInfo();

                CLIOutput.WriteToConsole("<color><yellow>SERVER-HEADER:   " + _client.serverHeader + "\n");
                CLIOutput.WriteToConsole("<color><yellow>SERVER-VERSION:  " + _client.serverVersion + "\n");

                _client.StartRemoteCLI();
            }
            catch (System.Security.Cryptography.CryptographicException)
            {
                CLIOutput.WriteToConsole("<color><red>CryptographicExecption: AES-key is wrong!\n");
            }
            catch (Exception e)
            {
                CLIOutput.WriteToConsole("<color><red>Execption: " + e.Message + "!\n");
            }

            CLIOutput.WriteToConsole("<color><red>Press any key to close ...");
            Console.ReadKey();

            return 0;
        }
    }

    public static class ConfigReader
    {
        public static void CreateConfig(string filename)
        {
            using (FileStream _file = new FileStream(filename,
                FileMode.Create, FileAccess.Write, FileShare.None))
            {
                XmlWriterSettings _settings = new XmlWriterSettings();
                _settings.ConformanceLevel = ConformanceLevel.Auto;
                _settings.Indent = true;

                using (XmlWriter _writer = XmlWriter.Create(_file, _settings))
                {
                    _writer.WriteStartDocument();
                    _writer.WriteStartElement("MadClient");

                    _writer.WriteElementString("target", "127.0.0.1");
                    _writer.WriteElementString("port", "2222");
                    _writer.WriteElementString("pass", "PASSWORD");

                    _writer.WriteEndElement();
                    _writer.WriteEndDocument();
                }
            }
        }

        public static void ReadConfig(string filename, out IPAddress ip,
            out int port, out string pass)
        {
            using (FileStream _file = new FileStream(filename,
                    FileMode.Open, FileAccess.Read, FileShare.None))
            {
                using (XmlReader _reader = XmlReader.Create(_file))
                {
                    _reader.Read();
                    _reader.Read();
                    _reader.Read();
                    _reader.Read();

                    string _buffer = _reader.ReadElementString("target");
                    if (!IPAddress.TryParse(_buffer, out ip))
                    {
                        IPAddress[] _temp = Dns.GetHostAddresses(_buffer);
                        ip = _temp[0];
                    }

                    port = Int32.Parse(_reader.ReadElementString("port"));
                    pass = _reader.ReadElementString("pass");
                }
            }
        }
    }
}
