using AIWolf.Client.Base.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIWolf
{
    public class TestRoleAssignPlayer : AbstractRoleAssignPlayer
    {

        public TestRoleAssignPlayer()
        {
            VillagerPlayer = new SimplePlayer();
            SeerPlayer = new SimplePlayer();
            MediumPlayer = new SimplePlayer();
            BodyguardPlayer = new SimplePlayer();
            PossessedPlayer = new SimplePlayer();
            WerewolfPlayer = new SimplePlayer();
        }

        public override string Name
        {
            get
            {
                return GetType().Name;
            }
        }
    }

}
