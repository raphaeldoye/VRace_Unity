using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRules : MonoBehaviour
{
	public static GameRules instance = null;
	[SerializeField] private int maxLap = 3;
	public string creationGameMenu = "CreationGameMenu";

	public enum MapsList
	{
		container, city, iceland
	}
	public MapsList selectedMap = MapsList.container;

	public enum CarsList
	{
		lambo, mustang, corvette
	}
	public CarsList selectedCar = CarsList.lambo;

	[Header("Developper settings")]
	public bool dynamicMapCreation = true;
	public bool simulateServer = false;
	public bool carSimulation = false;

	void Awake()
	{
		if (instance == null)
			instance = this;

		else if (instance != this)
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);
	}

	public void Start()
	{
		if (PlayerPrefs.HasKey("selectedCar"))
		{
			selectedCar = (CarsList)PlayerPrefs.GetInt("selectedCar");
		}

		if (PlayerPrefs.HasKey("selectedMap"))
		{
			selectedMap = (MapsList)PlayerPrefs.GetInt("selectedMap");
		}

		if (PlayerPrefs.HasKey("lapsNumber"))
		{
			maxLap = PlayerPrefs.GetInt("lapsNumber");
		}
	}

	public int GetMaxLap()
	{
		return maxLap;
	}

	public void SetMaxLap(int lap)
	{
		maxLap = lap;
	}
}
