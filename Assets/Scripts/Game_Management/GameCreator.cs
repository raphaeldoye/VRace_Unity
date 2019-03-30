using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameCreator : MonoBehaviour
{
	public string nextSceneName = "RaceMap";
	public string previousSceneName = "MainMenu";
	public float minimumLoadingDuration = 3f;
	[SerializeField] private MenuManager menuManager;

	private void Start()
	{
		CreateGame();
	}

	public IEnumerator ShowCarLoading()
	{
		menuManager.OpenLoading("Connecting to RC car...");
		float initialTime = Time.time;
		yield return null;

		if (IRacerController.instance.Connect())
		{
			float waiting = minimumLoadingDuration - (Time.time - initialTime);
			waiting = waiting < 0 ? 0 : waiting;
			yield return new WaitForSeconds(waiting);
			menuManager.OpenGameMenu();
		}
		else
		{
			float waiting = minimumLoadingDuration - (Time.time - initialTime);
			waiting = waiting < 0 ? 0 : waiting;
			yield return new WaitForSeconds(waiting);
			menuManager.OpenRetryLoading();
		}

	}

	public IEnumerator ShowServerLoading()
	{
		menuManager.OpenLoading("Connecting to Server...");
		float initialTime = Time.time;
		yield return null;

		if (UDPClient.instance.ConnectServer())
		{
			float waiting = minimumLoadingDuration - (Time.time - initialTime);
			waiting = waiting < 0 ? 0 : waiting;
			yield return new WaitForSeconds(waiting);
			SceneManager.LoadScene(nextSceneName);
		}
		else
		{
			float waiting = minimumLoadingDuration - (Time.time - initialTime);
			waiting = waiting < 0 ? 0 : waiting;
			yield return new WaitForSeconds(waiting);
			menuManager.OpenServerRetryLoading();
		}
	}

	public void CreateGame()
	{
		if (IRacerController.instance.connected)
		{
			ReturnToGameMenu();
		}
		else
		{
			menuManager.OpenPortSelection();
		}
	}

	public void ConnectCar()
	{
		StartCoroutine(ShowCarLoading());
	}

	public void QuitGame()
	{
		SceneManager.LoadScene(previousSceneName);
	}

	public void StartGame()
	{
		StartCoroutine(ShowServerLoading());
	}

	public void ReturnToGameMenu()
	{
		menuManager.OpenGameMenu();
	}
}
