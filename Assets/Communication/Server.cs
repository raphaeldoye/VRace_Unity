using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Client.Communication
{
    public class Server
    {
        private static UdpClient udpClient;
        private static IPEndPoint server;
        private Thread thread;

        public Server(string serverAddress, int port)
        {
            Console.WriteLine("Initialising server socket...");
            udpClient = new UdpClient();
            server = new IPEndPoint(IPAddress.Parse(serverAddress), port);
            thread = new Thread(new ThreadStart(GameStream));
        }

        public void Connect()
        {
            byte[] data = new byte[1024];
            string initMessage = "I want to play!";

            //CONNECT TO SERVER
            udpClient.Connect(server);
            Console.WriteLine("Connected to server");

            //SENDING INIT MESSAGE
            data = Encoding.ASCII.GetBytes(initMessage);
            udpClient.Send(data, data.Length);

            //CONFIRM ACCEPTANCE
            var receivedData = udpClient.Receive(ref server);
            var returnData = Encoding.ASCII.GetString(receivedData);

            Console.WriteLine("From server: " + returnData);
        }

        public void Send(string message)
        {
            byte[] data = new byte[1024];
            data = Encoding.ASCII.GetBytes(message);

            //SENDING
            udpClient.Send(data, data.Length);

            //RECEIVING
            var receivedData = udpClient.Receive(ref server);
            var receivedText = Encoding.ASCII.GetString(receivedData);

            Console.WriteLine("(Serveur: " + Encoding.ASCII.GetString(receivedData) + ")");
        }

        public string GetMapRecord()
        {
            byte[] data = new byte[1024];
            data = Encoding.ASCII.GetBytes("Record");

            //SENDING
            udpClient.Send(data, data.Length);

            //RECEIVING
            var receivedData = udpClient.Receive(ref server);
            var record = Encoding.ASCII.GetString(receivedData);

            Console.WriteLine("(Serveur: " + record + ")");

            return record;
        }

        public string GetExternalWalls()
        {
            byte[] data = new byte[1024];
            data = Encoding.ASCII.GetBytes("ExternalWalls");

            //SENDING
            udpClient.Send(data, data.Length);

            //RECEIVING
            var receivedData = udpClient.Receive(ref server);

            var walls = Encoding.Default.GetString(receivedData);

            Console.WriteLine(walls);

            return walls;
        }

        public string GetInternalWalls()
        {
            byte[] data = new byte[1024];
            data = Encoding.ASCII.GetBytes("InternalWalls");

            //SENDING
            udpClient.Send(data, data.Length);

            //RECEIVING
            var receivedData = udpClient.Receive(ref server);

            var walls = Encoding.Default.GetString(receivedData);

            Console.WriteLine(walls);

            return walls;
        }

        public string GetStartLine()
        {
            byte[] data = new byte[1024];
            data = Encoding.ASCII.GetBytes("StartLine");

            //SENDING
            udpClient.Send(data, data.Length);

            //RECEIVING
            var receivedData = udpClient.Receive(ref server);

            var startLine = Encoding.Default.GetString(receivedData);

            Console.WriteLine(startLine);

            return startLine;
        }

        public static string GetPosition()
        {
            byte[] data = new byte[1024];
            data = Encoding.ASCII.GetBytes("Position");

            //SENDING
            udpClient.Send(data, data.Length);

            //RECEIVING
            var receivedData = udpClient.Receive(ref server);

            var position = Encoding.Default.GetString(receivedData);

            Console.WriteLine(position);

            return position;
        }

        public void StartGame()
        {
            thread.Start();
        }

        public void EndGame()
        {
            thread.Join();
        }

        private static void GameStream()
        {
            //VehicleTransforms.Push(GetPosition());
            VehicleTransforms.Push(@"{
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
        }");
        }
    }
}
