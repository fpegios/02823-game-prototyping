using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalScript : MonoBehaviour {
	public LevelName level;
	private string mapSceneRelativePath = "Scenes/Other/MapScene";
	private SceneController sceneController;

	void Awake(){
        sceneController = FindObjectOfType<SceneController>();

        if(!sceneController)
            throw new UnityException("Scene Controller could not be found, ensure that it exists in the Persistent scene.");
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		SetLevelCompleteAndFadeToScene(level, mapSceneRelativePath);
	}

	private void SetLevelCompleteAndFadeToScene(LevelName level, string sceneRelativePath){
		StateController.stateController.CompletedLevels[level] = true;
		sceneController.FadeAndLoadScene(sceneRelativePath);
	}
}
