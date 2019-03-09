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
                            jsonData = @"{
                                'position': {
                                    'x': 178.67709350585938,
                'y': 0.00806492567062378,
                'z': 186.9085693359375
                                },
            'rotation': {
                                    'x': 0.0,
                'y': 90.12889099121094,
                'z': 0.0
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

                var message = Encoding.ASCII.GetBytes(jsonData);

                Console.WriteLine(Encoding.ASCII.GetString(data, 0, data.Length));
                newsock.Send(message, jsonData.Length, sender);
            }
        }
    }
}
