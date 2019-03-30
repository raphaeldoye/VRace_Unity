using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectServerAddress : MonoBehaviour
{

	[SerializeField] private InputField textInput;

	void Start()
    {
		string address = PlayerPrefs.GetString("serverAddress", "127.0.0.1");
		textInput.text = address;
		UDPClient.instance.ipAddress = address;

	}

	public void SetIpAddress()
	{
		string address = textInput.text;
		UDPClient.instance.ipAddress = address;
		PlayerPrefs.SetString("serverAddress", address);
	}
}
