﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	public float speed = 4.0f;
	private Rigidbody2D rb;
	private int[] directions = new int[] {-1, 1};
	private int direction;

	void Start () {
		rb = GetComponent<Rigidbody2D>();
		// get initial directionusing random function
		direction = directions[Random.Range( 0, 1 )];
	}
	
	void Update () {
		if (PlayerController.gameState == PlayerController.GameState.Play) {
			// update object's velocity per frame
			rb.velocity = new Vector3(direction * speed, 0, 0);
		} else {
			rb.velocity = Vector3.zero;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
    {	
		// change direction on collision with enemy limits
        if (collision.transform.CompareTag("EnemyLimit")) {
			direction = -1 * direction;
        }
	}
}
