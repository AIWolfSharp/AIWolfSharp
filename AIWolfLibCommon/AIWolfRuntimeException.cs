//
// AIWolfRuntimeException.cs
//
// Copyright (c) 2016 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

﻿using System;
using System.Runtime.Serialization;

namespace AIWolf.Common
{
    [Serializable]
    public class AIWolfRuntimeException : Exception
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
