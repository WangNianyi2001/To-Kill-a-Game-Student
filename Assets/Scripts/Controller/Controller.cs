using System;
using UnityEngine;

public class Controller : MonoBehaviour {
	[NonSerialized] public Vector3 position;
	[NonSerialized] public Quaternion rotation;

	[Range(0, 5)]
	public float damping = 0;

	void Start() {
		position = transform.position;
		rotation = transform.rotation;
	}

	protected virtual void Update() {
		damping = Mathf.Clamp(damping, 0, Mathf.Infinity);
		if(damping < .01f) {
			transform.position = position;
			transform.rotation = rotation;
			return;
		}
		float alpha = 1 / (1 + damping);
		
		// Position
		Vector3 deltaPos = position - transform.position;
		transform.position += deltaPos * alpha;

		// Rotation
		float angle;
		Vector3 axis;
		Quaternion deltaRot = rotation * Quaternion.Inverse(transform.rotation);
		deltaRot.ToAngleAxis(out angle, out axis);
		transform.rotation *= Quaternion.AngleAxis(angle * alpha, axis);
	}
}
