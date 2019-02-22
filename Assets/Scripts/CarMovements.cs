using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovements : MonoBehaviour
{
	[SerializeField] private RealityCarController realCar;
	[SerializeField] private VirtualCarController virtualCar;
	[SerializeField] private float maxEngineSoundPitch = 2.5f;
	private AudioSource EngineSound;


	public float speed;
	private bool simulation;
	private CarController car;
	private Rigidbody carRigidBody;

	private void Start()
	{
		simulation = true; ///TODO get isSimulation
		//get rigidBody
		carRigidBody = GetComponent<Rigidbody>();
		carRigidBody.centerOfMass = new Vector3(0.0f, -0.9f, 0.2f);
		//get engine audio source
		EngineSound = GetComponent<AudioSource>();

		if (simulation)
			car = virtualCar;
		else
			car = realCar;

		car.SetRigidBody(carRigidBody);
	}

	private void FixedUpdate()
	{
		UpdateCarControl();
	}

	private void Update()
	{
		UpdateEngineSound();
	}

	private void UpdateCarControl()
	{
		speed = car.CalculateSpeed();
		car.CarControl();
	}

	private void UpdateEngineSound()
	{
		EngineSound.pitch = 1+((maxEngineSoundPitch-1) * car.GetCurrentSpeed() / car.GetMaxSpeed());
	}
}
