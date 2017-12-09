using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	public float speed = 4.0f;
	public enum Direction {Left, Right};
 	public Direction direction = Direction.Left;
	private Rigidbody2D rb;
	private int directionValue;

	void Start () {
		rb = GetComponent<Rigidbody2D>();
        if (direction == Direction.Left)
        {
            directionValue = -1;

        }
        else if (direction == Direction.Right)
        {
            directionValue = 1;
        }
	}

	void FixedUpdate () {
		if (PlayerController.gameState == PlayerController.GameState.Play) {
			rb.velocity = new Vector3(directionValue * speed, 0, 0);
		} else {
			rb.velocity = Vector3.zero;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision) {
        // change direction on collision with enemy limits
        if (collision.transform.CompareTag("EnemyLimit"))
        {
            directionValue = -1 * directionValue;
            GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
        }
	}
}
