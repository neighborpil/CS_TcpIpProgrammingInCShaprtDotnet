using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SocketAsyncLib
{
    public class SocketAsyncClient
    {
        private IPAddress _serverIpAddress;

        private int _serverPort;

        private TcpClient _client;

        public SocketAsyncClient()
        {
            _serverIpAddress = null;
            _client = null;
            _serverPort = -1;
        }

        public IPAddress ServerIpAddress
        {
            get => _serverIpAddress;
        }

        public int ServerPort
        {
            get => _serverPort;
        }

        public bool SetServerIpAddress(string ipAddressServer)
        {
            IPAddress ipAddress = null;

            if (!IPAddress.TryParse(ipAddressServer, out ipAddress))
            {
                Console.WriteLine("Invalid server IP supplied.");
                return false;
            }

            _serverIpAddress = ipAddress;

            return true;
        }

        public bool SetPortNumber(string serverPort)
        {
            int portNumber = 0;
            if (!int.TryParse(serverPort, out portNumber))
            {
                Console.WriteLine("Invalid server port supplied.");
                return false;
            }

            if (portNumber <= 0 || portNumber > 65535)
            {
                Console.WriteLine("Port number must be between 0 and 65535.");
                return false;
            }

            _serverPort = portNumber;

            return true;
        }

        public async Task ConnectToServer()
        {
            if (_client == null)
            {
                _client = new TcpClient();
            }

            try
            {
                await _client.ConnectAsync(_serverIpAddress, _serverPort);
                Console.WriteLine($"Connected to server IP/Port: {_serverIpAddress} / {_serverPort}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

    }
}