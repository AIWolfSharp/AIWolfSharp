using AIWolf.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIWolf.Common.Net;

namespace AIWolf
{
    class TestPlayer : Player
    {
        GameInfo gi;

        Agent Player.Attack()
        {
            return gi.Agent;
        }

        void Player.DayStart()
        {
        }

        Agent Player.Divine()
        {
            return gi.Agent;
        }

        void Player.Finish()
        {
        }

        string Player.GetName()
        {
            return "TestPlayer";
        }

        Agent Player.Guard()
        {
            return gi.Agent;
        }

        void Player.Initialize(GameInfo gameInfo, GameSetting gameSetting)
        {
        }

        string Player.Talk()
        {
            return Talk.OVER;
        }

        void Player.Update(GameInfo gameInfo)
        {
            gi = gameInfo;
        }

        Agent Player.Vote()
        {
            return gi.Agent;
        }

        string Player.Whisper()
        {
            return Talk.OVER;
        }
    }
}
