﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Client.Communication;

public class UDPClient : MonoBehaviour
{
	public static UDPClient instance = null;
	public string ipAddress = "127.0.0.1";
	public int port = 53000;
	private Server server;
	private bool simulation;

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
		simulation = GameRules.instance.simulateServer;
	}

	public void ConnectServer()
	{
		if (!simulation)
		{
			server.Connect();
		}		
	}

	public void StartGame()
	{
		if (!simulation)
		{
			server.StartGame();
		}
	}

	public string GetExternalWalls()
	{
		if (!simulation)
			return server.GetExternalWalls();
		else
			return "";
	}

	public string GetInternalWalls()
	{
		if (!simulation)
			return server.GetInternalWalls();
		else
			return "";
	}
	public string GetStartLine()
	{
		if (!simulation)
			return server.GetStartLine();
		else
			return "";
	}
}
