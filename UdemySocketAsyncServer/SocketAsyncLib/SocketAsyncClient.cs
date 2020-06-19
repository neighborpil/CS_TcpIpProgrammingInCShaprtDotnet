using System;
using System.IO;
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


        public EventHandler<ConnectedEventArgs> RaiseServerConnectedEvent;

        public EventHandler<TextReceivedEventArgs> RaiseTextReceivedEvent;

        public EventHandler<DisconnectedEventArgs> RaiseServerDisconnectedEvent;

        public SocketAsyncClient()
        {
            _serverIpAddress = null;
            _client = null;
            _serverPort = -1;
        }

        protected virtual void OnRaiseServerConnectedEvent(ConnectedEventArgs e)
        {
            RaiseServerConnectedEvent?.Invoke(this, e);
        }

        protected virtual void OnRaiseTextReceivedEvent(TextReceivedEventArgs e)
        {
            RaiseTextReceivedEvent?.Invoke(this, e);
        }

        protected virtual void OnRaiseServerDisconnectedEvent(DisconnectedEventArgs e)
        {
            RaiseServerDisconnectedEvent?.Invoke(this, e);
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

                var args = new ConnectedEventArgs(_client.Client.RemoteEndPoint.ToString());
                OnRaiseServerConnectedEvent(args);

                ReadDataAsync(_client);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private async Task ReadDataAsync(TcpClient client)
        {
            try
            {
                var streamReader = new StreamReader(_client.GetStream());
                var buffer = new char[64];
                int readByteCount = 0;

                while (true)
                {
                    readByteCount = await streamReader.ReadAsync(buffer, 0, buffer.Length);

                    if (readByteCount <= 0) // 0바이트를 받았다는 것은 연결이 끊겼다는 것을 의미
                    {
                        OnRaiseServerDisconnectedEvent(new DisconnectedEventArgs(_client.Client.RemoteEndPoint.ToString()));

                        Console.WriteLine("Disconnected from server");
                        _client.Close();
                        break;
                    }

                    var receivedMessage = new string(buffer, 0, readByteCount);
                    Console.WriteLine($"Received bytes: {readByteCount} - Message: {receivedMessage}");
                    OnRaiseTextReceivedEvent(new TextReceivedEventArgs(client.Client.RemoteEndPoint.ToString(), receivedMessage));

                    Array.Clear(buffer, 0, buffer.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        public async Task SendToServer(string strInputUser)
        {
            if (string.IsNullOrWhiteSpace(strInputUser))
            {
                Console.WriteLine("Empty string supplied to send.");
                return;
            }

            if (_client != null)
            {
                if (_client.Connected)
                {
                    var streamWriter = new StreamWriter(_client.GetStream());
                    streamWriter.AutoFlush = true;

                    await streamWriter.WriteAsync(strInputUser);
                    Console.WriteLine("Data sent");
                }
            }

        }

        public void CloseAndDisconnect()
        {


            if (_client != null)
            {
                if (_client.Connected)
                {
                    _client.Close();
                }
            }

        }

        public IPAddress ResolveHostNameToIpAddress(string strHostName)
        {
            try
            {
                var addresses = Dns.GetHostAddresses(strHostName.Trim());
                foreach (var ipAddress in addresses)
                {
                    if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return ipAddress;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return null;
        }
    }
}