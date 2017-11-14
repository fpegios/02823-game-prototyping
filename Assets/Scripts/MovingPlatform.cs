using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

	public float frequency = 1.0f;
	public float amplitude = 3;
	private Vector3 initialPosition;

	void Start () {
		initialPosition = transform.position;
	}
	
	void Update () {
		if (PlayerController.gameState == PlayerController.GameState.Play) {
			// X = Xo + Asin(wt)
			transform.position = new Vector3(initialPosition.x + Mathf.Sin(Time.time * frequency) * amplitude, initialPosition.y, initialPosition.z);
		} else {
			transform.position = Vector3.zero;
		}
	}
}
