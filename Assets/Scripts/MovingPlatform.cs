using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

	public float frequency = 1.0f;
	public float amplitude = 3;

	public enum Direction {Horizontal, Vertical};
 	public Direction direction = Direction.Horizontal;
	private Vector3 initialPosition;
	private float oscillationTime = 0.0f;

	void Start () {
		initialPosition = transform.position;
	}

	void FixedUpdate () {
		if (PlayerController.gameState == PlayerController.GameState.Play) {
			oscillationTime += Time.deltaTime;
			// X = Xo + Asin(wt)
			if (direction == Direction.Horizontal) 
				transform.position = new Vector3(initialPosition.x + Mathf.Sin(oscillationTime * frequency) * amplitude, initialPosition.y, initialPosition.z);
			else if (direction == Direction.Vertical) 
				transform.position = new Vector3(initialPosition.x, initialPosition.y + Mathf.Sin(oscillationTime * frequency) * amplitude, initialPosition.z);
		}
	}
}
