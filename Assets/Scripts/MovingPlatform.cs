using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

	public float frequency = 1.0f;
	public float amplitude = 3;
	private Vector3 originalPosition;
	void Start () {
		originalPosition = transform.position;
	}
	
	void Update () {
		// X = Acos(wt) = Acos(2πf)
		transform.position = new Vector3(originalPosition.x + Mathf.Sin(Time.time * frequency) * amplitude, originalPosition.y, originalPosition.z);
	}
}
