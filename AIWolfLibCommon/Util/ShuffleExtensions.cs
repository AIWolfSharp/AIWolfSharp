//
// ShuffleExtensions.cs
//
// Copyright (c) 2016 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace AIWolf.Common.Util
{
    /// <summary>
    /// Class to define extension method.
    /// <para>
    /// Written by otsuki.
    /// </para>
    /// </summary>
    public static class ShuffleExtensions
    {
        public static IOrderedEnumerable<T> Shuffle<T>(this IEnumerable<T> s)
        {
            return s.OrderBy(x => Guid.NewGuid());
        }
    }
}
