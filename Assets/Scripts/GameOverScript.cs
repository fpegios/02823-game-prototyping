using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour {

	private enum GameOverMenu {Restart, MainMenu};
	private GameOverMenu currentChoice;
	private Vector3 indicatorPosition;
	private Text text;
	private string mapSceneRelativePath = "Scenes/Other/MapScene";
	private string levelSceneRelativePath = "Scenes/Levels/";
	private SceneController sceneController;

	void Awake () {
		sceneController = FindObjectOfType<SceneController>();
		if(!sceneController)
			throw new UnityException("Scene Controller could not be found, ensure that it exists in the Persistent scene.");
	}

	void Start () {
		// initiaize current choice to restart
		currentChoice = GameOverMenu.Restart;
		// initialize indicator's position
		indicatorPosition = GameObject.Find("Indicator").transform.position;
	}
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)) {	
			if (currentChoice == GameOverMenu.Restart) {
				Debug.Log("RESTART THE LEVEL!");

				// hide the game over menu
				this.gameObject.SetActive(false);

				// get active scene name and reload it
				Scene activeScene = SceneManager.GetActiveScene();
				sceneController.FadeAndLoadScene(levelSceneRelativePath + activeScene.name);
			} else if (currentChoice == GameOverMenu.MainMenu) {
				Debug.Log("GO TO MAIN MENU!");

				// hide the game over menu and 
				this.gameObject.SetActive(false);
				
				// load map scene
				sceneController.FadeAndLoadScene(mapSceneRelativePath);
			}
        }

		if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow)) {
			if (currentChoice == GameOverMenu.Restart) {

				// move indicator down
				indicatorPosition.y = indicatorPosition.y - 90;
				GameObject.Find("Indicator").transform.position = indicatorPosition;

				// update text colors
				GameObject.Find("Restart").GetComponent<Text>().color = new Color32(0xFF, 0xF7, 0xD8, 0xFF);
				GameObject.Find("MainMenu").GetComponent<Text>().color = new Color32(0xDB, 0x9B, 0x00, 0xFF);

				// update current choice
				currentChoice = GameOverMenu.MainMenu;
			} else if (currentChoice == GameOverMenu.MainMenu) {

				// move indicator up
				indicatorPosition.y = indicatorPosition.y + 90;
				GameObject.Find("Indicator").transform.position = indicatorPosition;

				// update text colors
				GameObject.Find("Restart").GetComponent<Text>().color = new Color32(0xDB, 0x9B, 0x00, 0xFF);
				GameObject.Find("MainMenu").GetComponent<Text>().color = new Color32(0xFF, 0xF7, 0xD8, 0xFF);

				// update current choice
				currentChoice = GameOverMenu.Restart;
			}
		}
	}
}
