using System;
using System.Reflection;

namespace AIWolf.NativePlayer
{
    /// <summary>
    /// 2段目のwrapper
    /// NativeWrapperから呼ぶため
    /// csc /t:module で CSWrapper.netmodule を生成すること
    /// 作者の実力ではここから実際のプレイヤーのインスタンスを生成できなかったため
    /// 3段目のwrapperであるNativePlayerを介している
    /// </summary>
    public class CSWrapper
    {
        // NativePlayerのdllファイル名
        string nativePlayerDllName = @"C:\somewhere\NativePlayer.dll";
        string nativePlayerClassName = "AIWolf.NativePlayer.NativePlayer";
        dynamic player;

        public string Name
        {
            get { return player.Name; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="playerDllFileName">Javaから指定されるNativePlayerに渡す実際のプレイヤーのdllファイル名</param>
        /// <param name="playerClassName">Javaから指定されるNativePlayerに渡す実際のプレイヤーのクラス名</param>
        public CSWrapper(string playerDllFileName, string playerClassName)
        {
            Assembly assembly = Assembly.LoadFrom(nativePlayerDllName);
            player = Activator.CreateInstance(assembly.GetType(nativePlayerClassName), new object[] { playerDllFileName, playerClassName });
        }

        public void Update(string packetString)
        {
            player.Update(packetString);
        }

        public void Initialize(string packetString)
        {
            player.Initialize(packetString);
        }

        public void DayStart()
        {
            player.DayStart();
        }

        public string Talk()
        {
            return player.Talk();
        }

        public string Whisper()
        {
            return player.Whisper();
        }

        public int Vote()
        {
            return player.Vote() ;
        }

        public int Attack()
        {
            return player.Attack();
        }

        public int Divine()
        {
            return player.Divine();
        }

        public int Guard()
        {
            return player.Guard();
        }

        public void Finish()
        {
            player.Finish();
        }
    }
}
