//
// Role.cs
//
// Copyright (c) 2016 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

﻿namespace AIWolf.Common.Data
{
    /// <summary>
    /// Roles of Player.
    /// <para>
    /// Original Java code was written by tori,
    /// and translated into C# by otsuki.
    /// </para>
    /// </summary>
    public enum Role
    {
        BODYGUARD,
        FREEMASON, // ver0.1.xでは使用されません
        MEDIUM,
        POSSESSED,
        SEER,
        VILLAGER,
        WEREWOLF
    }
}
