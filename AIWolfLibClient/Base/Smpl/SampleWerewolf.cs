using AIWolf.Client.Base.Player;
using AIWolf.Client.Lib;
using AIWolf.Common.Data;
using AIWolf.Common.Net;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AIWolf.Client.Base.Smpl
{
    class SampleWerewolf : AbstractWerewolf
    {

        // COする日にち
        int comingoutDay;

        // CO済みか否か
        bool isCameout;

        // 全体に偽占い(霊能)結果を報告済みのJudge
        List<Judge> declaredFakeJudgedAgentList = new List<Judge>();

        bool isSaidAllFakeResult;

        AdvanceGameInfo agi = new AdvanceGameInfo();

        // 今日投票しようと思っているプレイヤー
        Agent planningVoteAgent;

        // 自分が最後に宣言した「投票しようと思っているプレイヤー」
        Agent declaredPlanningVoteAgent;

        // 会話をどこまで読んだか
        int readTalkListNum;

        // 騙る役職
        Role fakeRole;

        // 偽の占い(or霊能)結果
        List<Judge> MyFakeJudgeList { get; } = new List<Judge>();
        // 狂人だと思うプレイヤー
        Agent maybePossesedAgent = null;

        public override void Initialize(GameInfo gameInfo, GameSetting gameSetting)
        {
            base.Initialize(gameInfo, gameSetting);

            List<Role> fakeRoles = gameSetting.RoleNumMap.Keys.ToList();
            List<Role> nonFakeRoleList = new Role[] { Role.BODYGUARD, Role.FREEMASON, Role.POSSESSED, Role.WEREWOLF }.ToList();
            fakeRoles.RemoveAll(role => nonFakeRoleList.Contains(role));
            fakeRole = fakeRoles[new Random().Next(fakeRoles.Count)];

            // 占い師，or霊能者なら1~3日目からランダムに選択してCO．村人ならCOしない．
            comingoutDay = new Random().Next(3) + 1;
            if (fakeRole == Role.VILLAGER)
            {
                comingoutDay = 1000;
            }
            isCameout = false;
        }

        public override void DayStart()
        {
            // 投票するプレイヤーの初期化，設定
            declaredPlanningVoteAgent = null;
            planningVoteAgent = null;
            SetPlanningVoteAgent();

            if (Day >= 1)
            {
                SetFakeResult();
            }
            isSaidAllFakeResult = false;

            readTalkListNum = 0;
        }

        public override string Talk()
        {
            // CO,霊能結果，投票先の順に発話の優先度高

            // 未CO，かつ設定したCOする日にちを過ぎていたらCO

            if (!isCameout && Day >= comingoutDay)
            {
                isCameout = true;
                return TemplateTalkFactory.Comingout(Me, fakeRole);
            }
            // COしているなら偽占い，霊能結果の報告
            else if (isCameout && !isSaidAllFakeResult)
            {
                foreach (Judge judge in MyFakeJudgeList)
                {
                    if (!declaredFakeJudgedAgentList.Contains(judge))
                    {
                        if (fakeRole == Role.SEER)
                        {
                            declaredFakeJudgedAgentList.Add(judge);
                            return TemplateTalkFactory.Divined(judge.Target, judge.Result);
                        }
                        else if (fakeRole == Role.MEDIUM)
                        {
                            declaredFakeJudgedAgentList.Add(judge);
                            return TemplateTalkFactory.Inquested(judge.Target, judge.Result);
                        }
                    }
                }
                isSaidAllFakeResult = true;
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

        public override string Whisper()
        {
            // 何も発しない
            return TemplateTalkFactory.Over();
        }

        public override Agent Vote()
        {
            return planningVoteAgent;
        }

        public override Agent Attack()
        {
            // 能力者COしているプレイヤーは襲撃候補
            // 襲撃候補がいればその中からランダムに選択(20%で全体からランダムに変更)
            // 襲撃候補がいなければ全体からランダム
            // （ただし，いずれの場合も人狼と狂人(暫定)は襲撃対象から除く）
            List<Agent> aliveAgentList = LatestDayGameInfo.AliveAgentList;
            aliveAgentList.RemoveAll(a => WolfList.Contains(a));
            aliveAgentList.Remove(maybePossesedAgent);

            List<Agent> attackCandidatePlayer = new List<Agent>();
            foreach (Agent agent in aliveAgentList)
            {
                if (agi.ComingoutMap.ContainsKey(agent))
                {
                    attackCandidatePlayer.Add(agent);
                }
            }

            Random rand = new Random();
            Agent attackAgent;

            if (attackCandidatePlayer.Count > 0 && new Random().NextDouble() < 0.8)
            {
                attackAgent = attackCandidatePlayer[rand.Next(attackCandidatePlayer.Count)];
            }
            else
            {
                attackAgent = aliveAgentList[rand.Next(aliveAgentList.Count)];
            }

            return attackAgent;
        }

        public override void Finish()
        {
        }

        // 今日投票予定のプレイヤーを設定する．
        public void SetPlanningVoteAgent()
        {
            // 下記のいずれの場合も人狼は投票候補に入れない．狂人が分かれば狂人も除く．
            // 村人騙りなら，自分以外からランダム
            // それ以外の場合↓
            // 対抗CO，もしくは自分が黒だと判定したプレイヤーからランダム
            // いなければ白判定を出したプレイヤー以外からランダム
            // それもいなければ生存プレイヤーからランダム

            List<Agent> aliveAgentList = LatestDayGameInfo.AliveAgentList;
            aliveAgentList.RemoveAll(a => WolfList.Contains(a));
            aliveAgentList.Remove(maybePossesedAgent);

            if (fakeRole == Role.VILLAGER)
            {
                if (aliveAgentList.Contains(planningVoteAgent))
                {
                    return;
                }
                else
                {
                    Random rand = new Random();
                    planningVoteAgent = aliveAgentList[rand.Next(aliveAgentList.Count)];
                }
            }

            // 偽占いで人間だと判定したプレイヤーのリスト
            List<Agent> fakeHumanList = new List<Agent>();

            List<Agent> voteAgentCandidate = new List<Agent>();
            foreach (Agent a in aliveAgentList)
            {
                if (agi.ComingoutMap.ContainsKey(a) && agi.ComingoutMap[a] == fakeRole)
                {
                    voteAgentCandidate.Add(a);
                }
            }
            foreach (Judge judge in MyFakeJudgeList)
            {
                if (judge.Result == Species.HUMAN)
                {
                    fakeHumanList.Add(judge.Target);
                }
                else
                {
                    voteAgentCandidate.Add(judge.Target);
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
                // 自分が白判定を出していないプレイヤーのリスト
                List<Agent> aliveAgentExceptHumanList = LatestDayGameInfo.AliveAgentList;
                aliveAgentExceptHumanList.RemoveAll(a => fakeHumanList.Contains(a));

                if (aliveAgentExceptHumanList.Count > 0)
                {
                    Random rand = new Random();
                    planningVoteAgent = aliveAgentExceptHumanList[rand.Next(aliveAgentExceptHumanList.Count)];
                }
                else
                {
                    Random rand = new Random();
                    planningVoteAgent = aliveAgentList[rand.Next(aliveAgentList.Count)];
                }
            }
            return;
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
                    // 自分以外で占い師COするプレイヤーが出たら投票先を変える
                    case Topic.COMINGOUT:
                        agi.ComingoutMap[talk.Agent] = utterance.Role;
                        if (utterance.Role == fakeRole)
                        {
                            SetPlanningVoteAgent();
                        }
                        break;

                    // 占い結果の発話の場合
                    // 人狼以外の占い，霊能結果で嘘だった場合は狂人だと判断
                    case Topic.DIVINED:
                        // AGIのJudgeListに結果を加える
                        Agent seerAgent = talk.Agent;
                        Agent inspectedAgent = utterance.Target;
                        Species inspectResult = (Species)utterance.Result;
                        Judge judge = new Judge(Day, seerAgent, inspectedAgent, inspectResult);
                        agi.AddInspectJudgeList(judge);

                        // ジャッジしたのが人狼以外の場合
                        if (!WolfList.Contains(judge.Agent))
                        {
                            Species judgeSpecies = judge.Result;
                            Species realSpecies;
                            if (WolfList.Contains(judge.Target))
                            {
                                realSpecies = Species.WEREWOLF;
                            }
                            else
                            {
                                realSpecies = Species.HUMAN;
                            }
                            if (judgeSpecies != realSpecies)
                            {
                                maybePossesedAgent = judge.Agent;
                                SetPlanningVoteAgent();
                            }
                        }
                        break;
                }
            }
            readTalkListNum = talkList.Count;
        }

        // 能力者騙りをする際に，偽の占い(or霊能)結果を作成する．
        public void SetFakeResult()
        {
            // 村人騙りなら不必要

            // 偽占い(or霊能)の候補．以下，偽占い候補
            List<Agent> fakeGiftTargetCandidateList = new List<Agent>();

            Agent fakeGiftTarget;

            Species fakeResult;

            if (fakeRole == Role.VILLAGER)
            {
                return;
            }
            else if (fakeRole == Role.SEER)
            {
                List<Agent> aliveAgentList = LatestDayGameInfo.AliveAgentList;
                aliveAgentList.Remove(Me);

                foreach (Agent agent in aliveAgentList)
                {
                    // まだ偽占いしてないプレイヤー，かつ対抗CO者じゃないプレイヤーは偽占い候補
                    if (!IsJudgedAgent(agent) && fakeRole != agi.ComingoutMap[agent])
                    {
                        fakeGiftTargetCandidateList.Add(agent);
                    }
                }

                if (fakeGiftTargetCandidateList.Count > 0)
                {
                    Random rand = new Random();
                    fakeGiftTarget = fakeGiftTargetCandidateList[rand.Next(fakeGiftTargetCandidateList.Count)];
                }
                else
                {
                    aliveAgentList.RemoveAll(a => fakeGiftTargetCandidateList.Contains(a));
                    Random rand = new Random();
                    fakeGiftTarget = aliveAgentList[rand.Next(aliveAgentList.Count)];
                }

                // 人狼が偽占い対象の場合
                if (WolfList.Contains(fakeGiftTarget))
                {
                    fakeResult = Species.HUMAN;
                }
                // 人間が偽占い対象の場合
                else
                {
                    // 狂人(暫定)，または非COプレイヤー
                    if (fakeGiftTarget == maybePossesedAgent || !agi.ComingoutMap.ContainsKey(fakeGiftTarget))
                    {
                        if (new Random().NextDouble() < 0.5)
                        {
                            fakeResult = Species.WEREWOLF;
                        }
                        else
                        {
                            fakeResult = Species.HUMAN;
                        }
                    }
                    // 能力者CO，かつ人間，非狂人(暫定)
                    else
                    {
                        fakeResult = Species.WEREWOLF;
                    }
                }
            }

            else if (fakeRole == Role.MEDIUM)
            {
                fakeGiftTarget = LatestDayGameInfo.ExecutedAgent;
                if (fakeGiftTarget == null)
                {
                    return;
                }
                 // 人狼が偽占い対象の場合
                if (WolfList.Contains(fakeGiftTarget))
                {
                    fakeResult = Species.HUMAN;
                }
                 // 人間が偽占い対象の場合
                else
                {
                    // 狂人(暫定)，または非COプレイヤー
                    if (fakeGiftTarget == maybePossesedAgent || !agi.ComingoutMap.ContainsKey(fakeGiftTarget))
                    {
                        if (new Random().NextDouble() < 0.5)
                        {
                            fakeResult = Species.WEREWOLF;
                        }
                        else
                        {
                            fakeResult = Species.HUMAN;
                        }
                    }
                    // 能力者CO，かつ人間，非狂人(暫定)
                    else
                    {
                        fakeResult = Species.WEREWOLF;
                    }
                }
            }
            else
            {
                return;
            }

            if (fakeGiftTarget != null)
            {
                MyFakeJudgeList.Add(new Judge(Day, Me, fakeGiftTarget, fakeResult));
            }
        }

        /// <summary>
        /// すでに占い(or霊能)対象にしたプレイヤーならtrue,まだ占っていない(霊能していない)ならばfalseを返す． 
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        public bool IsJudgedAgent(Agent agent)
        {
            foreach (Judge judge in MyFakeJudgeList)
            {
                if (judge.Agent == agent)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
