using AIWolf.Client.Base.Player;
using AIWolf.Common.Data;

namespace AIWolf.Client.Base.Smpl
{
    class SamplePossessed : AbstractPossessed
    {
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
    }
}
