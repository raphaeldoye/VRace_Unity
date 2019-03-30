using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class IRacerController : MonoBehaviour
{
	class MyExternalLib
	{
		[DllImport("MyLibrary.dll", CallingConvention = CallingConvention.Cdecl)] // DLL degeulasse, ça retourne 0 si la connexion est reussi et 1 si non...
		public static extern int InitComm(int number);

		[DllImport("MyLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool Disconnect();

		[DllImport("MyLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern float Move(float accel, float angle, bool brake);
	}

	public static IRacerController instance = null;
	public int PORT = 8;

	/*private float speed;
	private float direction;*/

	public float Speed { get; set; }
	public float Direction { get; set; }

	private void OnApplicationQuit()
	{
		Disconnect();
	}

	void Awake()
	{
		if (instance == null)
			instance = this;

		else if (instance != this)
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);
	}

	public bool Connect()
	{
		var numb = MyExternalLib.InitComm(PORT);
		return numb == 0;
	}

	public bool Disconnect()
	{
		return MyExternalLib.Disconnect();
	}

	public void SetMovement()
	{
		var a = MyExternalLib.Move(Speed, Direction, false);
	}
}
