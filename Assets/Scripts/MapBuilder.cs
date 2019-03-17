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
	public Wall startLine;
	static System.Random rnd;

	private GameObject map;

	public MapBuilder()
	{
		externalWalls = new Wall();
		internalWalls = new List<Wall>();
		startLine = new Wall();
		rnd = new System.Random();
	}

	public void BuildMap()
	{
		map = new GameObject("MAP");

		GameObject ground = CreateGround();
		ground.transform.SetParent(map.transform);

		CreateExternalWalls(ground);
		CreateInternalWalls();
		CreateStartLine();
	}

	public void BuildMiniMap()
	{

	}

	private void CreateStartLine()
	{
		GameObject line = new GameObject("StartLine");
		GameObject triggers = new GameObject("StartLineTriggers");
		const float decalage = 300f;

		line.transform.SetParent(map.transform);
		triggers.transform.SetParent(line.transform);

		List<GameObject> startLineObjects = MapStore.instance.GetSelectedMap().startLine_Floor;
		GameObject triggerObject = MapStore.instance.GetSelectedMap().startLineTrigger;
		GameObject secondTriggerObject = MapStore.instance.GetSelectedMap().startLineSecondTrigger;

		CreateWall(startLine.coordinates, startLineObjects, "startLine", line, 0);
		CreateWall(startLine.coordinates, triggerObject, "trigger1", triggers, Vector3.zero);

		Vector3 decalageVector = decalage * (new Vector3(1f,0,1f) - Vector3.Normalize(startLine.coordinates[1] - startLine.coordinates[0]));
		CreateWall(startLine.coordinates, secondTriggerObject, "trigger2", triggers, decalageVector);
	}

	private void CreateInternalWalls()
	{
		GameObject extWalls = new GameObject("InternalWalls");
		extWalls.transform.SetParent(map.transform);

		List<GameObject> walls = MapStore.instance.GetSelectedMap().internalWalls;

		for (int i = 0; i < internalWalls.Count; i++)
		{
			CreateWall(internalWalls[i].coordinates, walls, "InternalWall", extWalls, 0, true);
		}		
	}

	private void CreateExternalWalls(GameObject ground)
	{
		GameObject extWalls = new GameObject("ExternalWalls");
		extWalls.transform.SetParent(map.transform);

		List<GameObject> walls = MapStore.instance.GetSelectedMap().externalWalls;
		int addWall = MapStore.instance.GetSelectedMap().additionalWalls;

		CreateWall(GetWorldMeshCoordinates(ground), walls, "ExternalWall", extWalls, addWall);
	}


	private void CreateWall(List<Vector3> coordinates, List<GameObject> walls, string name, GameObject parent, int additionalWalls, bool internDecalage = false)
	{		 
		int coordinatesNb = coordinates.Count;

		for (int i = 0; i < coordinatesNb; i++)
		{
			float distance = Vector3.Distance(coordinates[i], coordinates[(i + 1)% coordinatesNb]);
			float rotation = GetAngle(coordinates[i], coordinates[(i + 1) % coordinatesNb]);
			int wallsNb = GetWallsNum(walls[0], distance);

			GameObject anchor = new GameObject(name + i); //container
			anchor.transform.SetParent(parent.transform);			

			for (int j = 0; j < wallsNb + additionalWalls; j++)
			{				
				GameObject newWall = GameObject.Instantiate(walls[rnd.Next(walls.Count)]); // instantiate a wall in the list randomly
				Vector3 newWallSize = ScaleWall(newWall, wallsNb, distance);
				newWall.transform.SetParent(anchor.transform);
				newWall.transform.position = GetPosition(coordinates[i], coordinates[(i + 1) % coordinatesNb],j, newWallSize, wallsNb, internDecalage);
				newWall.transform.eulerAngles = new Vector3(newWall.transform.eulerAngles.x, newWall.transform.eulerAngles.y + rotation, newWall.transform.eulerAngles.z);
			}
		}
	}

	private void CreateWall(List<Vector3> coordinates, GameObject wall, string name, GameObject parent, Vector3 decalage)
	{
		int coordinatesNb = coordinates.Count;

		for (int i = 0; i < coordinatesNb-1; i++)
		{
			float distance = Vector3.Distance(coordinates[i], coordinates[(i + 1) % coordinatesNb]);
			float rotation = GetAngle(coordinates[i], coordinates[(i + 1) % coordinatesNb]);
			int wallsNb = 1;

			GameObject anchor = new GameObject(name + i); //container
			anchor.transform.SetParent(parent.transform);

			for (int j = 0; j < wallsNb; j++)
			{
				GameObject newWall = GameObject.Instantiate(wall); // instantiate a wall in the list randomly
				Vector3 newWallSize = ScaleWall(newWall, wallsNb, distance);
				newWall.transform.SetParent(anchor.transform);
				newWall.transform.position = GetPosition(coordinates[i], coordinates[(i + 1) % coordinatesNb], j, newWallSize, wallsNb, false) + decalage;
				newWall.transform.eulerAngles = new Vector3(newWall.transform.eulerAngles.x, newWall.transform.eulerAngles.y + rotation, newWall.transform.eulerAngles.z);
			}
		}
	}

	private float GetAngle(Vector3 pointA, Vector3 pointB)
	{
		Vector3 difference = pointB - pointA;
		Vector3 newPoint = pointB;
		int rotationDirection = 1;
		float longestValue = 0;		

		if (Mathf.Abs(difference.x) > longestValue)
		{
			longestValue = Mathf.Abs(difference.x);

			if (difference.x > 0)
				newPoint = new Vector3(0, 0, pointA.z);
			else
				newPoint = new Vector3(pointB.x, 0, pointA.z);
		}

		if (Mathf.Abs(difference.z) > longestValue)
		{
			//longestValue = Mathf.Abs(difference.z);
			newPoint = new Vector3(0, 0, pointA.z);
		}

		rotationDirection = pointB.z >= pointA.z ? 1 : -1;		
		return rotationDirection * Vector3.Angle(pointB - pointA, newPoint - pointA);
	}
	private Vector3 GetPosition(Vector3 coordinateA, Vector3 coordinateB, int iteration, Vector3 wallSize, int wallsNb, bool internDecalage)
	{
		Vector3 position = coordinateA + iteration * (coordinateB - coordinateA) / wallsNb; // Because Logic!
		Vector3 differenceX = (coordinateB - coordinateA).normalized;
		Vector3 differenceZ = new Vector3(-differenceX.z, differenceX.y, differenceX.x);
		differenceX *= (wallSize.x / 2);
		differenceZ *= (wallSize.z / 2);

		if (internDecalage)
			differenceZ *= -1;	

		return new Vector3(position.x, position.y, position.z) + differenceX + differenceZ;
	}

	private List<Vector3> GetWorldMeshCoordinates(GameObject mesh)
	{
		List<Vector3> coordinates = mesh.GetComponent<MeshFilter>().mesh.vertices.ToList<Vector3>();
		for (int i = 0; i < coordinates.Count; i++)
		{
			coordinates[i] = mesh.transform.TransformPoint(coordinates[i]);
		}
		coordinates.RemoveAt(0);
		return coordinates;
	}

	private int GetWallsNum(GameObject wall, float distance)
	{
		Vector3 wallSize = wall.GetComponent<Renderer>().bounds.size;
		return Mathf.RoundToInt(distance / wallSize.x);
	}

	private Vector3 ScaleWall(GameObject wall, int wallsNb, float distance)
	{
		Vector3 wallSize = wall.GetComponent<Renderer>().bounds.size;
		float desireWallSize = distance / wallsNb;

		Vector3 scale = wall.transform.localScale;
		scale.x = desireWallSize * scale.x / wallSize.x;
		wall.transform.localScale = scale;

		return wall.GetComponent<Renderer>().bounds.size;
	}

	private GameObject CreateGround()
	{
		GameObject ground = new GameObject("Ground");
		CreateMesh(ground);
		Renderer rend = ground.AddComponent<MeshRenderer>();
		rend.material = MapStore.instance.GetSelectedMap().groundMaterial;
		return ground;
	}

	private void CreateMesh(GameObject newObject)
	{
		List<Vector3> poly = externalWalls.coordinates;
		MeshFilter mf = newObject.AddComponent<MeshFilter>();		

		Mesh mesh = new Mesh();
		mf.mesh = mesh;
		newObject.AddComponent<MeshCollider>();
		Rigidbody rb = newObject.AddComponent<Rigidbody>();
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

		newObject.transform.position = center;
		//newObject.transform.localScale = new Vector3(magicScaleNumber, magicScaleNumber, magicScaleNumber);
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
