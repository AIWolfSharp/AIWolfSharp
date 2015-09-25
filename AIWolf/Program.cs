using AIWolf.Common.Data;
using AIWolf.Common.Net;
using System;
using System.Collections.Generic;

namespace AIWolf
{
    class Program
    {
        public static void Main(string[] args)
        {
            Agent[] agents = new Agent[10];

            for (int i = 0; i < 10; i++)
            {
                agents[i] = Agent.GetAgent(i);
                System.Console.WriteLine(agents[i]);
            }

            Guard guard = new Guard(1, agents[2], agents[5]);
            System.Console.WriteLine(guard);

            Judge judge = new Judge(2, agents[1], agents[9], Species.WEREWOLF);
            System.Console.WriteLine(judge);

            foreach (Request r in Enum.GetValues(typeof(Request)))
            {
                System.Console.WriteLine(r + " has return : " + r.HasReturn());
            }

            foreach (Role r in Enum.GetValues(typeof(Role)))
            {
                System.Console.WriteLine(r + " belongs to team " + r.GetTeam());
                System.Console.WriteLine(r + " is " + r.GetSpecies());
            }

            Talk talk = new Talk(3, 5, agents[7], "Hello");
            System.Console.WriteLine(talk);

            Vote vote = new Vote(1, agents[2], agents[3]);
            System.Console.WriteLine(vote);

            DataConverter dc = DataConverter.GetInstance();
            GameInfoToSend gi = new GameInfoToSend();
            gi.agent = 7;
            gi.attackedAgent = 2;
            VoteToSend vote1 = new VoteToSend();
            vote1.day = 1;
            vote1.agent = 2;
            vote1.target = 3;
            VoteToSend vote2 = new VoteToSend();
            vote2.day = 1;
            vote2.agent = 4;
            vote2.target = 5;
            gi.attackVoteList.Add(vote1);
            gi.attackVoteList.Add(vote2);
            Dictionary<string, string> status = new Dictionary<string, string>();
            status.Add("0", "ALIVE");
            status.Add("2", "ALIVE");
            status.Add("4", "ALIVE");
            status.Add("5", "DEAD");
            status.Add("3", "DEAD");
            status.Add("1", "ALIVE");
            gi.statusMap = status;
            Dictionary<string, string> role = new Dictionary<string, string>();
            role.Add("0", "VILLAGER");
            role.Add("1", "WEREWOLF");
            gi.roleMap = role;
            System.Console.WriteLine(dc.Convert(gi));
        }
    }
}
