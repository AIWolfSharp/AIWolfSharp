//
// AbstractBodyguard.cs
//
// Copyright (c) 2016 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

﻿using AIWolf.Common.Data;

namespace AIWolf.Client.Base.Player
{
    public abstract class AbstractBodyguard : AbstractRole
    {
        public abstract override void DayStart();

        public abstract override string Talk();

        sealed public override string Whisper()
        {
            throw new UnsuspectedMethodCallException();
        }

        public abstract override Agent Vote();

        sealed public override Agent Attack()
        {
            throw new UnsuspectedMethodCallException();
        }

        sealed public override Agent Divine()
        {
            throw new UnsuspectedMethodCallException();
        }

        public abstract override Agent Guard();

        public abstract override void Finish();

        protected AbstractBodyguard()
        {
            MyRole = Role.BODYGUARD;
        }
    }
}
