using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;


[Serializable]
public class MapBuilder
{
	public Wall externalWalls;
	public List<Wall> internalWalls;

	private GameObject map;

	public MapBuilder()
	{
		externalWalls = new Wall();
		internalWalls = new List<Wall>();
	}

	public void BuildMap()
	{
		CreateGround();
	}

	public void BuildMiniMap()
	{

	}

	/*private void CreateExternalWall()
	{
		GameObject cube;

		for (int i = 0; i < externalWalls.coordinates.Count; i++)
		{
			cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		}
		cube.transform.position = new Vector3(0, 0.5f, 0);
		//https://answers.unity.com/questions/1379156/instantiate-objects-in-a-line-between-two-points.html
	}*/

	private void CreateGround()
	{
		GameObject ground = new GameObject("Ground");

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

		mesh.triangles = triangles.Reverse().ToArray();
		mesh.uv = BuildUVs(vertices);

		mesh.RecalculateBounds();
		mesh.RecalculateNormals();

		Renderer rend = ground.AddComponent<MeshRenderer>();
		rend.material = MapStore.instance.defaultMap.groundMaterial;
		
		ground.transform.position = center;
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
