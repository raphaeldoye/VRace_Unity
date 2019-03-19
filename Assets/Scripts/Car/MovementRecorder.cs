using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MovementRecorder : MonoBehaviour
{
//	VehicleTransforms transforms;
	bool savedOnce = false;
	public string path = "Assets/Resources/vehicleTransforms.txt";
	bool record = false;
	public Transform trackerTransform;
	[Header("Developper settings")]
	public bool addNoise = false;
	public int savedDataPerFrame = 1;
	static System.Random rnd;


	void Start()
    {
	//	transforms = new VehicleTransforms();
		Directory.CreateDirectory(Path.GetDirectoryName(path));
		rnd = new System.Random();
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
			if (addNoise)
			{
				AddCarPositionWithNoise(trackerTransform.position, trackerTransform.eulerAngles);
			}
			else
			{
				AddCarPosition(trackerTransform.position, trackerTransform.eulerAngles);
			}
		}
	}

	void AddCarPosition(Vector3 pos, Vector3 rot)
	{	
	/*	VehicleTransform v = new VehicleTransform
		{
			position = pos,
			rotation = rot
		};
		transforms.Push(v);*/
	}

	void AddCarPositionWithNoise(Vector3 pos, Vector3 rot)
	{
		for (int i = 0; i < savedDataPerFrame; i++)
		{
			if (rnd.Next(3) == 1)
			{
				if (rnd.Next(5) == 4) // gros bruit
				{
					AddCarPosition(pos, AddNoise(new Vector3(0, rot.y, 0), 20));
				}
				else // petit bruit
				{
					AddCarPosition(pos, AddNoise(new Vector3(0, rot.y, 0), 3));
				}
			}
			else
			{
				AddCarPosition(pos,new Vector3(0, rot.y, 0));
			}
			
		}
	}

Vector3 AddNoise(Vector3 value, int maxNoise)
	{
		float x = value.x;
		float y = value.y;
		float z = value.z;

		if (x > 1)
		{
			if(rnd.Next(2) == 1)
				x += (float)rnd.NextDouble() * maxNoise;
			else
				x -= (float)rnd.NextDouble() * maxNoise;
		}
		if (y > 1)
		{
			if (rnd.Next(2) == 1)
				y += (float)rnd.NextDouble() * maxNoise;
			else
				y -= (float)rnd.NextDouble() * maxNoise;
		}
		if (z > 1)
		{
			if (rnd.Next(2) == 1)
				z += (float)rnd.NextDouble() * maxNoise;
			else
				z -= (float)rnd.NextDouble() * maxNoise;
		}

		return new Vector3(x, y, z);
	}

	void SaveMovements()
	{
		/*string sTransforms = JsonUtility.ToJson(transforms, true);
		WriteToFile(sTransforms);*/
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


