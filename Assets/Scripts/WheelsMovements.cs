using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelsMovements : MonoBehaviour
{
	[SerializeField] private Transform[] frontWheels;
	[SerializeField] private Transform[] backWheels;
	[SerializeField] private int maxSpeed = 50;

	public void SpinWheels(int speedFraction) // -1 a 1
	{
		for (int i = 0; i < frontWheels.Length; i++)
		{
			//frontWheels[i].Rotate()
		}
	}

	public void TurnWheels(int intensity) // -1 a 1
	{

	}
}
