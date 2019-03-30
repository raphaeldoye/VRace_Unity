using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class RealityCarController : CarController
{
   

   

    [SerializeField] [Range(0f,1f)] protected float speedMultiplier;

    private void Start()
	{
       
        ///TODO
        /// Set maxSpeed to the maximum value sent to the car (only for the speed)
        maxSpeed = 16;
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
		return 0.0f;
		///TODO
		///return car speed between 0 and the maxSpeed
	}

	protected override void SetBrakeTorque(float intensity)
	{
		///TODO
		///set the logic of the brake (braking when opposite direction key is press)
	}

	protected override void SetMotorTorque(float intensity, AxleInfo axle = null)             
	{
		IRacerController.instance.Speed = intensity;
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
