namespace AIWolf.Client.Lib
{
    /// <summary>
    /// 役職，陣営等を表すenum．内部の処理で用いる
    /// <para>
    /// Original Java code was written by kengo,
    /// and translated into C# by otsuki.
    /// </para>
    /// </summary>
    enum State
    {
        // bodyguard
        bodyguard,

        // freemason
        freemason,

        // medium
        medium,

        // Possessed
        possessed,

        // Seer
        seer,

        // Villager
        villager,

        // WereWolf
        werewolf,

        villagerSide,

        werewolfSide,

        HUMAN,

        //Wolf,

        GIFTED
    }
}
