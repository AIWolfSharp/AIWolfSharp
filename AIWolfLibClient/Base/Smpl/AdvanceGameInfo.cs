//
// AdvanceGameInfo.cs
//
// Copyright (c) 2016 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

﻿using AIWolf.Common.Data;
using System.Collections.Generic;

namespace AIWolf.Client.Base.Smpl
{
    class AdvanceGameInfo
    {
        /// <summary>
        /// 発話で伝えられた占い結果のリスト．今回のプロトコルでは何日目に占ったのか分からないので，発話日に設定．
        /// </summary>
        public List<Judge> InspectJudgeList { get; set; } = new List<Judge>();

        /// <summary>
        /// 発話で伝えられた霊能結果のリスト．今回のプロトコルでは何日目に霊能したのか分からないので，発話日に設定．
        /// </summary>
        List<Judge> MediumJudgeList { get; set; } = new List<Judge>();

        public Dictionary<Agent, Role?> ComingoutMap { get; set; } = new Dictionary<Agent, Role?>();

        /// <summary>
        /// COしたプレイヤーをComingoutMapに加える．
        /// </summary>
        /// <param name="agent"></param>
        /// <param name="role"></param>
        public void PutComingoutMap(Agent agent, Role role)
        {
            ComingoutMap[agent] = role;
        }

        public void AddInspectJudgeList(Judge judge)
        {
            InspectJudgeList.Add(judge);
        }

        public void AddMediumJudgeList(Judge judge)
        {
            MediumJudgeList.Add(judge);
        }
    }
}