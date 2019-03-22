using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	[SerializeField] private List<AudioClip> soundsList;
	[SerializeField] private List<AudioClip> musicsList;

	[SerializeField] private AudioSource soundPlayer;
	[SerializeField] private AudioSource musicPlayer;

	public void PlaySound(int index)
	{
		soundPlayer.clip = soundsList[index];
		soundPlayer.Play();
	}
}
