//
// SampleRoleAssignPlayer.cs
//
// Copyright (c) 2016 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

﻿using AIWolf.Client.Base.Player;

namespace AIWolf.Client.Base.Smpl
{
    public class SampleRoleAssignPlayer : AbstractRoleAssignPlayer
    {
        public SampleRoleAssignPlayer()
        {
            VillagerPlayer = new SampleVillager();
            SeerPlayer = new SampleSeer();
            MediumPlayer = new SampleMedium();
            BodyguardPlayer = new SampleBodyguard();
            PossessedPlayer = new SamplePossessed();
            WerewolfPlayer = new SampleWerewolf();
        }

        public override string Name
        {
            get { return typeof(SampleRoleAssignPlayer).Name; }
        }
    }
}
