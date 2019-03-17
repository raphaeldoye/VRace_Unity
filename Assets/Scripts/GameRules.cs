using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRules : MonoBehaviour
{
	public static GameRules instance = null;
	[SerializeField] private int maxLap = 3;

	public enum MapsList
	{
		container, iceland
	}

	public MapsList selectedMap = MapsList.container;

	void Awake()
	{
		if (instance == null)
			instance = this;

		else if (instance != this)
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);
	}

	public int GetMaxLap()
	{
		return maxLap;
	}
}
