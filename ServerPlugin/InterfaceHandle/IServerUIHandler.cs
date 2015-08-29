using ServerPlugin.InterfaceHandle;

namespace MLRat.Server
{
    public interface IServerUIHandler
    {
        void AddContext(MLRatContextEntry entry);
        IMLRatColumn AddColumn(string name, string defaultVaule);
    }
}
