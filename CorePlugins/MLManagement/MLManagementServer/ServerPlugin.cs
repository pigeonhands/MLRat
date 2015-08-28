using MLRat.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerPlugin.InterfaceHandle;

namespace MLManagementServer
{
    public class ServerPlugin : IServerPlugin
    {
        public MLPluginInfomation PluginInfomation
        {
            get
            {
                MLPluginInfomation info = new MLPluginInfomation("MLManagement")
                {
                    Developer = "BahNahNah",
                    Description = "Core Plugin"
                };
                return info;
            }
        }

        public void OnClientConnect(IClient client)
        {
            
        }

        public void OnClientDisconnect(IClient client)
        {
            
        }

        public void OnDataRetrieved(IClient client, object[] data)
        {
            
        }

        public void OnPluginLoad(IServerUIHandler UIHost)
        {
            
        }
    }
}
