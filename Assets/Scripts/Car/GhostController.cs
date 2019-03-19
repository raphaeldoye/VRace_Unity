using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;


public class GhostController : MonoBehaviour
{
	public enum SporadicFilter { None, AutomateTreshold, ManualTreshold};
	public enum SelectedValueFilter { LastValue, Mean, Mode, Median}

	GhostTransforms vTransforms;
	GhostTransform vTransform;
	string path = "Assets/Resources/vehicleTransforms.txt";
	private bool locked = true;
	private bool enable = true;
	public Vector3 decalage = Vector3.zero;
	[Header("developper settings")]
	[Space(10)]
	public int readDataPerFrame = 1;

	public SporadicFilter filter_SporadicNoise = SporadicFilter.None;
	public SelectedValueFilter filter_selectedValue = SelectedValueFilter.LastValue;
	public float manualTreshold = 1.0f;
	public float sporadicSigmaMultiplier = 2.0f;
	public float averageAlpha = 0.5f;

	private Vector3 previousPosition;
	private Vector3 previousRotation;

	void Start()
    {
		gameObject.SetActive(false);
		vTransforms = JsonUtility.FromJson<GhostTransforms>(GameManager.instance.GetTextFromFile(path));
		if (vTransforms == null)
		{
			GameManager.instance.SetRecordTime(GameManager.DEFAULT_RECORD_TIME);
			enable = false;
		}
		else
		{
			GameManager.instance.SetRecordTime(vTransforms.time);
		}
	}

	private void FixedUpdate()
	{
		if (!locked && enable)
		{
			SetCarPosition();
		}
	}

	private void SetCarPosition()
	{
		previousPosition = transform.position;
		previousRotation = transform.eulerAngles;

		List<Vector3> positions = new List<Vector3>();
		List<Vector3> rotations = new List<Vector3>();
		Vector3 position = new Vector3();
		Vector3 rotation = new Vector3();
		if (PopTransforms(positions, rotations, readDataPerFrame))
		{

			if (filter_SporadicNoise != SporadicFilter.None)
			{
				rotations.Add(Average(rotations));

				positions.Insert(0, previousPosition);
				rotations.Insert(0, previousRotation);

				RemoveSporadicNoise(positions);
				RemoveSporadicNoise(rotations);

				positions.RemoveAt(0);
				rotations.RemoveAt(0);

				rotations.RemoveAt(rotations.Count - 1);
			}

				switch (filter_selectedValue)
				{
					case SelectedValueFilter.LastValue:
						position = positions[positions.Count - 1];
						rotation = rotations[rotations.Count - 1];
						break;
					case SelectedValueFilter.Mean:
						position = Average(positions);
						rotation = Average(rotations);
					break;
					case SelectedValueFilter.Mode:
						position = Mode(positions);
						rotation = Mode(rotations);
					break;
					case SelectedValueFilter.Median:
					position = Median(positions);
					rotation = Median(rotations);
					break;
					default:
						break;
				}
				position = Average(positions);
				rotation = Average(rotations);
		

			transform.position = position + decalage;
			transform.eulerAngles = rotation;
		}
		else
		{
			gameObject.SetActive(false);
		}

	}

	private bool PopTransforms(List<Vector3> pos, List<Vector3> rot, int iteration)
	{
		for (int i = 0; i < iteration; i++)
		{
			if (!vTransforms.Empty())
			{
				vTransform = vTransforms.Dequeue();
				pos.Add(vTransform.position);
				rot.Add(vTransform.rotation);
			}
			else
			{
				return false;
			}
		}

		return true;
	}

	private void RemoveSporadicNoise(List<Vector3> positions)
	{
		float treshold = filter_SporadicNoise == SporadicFilter.AutomateTreshold ? GetTreshold(positions): manualTreshold;
		int i = 1;
		while (i <= positions.Count-2)
		{
			if (Distance(positions[i],positions[i-1]) > treshold && (Distance(positions[i], positions[i + 1]) > treshold))
			{
				positions.RemoveAt(i);
			}
			else
			{
				i++;
			}
		}
	}

	private Vector3 Mode (List<Vector3> positions)
	{
		//Vector3 modeVector = new Vector3();
		var groups = positions.GroupBy(v => v);
		int maxCount = groups.Max(g => g.Count());
		Vector3 mode = groups.First(g => g.Count() == maxCount).Key;

		return mode;
	}

	private Vector3 Median(List<Vector3> positions)
	{
		var orderedPositions = positions.OrderBy(p => p.magnitude);
		Vector3 median = orderedPositions.ElementAt(positions.Count / 2) + orderedPositions.ElementAt((positions.Count - 1) / 2);
		median /= 2;
		return median;
	}

	private Vector3 Average (List<Vector3> positions)
	{
		//averageAlpha*
		Vector3 average = Vector3.zero;
		for (int i = 0; i < positions.Count; i++)
		{
			average += positions[i];
		}

		return average / positions.Count;
	}

	private float Distance(Vector3 pointA, Vector3 pointB)
	{
		return Vector3.Distance(pointA, pointB);
	}

	private float Distance(List<Vector3> positions)
	{
		float totaldistance = 0.0f;

		for (int i = 0; i < positions.Count-2; i++)
		{
			totaldistance += Distance(positions[i], positions[i + 1]);
		}

		return totaldistance;
	}

	private float SquareSum (List<Vector3> positions, float averageDistance)
	{
		float squareSum = 0.0f;

		for (int i = 0; i < positions.Count-2; i++)
		{
			float d = Distance(positions[i], positions[i + 1]);
			float difference = d - averageDistance;
			squareSum += difference * difference;
		}

		return squareSum;
	}

	private float GetTreshold(List<Vector3> positions)
	{
		float totalDistance = Distance(positions); // distance totale entre les points
		float averageDistance = totalDistance / (positions.Count - 1); // distance moyenne
		float squareSum = SquareSum(positions, averageDistance); // obtenir la somme des carré
		float sigma = Mathf.Sqrt(squareSum / (positions.Count - 2)); // écart type

		return averageDistance + (sporadicSigmaMultiplier * sigma);
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
