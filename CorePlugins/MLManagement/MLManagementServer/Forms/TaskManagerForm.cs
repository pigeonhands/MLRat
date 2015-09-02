using MLRat.Server;
using SharedCode.CustomObjects;
using SharedCode.Network;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MLManagementServer.Forms
{
    public partial class TaskManagerForm : Form
    {
        public IClient Client { get; set; }
        public TaskManagerForm(IClient c, IServerUIHandler ui)
        {
            InitializeComponent();
            Client = c;
            Image standardProc = ui.GetImage("cpu.png");
            Image thisProc = ui.GetImage("bug.png");
            Image appImage = ui.GetImage("application.png");
            ImageList images = new ImageList();

            if(standardProc != null)
            {
                images.Images.Add("background", standardProc);
                if(thisProc != null)
                    images.Images.Add("me", thisProc);
                else
                    images.Images.Add("me", standardProc);

                if(appImage != null)
                    images.Images.Add("app", appImage);
                else
                    images.Images.Add("app", standardProc);

                lvProcessList.SmallImageList = images;
            }
        }

        private void TaskManagerForm_Load(object sender, EventArgs e)
        {
            this.Text = string.Format("Task manager ({0})", Client.GetVariable<string>("Username", ""));
        }

        public void StartUpdate()
        {
            Invoke((MethodInvoker)delegate ()
            {
                lvProcessList.Items.Clear();
            });
        }

        public void AddProcess(string name, int id, string window, bool hasWindow, bool thisProc)
        {
            Invoke((MethodInvoker)delegate ()
            {
                ListViewItem i = new ListViewItem(window);
                i.SubItems.Add(id.ToString());
                i.SubItems.Add(name);
                i.Tag = id;
                i.Group = lvProcessList.Groups[hasWindow ? "app" : "background"];
                if (hasWindow)
                    i.ImageKey = thisProc ? "me" : "app";
                else
                    i.ImageKey = thisProc ? "me" : "background";
                lvProcessList.Items.Add(i);
            });
        }

        public void UpdateProcesses(TrimProcess[] procs)
        {
            
        }

        void AddProcToList(ListViewItem i)
        {
            Invoke((MethodInvoker)delegate ()
            {
                lvProcessList.Items.Add(i);
            });
        }

        void RemoveProcFromList(ListViewItem i)
        {
            Invoke((MethodInvoker)delegate ()
            {
                lvProcessList.Items.Remove(i);
            });
        }

        public void Destroy()
        {
            Invoke((MethodInvoker)delegate ()
            {
                Dispose();
            });
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Client.Send((byte)NetworkCommand.TaskManager, (byte)TaskManagerCommand.GetProcesses);
        }

        private void killToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvProcessList.SelectedItems.Count < 1)
                return;
            ListViewItem i = lvProcessList.SelectedItems[0];
            int id = (int)i.Tag;
            Client.Send((byte)NetworkCommand.KillProcess, id);
        }

        private void suspendToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvProcessList.SelectedItems.Count < 1)
                return;
            ListViewItem i = lvProcessList.SelectedItems[0];
            int id = (int)i.Tag;
            Client.Send((byte)NetworkCommand.SuspendProcess, id);
        }

        private void resumeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvProcessList.SelectedItems.Count < 1)
                return;
            ListViewItem i = lvProcessList.SelectedItems[0];
            int id = (int)i.Tag;
            Client.Send((byte)NetworkCommand.ResumeProcess, id);
        }
    }
}
