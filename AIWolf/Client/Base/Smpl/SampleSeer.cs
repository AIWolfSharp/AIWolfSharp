using AIWolf.Client.Base.Player;
using AIWolf.Common.Data;

namespace AIWolf.Client.Base.Smpl
{
    class SampleSeer : AbstractSeer
    {
        public override Agent Divine()
        {
            return Me;
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
