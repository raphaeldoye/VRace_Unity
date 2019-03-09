using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

[Serializable]
public static class VehicleTransforms
{
	public static List<VehicleTransform> transformsList;
    private static Semaphore _pool;

    static VehicleTransforms()
	{
		transformsList = new List<VehicleTransform>();
        _pool = new Semaphore(0, 1);
    }

	 //copy constructor 
	/*public VehicleTransforms(VehicleTransforms vTrans)
	{
		transformsList = new List<VehicleTransform>();
		transformsList.AddRange(vTrans.transformsList);
	}*/

	public static void Push(VehicleTransform transform)
	{
        _pool.WaitOne();
        transformsList.Add(transform);
        _pool.Release();
    }

	public static void Push(string jsonTransform)
	{
		VehicleTransform vt = JsonUtility.FromJson<VehicleTransform>(jsonTransform);
		Push(vt);
	}

	private static void SetList(List<VehicleTransform> transforms)
	{
		transformsList.AddRange(transforms);
	}

	public static List<VehicleTransform> PopAll()
	{
        _pool.WaitOne();
        List<VehicleTransform>  t = new List<VehicleTransform>();
		t.AddRange(transformsList);
		transformsList.Clear();
        _pool.Release();
        return t;
	}

	public static VehicleTransform Dequeue()
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

	public static bool Empty()
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