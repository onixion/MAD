using System;
using System.Collections.Generic;
using System.Threading;

namespace MAD
{
    public class JobThreadPool : IDisposable
    {
        private List<Thread> _threadList = new List<Thread>();
        private object _threadLock = new object();

        public JobThreadPool()
        { 
        
        }

        public void QueueWorkerThread(Delegate _delegate)
        { 
        
        }

        public void Dispose()
        { 
        
        }
    }
}
