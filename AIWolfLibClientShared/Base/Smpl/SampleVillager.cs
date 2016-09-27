//
// SampleVillager.cs
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
    class SampleVillager : AbstractVillager
    {
        // 投票アルゴリズム：人狼だと占われたエージェント，いなければ，ランダム
        // 発話：その日に投票しようとしているエージェントを報告．変化すれば報告．

        AdvanceGameInfo agi = new AdvanceGameInfo();

        // 今日投票しようと思っているプレイヤー
        Agent planningVoteAgent;

        // 自分が最後に宣言した「投票しようと思っているプレイヤー」
        Agent declaredPlanningVoteAgent;

        // 会話をどこまで読んだか
        int readTalkListNum;

        public override void DayStart()
        {
            declaredPlanningVoteAgent = null;
            planningVoteAgent = null;
            SetPlanningVoteAgent();
            readTalkListNum = 0;
        }

        public override string Talk()
        {
            if (declaredPlanningVoteAgent != planningVoteAgent)
            {
                declaredPlanningVoteAgent = planningVoteAgent;
                return TemplateTalkFactory.Vote(planningVoteAgent);
            }
            else
            {
                return TemplateTalkFactory.Over();
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
                    // カミングアウトの発話の場合
                    case Topic.COMINGOUT:
                        agi.ComingoutMap[talk.Agent] = utterance.Role;
                        break;

                    // 占い結果の発話の場合
                    case Topic.DIVINED:
                        // AGIのJudgeListに結果を加える
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
                SetPlanningVoteAgent();
            }
        }


        public void SetPlanningVoteAgent()
        {
            // 人狼だと占われたプレイヤーを指定している場合はそのまま
            if (planningVoteAgent != null)
            {
                foreach (Judge judge in agi.InspectJudgeList)
                {
                    if (judge.Target.Equals(planningVoteAgent))
                    {
                        return;
                    }
                }
            }

            // 投票先を未設定，または人狼だと占われたプレイヤー以外を投票先にしている場合
            // 人狼だと占われたプレイヤーがいれば，投票先をそのプレイヤーに設定
            // いなければ生存プレイヤーからランダムに選択
            List<Agent> voteAgentCandidate = new List<Agent>();

            List<Agent> aliveAgentList = LatestDayGameInfo.AliveAgentList;
            aliveAgentList.Remove(Me);

            foreach (Judge judge in agi.InspectJudgeList)
            {
                if (aliveAgentList.Contains(judge.Target) && judge.Result == Species.WEREWOLF)
                {
                    voteAgentCandidate.Add(judge.Target);
                }
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
