using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

[RequireComponent(typeof(NoiseFilter))]
public class GhostController : MonoBehaviour
{
	public enum SporadicFilter { None, AutomateTreshold, ManualTreshold};
	public enum SelectedValueFilter { LastValue, Mean, Mode, Median}

	GhostTransforms vTransforms;
	GhostTransform vTransform;
	public string path = "Assets/Resources/vehicleTransforms";
	public string extension = ".txt";
	private bool locked = true;
	private bool enable = true;
	public Vector3 decalage = Vector3.zero;

	public int readDataPerFrame = 1;

	private NoiseFilter noiseFilter;

	private Vector3 currentPosition;
	private Vector3 currentRotation;

	private void Awake()
	{
		noiseFilter = GetComponent<NoiseFilter>();
	}

	void Start()
    {
		gameObject.SetActive(true);
		vTransforms = JsonUtility.FromJson<GhostTransforms>(GameManager.instance.GetTextFromFile(GetFullPath()));
		if (vTransforms == null)
		{
			GameManager.instance.SetRecordTime(GameManager.DEFAULT_RECORD_TIME);
			enable = false;
		}
		else
		{
			GameManager.instance.SetRecordTime(vTransforms.time);
		}
		gameObject.SetActive(false);
	}

	private void FixedUpdate()
	{
		if (!locked && enable)
		{
			SetCarPosition();
		}
	}

	private string GetFullPath()
	{
		return path + GameRules.instance.GetMaxLap().ToString() + extension;
	}

	private void SetCarPosition()
	{
		currentPosition = transform.position;
		currentRotation = transform.eulerAngles;
		List<Vector3> positions = new List<Vector3>();
		List<Vector3> rotations = new List<Vector3>();
		if(PopTransforms(positions, rotations, readDataPerFrame))
		{
			noiseFilter.Filter(ref currentPosition, ref currentRotation, positions, rotations);
			transform.position = currentPosition + decalage;
			transform.eulerAngles = currentRotation;
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
