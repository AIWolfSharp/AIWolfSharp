using System;
using System.Runtime.Serialization;
using AIWolf.Common.Data;

namespace AIWolf.Common
{
#if !WINDOWS_UWP
    [Serializable]
#endif
    public class AIWolfAgentException : AIWolfRuntimeException
    {
        private Agent agent;
        private Exception exception;
        private string method;

        public AIWolfAgentException()
        {
        }

        public AIWolfAgentException(string message) : base(message)
        {
        }

        public AIWolfAgentException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public AIWolfAgentException(Agent agent, string method, Exception exception)
        {
            this.agent = agent;
            this.method = method;
            this.exception = exception;
        }

#if !WINDOWS_UWP
        protected AIWolfAgentException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
#endif
    }
}