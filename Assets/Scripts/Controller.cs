using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
	[SerializeField] private WheelsMovements wheels;

    void Update()
    {
		SetDirection(Input.GetAxis("Horizontal"));
		SetMovement(Input.GetAxis("Vertical"));

	}

	private void SetMovement(float movement)
	{
		wheels.SpinWheels(movement);
	}

	private void SetDirection(float direction)
	{
		wheels.TurnWheels(direction);
	}
}
