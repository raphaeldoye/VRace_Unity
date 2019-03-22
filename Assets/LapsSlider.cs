using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class LapsSlider : MonoBehaviour
{
	[SerializeField] private Text valueText;

	public void ChangeLapValue()
	{
		int value = (int)GetComponent<Slider>().value;
		valueText.text = value.ToString();
		GameRules.instance.SetMaxLap(value);
		PlayerPrefs.SetInt("lapsNumber", value);
	}
}
