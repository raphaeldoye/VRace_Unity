﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public static GameManager instance = null;
	[SerializeField] private CarManager myCar;
	[SerializeField] private GhostController ghostCar;
	[SerializeField] private MovementRecorder recorder;
	public int countdownTime = 3;	
	[SerializeField] private Canvas pauseMenu;
	[SerializeField] private Canvas loadingScreen;
	[SerializeField] private Text countdownText;
	

	private float carPauseSpeed;
	private bool paused = false;
	private bool started = false;

	//TEST
	[Header("Developer Settings")]
	public bool mapTestMode;
	public string externalWallsFilePath;
	public string internalWallsFilePath;
	public string startLineFilePath;
	//////

	void Awake()
	{
		if (instance == null)
			instance = this;

		else if (instance != this)
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);
	}

	private void Start()
	{
		StartRace();
	}

	private void Update()
	{
		if (Input.GetButtonDown("Pause"))
		{
			if (!paused)
				PauseGame();
			else
				ResumeGame();
		}
	}

	public void StartRace()
	{
		ShowLoadingScreen();
		BuildWalls();
		UDPClient.instance.server.StartGame();
		// maybe wait a little here
		HideLoadingScreen();
		StartCoroutine(StartCountdown());
	}

	void VerifyEndGame()
	{

		recorder.StoptRecording();
	}

	void BuildWalls()
	{
		MapBuilder map = new MapBuilder();

		if (mapTestMode)
		{
			JsonUtility.FromJsonOverwrite(GetTextFromFile(externalWallsFilePath), map);
			JsonUtility.FromJsonOverwrite(GetTextFromFile(internalWallsFilePath), map);
			JsonUtility.FromJsonOverwrite(GetTextFromFile(startLineFilePath), map);
		}
		else
		{
			JsonUtility.FromJsonOverwrite(""/*UdpClient.instance.server.GetExternalWalls()*/, map);
			JsonUtility.FromJsonOverwrite(""/*UdpClient.instance.server.GetInternalWalls()*/, map);
			JsonUtility.FromJsonOverwrite(""/*UdpClient.instance.server.GetStartLine()*/, map);
		}

		map.BuildMap();
		map.BuildMiniMap();
	}

	IEnumerator StartCountdown()
	{
		yield return new WaitForSeconds(1.0f);
		int timeLeft = countdownTime;
		while (timeLeft > 0)
		{
			countdownText.text = (timeLeft).ToString("0");
			timeLeft -= 1;
			yield return new WaitForSeconds(1.0f);
		}
		countdownText.text = "GO!";
		myCar.UnlockMovement();
		ghostCar.UnlockMovement();
		recorder.StartRecording();
		started = true;
		yield return new WaitForSeconds(1.0f);
		countdownText.text = "";
	}

	void ShowLoadingScreen()
	{
		loadingScreen.GetComponent<CanvasGroup>().alpha = 1;
	}

	void HideLoadingScreen()
	{
		loadingScreen.GetComponent<CanvasGroup>().alpha = 0;
	}

	public void PauseGame()
	{
		if (!paused && started)
		{
			Time.timeScale = 0;
			//carPauseSpeed = IRacer.instance.GetSpeed();
			//IRacerController.instance.SetSpeed(0);
			//IRacer.SendValues();
			ShowPauseMenu(true);
			paused = true;
		}		
	}

	public void ResumeGame()
	{
		if (paused && started)
		{
			//IRacerController.instance.SetSpeed(carPauseSpeed);
			//IRacer.SendValues();
			Time.timeScale = 1;
			ShowPauseMenu(false);
			paused = false;
		}
	}

	void ShowPauseMenu(bool show)
	{
		if (show)
		{
			pauseMenu.GetComponent<CanvasGroup>().alpha = 1;
		}
		else
		{
			pauseMenu.GetComponent<CanvasGroup>().alpha = 0;
		}
		
	}

	public void QuitGame()
	{

	}

	public string GetTextFromFile(string path)
	{
		StreamReader reader = null;
		string text = "";

		try
		{
			reader = new StreamReader(path);
			text = reader.ReadToEnd();
			reader.Close();
		}
		catch (System.Exception)
		{
			// nothing to do for now;
		}

		return text;
	}
}