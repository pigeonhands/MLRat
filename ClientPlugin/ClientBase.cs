namespace MLRat.Client
{
    public interface IClientConnection
    {
        void Send(params object[] data);
    }
}
