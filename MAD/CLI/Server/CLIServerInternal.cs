using System;
using System.Threading;

// ThreadPool
using Amib.Threading;

namespace MAD.CLI.Server
{
    public abstract class CLIServerInternal
    {
        # region member

        public Version internalVersion = new Version(0, 0, 1000);

        private Thread _listenThread;
        private bool _listenThreadRunning = false;
        private object _lock = new object();

        public bool listening { get { return _listenThreadRunning; } }

        private SmartThreadPool _threadPool = new SmartThreadPool();
        private int _threadPoolThreadStopTime = 5000;

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
                    // init listener thread
                    _listenThread = new Thread(new ThreadStart(HandleClientInternal));

                    if (!StartListener())
                    {
                        throw new Exception("Listener could not start!");
                    }

                    _listenThreadRunning = true;

                    // start listener thread
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

                    // wait for listen-thread to finish
                    _listenThread.Join();

                    // abort all threads
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
