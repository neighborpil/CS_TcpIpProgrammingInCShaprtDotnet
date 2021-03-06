﻿using System;
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
            _server.RaiseClientConnectedEvent += (s, e) =>
            {
                txtConsole.AppendText($"{DateTime.Now} - New client connected: {e.NewClient}{Environment.NewLine}");
            };
            _server.RaiseTextReceivedEvent += (s, e) =>
            {
                txtConsole.AppendText($"{DateTime.Now} - Received from {e.ClientWhoSentText}: {e.TextReceived}{Environment.NewLine}");
            };
            _server.RaiseClientDisconnectedEvent += (s, e) =>
            {
                txtConsole.AppendText($"{DateTime.Now} - New client disconnected: {e.DisconnectedPeer}{Environment.NewLine}");
            };
        }

        private void btnAcceptIncomingAsync_Click(object sender, EventArgs e)
        {
            _server.StartListeningForIncomingConnection(IPAddress.Any, 50005);
        }

        private void btnSendAll_Click(object sender, EventArgs e)
        {
            _server.SendToAll(txtMessage.Text.Trim());
        }

        private void btnStopServer_Click(object sender, EventArgs e)
        {
            _server.StopServer();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _server.StopServer();
        }
    }
}
