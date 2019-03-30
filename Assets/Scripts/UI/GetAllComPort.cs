using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GetAllComPort : MonoBehaviour
{
	[SerializeField] private Dropdown combo;
	[SerializeField] private float maxValue = 26;

	private void Awake()
	{
		combo.ClearOptions();
		List<string> options = new List<string>();

		for (int i = 0; i < maxValue; i++)
		{
			options.Add("COM" +  i.ToString());
		}

		combo.AddOptions(options);

	}

	private void Start()
	{
		combo.value = PlayerPrefs.GetInt("carPort", 0);
	}

	public void SetSelectedPort()
	{
		IRacerController.instance.PORT = combo.value;
		PlayerPrefs.SetInt("carPort", combo.value);
	}
}
