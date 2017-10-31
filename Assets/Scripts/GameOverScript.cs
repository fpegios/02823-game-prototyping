using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScript : MonoBehaviour {

	private enum GameOverMenu {Restart, MainMenu};
	private GameOverMenu currentChoice;
	private Vector3 indicatorPosition;
	
	private Text text;
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
			} else if (currentChoice == GameOverMenu.MainMenu) {
				Debug.Log("GO TO MAIN MENU!");
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
