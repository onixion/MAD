using System;

using System.Net;
using System.IO;
using System.Text;

namespace WebserverTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // http-Listener
            HttpListener listener = new HttpListener();

            // set PORT
            listener.Prefixes.Add("http://*:80/");

            // start listener, falls port schon belegt -> HttpListenerException
            listener.Start();

            Console.WriteLine("SERVER started ...");

            while(true)
            {
                // get context
                HttpListenerContext context = listener.GetContext();

                Console.WriteLine("INCOMING-REQUEST");

                // get response from context
                HttpListenerResponse response = context.Response;
                
                // get stream from response
                Stream stream = response.OutputStream;

                // set DATA to send
                byte[] data = Encoding.ASCII.GetBytes("<html><h1>LOL</h1></br>WAS GEHT?!</html>");

                // send byte[] to client
                stream.Write(data, 0, data.Length);

                // close stream
                stream.Close();
            }
        }
    }
}
