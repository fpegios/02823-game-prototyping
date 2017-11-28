using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingRock : MonoBehaviour {

	public AudioClip fallingRockSound;

	private void OnCollisionEnter2D(Collision2D collision) {
		if (collision.transform.CompareTag("Ground")) {
			SoundManager.instance.PlayEfx(fallingRockSound);
		}
	}
}
