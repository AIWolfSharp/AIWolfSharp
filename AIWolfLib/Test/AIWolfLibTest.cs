using AIWolf.Common.Data;
using AIWolf.Common.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;

namespace AIWolfLib.Test
{
    public class AIWolfLibTest
    {
        public static void Main(string[] args)
        {
            DataConverter dc = DataConverter.GetInstance();
            Agent agent1 = Agent.GetAgent(1);
            Agent agent2 = Agent.GetAgent(2);
            Console.WriteLine(dc.Convert(agent1));
            Guard guard = new Guard(1, agent1, agent2);
            Console.WriteLine(dc.Convert(guard));
            Judge judge = new Judge(2, agent2, agent1, Species.WEREWOLF);
            Console.WriteLine(dc.Convert(judge));
            Talk talk = new Talk(10, 3, agent1, "Hello");
            Console.WriteLine(dc.Convert(talk));
            Vote vote = new Vote(4, agent1, agent2);
            Console.WriteLine(dc.Convert(vote));
            GameInfo gameInfo = new GameInfo();
            Console.WriteLine(dc.Convert(gameInfo));
            GameInfoToSend gameInfoToSend = new GameInfoToSend();
            Console.WriteLine(dc.Convert(gameInfoToSend));
            GameSetting gameSetting = new GameSetting();
            Console.WriteLine(dc.Convert(gameSetting));
            JudgeToSend judgeToSend = new JudgeToSend(judge);
            Console.WriteLine(dc.Convert(judgeToSend));
            Packet packet = new Packet(Request.Initialize, gameInfoToSend, gameSetting);
            Console.WriteLine(dc.Convert(packet));
            TalkToSend talkToSend = new TalkToSend(talk);
            Console.WriteLine(dc.Convert(talkToSend));
            VoteToSend voteToSend = new VoteToSend(vote);
            Console.WriteLine(dc.Convert(voteToSend));
        }
    }
}
