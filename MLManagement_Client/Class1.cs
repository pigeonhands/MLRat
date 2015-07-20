using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MLRat.Client;

public class MLManagement_Client : IClientPlugin
{
    public void OnConnect(IClientConnection server)
    {
        Console.WriteLine("[MLManagement] Connected.");
    }

    public void OnDataRecieved(object[] data)
    {
        
    }

    public void OnDisconnect()
    {
        Console.WriteLine("[MLManagement] Disconnected");
    }

    public void OnPluginLoad()
    {
        Console.WriteLine("[MLManagement] Loaded!");
    }
}
