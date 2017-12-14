using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : ScriptableObject {

	public Vector2 Velocity {get; private set;}
	public Vector3 Position {get; private set;}
	public bool IsGrounded;

	public void Init(Vector2 velocity, Vector3 position, bool isGrounded){
		this.Velocity = velocity;
		this.Position = position;
		this.IsGrounded = isGrounded;
	}

	override
	public string ToString(){
		return string.Format("{0}, {1}, {2}, {3}, {4}", Velocity.x, Velocity.y, Position.x, Position.y, Position.z);
	}
}
