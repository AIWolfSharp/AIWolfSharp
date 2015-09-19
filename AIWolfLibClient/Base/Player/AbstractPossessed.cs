using AIWolf.Common.Data;

namespace AIWolf.Client.Base.Player
{
    public abstract class AbstractPossessed : AbstractRole
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

        protected AbstractPossessed()
        {
            MyRole = Role.POSSESSED;
        }
    }
}
