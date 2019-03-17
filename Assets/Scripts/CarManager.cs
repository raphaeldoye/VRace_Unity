using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarManager : MonoBehaviour
{
	[SerializeField] private RealityCarController realCar;
	[SerializeField] private VirtualCarController virtualCar;
	[SerializeField] private float maxEngineSoundPitch = 2.5f;
	private AudioSource EngineSound;


	public float speed;
	public bool simulation = false;
	private CarController car;
	private Rigidbody carRigidBody;
	private bool locked = true;

	private void Start()
	{
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
		if (!locked)
		{
			speed = car.CalculateSpeed();
			car.CarControl();
		}
	}

	private void UpdateEngineSound()
	{
		EngineSound.pitch = 1 + ((maxEngineSoundPitch - 1) * car.GetCurrentSpeed() / car.GetMaxSpeed());
	}

	public void LockMovement()
	{
		locked = true;
	}

	public void UnlockMovement()
	{
		locked = false;
	}
}
