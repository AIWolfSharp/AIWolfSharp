using AIWolf.Client.Base.Player;
using AIWolf.Client.Lib;
using AIWolf.Common.Data;
using AIWolf.Common.Net;
using System;
using System.Collections.Generic;

namespace AIWolf.Client.Base.Smpl
{
    class SampleSeer : AbstractSeer
    {
        // COする日にち
        int comingoutDay;

        // CO済みか否か
        bool isCameout;

        // 全体に占い結果を報告済みのプレイヤー
        List<Judge> declaredJudgedAgentList = new List<Judge>();

        bool isSaidAllDivineResult;

        AdvanceGameInfo agi = new AdvanceGameInfo();

        // 今日投票しようと思っているプレイヤー
        Agent planningVoteAgent;

        // 自分が最後に宣言した「投票しようと思っているプレイヤー」
        Agent declaredPlanningVoteAgent;

        // 会話をどこまで読んだか
        int readTalkListNum;

        public override void Initialize(GameInfo gameInfo, GameSetting gameSetting)
        {
            base.Initialize(gameInfo, gameSetting);
            comingoutDay = new Random().Next(3) + 1;
            isCameout = false;
        }

        public override void DayStart()
        {
            base.DayStart();

            // 投票するプレイヤーの初期化，設定
            declaredPlanningVoteAgent = null;
            planningVoteAgent = null;
            SetPlanningVoteAgent();

            isSaidAllDivineResult = false;

            readTalkListNum = 0;
        }

        public override string Talk()
        {
            // CO,霊能結果，投票先の順に発話の優先度高

            // 未CO，かつ設定したCOする日にちを過ぎていたらCO
            if (!isCameout && Day >= comingoutDay)
            {
                isCameout = true;
                return TemplateTalkFactory.Comingout(Me, (Role)MyRole);
            }
            // COしているなら占い結果の報告
            else if (isCameout && !isSaidAllDivineResult)
            {
                foreach (Judge judge in MyJudgeList)
                {
                    if (!declaredJudgedAgentList.Contains(judge))
                    {
                        declaredJudgedAgentList.Add(judge);
                        return TemplateTalkFactory.Divined(judge.Target, judge.Result);
                    }
                }
                isSaidAllDivineResult = true;
            }

            // 今日投票するプレイヤーの報告
            // 前に報告したプレイヤーと同じ場合は報告なし
            if (declaredPlanningVoteAgent != planningVoteAgent)
            {
                declaredPlanningVoteAgent = planningVoteAgent;
                return TemplateTalkFactory.Vote(planningVoteAgent);
            }
            else
            {
                return Common.Data.Talk.OVER;
            }
        }

        public override Agent Vote()
        {
            return planningVoteAgent;
        }

        public override Agent Divine()
        {
            // まだ占っていないプレイヤーの中からランダムに選択
            List<Agent> nonInspectedAgentList = new List<Agent>();
            foreach (Agent agent in LatestDayGameInfo.AliveAgentList)
            {
                if (!IsJudgedAgent(agent))
                {
                    nonInspectedAgentList.Add(agent);
                }
            }
            if (nonInspectedAgentList.Count == 0)
            {
                return Me;
            }
            else
            {
                return nonInspectedAgentList[new Random().Next(nonInspectedAgentList.Count)];
            }
        }

        public override void Finish()
        {
        }

        public override void Update(GameInfo gameInfo)
        {
            base.Update(gameInfo);

            List<Talk> talkList = gameInfo.TalkList;
            // talkListからCO，占い結果の抽出
            for (int i = readTalkListNum; i < talkList.Count; i++)
            {
                Talk talk = talkList[i];
                Utterance utterance = new Utterance(talk.Content);
                switch (utterance.Topic)
                {
                    // カミングアウトの発話の場合
                    case Topic.COMINGOUT:
                        agi.ComingoutMap[talk.Agent] = (Role)utterance.Role;
                        if (utterance.Role == MyRole)
                        {
                            SetPlanningVoteAgent();
                        }
                        break;
                }
            }
            readTalkListNum = talkList.Count;
        }

        private void SetPlanningVoteAgent()
        {
            // 自分以外の占い師COのプレイヤー．または自分が黒判定を出したプレイヤー
            // いなければ，白判定を出したプレイヤー以外でランダム
            List<Agent> aliveAgentList = LatestDayGameInfo.AliveAgentList;
            aliveAgentList.Remove(Me);

            List<Agent> voteAgentCandidate = new List<Agent>();
            foreach (Agent agent in aliveAgentList)
            {
                if (agi.ComingoutMap.ContainsKey(agent) && agi.ComingoutMap[agent] == MyRole)
                {
                    voteAgentCandidate.Add(agent);
                }
                else
                {
                    foreach (Judge judge in MyJudgeList)
                    {
                        if (judge.Target.Equals(agent) && judge.Result == Species.WEREWOLF)
                        {
                            voteAgentCandidate.Add(agent);
                        }
                    }
                }
            }

            if (voteAgentCandidate.Contains(planningVoteAgent))
            {
                return;
            }

            if (voteAgentCandidate.Count > 0)
            {
                Random rand = new Random();
                planningVoteAgent = voteAgentCandidate[rand.Next(voteAgentCandidate.Count)];
            }
            else
            {
                Random rand = new Random();
                planningVoteAgent = aliveAgentList[rand.Next(aliveAgentList.Count)];
            }
            return;
        }
    }
}
