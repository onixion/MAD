using System;

namespace MAD.JobSystemCore
{
    public class JobSystemException : Exception
    {
        public JobSystemException(string message, Exception innerEx)
            : base(message, innerEx) { }
    }

    public class JobSceduleException : Exception
    {
        public JobSceduleException(string message, Exception innerEx)
            : base(message, innerEx) { }
    }

    public class JobNodeException : Exception
    {
        public JobNodeException(string message, Exception innerEx)
            : base(message, innerEx) { }
    }

    public class JobException : Exception
    {
        public JobException(string message, Exception innerEx)
            : base(message, innerEx) { }
    }
}
