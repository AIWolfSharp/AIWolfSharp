namespace AIWolf.Common.Net
{
    public interface GameClient
    {
        object Recieve(Packet packet);
    }
}
