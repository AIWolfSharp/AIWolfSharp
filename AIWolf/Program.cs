using AIWolf.Common.Data;
using System;

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
        }
    }
}
