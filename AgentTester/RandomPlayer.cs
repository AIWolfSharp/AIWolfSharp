﻿using AIWolf.Client.Lib;
using AIWolf.Common.Data;
using AIWolf.Common.Net;
using AIWolf.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AIWolf.AgentTester
{
    class RandomPlayer : IPlayer
    {
        Dictionary<int, GameInfo> gameInfoMap = new Dictionary<int, GameInfo>();

        int day;

        Agent me;

        Role myRole;

        GameSetting gameSetting;

        List<Agent> aliveAgent;

        public Agent Attack()
        {
            return aliveAgent.Shuffle().First();
        }

        public void DayStart()
        {
            aliveAgent = LatestDayGameInfo.AliveAgentList;
            aliveAgent.Remove(LatestDayGameInfo.Agent);
        }

        public Agent Divine()
        {
            return aliveAgent.Shuffle().First();
        }

        public void Finish()
        {
        }

        public string Name { get { return "RandomAgent"; } }

        public Agent Guard()
        {
            return aliveAgent.Shuffle().First();
        }

        public void Initialize(GameInfo gameInfo, GameSetting gameSetting)
        {
            gameInfoMap.Clear();
            this.gameSetting = gameSetting;
            day = gameInfo.Day;
            gameInfoMap[day] = gameInfo;
            myRole = (Role)gameInfo.Role;
            me = gameInfo.Agent;
            return;
        }

        public GameInfo LatestDayGameInfo { get { return gameInfoMap[day]; } }

        public string Talk()
        {
            Random r = new Random(Guid.NewGuid().GetHashCode());

            TalkType[] talkTypes = (TalkType[])Enum.GetValues(typeof(TalkType));
            List<Agent> allAgent = LatestDayGameInfo.AgentList.Shuffle().ToList();
            Species[] species = (Species[])Enum.GetValues(typeof(Species));
            Role[] roles = (Role[])Enum.GetValues(typeof(Role));

            switch (r.Next(10))
            {

                case 0:
                    return TemplateTalkFactory.Agree(talkTypes[r.Next(talkTypes.Length)], r.Next(100), r.Next(100));
                case 1:
                    return TemplateTalkFactory.Comingout(allAgent[0], roles[r.Next(roles.Length)]);
                case 2:
                    return TemplateTalkFactory.Disagree(talkTypes[r.Next(talkTypes.Length)], r.Next(100), r.Next(100));
                case 3:
                    return TemplateTalkFactory.Divined(allAgent[0], species[r.Next(species.Length)]);
                case 4:
                    return TemplateTalkFactory.Estimate(allAgent[0], roles[r.Next(roles.Length)]);
                case 5:
                    return TemplateTalkFactory.Guarded(allAgent[0]);
                case 6:
                    return TemplateTalkFactory.Inquested(allAgent[0], species[r.Next(species.Length)]);
                case 7:
                    return TemplateTalkFactory.Over();
                case 8:
                    return TemplateTalkFactory.Skip();
                case 9:
                    return TemplateTalkFactory.Vote(allAgent[0]);
            }
            return TemplateTalkFactory.Over();
        }

        public void Update(GameInfo gameInfo)
        {
            day = gameInfo.Day;

            gameInfoMap[day] = gameInfo;
        }

        public Agent Vote()
        {
            return aliveAgent.Shuffle().First();
        }

        public string Whisper()
        {
            Random r = new Random(Guid.NewGuid().GetHashCode());

            TalkType[] talkTypes = (TalkType[])Enum.GetValues(typeof(TalkType));
            List<Agent> allAgent = LatestDayGameInfo.AgentList.Shuffle().ToList();
            Species[] species = (Species[])Enum.GetValues(typeof(Species));
            Role[] roles = (Role[])Enum.GetValues(typeof(Role));

            switch (r.Next(10))
            {

                case 0:
                    return TemplateWhisperFactory.Agree(talkTypes[r.Next(talkTypes.Length)], r.Next(100), r.Next(100));
                case 1:
                    return TemplateWhisperFactory.Comingout(allAgent[0], roles[r.Next(roles.Length)]);
                case 2:
                    return TemplateWhisperFactory.Disagree(talkTypes[r.Next(talkTypes.Length)], r.Next(100), r.Next(100));
                case 3:
                    return TemplateWhisperFactory.Divined(allAgent[0], species[r.Next(species.Length)]);
                case 4:
                    return TemplateWhisperFactory.Estimate(allAgent[0], roles[r.Next(roles.Length)]);
                case 5:
                    return TemplateWhisperFactory.Guarded(allAgent[0]);
                case 6:
                    return TemplateWhisperFactory.Inquested(allAgent[0], species[r.Next(species.Length)]);
                case 7:
                    return TemplateWhisperFactory.Over();
                case 8:
                    return TemplateWhisperFactory.Skip();
                case 9:
                    return TemplateWhisperFactory.Vote(allAgent[0]);
                case 10:
                    return TemplateWhisperFactory.Attack(allAgent[0]);
            }

            return TemplateTalkFactory.Over();
        }
    }
}