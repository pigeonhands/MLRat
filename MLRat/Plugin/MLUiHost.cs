using MLRat.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLRat.Plugin
{
    public delegate void OnContextMenuAdd(MLPlugin plugin, MLRatContextEntry contextEntry);
    public class MLUiHost : IServerUIHandler
    {
        private OnContextMenuAdd OnContextAdd;
        private MLPlugin _plugin;

        public MLUiHost(MLPlugin plugin, OnContextMenuAdd callback)
        {
            _plugin = plugin;
            OnContextAdd = callback;
        }
        public void AddContext(MLRatContextEntry entry)
        {
            if (OnContextAdd != null)
                OnContextAdd(_plugin, entry);
        }
    }
}
