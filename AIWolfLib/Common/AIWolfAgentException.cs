using System;
using System.Runtime.Serialization;
using AIWolf.Common.Data;

namespace AIWolf.Common
{
    [Serializable]
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

        public AIWolfAgentException(Agent agent, string mathod, Exception exception)
        {
            this.agent = agent;
            this.method = mathod;
            this.exception = exception;
        }

        protected AIWolfAgentException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}