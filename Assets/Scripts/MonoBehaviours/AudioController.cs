using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {

	public float fadeTime;
	public float startVolume;
	[Range(0, 1)]
	public float maxVolume;

	private AudioSource audioSource;

	private SceneController sceneController;
	void Awake()
	{
		sceneController = FindObjectOfType<SceneController>();
        if(!sceneController)
            throw new UnityException("Scene Controller could not be found, ensure that it exists in the Persistent scene.");	

		audioSource = GetComponentInChildren<AudioSource>();
		if(!audioSource)
		    throw new UnityException("Audio Source could not be found");
	}

	void Start() {
		FadeIn();
	}

	private void FadeIn(){
        audioSource.volume = 0;
        audioSource.Play();
 
        while (audioSource.volume < maxVolume)
        {       
            audioSource.volume += startVolume * Time.deltaTime / fadeTime;
        }
 
        audioSource.volume = maxVolume;	
	}
	
	void FadeOut(){
        float startVolume = audioSource.volume;
 
        while (audioSource.volume > 0) {
            audioSource.volume -= startVolume * Time.deltaTime / fadeTime;
         }
 
        audioSource.Stop ();
        audioSource.volume = startVolume;		
	}

	void OnEnable()
	{
		sceneController.BeforeSceneUnload += FadeOut;
		sceneController.AfterSceneLoad += FadeIn;
	}

	void OnDisable()
	{
		sceneController.BeforeSceneUnload -= FadeOut;
		sceneController.AfterSceneLoad -= FadeIn;
	}
}
