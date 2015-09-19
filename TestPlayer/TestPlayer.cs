using AIWolf.Client.Base.Player;

namespace AIWolf.TestPlayer
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
