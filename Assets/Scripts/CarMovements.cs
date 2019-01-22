using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovements : MonoBehaviour
{
	[SerializeField] RealityCarController realCar;
	[SerializeField] VirtualCarController virtualCar;

	public float speed;
	private bool simulation;
	private CarController car;
	private Rigidbody rigidBody;

	private void Start()
	{
		simulation = true; ///TODO get isSimulation
		rigidBody = GetComponent<Rigidbody>();
		rigidBody.centerOfMass = new Vector3(0.0f, -0.9f, 0.2f);

		if (simulation)
			car = virtualCar;
		else
			car = realCar;
	}

	private void FixedUpdate()
	{
		speed = car.CalculateSpeed(rigidBody);
		car.CarControl(rigidBody);
	}	
}
