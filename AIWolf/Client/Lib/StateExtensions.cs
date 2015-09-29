using AIWolf.Common.Data;
using System.Collections.Generic;

namespace AIWolf.Client.Lib
{
    /// <summary>
    /// Class to define extension method.
    /// <para>
    /// written by otsuki
    /// </para>
    /// </summary>
    public static class StateExtensions
    {
        private static Dictionary<State, EnumType> stateEnumTypeMap = new Dictionary<State, EnumType>();

        static StateExtensions()
        {
            stateEnumTypeMap.Add(State.bodyguard, EnumType.ROLE);
            stateEnumTypeMap.Add(State.freemason, EnumType.ROLE);
            stateEnumTypeMap.Add(State.medium, EnumType.ROLE);
            stateEnumTypeMap.Add(State.possessed, EnumType.ROLE);
            stateEnumTypeMap.Add(State.seer, EnumType.ROLE);
            stateEnumTypeMap.Add(State.villager, EnumType.ROLE);
            stateEnumTypeMap.Add(State.werewolf, EnumType.ROLE);
            stateEnumTypeMap.Add(State.villagerSide, EnumType.TEAM);
            stateEnumTypeMap.Add(State.werewolfSide, EnumType.TEAM);
            stateEnumTypeMap.Add(State.HUMAN, EnumType.SPECIES);
            //stateEnumTypeMap.Add(State.Wolf, EnumType.SPECIES);
            stateEnumTypeMap.Add(State.GIFTED, EnumType.GIFTED);
        }

        public static EnumType GetEnumType(this State state)
        {
            return stateEnumTypeMap[state];
        }

        public static Role? ToRole(this State? state)
        {
            switch (state)
            {
                case State.bodyguard:
                    return Role.BODYGUARD;
                case State.freemason:
                    return Role.FREEMASON;
                case State.medium:
                    return Role.MEDIUM;
                case State.possessed:
                    return Role.POSSESSED;
                case State.seer:
                    return Role.SEER;
                case State.villager:
                    return Role.VILLAGER;
                case State.werewolf:
                    return Role.WEREWOLF;
                default:
                    return null;
            }
        }

        public static Species? ToSpecies(this State? state)
        {
            switch (state)
            {
                case State.HUMAN:
                    return Species.HUMAN;
                case State.werewolf:
                    return Species.WEREWOLF;
                default:
                    return null;
            }
        }
    }
}
