using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class GhostController : MonoBehaviour
{
	VehicleTransforms vTransforms;
	VehicleTransform vTransform;
	string path = "Assets/Resources/vehicleTransforms.txt";
	bool dataToRead = false;

	// Start is called before the first frame update
	void Start()
    {
		ReadFile();

		if (!dataToRead)
		{
			gameObject.SetActive(false);
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void FixedUpdate()
	{
		if (!vTransforms.Empty())
		{
			vTransform = vTransforms.Dequeue();
			transform.position = vTransform.position;
			transform.eulerAngles = vTransform.rotation;
		}
		else
		{
			gameObject.SetActive(false);
		}
	}

	void ReadFile()
	{
		StreamReader reader = null;

		try
		{
			reader = new StreamReader(path);
			vTransforms = JsonUtility.FromJson<VehicleTransforms>(reader.ReadToEnd());
			reader.Close();
			if (!vTransforms.Empty())
			{
				dataToRead = true;
			}			
		}
		catch (System.Exception)
		{
			// nothing to do for now;
		}

	}
}
