using AIWolf.Client.Base.Player;
using AIWolf.Common.Data;

namespace AIWolf.Client.Base.Smpl
{
    class SampleBodyguard : AbstractBodyguard
    {
        public override void DayStart()
        {
        }

        public override void Finish()
        {
        }

        public override Agent Guard()
        {
            return Me;
        }

        public override string Talk()
        {
            return Common.Data.Talk.OVER;
        }

        public override Agent Vote()
        {
            return Me;
        }
    }
}
