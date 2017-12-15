using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatingBackground : MonoBehaviour {

	private SpriteRenderer spriteRenderer;
	private float groundHorizontalLength;
	private GameObject player;
	private bool respawnPositionChecked = false;

	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
		groundHorizontalLength = spriteRenderer.bounds.size.x;
		player = GameObject.Find("Player");
	}
	
	void Update () {
		if (!PlayerController.isRespawning) {
			if (player.transform.position.x - transform.position.x > (groundHorizontalLength)) {
				RepositionBackground();
			}
			respawnPositionChecked = false;
		} else {
			if (!respawnPositionChecked) {
				if (PlayerController.respawnPosition.x < transform.position.x - (1.5f * groundHorizontalLength)) {
					BackwardRepositionBackground();
				}
				respawnPositionChecked = true;
			}
		}
	}

	private void RepositionBackground() {
		transform.position = new Vector3(transform.position.x + groundHorizontalLength *1.99f, transform.position.y, transform.position.z);
	}

	private void BackwardRepositionBackground() {
		transform.position = new Vector3(transform.position.x - groundHorizontalLength *1.99f, transform.position.y, transform.position.z);
	}
}
