using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class GhostTransforms
{
	public float time;
	public List<GhostTransform> transformsList;

    public GhostTransforms()
	{
		time = 0;
		transformsList = new List<GhostTransform>();
    }

	 //copy constructor 
	/*public VehicleTransforms(VehicleTransforms vTrans)
	{
		transformsList = new List<VehicleTransform>();
		transformsList.AddRange(vTrans.transformsList);
	}*/

	public void Push(GhostTransform transform)
	{
        transformsList.Add(transform);
    }

	public void Push(string jsonTransform)
	{
		GhostTransform vt = JsonUtility.FromJson<GhostTransform>(jsonTransform);
		Push(vt);
	}

	private void SetList(List<GhostTransform> transforms)
	{
		transformsList.AddRange(transforms);
	}

	public List<GhostTransform> PopAll()
	{
        List<GhostTransform>  t = new List<GhostTransform>();
		t.AddRange(transformsList);
		transformsList.Clear();
        return t;
	}

	public GhostTransform Dequeue()
	{
		GhostTransform vt = new GhostTransform
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

	public void Clear()
	{
		transformsList.Clear();
	}
}

[Serializable]
public struct GhostTransform
{
	public Vector3 position;
	public Vector3 rotation;
}