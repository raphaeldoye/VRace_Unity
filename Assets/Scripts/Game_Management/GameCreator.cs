using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameCreator : MonoBehaviour
{
	public string nextSceneName = "RaceMap";
	public void CreateGame()
	{
		//IRacerController.instance.Connect();
	}

	public void StartGame()
	{
		UDPClient.instance.ConnectServer();
		SceneManager.LoadScene(nextSceneName);
	}
}
