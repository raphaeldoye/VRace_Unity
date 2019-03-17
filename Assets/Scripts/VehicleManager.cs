using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleManager : MonoBehaviour
{
	[SerializeField] private Vector3 decalage = Vector3.zero;

	private void FixedUpdate()
	{
		UpdateTransforms();
	}

	private void UpdateTransforms()
	{
		List<VehicleTransform> realTransform = VehicleTransforms.PopAll();

		if (realTransform.Count > 0)
		{
			transform.position = realTransform[realTransform.Count - 1].position + decalage;
			transform.eulerAngles = realTransform[realTransform.Count - 1].rotation;
		}
	}
}
