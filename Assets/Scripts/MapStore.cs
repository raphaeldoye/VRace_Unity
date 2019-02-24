using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class MapStore : MonoBehaviour
{
	public static MapStore instance = null;

	public DefaultMap defaultMap;

	void Awake()
	{
		if (instance == null)
			instance = this;

		else if (instance != this)
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);
	}
}
[Serializable]
public struct DefaultMap
{
	public Material groundMaterial;
	public GameObject externalWall;
}
