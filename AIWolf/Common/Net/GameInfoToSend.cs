using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIWolf.Common.Net
{
    /// <summary>
    /// Game information which send to each player
    /// <para>
    /// Original Java code was written by tori,
    /// and translated into C# by otsuki.
    /// </para>
    /// </summary>
    public class GameInfoToSend
    {
        public int Day { get; set; }
        public int Agent { get; set; }
    }
}
