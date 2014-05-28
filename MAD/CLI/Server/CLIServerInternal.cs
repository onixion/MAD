using System;
using System.Threading;
using System.IO;
using System.Text;
using System.Net.Sockets;
using System.Security.Cryptography;

// ThreadPool
using Amib.Threading;

namespace MAD.CLI
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

                    // wait for threads to finish
                    _threadPool.WaitForIdle(_threadPoolThreadStopTime);
                    // after time, abort all threads
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

        public void SendAES(NetworkStream stream, string cryptPass, string data)
        { 
            
        }

        public void TestAES()
        { 
            byte[] pass = System.Text.Encoding.ASCII.GetBytes("lol123");

            string data = "LOLOLOLO";
            byte[] encrpyted;

            using (Aes aes = Aes.Create())
            {
                encrpyted = AESDecrypt(aes.Key, aes.IV, data); 
            }
        }

        public byte[] AESEncrypt(byte[] key, byte[] iv, string data)
        {
            if (key.Length != 0 | iv.Length != 0 | data.Length != 0)
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = key;
                    aes.IV = iv;

                    ICryptoTransform encryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter writer = new StreamWriter(cs))
                            {
                                writer.Write(data);
                            }

                            return ms.ToArray();
                        }
                    }
                }
            }
            else
            {
                return null;
            }
        }

        public byte[] AESDecrypt(byte[] key, byte[] iv, string data)
        {
            if (key.Length != 0 | iv.Length != 0 | data.Length != 0)
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = key;
                    aes.IV = iv;

                    ICryptoTransform encryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                    using (MemoryStream ms = new MemoryStream(Encoding.ASCII.GetBytes(data)))
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader reader = new StreamReader(cs))
                            {
                                 return Encoding.ASCII.GetBytes(reader.ReadToEnd());
                            }
                        }
                    }
                }
            }
            else
            {
                return null;
            }
        }
    }
}
