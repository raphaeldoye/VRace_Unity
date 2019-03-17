using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Client.Communication;

public class UDPClient : MonoBehaviour
{
	public static UDPClient instance = null;
	public bool serverOn = true;
	public string ipAddress = "127.0.0.1";
	public int port = 53000;
	private Server server;

	private void Awake()
	{
		if (instance == null)
			instance = this;

		else if (instance != this)
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);
	}

	private void Start()
	{
		server = new Server(ipAddress, port);
	}

	public void ConnectServer()
	{
		if (serverOn)
		{
			server.Connect();
		}		
	}

	public void StartGame()
	{
		if (serverOn)
		{
			server.StartGame();
		}
	}
}
