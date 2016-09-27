//
// SampleMedium.cs
//
// Copyright (c) 2016 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

﻿using AIWolf.Client.Base.Player;
using AIWolf.Client.Lib;
using AIWolf.Common.Data;
using AIWolf.Common.Net;
using System;
using System.Collections.Generic;

namespace AIWolf.Client.Base.Smpl
{
    class SampleMedium : AbstractMedium
    {
        // COする日にち
        int comingoutDay;

        // CO済みか否か
        bool isCameout;

        // 全体に霊能結果を報告済みのJudge
        List<Judge> declaredJudgedAgentList = new List<Judge>();

        bool isSaidAllInquestResult;

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
            setPlanningVoteAgent();
            isSaidAllInquestResult = false;
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
            else if (isCameout && !isSaidAllInquestResult)
            {
                foreach (Judge judge in MyJudgeList)
                {
                    if (!declaredJudgedAgentList.Contains(judge))
                    {
                        declaredJudgedAgentList.Add(judge);
                        return TemplateTalkFactory.Inquested(judge.Target, judge.Result);
                    }
                }
                isSaidAllInquestResult = true;
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

        public override void Finish()
        {
        }

        public override void Update(GameInfo gameInfo)
        {
            base.Update(gameInfo);

            List<Talk> talkList = gameInfo.TalkList;
            bool existInspectResult = false;

            // talkListからCO，占い結果の抽出
            for (int i = readTalkListNum; i < talkList.Count; i++)
            {
                Talk talk = talkList[i];
                Utterance utterance = new Utterance(talk.Content);
                switch (utterance.Topic)
                {

                    //カミングアウトの発話の場合
                    case Topic.COMINGOUT:
                        agi.ComingoutMap[talk.Agent] = utterance.Role;
                        if (utterance.Role == MyRole)
                        {
                            setPlanningVoteAgent();
                        }
                        break;

                    //占い結果の発話の場合
                    case Topic.DIVINED:
                        //AGIのJudgeListに結果を加える
                        Agent seerAgent = talk.Agent;
                        Agent inspectedAgent = utterance.Target;
                        Species inspectResult = (Species)utterance.Result;
                        Judge judge = new Judge(Day, seerAgent, inspectedAgent, inspectResult);
                        agi.AddInspectJudgeList(judge);

                        existInspectResult = true;
                        break;
                }
            }
            readTalkListNum = talkList.Count;

            // 新しい占い結果があれば投票先を変える．(新たに黒判定が出た，または投票先のプレイヤーに白判定が出た場合)
            if (existInspectResult)
            {
                setPlanningVoteAgent();
            }
        }

        public void setPlanningVoteAgent()
        {
            // 投票先を未設定，または人狼だと占われたプレイヤー以外を投票先にしている場合
            // 人狼だと占われたプレイヤーがいれば，投票先をそのプレイヤーに設定
            // いなければ生存プレイヤーからランダムに選択

            List<Agent> voteAgentCandidate = new List<Agent>();

            List<Agent> aliveAgentList = LatestDayGameInfo.AliveAgentList;
            aliveAgentList.Remove(Me);

            foreach (Agent agent in aliveAgentList)
            {
                // 自分以外に霊能COしているプレイヤーがいれば投票候補
                if (agi.ComingoutMap.ContainsKey(agent) && agi.ComingoutMap[agent] == Role.MEDIUM)
                {
                    voteAgentCandidate.Add(agent);
                }
            }

            foreach (Judge myJudge in MyJudgeList)
            {
                foreach (Judge otherJudge in agi.InspectJudgeList)
                {
                    if (!aliveAgentList.Contains(otherJudge.Agent))
                    {
                        continue;
                    }
                    // 自分と同じ相手について占っている場合
                    if (myJudge.Target.Equals(otherJudge.Target))
                    {
                        // 自分の占い(霊能)結果と異なる結果を出していたら投票候補
                        if (myJudge.Result != otherJudge.Result)
                        {
                            voteAgentCandidate.Add(otherJudge.Agent);
                        }
                    }
                }
            }

            // すでに投票先に指定しているプレイヤーが投票候補内に含まれていたらそのまま
            if (planningVoteAgent != null && voteAgentCandidate.Contains(planningVoteAgent))
            {
                return;
            }
            else
            {
                if (voteAgentCandidate.Count > 0)
                {
                    Random rand = new Random();
                    planningVoteAgent = voteAgentCandidate[rand.Next(voteAgentCandidate.Count)];
                }
                else
                {

                    // 投票候補がいない場合は占いで黒判定されているプレイヤーからランダムに選択
                    List<Agent> subVoteAgentCandidate = new List<Agent>();
                    foreach (Judge judge in agi.InspectJudgeList)
                    {
                        if (aliveAgentList.Contains(judge.Target) && judge.Result == Species.WEREWOLF)
                        {
                            subVoteAgentCandidate.Add(judge.Target);
                        }
                    }

                    if (subVoteAgentCandidate.Count > 0)
                    {
                        Random rand = new Random();
                        planningVoteAgent = subVoteAgentCandidate[rand.Next(subVoteAgentCandidate.Count)];
                    }
                    else
                    {
                        // 黒判定されているプレイヤーもいなければ生存プレイヤーからランダムに選択
                        Random rand = new Random();
                        planningVoteAgent = aliveAgentList[rand.Next(aliveAgentList.Count)];
                    }
                }
            }
        }
    }
}
