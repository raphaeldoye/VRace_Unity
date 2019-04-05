using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarManager : MonoBehaviour
{
	[SerializeField] private RealityCarController realCar;
	[SerializeField] private VirtualCarController virtualCar;
	[SerializeField] private float maxEngineSoundPitch = 2.5f;
	[SerializeField] private List<Light> rearLights;
	private AudioSource EngineSound;


	public float speed;
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

		if (GameRules.instance.carSimulation)
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
		//	speed = car.GetCurrentSpeed();
			//car.CarControl();
			UpdateRearLights();
		}
	}

	private void UpdateEngineSound()
	{
		//EngineSound.pitch = 1 + ((maxEngineSoundPitch - 1) * car.GetCurrentSpeed());
			
		float newPitch = 1 + ((maxEngineSoundPitch - 1) * car.GetCurrentSpeed() / car.GetMaxSpeed());
		if (Mathf.Abs(newPitch) - EngineSound.pitch < maxEngineSoundPitch - 1)
		{
			EngineSound.pitch = newPitch;
		}
		
	}

	private void UpdateRearLights()
	{
		if(Input.GetAxis("Vertical") < 0)
		{
			for (int i = 0; i < rearLights.Count; i++)
			{
				rearLights[i].intensity = 4;
			}
		}
		else
		{
			for (int i = 0; i < rearLights.Count; i++)
			{
				rearLights[i].intensity = 0;
			}
		}
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
