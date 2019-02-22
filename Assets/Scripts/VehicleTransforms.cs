using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class VehicleTransforms
{
	public List<VehicleTransform> transformsList;

	public VehicleTransforms()
	{
		transformsList = new List<VehicleTransform>();
	}

	 //copy constructor 
	/*public VehicleTransforms(VehicleTransforms vTrans)
	{
		transformsList = new List<VehicleTransform>();
		transformsList.AddRange(vTrans.transformsList);
	}*/

	public void Push(VehicleTransform transform)
	{
		transformsList.Add(transform);
	}

	public void Push(string jsonTransform)
	{
		VehicleTransform vt = JsonUtility.FromJson<VehicleTransform>(jsonTransform);
		Push(vt);
	}

	private void SetList(List<VehicleTransform> transforms)
	{
		transformsList.AddRange(transforms);
	}

	public List<VehicleTransform> PopAll()
	{
		List<VehicleTransform>  t = new List<VehicleTransform>();
		t.AddRange(transformsList);
		transformsList.Clear();
		return t;
	}

	public VehicleTransform Dequeue()
	{
		VehicleTransform vt = new VehicleTransform
		{
			position = Vector3.zero,
			rotation = Vector3.zero
		};
		if (!Empty())
		{
			vt = transformsList[0];
			transformsList.RemoveAt(0);
		}
		return vt;
	}

	public bool Empty()
	{
		return transformsList.Count <= 0;
	}
}

[Serializable]
public struct VehicleTransform
{
	public Vector3 position;
	public Vector3 rotation;
}