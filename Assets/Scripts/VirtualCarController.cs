using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualCarController : CarController
{
	[SerializeField] protected float maxMotorTorque;
	[SerializeField] protected float maxSteeringAngle;
	[SerializeField] protected float minSteeringAngle;
	[SerializeField] protected float maxSpeed;
	[SerializeField] protected List<AxleInfo> axleInfos;

	public bool forward = true;
	public bool directionChanged = false;

	public override float CalculateSpeed(Rigidbody rigidBody)
	{
		speed = rigidBody.velocity.magnitude * speedConstant;
		return speed;
	}

	public override void CarControl(Rigidbody rigidBody)
	{
		float torqueIntensity = Input.GetAxis("Vertical");
		float steeringIntensity = Input.GetAxis("Horizontal");

		//set brake
		SetBrakeTorque(torqueIntensity);

		foreach (var axleInfo in axleInfos)
		{
			if (axleInfo.motor && !isBraking)
				SetMotorTorque(axleInfo, torqueIntensity);
			if (axleInfo.steering)
				SetSteeringAngle(axleInfo, steeringIntensity);
		}
	}

	protected override void SetBrakeTorque(float intensity)
	{
		if ((intensity < 0 && speed > 0.5 && forward) || (intensity > 0 && speed > 0.5 && !forward))
		{
			isBraking = true;

			foreach (var axle in axleInfos)
			{
				if (axle.brake)
				{
					axle.rightWheel.brakeTorque = brake;
					axle.leftWheel.brakeTorque = brake;
					axle.rightWheel.motorTorque = 0;
					axle.leftWheel.motorTorque = 0;
				}
			}
			directionChanged = true;
		}
		else if (directionChanged)
		{
			foreach (var axle in axleInfos)
			{
				if (axle.brake)
				{
					axle.rightWheel.brakeTorque = 0;
					axle.leftWheel.brakeTorque = 0;
				}
			}
			isBraking = false;
			forward = !forward;
			directionChanged = false;
		}
	}

	protected override void SetMotorTorque(AxleInfo axle, float intensity)
	{
		if (intensity == 0 && Mathf.Abs(speed) > 5) // freinage pour la deceleration de la voiture lorsqu'on appuie pas sur le gaz. on arrête le freinage en dessous de 10kmh
		{
			axle.rightWheel.brakeTorque = speed * speedConstant;
			axle.leftWheel.brakeTorque = speed * speedConstant;
			axle.rightWheel.motorTorque = 0;
			axle.leftWheel.motorTorque = 0;
		}
		else if (Mathf.Abs(speed) < maxSpeed)
		{
			axle.rightWheel.brakeTorque = 0;
			axle.leftWheel.brakeTorque = 0;
			axle.rightWheel.motorTorque = maxMotorTorque * intensity;
			axle.leftWheel.motorTorque = maxMotorTorque * intensity;
		}
		else
		{
			axle.rightWheel.brakeTorque = 0;
			axle.leftWheel.brakeTorque = 0;
			axle.rightWheel.motorTorque = 0;
			axle.leftWheel.motorTorque = 0;
		}
	}

	protected override void SetSteeringAngle(AxleInfo axle, float intensity)
	{
		axle.rightWheel.steerAngle = (-((maxSteeringAngle - minSteeringAngle) / maxSpeed) * speed + maxSteeringAngle) * intensity;
		axle.leftWheel.steerAngle = (-((maxSteeringAngle - minSteeringAngle) / maxSpeed) * speed + maxSteeringAngle) * intensity;
	}
}
