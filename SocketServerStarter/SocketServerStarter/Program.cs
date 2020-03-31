using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketServerStarter
{
    class Program
    {
        static void Main(string[] args)
        {
            var listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            var ipAddress = IPAddress.Any;

            var ipEndPoint = new IPEndPoint(ipAddress, 23000);

            listenerSocket.Bind(ipEndPoint);

            listenerSocket.Listen(5);

            Console.WriteLine("About to accept client connection.");

            Socket client = listenerSocket.Accept(); // blocking operation, synchronous operation

            Console.WriteLine($"Client connected. {client.ToString()}: Remote end point - {client.RemoteEndPoint.ToString()}" );

            var buff = new byte[128];

            int numberOfReceivedBytes = 0;

            numberOfReceivedBytes = client.Receive(buff);

            Console.WriteLine($"Number of received bytes: {numberOfReceivedBytes}");

            Console.WriteLine($"Data sent by client: {buff}");

        }
    }
}
