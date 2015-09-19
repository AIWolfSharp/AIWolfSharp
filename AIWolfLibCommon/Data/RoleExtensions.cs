using System.Collections.Generic;

namespace AIWolf.Common.Data
{
    /// <summary>
    /// Class to define extension mathod.
    /// <para>
    /// Written by otsuki.
    /// </para>
    /// </summary>
    public static class RoleExtensions
    {
        static Dictionary<Role, Team> roleTeamMap = new Dictionary<Role, Team>();
        static Dictionary<Role, Species> roleSpeciesMap = new Dictionary<Role, Species>();

        static RoleExtensions()
        {
            roleTeamMap[Role.BODYGUARD] = Team.VILLAGER;
            roleSpeciesMap[Role.BODYGUARD] = Species.HUMAN;

            //ver0.1.xでは使用されません
            roleTeamMap[Role.FREEMASON] = Team.VILLAGER;
            roleSpeciesMap[Role.FREEMASON] = Species.HUMAN;

            roleTeamMap[Role.MEDIUM] = Team.VILLAGER;
            roleSpeciesMap[Role.MEDIUM] = Species.HUMAN;

            roleTeamMap[Role.POSSESSED] = Team.WEREWOLF;
            roleSpeciesMap[Role.POSSESSED] = Species.HUMAN;

            roleTeamMap[Role.SEER] = Team.VILLAGER;
            roleSpeciesMap[Role.SEER] = Species.HUMAN;

            roleTeamMap[Role.VILLAGER] = Team.VILLAGER;
            roleSpeciesMap[Role.VILLAGER] = Species.HUMAN;

            roleTeamMap[Role.WEREWOLF] = Team.WEREWOLF;
            roleSpeciesMap[Role.WEREWOLF] = Species.WEREWOLF;
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
