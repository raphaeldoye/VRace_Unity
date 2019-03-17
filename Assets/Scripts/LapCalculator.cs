using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapCalculator : MonoBehaviour
{
	[SerializeField] private int increment = 1;

	private void OnTriggerEnter(Collider other)
	{
		GameManager.instance.SetNewLap(increment);
	}
}
