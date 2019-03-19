//***
// Reference: https://docs.unity3d.com/Manual/WheelColliderTutorial.html
//***

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class CarController : MonoBehaviour
{
	//variables	
	[SerializeField]protected float deceleration;
	[SerializeField]protected float brake;
	[SerializeField] protected float maxSpeed;
	public float speed;
	public bool isBraking = false;
	protected float speedConstant = 3.6f;
	protected Rigidbody carRigidBody;

	public void SetRigidBody(Rigidbody rigidBody)
	{
		carRigidBody = rigidBody;
	}

	public float GetMaxSpeed()
	{
		return maxSpeed;
	}

	public float CalculateSpeed()
	{
		speed = carRigidBody.velocity.magnitude * speedConstant;
		return speed;
	}

	//abstract
	public abstract void CarControl();
	public abstract float GetCurrentSpeed();
	protected abstract void SetBrakeTorque(float intensity);
	protected abstract void SetMotorTorque(float intensity, AxleInfo axle = null);
	protected abstract void SetSteeringAngle(float intensity, AxleInfo axle = null);
}

[System.Serializable]
public class AxleInfo
{
	public WheelCollider leftWheel;
	public WheelCollider rightWheel;
	public bool motor;
	public bool steering;
	public bool brake;
}