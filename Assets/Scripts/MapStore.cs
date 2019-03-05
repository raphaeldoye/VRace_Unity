using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class MapStore : MonoBehaviour
{
	public static MapStore instance = null;

	[SerializeField] private Map containerMap;
	[SerializeField] private Map iceLandMap;
	private Map selectedMap;

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
		switch (GameRules.instance.selectedMap)
		{
			case GameRules.MapsList.container:
				selectedMap = containerMap;
				break;
			case GameRules.MapsList.iceland:
				selectedMap = iceLandMap;
				break;
			default:
				selectedMap = containerMap;
				break;
		}
	}

	public Map GetSelectedMap()
	{
		return selectedMap;
	}
}
[Serializable]
public class Map
{
	public Material groundMaterial;
	[Header("external walls settings")]
	public List<GameObject> externalWalls;
	public int additionalWalls = 0;
	[Header("internal walls settings")]
	public List<GameObject> internalWalls;
	[Header("start Line settings")]
	public List<GameObject> startLine_Floor;

}
