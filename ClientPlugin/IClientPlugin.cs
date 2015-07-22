namespace MLRat.Client
{
    public interface IClientPlugin
    {
        void OnDataRecieved(object[] data);
        void OnConnect(IClientConnection server);
        void OnDisconnect();
        void OnPluginLoad();
    }
}
