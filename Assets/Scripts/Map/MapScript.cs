using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScript : MonoBehaviour {

	public int currentLevel;
	public int clickedLevel;
	public int maxAvailableLevel;
	public Vector3 currentPosition;
	public Vector3 targetPosition;
	public float playerSpeed;
	private SceneController sceneController;

	void Start () {
		Debug.Log("STARTED");

		sceneController = FindObjectOfType<SceneController> ();

		// set current level
		currentLevel = 1;

		// set max available level
		maxAvailableLevel = 3;
		
		// define player's speed
		playerSpeed = 0.05f;

		// get current level position
		currentPosition = GameObject.Find("Level_" + currentLevel).transform.position;

		// move player to the current level position
		GameObject.Find("Player").transform.position = currentPosition;

		// set target position
		targetPosition = currentPosition;
	}
	
	void Update () {

		// if target position is different from the current one
		// move the player gradually to it per frame
		if (targetPosition != currentPosition) {
			// update current position by moving it towards the targeted one
			currentPosition = Vector3.MoveTowards(currentPosition, targetPosition, playerSpeed);

			// update player position
			GameObject.Find("Player").transform.position = currentPosition;
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
				clickedLevel = int.Parse(hit.collider.name.Substring(hit.collider.name.Length-1, 1));
				
				// check if the clicked level is available
				if ( clickedLevel <= maxAvailableLevel) {
					// update current level
					currentLevel = clickedLevel;

					// update target position
					targetPosition = GameObject.Find("Level_" + currentLevel).transform.position;

					if(clickedLevel == 2){
						sceneController.FadeAndLoadScene("Scenes/Levels/Level1");
					}
				}
				
			}
        }
	}
}
