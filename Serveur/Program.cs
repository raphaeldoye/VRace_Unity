using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Serveur
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] data = new byte[1024];
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 53000);
            UdpClient newsock = new UdpClient(ipep);

            Console.WriteLine("Waiting for a client...");

            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);

            data = newsock.Receive(ref sender);

            Console.WriteLine("Message received from {0}:", sender.ToString());
            Console.WriteLine(Encoding.ASCII.GetString(data, 0, data.Length));

            string welcome = "Welcome to VRace Server!";
            data = Encoding.ASCII.GetBytes(welcome);
            newsock.Send(data, data.Length, sender);

            while (true)
            {
                data = newsock.Receive(ref sender);

                var request = Encoding.ASCII.GetString(data, 0, data.Length);
                string jsonData = "";

                switch (request)
                {
                    case ClientRequest.Record:
                        {
                            jsonData = "Record"; //CALL DLL
                            break;
                        }
                    case ClientRequest.ExternalWalls:
                        {
                            jsonData = "ExternalWalls"; //CALL DLL
                            break;
                        }
                    case ClientRequest.InternalWalls:
                        {
                            jsonData = "InternalWalls"; //CALL DLL
                            break;
                        }
                    case ClientRequest.StartLine:
                        {
                            jsonData = "StartLine"; //CALL DLL
                            break;
                        }
                    case ClientRequest.Position:
                        {
                            jsonData = @"
                  {
                    ""position"": {
                            ""x"": 460.07171630859377,
                            ""y"": 0.007579803466796875,
                            ""z"": 868.8156127929688
                        },
                    ""rotation"": {
                            ""x"": 0.0,
                            ""y"": 168.6186981201172,
                            ""z"": 0.0
                        }
                  }
                "; //CALL DLL
                            break;
                        }
                    default:
                        {
                            jsonData = "Invalid request";
                            break;
                        }
                }

                var message = Encoding.Default.GetBytes(jsonData);

                Console.WriteLine(Encoding.ASCII.GetString(data, 0, data.Length));
                newsock.Send(message, jsonData.Length, sender);
            }
        }
    }
}
