using SharedCode.CustomObjects;
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
        Dictionary<int, TrimProcess> processHandler = new Dictionary<int, TrimProcess>();
        public TaskManagerForm()
        {
            InitializeComponent();
        }

        private void TaskManagerForm_Load(object sender, EventArgs e)
        {

        }

        public void UpdateProcesses(TrimProcess[] procs)
        {
            foreach(TrimProcess p in procs)
            {
                if(!processHandler.ContainsKey(p.ID))
                {
                    ListViewItem i = new ListViewItem(p.Name);
                    i.SubItems.Add(p.ID.ToString());
                    p.Tag = i;
                    processHandler.Add(p.ID, p);
                }
                foreach(var e in processHandler)
                {
                    
                }
            }
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
    }
}
