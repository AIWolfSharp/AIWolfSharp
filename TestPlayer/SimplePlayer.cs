using AIWolf.Client.Base.Player;
using AIWolf.Common.Data;

namespace AIWolf.TestPlayer
{
    public class SimplePlayer : AbstractRole
    {
        public override void DayStart()
        {
        }

        public override string Talk()
        {
            return Common.Data.Talk.OVER;
        }

        public override string Whisper()
        {
            return Common.Data.Talk.OVER;
        }

        public override Agent Vote()
        {
            return Me;
        }

        public override Agent Attack()
        {
            return Me;
        }

        public override Agent Divine()
        {
            return Me;
        }

        public override Agent Guard()
        {
            return Me;
        }

        public override void Finish()
        {
        }
    }
}
