using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealityCarController : CarController
{
	[SerializeField] [Range(0f,1f)] protected float maxSpeed;

	public override float CalculateSpeed(Rigidbody rigidBody)
	{
		speed = 10f;
		return speed; 
		///TODO
		///calculate car speed
	}
	public override void CarControl(Rigidbody rigidBody)
	{
		///TODO
		///set the logic of the car control
	}

	protected override void SetBrakeTorque(float intensity)
	{
		///TODO
		///set the logic of the brake (braking when opposite direction key is press)
	}

	protected override void SetMotorTorque(float intensity)
	{
		///TODO
		/// set the acceleration logic
	}

	protected override void SetSteeringAngle(float intensity)
	{
		///TODO
		/// set the steering logic
	}
}
