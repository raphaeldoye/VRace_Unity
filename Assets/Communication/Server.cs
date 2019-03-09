﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client.Communication
{
    public class Server
    {
        private UdpClient udpClient;
        private IPEndPoint server;

        public Server(string serverAddress, int port)
        {
            Console.WriteLine("Initialising server socket...");
            udpClient = new UdpClient();
            server = new IPEndPoint(IPAddress.Parse(serverAddress), port);
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
    }
}
