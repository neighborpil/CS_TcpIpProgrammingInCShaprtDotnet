using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using SocketAsyncLib;

namespace UdemySocketAsyncClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new SocketAsyncClient();
            client.RaiseServerConnectedEvent += (s, e) =>
            {
                Console.WriteLine($"{DateTime.Now} - Connected: {e.NewClient}{Environment.NewLine}");
            };
            client.RaiseTextReceivedEvent += (s, e) =>
            {
                Console.WriteLine($"{DateTime.Now} - Received: {e.TextReceived}{Environment.NewLine}");
            };
            client.RaiseServerDisconnectedEvent += (s, e) =>
            {
                Console.WriteLine($"{DateTime.Now} - Disconnected: {e.DisconnectedPeer}{Environment.NewLine}");
            };

            Console.WriteLine("***Socket Async Client example***");
            Console.WriteLine("Please type a valid server IP address and press enter:");

            string strIpAddress = Console.ReadLine();

            Console.WriteLine("Please supply a valid port number 0 - 65535 and press enter:");
            string strPortInput = Console.ReadLine();

            if (strIpAddress.StartsWith("<HOST>"))
            {
                strIpAddress = strIpAddress.Replace("<HOST>", string.Empty);
                strIpAddress = client.ResolveHostNameToIpAddress(strIpAddress)?.ToString();
            }

            if(string.IsNullOrWhiteSpace(strIpAddress))
            {
                Console.WriteLine("No IP Address supplied");
                Environment.Exit(0);
            }

            if (!client.SetServerIpAddress(strIpAddress) || !client.SetPortNumber(strPortInput))
            {
                Console.WriteLine($"Wrong IP adress or port number supplied - {strIpAddress} / {strPortInput}");
                Console.ReadKey();
                return;
            }

            client.ConnectToServer();

            string strInputUser = null;

            do
            {
                strInputUser = Console.ReadLine();

                if (strInputUser.Trim() != "<EXIT>")
                {
                    client.SendToServer(strInputUser);
                }
                else if (strInputUser.Trim() == "<EXIT>")
                {
                    client.CloseAndDisconnect();
                    Console.WriteLine("Tcp client is closed.");
                }

            } while (strInputUser != "<EXIT>");

            Console.ReadKey();
        }
    }
}
