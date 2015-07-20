using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MLRat.Server;
using ServerPlugin.InterfaceHandle;

public class MLManagement_Server : IServerPlugin 
{
    public void OnClientConnect(IClient client)
    {
        throw new NotImplementedException();
    }

    public void OnClientDisconnect(IClient client)
    {
        throw new NotImplementedException();
    }

    public void OnDataRetrieved(IClient client, object[] data)
    {
        throw new NotImplementedException();
    }

    public void OnPluginLoad()
    {
        throw new NotImplementedException();
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