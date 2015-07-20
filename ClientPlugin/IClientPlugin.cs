using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
