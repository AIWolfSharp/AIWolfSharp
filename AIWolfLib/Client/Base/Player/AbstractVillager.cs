using AIWolf.Common.Data;

namespace AIWolf.Client.Base.Player
{
    abstract class AbstractVillager : AbstractRole
    {
        public abstract override void DayStart();

        public abstract override string Talk();

        sealed public override string Whisper()
        {
            throw new UnsuspectedMethodCallException();
        }

        public abstract override Agent Vote();

        sealed public override Agent Attack()
        {
            throw new UnsuspectedMethodCallException();
        }

        sealed public override Agent Divine()
        {
            throw new UnsuspectedMethodCallException();
        }

        sealed public override Agent Guard()
        {
            throw new UnsuspectedMethodCallException();
        }

        public abstract override void Finish();

        protected AbstractVillager()
        {
            MyRole = Role.VILLAGER;
        }
    }
}
