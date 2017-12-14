using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapScript : MonoBehaviour {
	public float playerSpeed = 0.045f;
	private SceneController sceneController;
	private bool isPlayerMoving;
	private int nextLevelNo;
	private Vector3 destination;

	private Vector3 currentPosition;

	private Text levelName;

	private Text levelDescription;

	#region Awake
	void Awake()
	{
        sceneController = FindObjectOfType<SceneController>();
        if(!sceneController)
            throw new UnityException("Scene Controller could not be found, ensure that it exists in the Persistent scene.");

		levelName = GameObject.Find("LevelName").GetComponent<Text>();
		levelDescription = GameObject.Find("LevelDescription").GetComponent<Text>();

	}
	#endregion

	#region Start

	void Start () {
		ColorMilestones(StateController.instance.Levels);
		var reachedLevel = GetReachedLevel();
		SetPlayerSpawnPosition(reachedLevel);
		UpdateUiText(reachedLevel.LevelNo);
	}



    private Level GetReachedLevel(){
		if(StateController.instance.HasCompletedIntro)
			return StateController.instance.GetMaxReachedLevel();
		else
			return StateController.instance.Levels.Find(x => x.LevelNo == 0);
	}

	private void ColorMilestones(List<Level> levels)
    {
		foreach(var level in levels){
			if(level.IsComplete)
				ColorMilestone(level, new Color(0, 1, 0, 1));
			else if(level.IsLocked)
				ColorMilestone(level, new Color(0, 0, 1, 1));
		}
    }

    private void ColorMilestone(Level level, Color color)
	{
		var milestone = GameObject.Find(level.GetLevelName());
		var sr = milestone.GetComponent<SpriteRenderer>();
		sr.color = color;
	}

	private void SetPlayerSpawnPosition(Level reachedLevel)
    {
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
	#endregion
	
	#region Update
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
				UpdateUiText(nextLevelNo);
			}
		}

		else if(Input.GetKey(KeyCode.LeftArrow)){
			if(StateController.instance.PlayerPositionInMapScene > 0){
				nextLevelNo = StateController.instance.PlayerPositionInMapScene - 1;
				SetNewPositionAndToggleIsPlayerMoving(nextLevelNo);
				UpdateUiText(nextLevelNo);
			}
		}    
	}

    private void UpdateUiText(int levelNo)
    {
        switch(levelNo){
			case 0: levelName.text = "Intro week"; break;
			case 1: levelName.text = "First year"; break;
			case 2: levelName.text = "Second year"; break;
			case 3: levelName.text = "Third year"; break;
			case 4: levelName.text = "Fourth year"; break;
			case 5: levelName.text = "Fifth year"; break;
			case 6: levelName.text = "Congratulations! You graduated!"; break;
			default: throw new UnityException("No UI text matching level number");
		}
    }

    private void SetNewPositionAndToggleIsPlayerMoving(int nextLevelNo){
        destination = GameObject.Find("Level" + this.nextLevelNo).transform.position;
		isPlayerMoving = true;
	}
	#endregion
}
