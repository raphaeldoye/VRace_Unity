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
	public float speed;
	public bool isBraking = false;
	protected float speedConstant = 3.6f;

	//common
	public abstract float CalculateSpeed(Rigidbody rigidBody);
	public abstract void CarControl(Rigidbody rigidBody);
	protected abstract void SetBrakeTorque(float intensity);

	//virtual
	protected virtual void SetMotorTorque(AxleInfo axle, float intensity) { }
	protected virtual void SetSteeringAngle(AxleInfo axle, float intensity) { }

	//reality
	
	protected virtual void SetMotorTorque(float intensity) { }
	protected virtual void SetSteeringAngle(float intensity) { }
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