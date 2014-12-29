using System;
using System.Threading;

using MadNet;

namespace MadProbe
{
    class MadProbe
    {
        private Thread _thread;

        private AES _aes;
        private int _port;

        public MadProbe(string aes, int port)
        {
            _aes = new AES(aes);
            _port = port;
        }

        public void Start()
        { 
            //_thread = new Thread(
        }

        private void Worker()
        { 
        
        }
    }

}
