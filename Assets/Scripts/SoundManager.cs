using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	public AudioSource userSfxSource_1, userSfxSource_2, collisionSfxSource;
	public AudioSource musicSource;
	public static SoundManager instance = null;


	void Awake () {
		if (instance == null)
			instance = this;
		else if (instance != this) 
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);
	}
	
	public void PlayUserSfx_1 (AudioClip clip) {
		userSfxSource_1.clip = clip;
		userSfxSource_1.Play();
	}

	public void StopUserSfx_1 (AudioClip clip) {
		userSfxSource_1.clip = clip;
		userSfxSource_1.Stop();
	}

	public void PlayUserSfx_2 (AudioClip clip) {
		userSfxSource_2.clip = clip;
		userSfxSource_2.Play();
	}

	public void StopUserSfx_2 (AudioClip clip) {
		userSfxSource_2.clip = clip;
		userSfxSource_2.Stop();
	}

	public void PlayCollisionSfx (AudioClip clip) {
		collisionSfxSource.clip = clip;
		collisionSfxSource.Play();
	}

	public void StopCollisionSfx (AudioClip clip) {
		collisionSfxSource.clip = clip;
		collisionSfxSource.Stop();
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
