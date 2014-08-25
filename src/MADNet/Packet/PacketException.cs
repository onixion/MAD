using System;

namespace MadNet
{
    public class PacketException : Exception
    {
        public PacketException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
