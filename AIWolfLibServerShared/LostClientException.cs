using AIWolf.Common;
using AIWolf.Common.Data;
using System;
using System.Runtime.Serialization;

namespace AIWolf.Server
{
    [Serializable]
    class LostClientException : AIWolfRuntimeException
    {
        public Agent Agent { get; }

        public LostClientException()
        {
        }

        public LostClientException(string message) : base(message)
        {
        }

        public LostClientException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public LostClientException(string message, Exception innerException, Agent agent) : base(message, innerException)
        {
            Agent = agent;
        }

        protected LostClientException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}