using System;
using System.Drawing;
using MLRat.Server;
using ServerPlugin.InterfaceHandle;

namespace MLRat.Plugin
{
    public delegate void OnContextMenuAdd(MLPlugin plugin, MLRatContextEntry contextEntry);
    public delegate IMLRatColumn OnColumnItemAdd(MLPlugin plugin, string name, string defaultValue);
    public delegate Image GetImageDelegate(string name);
    public class MLUiHost : IServerUIHandler
    {
        private OnContextMenuAdd OnContextAdd;
        private OnColumnItemAdd OnColumnAdd;
        private MLPlugin _plugin;
        private GetImageDelegate _getImage;

        public MLUiHost(MLPlugin plugin, OnContextMenuAdd callback, OnColumnItemAdd _columnAdd, GetImageDelegate _getImagecallback)
        {
            _getImage = _getImagecallback;
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

        public Image GetImage(string name)
        {
            if (_getImage != null)
                return _getImage(name);
            return null;
        }
    }
}
