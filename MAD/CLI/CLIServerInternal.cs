using System;
using System.Threading;

// ThreadPool
using Amib.Threading;

using MadNet;

namespace MAD.CLIServerCore
{
    public abstract class CLIServerInternal
    {
        # region member

        private Thread _listenThread;
        private SmartThreadPool _threadPool = new SmartThreadPool();

        private bool _listenThreadRunning = false;
        private object _lock = new object();

        public bool IsListening { get { return _listenThreadRunning; } }

        public int serverPort = 0;

        #endregion

        #region methodes

        protected abstract bool StartListener();
        protected abstract void StopListener();
        protected abstract object GetClient();
        protected abstract object HandleClient(object clientObject);

        public void Start()
        {
            lock (_lock)
            {
                if (!_listenThreadRunning)
                {
                    _listenThread = new Thread(new ThreadStart(HandleClientInternal));

                    if (!StartListener())
                    {
                        throw new Exception("Listener could not start!");
                    }

                    _listenThreadRunning = true;
                    _listenThread.Start();
                }
                else
                {
                    throw new Exception("Server already running!");
                }
            }
        }

        public void Stop()
        {
            lock (_lock)
            {
                if (_listenThreadRunning)
                {
                    _listenThreadRunning = false;
                    StopListener();

                    _listenThread.Join();
                    _threadPool.Cancel();
                }
                else
                {
                    throw new Exception("Server already stopped!");
                }
            }
        }

        private void HandleClientInternal()
        {
            while (_listenThreadRunning)
            {
                try
                {
                    object clientObject = GetClient();
                    _threadPool.QueueWorkItem(new WorkItemCallback(HandleClient), clientObject);
                }
                catch (Exception e)
                {
                    if (_listenThreadRunning == true)
                    {
                        throw new Exception("Listening execption: " + e.Message);
                    }
                }
            }
        }

        #endregion
    }
}
