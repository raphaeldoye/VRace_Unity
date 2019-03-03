using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MovementRecorder : MonoBehaviour
{
	VehicleTransforms transforms;
	bool savedOnce = false;
	string path = "Assets/Resources/vehicleTransforms.txt";
	bool record = false;
	public Transform trackerTransform;


	void Start()
    {
		transforms = new VehicleTransforms();
		Directory.CreateDirectory(Path.GetDirectoryName(path));
    }

	private void Update()
	{
		if(Input.GetButtonDown("Save") && !savedOnce)
		{
			SaveMovements();
			savedOnce = true;
		}
	}

	private void FixedUpdate()
	{
		if (!savedOnce && record)
		{
			AddCarPosition();
		}
	}

	void AddCarPosition()
	{

		VehicleTransform v = new VehicleTransform
		{
			position = trackerTransform.position,
			rotation = transform.eulerAngles
		};
		transforms.Push(v);
	}

	void SaveMovements()
	{
		string sTransforms = JsonUtility.ToJson(transforms, true);
		WriteToFile(sTransforms);
	}

	void WriteToFile(string jsonText)
	{
		File.WriteAllText(path, jsonText);
	}

	public void StartRecording()
	{
		record = true;
	}

	public void StoptRecording()
	{
		record = false;
		SaveMovements();
		savedOnce = true;
	}
}


