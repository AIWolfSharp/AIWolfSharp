using AIWolf.Common;
using System;
using System.Runtime.Serialization;

namespace AIWolf.Server
{
    [Serializable]
    class IllegalPlayerNumException : AIWolfRuntimeException
    {
        public IllegalPlayerNumException()
        {
        }

        public IllegalPlayerNumException(string message) : base(message)
        {
        }

        public IllegalPlayerNumException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected IllegalPlayerNumException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}