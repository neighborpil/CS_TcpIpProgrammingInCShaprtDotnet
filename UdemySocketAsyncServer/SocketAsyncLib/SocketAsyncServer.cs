using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace SocketAsyncLib
{
    public class SocketAsyncServer
    {
        private IPAddress _ipAddress;

        private int _port;

        private TcpListener _tcpListener;

        public bool KeepRunning { get; set; }

        public async void StartListeningForIncomingConnection(IPAddress ipAddress = null, int port = 50001)
        {
            if (ipAddress == null)
            {
                ipAddress = IPAddress.Any;
            }

            if (port <= 0)
            {
                port = 50001;
            }

            _ipAddress = ipAddress;
            _port = port;

            Debug.WriteLine($"IP Address: {_ipAddress}, Port: {_port}");

            _tcpListener = new TcpListener(_ipAddress, _port);
            try
            {
                _tcpListener.Start();

                while (KeepRunning)
                {
                    var returnedByAccept = await _tcpListener.AcceptTcpClientAsync();

                    Debug.WriteLine($"Client connected successfully: {returnedByAccept}");

                    TakeCareOfTcpClient(returnedByAccept);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }


        }

        private async void TakeCareOfTcpClient(TcpClient client)
        {
            NetworkStream stream = null;
            StreamReader reader = null;

            try
            {
                stream = client.GetStream();
                reader = new StreamReader(stream);




            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
