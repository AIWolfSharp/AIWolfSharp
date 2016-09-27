//
// AbstractWerewolf.cs
//
// Copyright (c) 2016 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

﻿using AIWolf.Common.Data;
using System.Collections.Generic;

namespace AIWolf.Client.Base.Player
{
    public abstract class AbstractWerewolf : AbstractRole
    {
        protected List<Agent> WolfList
        {
            get
            {
                List<Agent> wolfList = new List<Agent>();
                foreach (var pair in LatestDayGameInfo.RoleMap)
                {
                    if (pair.Value == Role.WEREWOLF)
                    {
                        wolfList.Add(pair.Key);
                    }
                }
                return wolfList;
            }
        }

        public abstract override void DayStart();

        public abstract override string Talk();

        public abstract override string Whisper();

        public abstract override Agent Vote();

        public abstract override Agent Attack();

        sealed public override Agent Divine()
        {
            throw new UnsuspectedMethodCallException();
        }

        sealed public override Agent Guard()
        {
            throw new UnsuspectedMethodCallException();
        }

        public abstract override void Finish();

        protected AbstractWerewolf()
        {
            MyRole = Role.WEREWOLF;
        }
    }
}
