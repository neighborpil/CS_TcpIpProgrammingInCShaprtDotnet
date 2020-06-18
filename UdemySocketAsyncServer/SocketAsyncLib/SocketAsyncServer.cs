using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketAsyncLib
{
    public class SocketAsyncServer
    {
        private IPAddress _ipAddress;

        private int _port;

        private TcpListener _tcpListener; // tcp 연결 쉽게 해주는 helper class

        private readonly List<TcpClient> _clients;

        public bool _keepRunning { get; set; }

        public SocketAsyncServer()
        {
            _clients = new List<TcpClient>();
        }

        public async void StartListeningForIncomingConnection(IPAddress ipAddress = null, int port = 50001)
        {
            SetConnectionVariables(ipAddress, port);

            _tcpListener = new TcpListener(_ipAddress, _port);
            try
            {
                _tcpListener.Start();

                _keepRunning = true;

                while (_keepRunning)
                {
                    var connectedClient = await _tcpListener.AcceptTcpClientAsync();

                    _clients.Add(connectedClient);

                    Console.WriteLine($"Client({_clients.Count}) connected successfully: {connectedClient}");

                    TakeCareOfTcpClient(connectedClient);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
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

                var buffer = new char[64];

                while (_keepRunning)
                {
                    Console.WriteLine("***Ready to read***");

                    var length = await reader.ReadAsync(buffer, 0, buffer.Length);

                    Console.WriteLine($"Return: {length}");

                    if (length == 0)
                    {
                        RemoveClient(client);
                        Console.WriteLine("Socket is disconnected");
                        break;
                    }

                    string receivedMessage = new string(buffer, 0, length);
                    Console.WriteLine($"***Received: {receivedMessage}");

                    Array.Clear(buffer, 0, buffer.Length);
                }
            }
            catch (Exception ex)
            {
                RemoveClient(client);

                Console.WriteLine(ex.ToString());
            }
            //finally
            //{
            //    if (reader != null)
            //    {
            //        reader.Close();
            //        reader.Dispose();
            //    }

            //    if (stream != null)
            //    {
            //        stream.Close();
            //        stream.Dispose();
            //    }
            //}
        }

        private void RemoveClient(TcpClient client)
        {
            if (_clients.Contains(client))
            {
                _clients.Remove(client);
                Console.WriteLine($"Client removed, Count: {_clients.Count()}");
            }

        }

        private void SetConnectionVariables(IPAddress ipAddress, int port)
        {
            if (ipAddress == null)
                _ipAddress = IPAddress.Any;
            else
                _ipAddress = ipAddress;
            if (port <= 0 || port > 65535)
                _port = 50001;
            else
                _port = port;

            Console.WriteLine($"IP Address: {_ipAddress} - Port: {_port}");
        }

        public async void SendToAll(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            try
            {
                var bufferMessage = Encoding.ASCII.GetBytes(message);
                _clients.ForEach(c => c.GetStream().WriteAsync(bufferMessage, 0, bufferMessage.Length));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void StopServer()
        {
            try
            {
                _tcpListener?.Stop();
                _clients.ForEach(c => c.Close());
                _clients.Clear();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }
    }
}
 