using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SocketAsyncLib;

namespace UdemySocketAsyncServer
{
    public partial class Form1 : Form
    {
        private SocketAsyncServer _server;
        public Form1()
        {
            InitializeComponent();

            _server = new SocketAsyncServer();
        }

        private void btnAcceptIncomingAsync_Click(object sender, EventArgs e)
        {
            _server.StartListeningForIncomingConnection(IPAddress.Any, 50005);
        }

        private void btnSendAll_Click(object sender, EventArgs e)
        {
            _server.SendToAll(txtMessage.Text.Trim());
        }
    }
}
