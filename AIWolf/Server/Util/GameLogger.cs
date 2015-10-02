namespace AIWolf.Server.Util
{
    interface GameLogger
    {
        void Log(string log);
        void Flush();
        void Close();
    }
}
