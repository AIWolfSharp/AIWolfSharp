using AIWolf.Client.Base.Smpl;
using AIWolf.Common.Data;
using AIWolf.Common.Net;

namespace AIWolf.Client.Base.Player
{
    abstract class AbstractRoleAssignPlayer : AbstractPlayer
    {
        protected AbstractRole VillagerPlayer { get; set; } = new SampleVillager();
        protected AbstractRole SeerPlayer { get; set; } = new SampleSeer();
        protected AbstractRole MediumPlayer { get; set; } = new SampleMedium();
        protected AbstractRole BodyguardPlayer { get; set; } = new SampleBodyguard();
        protected AbstractRole PossessedPlayer { get; set; } = new SamplePossessed();
        protected AbstractRole WerewolfPlayer { get; set; } = new SampleWerewolf();

        AbstractRole rolePlayer;

        public abstract override string GetName();

        sealed public override void Update(GameInfo gameInfo)
        {
            rolePlayer.Update(gameInfo);
        }

        sealed public override void Initialize(GameInfo gameInfo, GameSetting gameSetting)
        {
            Role? myRole = gameInfo.Role;
            switch (myRole)
            {
                case Role.VILLAGER:
                    rolePlayer = VillagerPlayer;
                    break;
                case Role.SEER:
                    rolePlayer = SeerPlayer;
                    break;
                case Role.MEDIUM:
                    rolePlayer = MediumPlayer;
                    break;
                case Role.BODYGUARD:
                    rolePlayer = BodyguardPlayer;
                    break;
                case Role.POSSESSED:
                    rolePlayer = PossessedPlayer;
                    break;
                case Role.WEREWOLF:
                    rolePlayer = WerewolfPlayer;
                    break;
                default:
                    rolePlayer = VillagerPlayer;
                    break;
            }
            rolePlayer.Initialize(gameInfo, gameSetting);
        }

        sealed public override void DayStart()
        {
            rolePlayer.DayStart();
        }

        sealed public override string Talk()
        {
            return rolePlayer.Talk();
        }

        sealed public override string Whisper()
        {
            return rolePlayer.Whisper();
        }

        sealed public override Agent Vote()
        {
            return rolePlayer.Vote();
        }

        sealed public override Agent Attack()
        {
            return rolePlayer.Attack();
        }

        sealed public override Agent Divine()
        {
            return rolePlayer.Divine();
        }

        sealed public override Agent Guard()
        {
            return rolePlayer.Guard();
        }

        sealed public override void Finish()
        {
            rolePlayer.Finish();
        }
    }
}
