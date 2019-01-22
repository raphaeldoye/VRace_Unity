using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelsMovements : MonoBehaviour
{
	[SerializeField] private Transform[] frontWheels;
	[SerializeField] private Transform[] backWheels;
	[SerializeField] private int maxSpeed = 50;
	[SerializeField] private int maxTurn = 25;

	public void SpinWheels(float speedFraction) // -1 a 1
	{
		for (int i = 0; i < frontWheels.Length; i++)
		{
			frontWheels[i].Rotate(Time.deltaTime*(maxSpeed*speedFraction), 0, 0);
		}

		for (int i = 0; i < backWheels.Length; i++)
		{
			backWheels[i].Rotate(Time.deltaTime * (maxSpeed * speedFraction), 0, 0);
		}
	}

	public void TurnWheels(float intensity) // -1 a 1
	{
		for (int i = 0; i < frontWheels.Length; i++)
		{
			frontWheels[i].Rotate(0, Time.deltaTime * (maxTurn * intensity), 0);
		}
	}
}
