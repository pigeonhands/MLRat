using MLRat.Server;
using ServerPlugin.InterfaceHandle;

namespace MLRat.Plugin
{
    public delegate void OnContextMenuAdd(MLPlugin plugin, MLRatContextEntry contextEntry);
    public delegate IMLRatColumn OnColumnItemAdd(MLPlugin plugin, string name, string defaultValue);
    public class MLUiHost : IServerUIHandler
    {
        private OnContextMenuAdd OnContextAdd;
        private OnColumnItemAdd OnColumnAdd;
        private MLPlugin _plugin;

        public MLUiHost(MLPlugin plugin, OnContextMenuAdd callback, OnColumnItemAdd _columnAdd)
        {
            _plugin = plugin;
            OnContextAdd = callback;
            OnColumnAdd = _columnAdd;
        }

        public IMLRatColumn AddColumn(string name, string defaultValue)
        {
            return OnColumnAdd(_plugin, name, defaultValue);
        }

        public void AddContext(MLRatContextEntry entry)
        {
            if (OnContextAdd != null)
                OnContextAdd(_plugin, entry);
        }
    }
}
