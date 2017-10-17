using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScript : MonoBehaviour {

	void Start () {
		Debug.Log("STARTED");
	}
	
	void Update () {
		
		// Event on mouse click
		if (Input.GetMouseButtonDown (0)) {

			// get the clicked position
			Vector3 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			// get the hit sprite
			RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
			
			// Check if user clicked a sprite
			if (hit.collider != null) {
				Debug.Log ("You Clicked: " + hit.collider.name);

				// ... paste here the code for transition to another scene
			}
        }
	}
}
