using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class RealityCarController : CarController
{
    [SerializeField] [Range(0f,1f)] protected float speedMultiplier;
	Vector3 previousFrame;

    private void Start()
	{
       
        ///TODO
        /// Set maxSpeed to the maximum value sent to the car (only for the speed)
       // maxSpeed = 16;
		previousFrame = new Vector3();
	}

	public override void CarControl()
	{
        if (Input.GetButtonDown("CloseDLL"))
        {
      //      MyExternalLib.Disconnect();
        }
		SetMotorTorque(Input.GetAxis("Vertical"));
		SetSteeringAngle(Input.GetAxis("Horizontal"));
		IRacerController.instance.SetMovement();
    }

    public override float GetCurrentSpeed()
	{
		float currentSpeed = 0;

		if (previousFrame != Vector3.zero)
		{
			currentSpeed = (carRigidBody.position - previousFrame).magnitude / Time.fixedDeltaTime * speedConstant;
		}
		previousFrame = carRigidBody.position;

		return currentSpeed;
	}

	protected override void SetBrakeTorque(float intensity)
	{
		///TODO
		///set the logic of the brake (braking when opposite direction key is press)
	}

	protected override void SetMotorTorque(float intensity, AxleInfo axle = null)             
	{
		IRacerController.instance.Speed = intensity;
		//speed = Mathf.Abs(intensity);
	}

	protected override void SetSteeringAngle(float intensity, AxleInfo axle = null)
	{
		if (intensity < -0.5f)
		{
			intensity = -1f;
		}
		else if (intensity > 0.5f)
		{
			intensity = 1f;
		}
		else
		{
			intensity = 0f;
		}

		Debug.Log(intensity);
		IRacerController.instance.Direction = intensity;
	}
}
