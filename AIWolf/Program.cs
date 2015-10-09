using AIWolf.Common.Data;
using AIWolf.Common.Net;
using AIWolf.Common.Util;
using AIWolf.Server.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AIWolf
{
    class Program
    {
        public static void Main(string[] args)
        {
            DataConverter dc = DataConverter.GetInstance();
            Agent agent = Agent.GetAgent(2);
            Console.WriteLine(dc.Convert(agent));
        }

        public static void Test()
        {
            Agent[] agents = new Agent[10];
            Random rand = new Random();

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
            gi.Agent = 7;
            gi.AttackedAgent = 2;
            VoteToSend vote1 = new VoteToSend();
            vote1.Day = 1;
            vote1.Agent = 2;
            vote1.Target = 3;
            VoteToSend vote2 = new VoteToSend();
            vote2.Day = 1;
            vote2.Agent = 4;
            vote2.Target = 5;
            gi.AttackVoteList.Add(vote1);
            gi.AttackVoteList.Add(vote2);
            Dictionary<int, string> status = new Dictionary<int, string>();
            status.Add(0, "ALIVE");
            status.Add(2, "ALIVE");
            status.Add(4, "ALIVE");
            status.Add(5, "DEAD");
            status.Add(3, "DEAD");
            status.Add(1, "ALIVE");
            gi.StatusMap = status;
            Dictionary<int, string> role = new Dictionary<int, string>();
            role.Add(0, "VILLAGER");
            role.Add(1, "WEREWOLF");
            gi.RoleMap = role;
            Console.WriteLine(dc.Convert(gi));

            Console.WriteLine(CalendarTools.ToDateTime(DateTime.Now.Ticks));
            Console.WriteLine(CalendarTools.ToTimeString(DateTime.Now));
            //var logger = new FileGameLogger("Log.log");
            //logger.Log("Log.log");
            //logger.Flush();
            //logger.Close();

            List<Agent> aList = agents.Shuffle().ToList();
            Dictionary<Agent, int> counter = new Dictionary<Agent, int>();
            foreach (Agent a in aList)
            {
                if (!counter.ContainsKey(a))
                {
                    counter.Add(a, rand.Next());
                }
                else
                {
                    counter[a]++;
                }
            }
            foreach (Agent a in counter.Keys)
            {
                Console.WriteLine(a + " " + counter[a]);
            }
            foreach (Agent a in aList)
            {
                if (!counter.ContainsKey(a))
                {
                    counter.Add(a, rand.Next());
                }
                else
                {
                    counter[a]++;
                }
            }
            Console.WriteLine();
            var v = counter.OrderBy(x => x.Value);
            foreach (var x in v)
            {
                Console.WriteLine(x.Key + " " + x.Value);
            }
            Console.WriteLine("max =" + v.Last().Value);
            Console.WriteLine(aList.Shuffle().First());
        }
    }
}
