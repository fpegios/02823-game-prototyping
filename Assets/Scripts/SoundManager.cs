using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	public AudioSource efxSource;
	public AudioSource musicSource;
	public static SoundManager instance = null;


	void Awake () {
		if (instance == null)
			instance = this;
		else if (instance != this) 
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);
	}
	
	public void PlayEfx (AudioClip clip) {
		efxSource.clip = clip;
		efxSource.Play();
	}

	public void StopEfx (AudioClip clip) {
		efxSource.clip = clip;
		efxSource.Stop();
	}

	public void PlayMusic (AudioClip clip) {
		musicSource.clip = clip;
		musicSource.Play();
	}

	public void StopMusic (AudioClip clip) {
		musicSource.clip = clip;
		musicSource.Stop();
	}
}
