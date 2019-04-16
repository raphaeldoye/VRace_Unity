using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Serveur
{
    class VisionDLL
    {
        [DllImport("MyLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetInstance(string options, int cameraID);

        [DllImport("MyLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern string GetExternalWalls();

        [DllImport("MyLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern string GetInternalWalls();

        [DllImport("MyLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern string GetStartingLine();

        [DllImport("MyLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern string GetCarPositionAndRotation();
    }

    class Program
    {
        private static Thread thread;
        private static bool GameIsFinished = false;
        private static IPEndPoint ipep;
        private static UdpClient newsock;
        private static IPEndPoint sender;

        private static void GameStream()
        {
            while (!GameIsFinished)
            {
                byte[] data = new byte[1024];

                data = newsock.Receive(ref sender);
                var request = Encoding.ASCII.GetString(data, 0, data.Length);
                if (request == ClientRequest.EndGame)
                {
                    GameIsFinished = true;
                }
            }
            
        }

        static void Main(string[] args)
        {
            VisionDLL.SetInstance("", 1);

            byte[] data = new byte[1024];
            ipep = new IPEndPoint(IPAddress.Any, 53000);
			newsock = new UdpClient(ipep);
			sender = new IPEndPoint(IPAddress.Any, 0);

			while (true)
			{
				Console.WriteLine("Waiting for a client...");
				data = newsock.Receive(ref sender);

				Console.WriteLine("Message received from {0}:", sender.ToString());
				Console.WriteLine(Encoding.ASCII.GetString(data, 0, data.Length));

				string welcome = "Welcome to VRace Server!";
				data = Encoding.ASCII.GetBytes(welcome);
				newsock.Send(data, data.Length, sender);
				GameIsFinished = false;


				while (!GameIsFinished)
				{
					data = newsock.Receive(ref sender);

					var request = Encoding.ASCII.GetString(data, 0, data.Length);
					string jsonData = "";

					switch (request)
					{
						case ClientRequest.Record:
							{
								jsonData = "record"; //CALL DLL
								break;
							}
						case ClientRequest.ExternalWalls:
							{
								jsonData = VisionDLL.GetExternalWalls(); //CALL DLL
								break;
							}
						case ClientRequest.InternalWalls:
							{
								jsonData = VisionDLL.GetInternalWalls(); //CALL DLL
								break;
							}
						case ClientRequest.StartLine:
							{
								jsonData = VisionDLL.GetStartingLine(); //CALL DLL
								break;
							}
						case ClientRequest.Position:
							{
								thread = new Thread(new ThreadStart(GameStream));
								thread.Start();

								while (!GameIsFinished)
								{
									// jsonData = VisionDLL.GetCarPositionAndRotation();
									jsonData = @"
								  {
									""position"": {
											""x"": 460.07171630859377,
											""y"": 0.007579803466796875,
											""z"": 868.8156127929688
										},
									""rotation"": {
											""x"": 0.0,
											""y"": 90.6186981201172,
											""z"": 0.0
										}
								  }
								"; //CALL DLL

									var positions = Encoding.Default.GetBytes(jsonData);
									newsock.Send(positions, jsonData.Length, sender);
								}

								break;
							}
						default:
							{
								jsonData = "Invalid request";
								break;
							}
					}
					if (!GameIsFinished)
					{
						var message = Encoding.Default.GetBytes(jsonData);
						newsock.Send(message, jsonData.Length, sender);
					}
					else
					{
						//newsock.Close();
						//newsock.Dispose();
						thread.Join();
						Console.Clear();
					}
				}
			}
        }
    }
}
