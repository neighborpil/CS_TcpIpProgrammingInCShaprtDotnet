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

            Console.WriteLine("***Socket Async Client example***");
            Console.WriteLine("Please type a valid server IP address and press enter:");

            string strIpAddress = Console.ReadLine();

            Console.WriteLine("Please supply a valid port number 0 - 65535 and press enter:");
            string strPortInput = Console.ReadLine();

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
            } while (strInputUser != "<EXIT>");
        }
    }
}
