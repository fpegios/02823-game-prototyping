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
<<<<<<< HEAD
		// X = Acos(wt) = Acos(2πf)
		transform.position = new Vector3(originalPosition.x + Mathf.Sin(Time.time * frequency) * amplitude, originalPosition.y, originalPosition.z);
=======
		// X = Xo + Asin(wt)
		transform.position = new Vector3(initialPosition.x + Mathf.Sin(Time.time * frequency) * amplitude, initialPosition.y, initialPosition.z);
>>>>>>> 3b355609dd09aebe9d32a7bd4a9070755f1ec0d4
	}
}
