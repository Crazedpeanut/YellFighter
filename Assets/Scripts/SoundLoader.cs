using UnityEngine;
using System.Collections;

public class SoundLoader : MonoBehaviour {

	public AudioClip shootSound;
	public AudioClip punchSound;
	public AudioClip kickSound;

	AudioSource audioSource;

	void Start()
	{
		audioSource = this.gameObject.GetComponent<AudioSource> ();
	}

	public void playShoot()
	{
		audioSource.clip = shootSound;
		audioSource.Play ();
	}

	public void playPunch()
	{
		audioSource.clip = punchSound;
		audioSource.Play ();
	}

	public void playKick()
	{
		audioSource.clip = kickSound;
		audioSource.Play ();
	}
}
