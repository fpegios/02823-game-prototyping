using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPlayerController : MonoBehaviour {

	private float playerSpeed = 0.05f;
	private SceneController sceneController;
	private bool isPlayerMoving;
	private int nextLevelNo;
	private Vector3 destination;

	private Vector3 currentPosition;

	#region Awake
	void Awake()
	{
        sceneController = FindObjectOfType<SceneController>();
        if(!sceneController)
            throw new UnityException("Scene Controller could not be found, ensure that it exists in the Persistent scene.");
	}
	#endregion
	void Start()
	{
		SetPlayerSpawnPosition();
	}
	private void SetPlayerSpawnPosition()
    {		
		var reachedLevel = StateController.instance.GetMaxReachedLevel();
		var spawnPosition = GetPlayerSpawnPosition(reachedLevel);
		GameObject.Find("Player").transform.position = spawnPosition;

		currentPosition = spawnPosition;
		destination = currentPosition;

		StateController.instance.PlayerPositionInMapScene = reachedLevel.LevelNo;
		nextLevelNo = StateController.instance.PlayerPositionInMapScene;    
	}

    private Vector3 GetPlayerSpawnPosition(Level reachedLevel)
    {
		return GameObject.Find(reachedLevel.GetLevelName()).transform.position;
    }

    void Update () {
		if(currentPosition != destination)
			UpdateAndSetPlayerPosition();
		else
			ToggleIsPlayerMovingAndSaveState();

		if(Input.anyKeyDown)
			HandleKeyPress();
	}

	private void UpdateAndSetPlayerPosition(){
		currentPosition = Vector3.MoveTowards(currentPosition, destination, playerSpeed);
		GameObject.Find("Player").transform.position = currentPosition;
	}
	private void ToggleIsPlayerMovingAndSaveState(){
		isPlayerMoving = false;
		StateController.instance.PlayerPositionInMapScene = nextLevelNo;
	}

    private void HandleKeyPress()
    {
		if(Input.GetKey(KeyCode.Space)){
			if(!isPlayerMoving)
				sceneController.FadeAndLoadScene("Scenes/Levels/Level" + StateController.instance.PlayerPositionInMapScene);
		}

		else if(Input.GetKey(KeyCode.RightArrow)){
			var reachedLevel = StateController.instance.GetMaxReachedLevel().LevelNo;
			if(StateController.instance.PlayerPositionInMapScene < reachedLevel){
				nextLevelNo = StateController.instance.PlayerPositionInMapScene + 1;
				SetNewPositionAndToggleIsPlayerMoving(nextLevelNo);
			}
		}

		else if(Input.GetKey(KeyCode.LeftArrow)){
			if(StateController.instance.PlayerPositionInMapScene > 1){
				nextLevelNo = StateController.instance.PlayerPositionInMapScene - 1;
				SetNewPositionAndToggleIsPlayerMoving(nextLevelNo);
			}
		}    
	}

	private void SetNewPositionAndToggleIsPlayerMoving(int nextLevelNo){
        destination = GameObject.Find("Level" + this.nextLevelNo).transform.position;
		isPlayerMoving = true;
	}
}
