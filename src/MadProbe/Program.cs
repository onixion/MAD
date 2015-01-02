using System;
using System.Collections.Generic;

namespace MadProbe
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("(CONF) Loading conf ...");

            try
            {
                MadConf.LoadConf("probe.conf");
            }
            catch (Exception)
            {
                Console.WriteLine("(CONF) No conf found. Creating one ...");
                MadConf.SetToDefault();
                MadConf.SaveConf("probe.conf");
            }

            Console.WriteLine("AES-PASS: " + "".PadLeft(MadConf.conf.aesPass.Length, '*'));
            Console.WriteLine("Port:     " + MadConf.conf.port + "\n");

            MadProbe _probe = new MadProbe(MadConf.conf.aesPass, MadConf.conf.port);
            Console.WriteLine("Probe started successfully.");
            _probe.Start();
        }
    }
}
