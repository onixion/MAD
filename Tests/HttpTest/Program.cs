using System;
using System.Net;

namespace HttpTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "http://localhost:80";
            WebRequest request = WebRequest.Create(url);

            try
            {
                WebResponse response = request.GetResponse();
                Console.WriteLine("HTTP-Webserver found!");
                Console.WriteLine(response.Headers.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("No webserver found at: " + request.RequestUri.ToString());
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to close program ... ");
            Console.ReadKey();
        }
    }
}
