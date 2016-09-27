//
// Topic.cs
//
// Copyright (c) 2016 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

﻿namespace AIWolf.Client.Lib
{
    /// <summary>
    /// 文章の動詞，補語を表す．
    /// <para>
    /// Original Java code was written by kengo,
    /// and translated into C# by otsuki.
    /// </para>
    /// </summary>
    public enum Topic
    {
        // AgentはRoleだと思う
        // ESTIMATE Agent Role
        ESTIMATE,

        // AgentがRoleをカミングアウトする
        // COMINGOUT Agent Role
        COMINGOUT,

        // AgentがSpeciesだと占われる
        // DIVINED Agent Species
        DIVINED,

        // AgentがSpeciesだと霊能される
        // INQUESTED Agent Species
        INQUESTED,

        // Agentが護衛される
        // GUARED Agent
        GUARDED,

        // Agentに投票する
        // VOTE Agent
        VOTE,

        // Agentを襲撃する
        // ATTACK Agent
        ATTACK,

        // 発話[Day][Number]に同意する
        // AGREE Day Number
        AGREE,

        // 発話[Day][Number]に同意しない
        // DISAGREE Day Number
        DISAGREE,

        // もう発話することが無い場合
        // OVER
        OVER,

        // まだ発話したいことがある場合
        // SKIP
        SKIP
    }
}
