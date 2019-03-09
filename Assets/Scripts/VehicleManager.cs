using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void FixedUpdate()
	{
		UpdateTransforms();
	}

	private void UpdateTransforms()
	{
		List<VehicleTransform> realTransform = VehicleTransforms.PopAll();

		if (realTransform.Count > 0)
		{
			transform.position = realTransform[realTransform.Count - 1].position;
			transform.eulerAngles = realTransform[realTransform.Count - 1].rotation;
		}
	}
}
