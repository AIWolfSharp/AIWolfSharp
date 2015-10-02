using AIWolf.Client.Base.Player;

namespace AIWolf.Client.Base.Smpl
{
    class SampleRoleAssignPlayer : AbstractRoleAssignPlayer
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
            get
            {
                return typeof(SampleRoleAssignPlayer).Name;
            }
        }
    }
}
