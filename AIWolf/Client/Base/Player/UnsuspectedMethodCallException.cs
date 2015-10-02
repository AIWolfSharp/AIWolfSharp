using AIWolf.Common;
using System;
using System.Runtime.Serialization;

namespace AIWolf.Client.Base.Player
{
    [Serializable]
    class UnsuspectedMethodCallException : AIWolfRuntimeException
    {
        public UnsuspectedMethodCallException()
        {
        }

        public UnsuspectedMethodCallException(string message) : base(message)
        {
        }

        public UnsuspectedMethodCallException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnsuspectedMethodCallException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}