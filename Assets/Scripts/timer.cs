using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class timer : MonoBehaviour
{
	public static timer instance = null;
	[SerializeField] private float time = 0f;
	private bool timerEnable = false;

	private void Awake()
	{
		if (instance == null)
			instance = this;

		else if (instance != this)
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);
	}

	void Update()
    {
		if (timerEnable)
		{
			time += Time.deltaTime;
			GetComponent<Text>().text = time.ToString("00.00");
		}
    }

	public void StartTimer()
	{
		timerEnable = true;
	}

	public void StopTimer()
	{
		timerEnable = false;
	}

	public float GetTime()
	{
		return time;
	}
}
