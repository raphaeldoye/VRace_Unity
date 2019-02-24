using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;


[Serializable]
public class MapBuilder
{
	public float magicScaleNumber = 1.01f;
	public Wall externalWalls;
	public List<Wall> internalWalls;

	private GameObject map;
	private GameObject ground;

	public MapBuilder()
	{
		externalWalls = new Wall();
		internalWalls = new List<Wall>();
	}

	public void BuildMap()
	{
		CreateGround();
		CreateExternalWall();
	}

	public void BuildMiniMap()
	{

	}

	private void CreateExternalWall()
	{
	//	float[] rotationList = { -90f, 0f, 90f, 180f };
		GameObject wall = MapStore.instance.defaultMap.externalWall;
		Vector3 wallSize = wall.GetComponent<Renderer>().bounds.size;
		//int coordinatesNb = externalWalls.coordinates.Count;
		List<Vector3> coordinates = ground.GetComponent<MeshFilter>().mesh.vertices.ToList<Vector3>();
		coordinates.RemoveAt(0);
		int coordinatesNb = coordinates.Count;

		for (int i = 0; i < coordinatesNb; i++)
		{
			coordinates[i] = ground.transform.TransformPoint(coordinates[i]);
		}

		for (int i = 0; i < coordinatesNb; i++)
		{
			float distance = Vector3.Distance(coordinates[i], coordinates[(i + 1)% coordinatesNb]);
			float rotation = GetAngle(coordinates[i], coordinates[(i + 1) % coordinatesNb]);// Vector3.Angle(externalWalls.coordinates[i], externalWalls.coordinates[(i + 1)% coordinatesNb]);
			float wallsNb = distance / wallSize.x;
			wallsNb++;

			GameObject anchor = new GameObject("anchor" + i);
			//Vector3 decalage = new Vector3(wallSize.x / 2, 0, wallSize.z / 2);
//			anchor.transform.position = externalWalls.coordinates[i];

			for (int j = 0; j < wallsNb; j++)
			{
				Vector3 position = coordinates[i] + j * (coordinates[(i + 1)%coordinatesNb] - coordinates[i]) / wallsNb; // Because Logic!
				GameObject newWall = GameObject.Instantiate(wall);
				newWall.transform.SetParent(anchor.transform);
			//	position = new Vector3(position.x - (wallSize.x / 2), 0, position.z - wallSize.z / 2.0f);
				newWall.transform.position = position;//new Vector3((wallSize.x / 2) + (wallSize.x * (j)), 0, wallSize.z/ 2.0f);
				newWall.transform.eulerAngles = new Vector3(newWall.transform.eulerAngles.x, newWall.transform.eulerAngles.y + rotation, newWall.transform.eulerAngles.z);
			}
			//anchor.transform.eulerAngles = new Vector3(0, rotationList[i], 0);
		}
		//https://answers.unity.com/questions/1379156/instantiate-objects-in-a-line-between-two-points.html
	}

	private float GetAngle(Vector3 pointA, Vector3 pointB)
	{
		Vector3 difference = pointB - pointA;
		Vector3 newPoint = pointB;
		float longestValue = 0;


		if (Mathf.Abs(difference.x) > longestValue)
		{
			longestValue = Mathf.Abs(difference.x);
			newPoint = new Vector3(pointB.x, 0, pointA.z);
		}

		if (Mathf.Abs(difference.z) > longestValue)
		{
			longestValue = Mathf.Abs(difference.z);
			newPoint = new Vector3(pointB.z, 0, pointA.z);
		}

		float rotation = Vector3.Angle(pointA, pointB);
		
		return -Vector3.Angle(pointB - pointA, newPoint - pointA);
	}

	private void CreateGround()
	{
		ground = new GameObject("Ground");
		//Vector3 wallSize = MapStore.instance.defaultMap.externalWall.GetComponent<Renderer>().bounds.size;

		List<Vector3> poly = externalWalls.coordinates;
		MeshFilter mf = ground.AddComponent<MeshFilter>();
		

		Mesh mesh = new Mesh();
		mf.mesh = mesh;
		ground.AddComponent<MeshCollider>();
		Rigidbody rb = ground.AddComponent<Rigidbody>();
		rb.useGravity = false;
		rb.isKinematic = true;

		Vector3 center = FindCenter(externalWalls.coordinates);

		Vector3[] vertices = new Vector3[poly.Count + 1];
		vertices[0] = Vector3.zero;

		for (int i = 0; i < poly.Count; i++)
		{
			//poly[i].Set(poly[i].x, 0, poly[i].z);
			vertices[i + 1] = poly[i] - center;
		}

		mesh.vertices = vertices;

		int[] triangles = new int[poly.Count * 3];

		for (int i = 0; i < poly.Count - 1; i++)
		{
			triangles[i * 3] = i + 2;
			triangles[i * 3 + 1] = 0;
			triangles[i * 3 + 2] = i + 1;
		}

		triangles[(poly.Count - 1) * 3] = 1;
		triangles[(poly.Count - 1) * 3 + 1] = 0;
		triangles[(poly.Count - 1) * 3 + 2] = poly.Count;

		mesh.triangles = triangles;//.Reverse().ToArray();
		mesh.uv = BuildUVs(vertices);

		mesh.RecalculateBounds();
		mesh.RecalculateNormals();

		Renderer rend = ground.AddComponent<MeshRenderer>();
		rend.material = MapStore.instance.defaultMap.groundMaterial;
		
		ground.transform.position = center;
		ground.transform.localScale = new Vector3(magicScaleNumber, magicScaleNumber, magicScaleNumber);
	}

	Vector3 FindCenter(List<Vector3> coordinates)
	{
		Vector3 center = Vector3.zero;
		foreach (Vector3 v3 in coordinates)
		{
			center += v3;
		}
		return center / coordinates.Count;
	}

	Vector2[] BuildUVs(Vector3[] vertices)
	{

		float xMin = Mathf.Infinity;
		float zMin = Mathf.Infinity;
		float xMax = -Mathf.Infinity;
		float zMax = -Mathf.Infinity;

		foreach (Vector3 v3 in vertices)
		{
			if (v3.x < xMin)
				xMin = v3.x;
			if (v3.z < zMin)
				zMin = v3.z;
			if (v3.x > xMax)
				xMax = v3.x;
			if (v3.z > zMax)
				zMax = v3.z;
		}

		float xRange = xMax - xMin;
		float yRange = zMax - zMin;

		Vector2[] uvs = new Vector2[vertices.Length];
		for (int i = 0; i < vertices.Length; i++)
		{
			uvs[i].x = (vertices[i].x - xMin) / xRange;
			uvs[i].y = (vertices[i].z - zMin) / yRange;
		}
		return uvs;
	}
}

[Serializable]
public class Wall
{
	public List<Vector3> coordinates;

	public Wall ()
	{
		coordinates = new List<Vector3>();
	}
}
