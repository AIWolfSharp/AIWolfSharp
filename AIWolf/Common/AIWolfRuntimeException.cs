using System;

namespace AIWolf.Common
{
    class AIWolfRuntimeException : Exception
    {
        public AIWolfRuntimeException()
        {
        }

        public AIWolfRuntimeException(string arg0) : base(arg0)
        {
        }

        public AIWolfRuntimeException(Exception arg0) : base(arg0.ToString(), arg0)
        {
        }
    }
}
