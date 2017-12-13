using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalScript : MonoBehaviour {
	public int level;
	private string mapSceneRelativePath = "Scenes/Other/MapScene";
	private SceneController sceneController;

	void Awake(){
        sceneController = FindObjectOfType<SceneController>();

        if(!sceneController)
            throw new UnityException("Scene Controller could not be found, ensure that it exists in the Persistent scene.");
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.transform.gameObject.CompareTag("Player"))
			SetLevelCompleteAndFadeToScene(level, mapSceneRelativePath);		
	}

	private void SetLevelCompleteAndFadeToScene(int levelNo, string sceneRelativePath){
		StateController.instance.SetLevelCompletedAndUnlockNextLevel(levelNo);
		sceneController.FadeAndLoadScene(sceneRelativePath);
	}
}
