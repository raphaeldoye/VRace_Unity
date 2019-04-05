using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NoiseFilter))]
public class VehicleManager : MonoBehaviour
{

	[SerializeField] private Vector3 decalage = Vector3.zero;
	private NoiseFilter noiseFilter;

	private Vector3 currentPosition;
	private Vector3 currentRotation;

	private void Awake()
	{
		noiseFilter = GetComponent<NoiseFilter>();
	}

	private void FixedUpdate()
	{
		UpdateTransforms();
	}

	private void UpdateTransforms()
	{
		// obtains last position and rotation received from the server
		List<VehicleTransform> realTransform = VehicleTransforms.PopAll();

		// split informations in two list of positions and rotations
		List<Vector3> positions = new List<Vector3>();
		List<Vector3> rotations = new List<Vector3>();		
		GetPositionsList(ref positions, realTransform);
		GetRotationsList(ref rotations, realTransform);

		// get the current position and rotation of the car
		currentPosition = transform.position;
		currentRotation = transform.eulerAngles;

		// call the filter algorithm
		noiseFilter.Filter(ref currentPosition, ref currentRotation, positions, rotations);

		// affect the position and rotation to the car
		transform.position = currentPosition + decalage;
		transform.eulerAngles = currentRotation;
	}

	private void GetPositionsList(ref List<Vector3> positions, List<VehicleTransform> vt)
	{
		for (int i = 0; i < vt.Count; i++)
		{
			positions.Add(vt[i].position);
		}
	}

	private void GetRotationsList(ref List<Vector3> rotations, List<VehicleTransform> vt)
	{
		for (int i = 0; i < vt.Count; i++)
		{
			rotations.Add(vt[i].rotation);
		}
	}
}
