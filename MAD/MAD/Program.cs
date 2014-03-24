// MAD - Network Monitoring
// VERSION 0.0.0.1
// _____________________________

using System;

namespace MAD
{
    class Program
    {
        static void Main(string[] args)
        {
            // init MadCLI
            MadCLI cli = new MadCLI();
            cli.UpdateWindowTitle();
            cli.PrintHeader();
            cli.Start();
        }
    }
}
