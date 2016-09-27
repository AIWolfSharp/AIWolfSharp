//
// AIWolfAgentException.cs
//
// Copyright (c) 2016 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

﻿using System;
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

        public AIWolfAgentException(Agent agent, string method, Exception exception)
        {
            this.agent = agent;
            this.method = method;
            this.exception = exception;
        }

        protected AIWolfAgentException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}