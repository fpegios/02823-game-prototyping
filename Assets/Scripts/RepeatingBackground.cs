using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatingBackground : MonoBehaviour {

	private SpriteRenderer spriteRenderer;
	private float groundHorizontalLength;
	private GameObject player;

	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
		groundHorizontalLength = spriteRenderer.bounds.size.x;
		player = GameObject.Find("Player");
	}
	
	void Update () {
		if (player.transform.position.x - transform.position.x > (groundHorizontalLength)) {
			RepositionBackground();
		}
	}

	private void RepositionBackground() {
		transform.position = new Vector3(transform.position.x + groundHorizontalLength *1.99f, transform.position.y, transform.position.z);
	}
}
