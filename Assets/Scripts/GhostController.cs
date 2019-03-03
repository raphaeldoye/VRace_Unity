using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class GhostController : MonoBehaviour
{
	VehicleTransforms vTransforms;
	VehicleTransform vTransform;
	string path = "Assets/Resources/vehicleTransforms.txt";
	private bool locked = true;
	public Vector3 decalage = Vector3.zero;

	void Start()
    {
		gameObject.SetActive(false);
		vTransforms = JsonUtility.FromJson<VehicleTransforms>(GameManager.instance.GetTextFromFile(path));
    }

	private void FixedUpdate()
	{
		if (!locked)
		{
			if (!vTransforms.Empty())
			{
				vTransform = vTransforms.Dequeue();
				transform.position = vTransform.position + decalage;
				transform.eulerAngles = vTransform.rotation;
			}
			else
			{
				gameObject.SetActive(false);
			}
		}
	}

	public void UnlockMovement()
	{
		locked = false;
		gameObject.SetActive(true);

	}

	public void LockMovement()
	{
		locked = true;
	}
}
