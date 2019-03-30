using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager instance = null;
	[SerializeField] private CarManager myCar;
	[SerializeField] private GhostController ghostCar;
	[SerializeField] private MovementRecorder recorder;
	[SerializeField] private Animation endRaceAnimation;
	public int countdownTime = 3;
	public int currentLap = 0;
	[Header("UI settings")]
	[SerializeField] private CanvasGroup pauseMenu;
	[SerializeField] private Canvas loadingScreen;
	[SerializeField] private Text countdownText;
	[SerializeField] private Text currentLapText;
	[SerializeField] private Text maxLapText;
	[SerializeField] private Text recordTimeText;
	[SerializeField] private Text EndGameText;
	[SerializeField] private Text EndGameRecordText;
	[SerializeField] private CanvasGroup blackScreen;

	public const float DEFAULT_RECORD_TIME = 999999f;


	private float carPauseSpeed;
	private bool paused = false;
	private bool started = false;
	private int lapIncrementValidator = 0;
	private float recordTime = DEFAULT_RECORD_TIME;
	private bool EndGameOnce = false;

	//TEST
	[Header("Developer Settings")]
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
		maxLapText.text = "/" + GameRules.instance.GetMaxLap().ToString();
		ShowPauseMenu(false);
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

	public void SetRecordTime (float time)
	{
		recordTime = time;
		if (time == DEFAULT_RECORD_TIME)
			recordTimeText.text = "Record: --.--";
		else
			recordTimeText.text = "Record: " + time.ToString("00.00");

	}

	public void StartRace()
	{
		ShowLoadingScreen();
		BuildWalls();
		UDPClient.instance.StartGame();
		// maybe wait a little here
		HideLoadingScreen();
		StartCoroutine(StartCountdown());
	}

	void VerifyEndGame()
	{
		if (!EndGameOnce)
		{
			bool win = false;
			if (currentLap > GameRules.instance.GetMaxLap())
			{
				EndGameOnce = true;
				timer.instance.StopTimer();
				if (timer.instance.GetTime() < recordTime)
				{
					recorder.StopRecording(timer.instance.GetTime());
					win = true;
				}
				else
				{
					recorder.StopRecording();
				}

				currentLapText.text = GameRules.instance.GetMaxLap().ToString();
				StartCoroutine(EndGame(win));
			}
			else
			{
				currentLapText.text = currentLap.ToString();
			}
		}
	}

	IEnumerator EndGame(bool win)
	{
		//Play camera animation
		endRaceAnimation.Play();
		yield return new WaitForSeconds(1.5f);
		// display end game text
		if (win)
		{
			EndGameText.text = "New Record";
			EndGameRecordText.text = timer.instance.GetTime().ToString("00.00");
		}
		else
		{
			EndGameText.text = "Game Over";
		}

		while (endRaceAnimation.isPlaying)
		{
			yield return null;
		}
		yield return new WaitForSeconds(1f);

		//BlackScreen Fade
		float fadeDuration = 2f;
		while (blackScreen.alpha < 1)
		{
			blackScreen.alpha += Time.deltaTime / fadeDuration;
			yield return null;
		}
		//Lock car movement and stop vehicle
		myCar.LockMovement();
		IRacerController.instance.Speed = 0;
		IRacerController.instance.SetMovement();
		QuitGame();
	}

	void BuildWalls()
	{
		MapBuilder map = new MapBuilder();

		if (GameRules.instance.dynamicMapCreation)
		{			
			JsonUtility.FromJsonOverwrite(UDPClient.instance.GetExternalWalls(), map);
			JsonUtility.FromJsonOverwrite(UDPClient.instance.GetInternalWalls(), map);
			JsonUtility.FromJsonOverwrite(UDPClient.instance.GetStartLine(), map);

			string jsonMap = JsonUtility.ToJson(map, true);
			File.WriteAllText("Assets/Resources/ServerDataMap.txt", jsonMap);
		}
		else
		{
			JsonUtility.FromJsonOverwrite(GetTextFromFile(externalWallsFilePath), map);
			JsonUtility.FromJsonOverwrite(GetTextFromFile(internalWallsFilePath), map);
			JsonUtility.FromJsonOverwrite(GetTextFromFile(startLineFilePath), map);
		}

		map.BuildMap();
		//map.BuildMiniMap();
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
		timer.instance.StartTimer();
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
			myCar.LockMovement();
			ghostCar.LockMovement();
			timer.instance.StopTimer();
			carPauseSpeed = IRacerController.instance.Speed;
			IRacerController.instance.Speed = 0;
			IRacerController.instance.SetMovement();
			ShowPauseMenu(true);
			paused = true;
		}		
	}

	public void ResumeGame()
	{
		if (paused && started)
		{
			myCar.UnlockMovement();
			ghostCar.UnlockMovement();
			IRacerController.instance.Speed = carPauseSpeed;
			IRacerController.instance.SetMovement();
			timer.instance.StartTimer();
			ShowPauseMenu(false);
			paused = false;
		}
	}

	public void QuitGame ()
	{
		UDPClient.instance.StopGame();
		IRacerController.instance.Disconnect();
		SceneManager.LoadScene(GameRules.instance.creationGameMenu);
	}

	void ShowPauseMenu(bool show)
	{
		pauseMenu.alpha = show ? 1 : 0;
		pauseMenu.interactable = show;
		pauseMenu.blocksRaycasts = show;
	}

	public void SetNewLap(int increment)
	{
		if (lapIncrementValidator == 0 && increment == 1)
		{
			lapIncrementValidator += increment;
			currentLap += 1;
		}
		else if (lapIncrementValidator == 1 && increment == -1)
		{
			lapIncrementValidator += increment;
		}
		else if (lapIncrementValidator == 1 && increment == 1)
		{
			lapIncrementValidator -= increment;
			currentLap -= 1;
		}
		else if (lapIncrementValidator == 0 && increment == -1)
		{
			lapIncrementValidator -= increment;
		}

		VerifyEndGame();
	}

	public string GetTextFromFile(string path)
	{
		StreamReader reader = null;
		string text = "";
		try
		{
			if (File.Exists(path))
			{ 
				reader = new StreamReader(path);
				text = reader.ReadToEnd();
				reader.Close();
			}
		}
		catch (NullReferenceException e)
		{
			Debug.Log(e);
		}

		return text;
	}
}