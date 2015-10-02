using AIWolf.Common.Data;
using System.Collections.Generic;

namespace AIWolf.Client.Lib
{
    /// <summary>
    /// Class to define extension method.
    /// <para>
    /// Written by otsuki.
    /// </para>
    /// </summary>
    static class StateExtensions
    {
        static Dictionary<State, EnumType> stateEnumTypeMap = new Dictionary<State, EnumType>();

        static StateExtensions()
        {
            stateEnumTypeMap[State.bodyguard] = EnumType.ROLE;
            stateEnumTypeMap[State.freemason] = EnumType.ROLE;
            stateEnumTypeMap[State.medium] = EnumType.ROLE;
            stateEnumTypeMap[State.possessed] = EnumType.ROLE;
            stateEnumTypeMap[State.seer] = EnumType.ROLE;
            stateEnumTypeMap[State.villager] = EnumType.ROLE;
            stateEnumTypeMap[State.werewolf] = EnumType.ROLE;
            stateEnumTypeMap[State.villagerSide] = EnumType.TEAM;
            stateEnumTypeMap[State.werewolfSide] = EnumType.TEAM;
            stateEnumTypeMap[State.HUMAN] = EnumType.SPECIES;
            //stateEnumTypeMap[State.Wolf]= EnumType.SPECIES;
            stateEnumTypeMap[State.GIFTED] = EnumType.GIFTED;
        }

        static EnumType GetEnumType(this State state)
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
