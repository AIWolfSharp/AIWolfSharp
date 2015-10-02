using System;
using System.Runtime.Serialization;

namespace AIWolf.Common
{
    [Serializable]
    class AIWolfRuntimeException : Exception
    {
        public AIWolfRuntimeException()
        {
        }

        public AIWolfRuntimeException(string message) : base(message)
        {
        }

        public AIWolfRuntimeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AIWolfRuntimeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
