using AIWolf.Client.Base.Player;
using AIWolf.Common.Data;

namespace AIWolf.Client.Base.Smpl
{
    class SampleWerewolf : AbstractWerewolf
    {
        public override Agent Attack()
        {
            return Me;
        }

        public override void DayStart()
        {
        }

        public override void Finish()
        {
        }

        public override string Talk()
        {
            return Common.Data.Talk.OVER;
        }

        public override Agent Vote()
        {
            return Me;
        }

        public override string Whisper()
        {
            return Common.Data.Talk.OVER;
        }
    }
}
