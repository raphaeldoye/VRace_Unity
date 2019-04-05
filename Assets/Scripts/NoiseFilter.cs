using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NoiseFilter : MonoBehaviour
{
	public enum SporadicFilter { None, AutomateTreshold, ManualTreshold, MaxValueTreshold};
	public enum FrameGeneration { LastFrame, FromMagnitude}
	public enum SelectedValueFilter { LastValue, Mean, Mode, Median }

	public SporadicFilter positionFilter_SporadicNoise = SporadicFilter.None;
	public float positionTreshold = 1.0f;
	public SporadicFilter rotationFilter_SporadicNoise = SporadicFilter.None;
	public float rotationTreshold = 1.0f;
	[Space(3)]
	public float sporadicSigmaMultiplier = 2.0f;
	[Space(10)]
	public FrameGeneration positionFilter_FrameGeneration = FrameGeneration.LastFrame;
	public FrameGeneration rotationFilter_FrameGeneration = FrameGeneration.LastFrame;
	[Space(10)]
	public SelectedValueFilter positionFilter_selectedValue = SelectedValueFilter.LastValue;
	public SelectedValueFilter rotationFilter_selectedValue = SelectedValueFilter.LastValue;
	public float initialFramesWithoutFilter = 5;
	private float currentFramesWithoutFilter = 0;

	private Vector3 previousPosition;
	private Vector3 previousRotation;

	public void Filter(ref Vector3 currentPosition, ref Vector3 currentRotation, List<Vector3> positions, List<Vector3> rotations)
	{
		Vector3 positionVelocity = (currentPosition - previousPosition) / Time.fixedDeltaTime;
		Vector3 angularVelocity = (currentRotation - previousRotation) / Time.fixedDeltaTime;

		previousPosition = currentPosition;
		previousRotation = currentRotation;

		//sporadic filter
		if (currentFramesWithoutFilter >= initialFramesWithoutFilter)
		{
			SporadicNoiseFilter(previousPosition, positions, positionFilter_SporadicNoise, positionTreshold);
			SporadicNoiseFilter(previousRotation, rotations, rotationFilter_SporadicNoise, rotationTreshold, false);
		}
		else
		{
			currentFramesWithoutFilter++;
		}

		Vector3 position = new Vector3();
		Vector3 rotation = new Vector3();

		//position value selection
		if (positions.Count == 0)
			position = CreateFrame(previousPosition, positionVelocity, positionFilter_FrameGeneration);
		else
			position = ValueSelector(positions, positionFilter_selectedValue);


		//rotation value selection
		if (rotations.Count == 0)
			rotation = CreateFrame(previousRotation, angularVelocity, rotationFilter_FrameGeneration);
		else
			rotation = ValueSelector(rotations, rotationFilter_selectedValue);

		position.y = 0;
		currentPosition = position;
		currentRotation = rotation;
		
	}

	private Vector3 CreateFrame(Vector3 previousCoordinate, Vector3 velocity, FrameGeneration filter)
	{
		Vector3 response = new Vector3();
		switch (filter)
		{
			case FrameGeneration.LastFrame:
				response = previousCoordinate;
				break;
			case FrameGeneration.FromMagnitude:
				response = previousCoordinate + velocity * Time.deltaTime;
				break;
			default:
				break;
		}

		return response;
	}

	private void SporadicNoiseFilter(Vector3 previousCoordinate, List<Vector3> coordinates, SporadicFilter filter, float filterTreshold, bool isPosition = true)
	{
		if (filter != SporadicFilter.None)
		{
			if (filter == SporadicFilter.MaxValueTreshold)
			{
				RemoveMaxValueTreshold(previousCoordinate, coordinates, filterTreshold, isPosition);
			}
			else
			{
				coordinates.Add(Average(coordinates));
				coordinates.Insert(0, previousRotation);

				RemoveSporadicNoise(coordinates, filter, filterTreshold);

				coordinates.RemoveAt(0);
				coordinates.RemoveAt(coordinates.Count - 1);
			}
		}		
	}

	private Vector3 ValueSelector(List<Vector3> coordinates, SelectedValueFilter filter)
	{
		Vector3 response = new Vector3();

		switch (filter)
		{
			case SelectedValueFilter.LastValue:
				response = coordinates[coordinates.Count - 1];
				break;
			case SelectedValueFilter.Mean:
				response = Average(coordinates);
				break;
			case SelectedValueFilter.Mode:
				response = Mode(coordinates);
				break;
			case SelectedValueFilter.Median:
				response = Median(coordinates);
				break;
			default:
				break;
		}

		return response;
	}


	private void RemoveMaxValueTreshold(Vector3 referenceCoordinate, List<Vector3> coordinates, float filterTreshold, bool isPosition = true)
	{
		for (int i = coordinates.Count-1; i >= 0 ; i--)
		{
			if (isPosition)
			{			
				if (Distance(coordinates[i], referenceCoordinate) > filterTreshold)
				{
					coordinates.RemoveAt(i);
				}
			}
			else
			{
				if (Mathf.Abs(coordinates[i].y - referenceCoordinate.y) > filterTreshold && Mathf.Abs(coordinates[i].y - referenceCoordinate.y) < 360 - filterTreshold)
				{
					coordinates.RemoveAt(i);
				}
			}
		}
	}

	private void RemoveSporadicNoise(List<Vector3> coordinates, SporadicFilter filter, float filterTreshold)
	{
		float treshold = filter == SporadicFilter.AutomateTreshold ? GetTreshold(coordinates) : filterTreshold;
		int i = 1;
		while (i <= coordinates.Count - 2)
		{
			if (Distance(coordinates[i], coordinates[i - 1]) > treshold && (Distance(coordinates[i], coordinates[i + 1]) > treshold))
			{
				coordinates.RemoveAt(i);
			}
			else
			{
				i++;
			}
		}
	}

	private Vector3 Mode(List<Vector3> positions)
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

	private Vector3 Average(List<Vector3> positions)
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

		for (int i = 0; i < positions.Count - 2; i++)
		{
			totaldistance += Distance(positions[i], positions[i + 1]);
		}

		return totaldistance;
	}

	private float SquareSum(List<Vector3> positions, float averageDistance)
	{
		float squareSum = 0.0f;

		for (int i = 0; i < positions.Count - 2; i++)
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
}
