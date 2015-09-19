namespace AIWolf.Server.Util
{
    public interface IGameLogger
    {
        void Log(string log);
        void Flush();
        void Close();
    }
}
