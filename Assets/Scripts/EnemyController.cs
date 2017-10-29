using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	private Rigidbody2D rb;
	private int[] directions = new int[] {-1, 1};
	private int direction;
	
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		// get initial directionusing random function
		direction = directions[Random.Range( 0, 1 )];
	}
	
	void Update () {
		// update object's velocity per frame
		rb.velocity = new Vector3(direction * 4, 0, 0);
	}

	private void OnTriggerEnter2D(Collider2D collision)
    {	
		// change direction on collision with enemy limits
        if (collision.transform.CompareTag("EnemyLimit")) {
			direction = -1 * direction;
        }
	}
}
