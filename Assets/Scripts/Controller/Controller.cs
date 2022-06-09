using System;
using UnityEngine;

public class Controller : MonoBehaviour {
	[NonSerialized] public Vector3 position;
	[NonSerialized] public Quaternion rotation;

	[Range(0, 100)]
	public float positionDamping = 0;
	[Range(0, 100)]
	public float rotationDamping = 0;

	public void Jump() {
		transform.position = position;
		transform.rotation = rotation;
	}

	public virtual void Step() {
		transform.position = Vector3.Lerp(transform.position, position, 1 / (1 + positionDamping));
		transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 1 / (1 + rotationDamping));
	}

	void Start() {
		position = transform.position;
		rotation = transform.rotation;
	}

	void FixedUpdate() {
		Step();
	}
}
