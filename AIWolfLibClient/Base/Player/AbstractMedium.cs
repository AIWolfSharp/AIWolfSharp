using AIWolf.Common.Data;
using System.Collections.Generic;

namespace AIWolf.Client.Base.Player
{
    abstract class AbstractMedium : AbstractRole
    {
        // 占い結果のリスト
        protected List<Judge> MyJudgeList { get; set; } = new List<Judge>();

        public override void DayStart()
        {
            //霊能結果をJudgeListに格納
            if (GameInfoMap[Day].MediumResult != null)
            {
                MyJudgeList.Add(LatestDayGameInfo.MediumResult);
            }
        }

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

        protected AbstractMedium()
        {
            MyRole = Role.MEDIUM;
        }

        /// <summary>
        /// すでに占い(or霊能)対象にしたプレイヤーならtrue,まだ占っていない(霊能していない)ならばfalseを返す．
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        protected bool IsJudgedAgent(Agent agent)
        {
            foreach (Judge judge in MyJudgeList)
            {
                if (judge.Target == agent)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
