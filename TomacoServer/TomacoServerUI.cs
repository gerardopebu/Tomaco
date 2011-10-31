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
using TomacoServer.Infrastructure.UI;

namespace TomacoServer
{
    public partial class TomacoServerUI : Form
    {
        Server.TomacoServer server;

        public TomacoServerUI()
        {
            InitializeComponent();
            RedirigirSalidaConsola();
        }

        private void RedirigirSalidaConsola()
        {
            Console.SetOut(new TextBoxStreamWriter(output));
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            if(server==null)
                server = new Server.TomacoServer(new Uri(address.Text));
            server.OpenServer();
        }
    }
}
