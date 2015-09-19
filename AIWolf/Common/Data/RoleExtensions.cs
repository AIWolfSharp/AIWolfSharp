using System.Collections.Generic;

namespace AIWolf.Common.Data
{
    /// <summary>
    /// Class to define extension mathod.
    /// <para>
    /// written by otsuki
    /// </para>
    /// </summary>
    public static class RoleExtensions
    {
        private static Dictionary<Role, Team> roleTeamMap = new Dictionary<Role, Team>();
        private static Dictionary<Role, Species> roleSpeciesMap = new Dictionary<Role, Species>();

        static RoleExtensions()
        {
            roleTeamMap.Add(Role.BODYGUARD, Team.VILLAGER);
            roleSpeciesMap.Add(Role.BODYGUARD, Species.HUMAN);

            //ver0.1.xでは使用されません
            roleTeamMap.Add(Role.FREEMASON, Team.VILLAGER);
            roleSpeciesMap.Add(Role.FREEMASON, Species.HUMAN);

            roleTeamMap.Add(Role.MEDIUM, Team.VILLAGER);
            roleSpeciesMap.Add(Role.MEDIUM, Species.HUMAN);

            roleTeamMap.Add(Role.POSSESSED, Team.WEREWOLF);
            roleSpeciesMap.Add(Role.POSSESSED, Species.HUMAN);

            roleTeamMap.Add(Role.SEER, Team.VILLAGER);
            roleSpeciesMap.Add(Role.SEER, Species.HUMAN);

            roleTeamMap.Add(Role.VILLAGER, Team.VILLAGER);
            roleSpeciesMap.Add(Role.VILLAGER, Species.HUMAN);

            roleTeamMap.Add(Role.WEREWOLF, Team.WEREWOLF);
            roleSpeciesMap.Add(Role.WEREWOLF, Species.WEREWOLF);
        }

        public static Team GetTeam(this Role role)
        {
            return roleTeamMap[role];
        }

        public static Species GetSpecies(this Role role)
        {
            return roleSpeciesMap[role];
        }
    }
}
