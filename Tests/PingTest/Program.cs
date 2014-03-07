using System;

// needed for Ping
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Text;
//

namespace PingTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // variables
            int count = 10;
            byte[] data = Encoding.ASCII.GetBytes("LOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLO"); // 32Byte
            int timeout = 3000;
            IPAddress target = IPAddress.Parse("127.0.0.1");
            int ttl = 200;

            // ping class
            Ping ping = new Ping();
            PingOptions pingOptions = new PingOptions(ttl, true);
            PingReply pingReply;
            IPStatus status;
            
            for(uint i = 0; i < count; i++)
            {
                pingReply = ping.Send(target, timeout, data, pingOptions);
                status = pingReply.Status;

                // console output
                Console.WriteLine((i+1) + ".Ping to " + target.ToString() + ":");
                Console.WriteLine("TTL:       " + ttl);
                Console.WriteLine("Fragment:  " + pingReply.Options.DontFragment);
                Console.WriteLine("Status:    " + status.ToString());
                Console.WriteLine();
            }

            Console.WriteLine("Press any key to close program ... ");
            Console.ReadKey();
        }
    }
}
