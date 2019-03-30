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
		private static bool gameStreamEnable = false;

        public Server(string serverAddress, int port)
        {
            Console.WriteLine("Initialising server socket...");
            udpClient = new UdpClient();
			udpClient.Client.ReceiveTimeout = 3000;
			udpClient.Client.SendTimeout = 3000;
			server = new IPEndPoint(IPAddress.Parse(serverAddress), port);
          //  thread = new Thread(new ThreadStart(GameStream));
        }

        public bool Connect()
        {
            byte[] data = new byte[1024];
            string initMessage = "I want to play!";
			bool connected = true;
			try
			{			
				//CONNECT TO SERVER
				udpClient.Connect(server);
				Console.WriteLine("Connected to server");

				//SENDING INIT MESSAGE
				data = Encoding.ASCII.GetBytes(initMessage);
				udpClient.Send(data, data.Length);

				//CONFIRM ACCEPTANCE
				var receivedData = udpClient.Receive(ref server);
				string stringData = Encoding.ASCII.GetString(receivedData, 0, receivedData.Length);
				while (stringData != "Welcome to VRace Server!")
				{
					receivedData = udpClient.Receive(ref server);
					stringData = Encoding.ASCII.GetString(receivedData, 0, receivedData.Length);
				}
				var returnData = Encoding.ASCII.GetString(receivedData);

				Console.WriteLine("From server: " + returnData);
			}
			catch (SocketException se)
			{
				connected = false;
			}
			return connected;
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

        public static void StartGameStream()
        {
			gameStreamEnable = true;
			byte[] data = new byte[1024];
            data = Encoding.ASCII.GetBytes("Position");

            //SENDING
            udpClient.Send(data, data.Length);
        }

        public static string GetPosition()
        {
            //RECEIVING
            var receivedData = udpClient.Receive(ref server);

            return Encoding.Default.GetString(receivedData);
        }

        public void StartGame()
        {
			thread = new Thread(new ThreadStart(GameStream));
			thread.Start();
        }

        public void EndGame()
        {
            byte[] data = new byte[1024];
            data = Encoding.ASCII.GetBytes("EndGame");

			//SENDING           
			if (thread != null && thread.IsAlive)
			{
				udpClient.Send(data, data.Length);
				gameStreamEnable = false;
				thread.Join();
			}
		}

        private static void GameStream()
        {
            StartGameStream();
			while (gameStreamEnable)
			{			
				VehicleTransforms.Push(GetPosition());
			}
		}
	}
}
