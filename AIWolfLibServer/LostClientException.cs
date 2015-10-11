using AIWolf.Common;
using System;
using System.Runtime.Serialization;

namespace AIWolf.Server
{
    [Serializable]
    class LostClientException : AIWolfRuntimeException
    {
        public LostClientException()
        {
        }

        public LostClientException(string message) : base(message)
        {
        }

        public LostClientException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected LostClientException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}