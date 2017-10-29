﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScript : MonoBehaviour {


	// current level
	public int currentLevel = 1; // it is set to 1 for testing purposes

	// maximum available level
	public int maxAvailableLevel = 3; // it is set to 3 for testing purposes

	// selected level
	private int selectedLevel = 1;

	// clicked level
	// WARNING! It is used
	private int tempClickedLevel;

	// current position of the user
	// WARNING! it is used only for the smooth movement
	private Vector3 currentPosition;

	// player's speed
	private float playerSpeed = 0.05f;

	// scene controller object
	private SceneController sceneController;

	// boolean variable which shows if the player is moving
	private bool isPlayerMoving = false; 
	
	void Start () {
		Debug.Log("Map Scene Started!");

		// get scene controller object
		sceneController = FindObjectOfType<SceneController> ();

		// initialize player's position to the current level
		currentPosition = GameObject.Find("Level_" + currentLevel).transform.position;
		GameObject.Find("Player").transform.position = GameObject.Find("Level_" + currentLevel).transform.position;
	}
	
	void Update () {

		// if selected level is different from the current one
		// move the player gradually to the selected level per frame
		if (currentPosition != GameObject.Find("Level_" + selectedLevel).transform.position) {

			// update current position by moving it towards the selected level
			currentPosition = Vector3.MoveTowards(currentPosition, GameObject.Find("Level_" + selectedLevel).transform.position, playerSpeed);

			// update player position
			GameObject.Find("Player").transform.position = currentPosition;
		} else {
			// when the below condition is true, 
			// it means that the player has reached the selected level
			if (currentLevel != selectedLevel) {
				
				// update current level
				currentLevel = selectedLevel;

				Debug.Log("Transition to Level_" + currentLevel + "!");
				// sceneController.FadeAndLoadScene("Scenes/Levels/Level" + selectedLevel);
			}
		}
		
		// Event on mouse click
		if (Input.GetMouseButtonDown (0)) {

			// get the clicked position
			Vector3 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			// get the hit sprite
			RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
			
			// check if user clicked a sprite
			if (hit.collider != null) {

				// get clicked level
				tempClickedLevel = int.Parse(hit.collider.name.Substring(hit.collider.name.Length-1, 1));
				
				// check if the clicked level is available
				if (tempClickedLevel <= maxAvailableLevel) {

					// player is moving
					isPlayerMoving = true;

					// set the selected level
					selectedLevel = tempClickedLevel;

					Debug.Log("Start moving to Level_" + selectedLevel + "!");
				}
				
			}
        }
	}
}