using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	[SerializeField] private List<AudioClip> musicsList;
	[SerializeField] private AudioSource musicPlayer;
	static System.Random rnd;

	private void Start()
	{
		rnd = new System.Random();
	}

	private void Update()
	{
		if (!musicPlayer.isPlaying)
		{
			PlaySound(rnd.Next(musicsList.Count - 1));
		}
	}

	public void PlaySound(int index)
	{
		musicPlayer.clip = musicsList[index];
		musicPlayer.Play();
	}
}
