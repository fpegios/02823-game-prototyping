using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour {

	private enum PauseMenu {Resume, MainMenu};
	private PauseMenu currentChoice;
	private Vector3 indicatorPosition;
	private Text text;
	private string mapSceneRelativePath = "Scenes/Other/MapScene";
	private SceneController sceneController;
	private PlayerController playerController;

	void Awake () {
		sceneController = FindObjectOfType<SceneController>();
		if(!sceneController)
			throw new UnityException("Scene Controller could not be found, ensure that it exists in the Persistent scene.");
		
		playerController = FindObjectOfType<PlayerController>();
	}

	void Start () {
		// initiaize current choice to restart
		currentChoice = PauseMenu.Resume;
		// initialize indicator's position
		indicatorPosition = GameObject.Find("Indicator").transform.position;
	}
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)) {	
			if (currentChoice == PauseMenu.Resume) {
				// hide the pause menu
				this.gameObject.SetActive(false);
				playerController.ToggleGameState();
			} else if (currentChoice == PauseMenu.MainMenu) {

				// hide the pause menu 
				this.gameObject.SetActive(false);
				// load map scene
				sceneController.FadeAndLoadScene(mapSceneRelativePath);
			}
        }

		if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow)) {
			if (currentChoice == PauseMenu.Resume) {

				// move indicator down
				indicatorPosition.y = indicatorPosition.y - 90;
				GameObject.Find("Indicator").transform.position = indicatorPosition;

				// update text colors
				GameObject.Find("Resume").GetComponent<Text>().color = new Color32(0xFF, 0xF7, 0xD8, 0xFF);
				GameObject.Find("MainMenu").GetComponent<Text>().color = new Color32(0xDB, 0x9B, 0x00, 0xFF);

				// update current choice
				currentChoice = PauseMenu.MainMenu;
			} else if (currentChoice == PauseMenu.MainMenu) {

				// move indicator up
				indicatorPosition.y = indicatorPosition.y + 90;
				GameObject.Find("Indicator").transform.position = indicatorPosition;

				// update text colors
				GameObject.Find("Resume").GetComponent<Text>().color = new Color32(0xDB, 0x9B, 0x00, 0xFF);
				GameObject.Find("MainMenu").GetComponent<Text>().color = new Color32(0xFF, 0xF7, 0xD8, 0xFF);

				// update current choice
				currentChoice = PauseMenu.Resume;
			}
		}
	}
}
