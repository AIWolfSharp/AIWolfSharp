using AIWolf.Common.Data;
using AIWolf.Common.Net;
using System;
using System.Reflection;

namespace AIWolf.NativePlayer
{
    /// <summary>
    /// 実際のプレイヤークラスのインスタンスを生成する3段目のwrapper
    /// </summary>
    public class NativePlayer
    {
        IPlayer player;
        DataConverter dc;

        public NativePlayer(string dllFileName, string playerClassName)
        {
            dc = DataConverter.GetInstance();
            Assembly assembly = Assembly.LoadFrom(dllFileName);
            player = (IPlayer)Activator.CreateInstance(assembly.GetType(playerClassName));
            Name = player.Name + "(.NET)";
        }

        public string Name { get; }

        public int Attack()
        {
            return player.Attack().AgentIdx;
        }

        public void DayStart()
        {
            player.DayStart();
        }

        public int Divine()
        {
            return player.Divine().AgentIdx;
        }

        public void Finish()
        {
            player.Finish();
        }

        public int Guard()
        {
            return player.Guard().AgentIdx;
        }

        public void Initialize(string packetString)
        {
            Packet packet = dc.ToPacket(packetString);
            player.Initialize(packet.GameInfo.ToGameInfo(), packet.GameSetting);
        }

        public string Talk()
        {
            return player.Talk();
        }

        public void Update(string packetString)
        {
            Packet packet = dc.ToPacket(packetString);
            player.Update(packet.GameInfo.ToGameInfo());
        }

        public int Vote()
        {
            return player.Vote().AgentIdx;
        }

        public string Whisper()
        {
            return player.Whisper();
        }
    }
}
