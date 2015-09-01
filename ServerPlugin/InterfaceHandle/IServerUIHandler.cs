using ServerPlugin.InterfaceHandle;
using System.Drawing;

namespace MLRat.Server
{
    public interface IServerUIHandler
    {
        void AddContext(MLRatContextEntry entry);
        IMLRatColumn AddColumn(string name, string defaultVaule);
        Image GetImage(string name);
    }
}
