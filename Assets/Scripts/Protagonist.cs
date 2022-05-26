using UnityEngine;
using UnityEngine.InputSystem;

public class Protagonist : MonoBehaviour {
	new Rigidbody rigidbody;
	new SpriteRenderer renderer;
	public InputAction movement;
	public float speed = 2;

	void Start() {
		rigidbody = GetComponent<Rigidbody>();
		renderer = GetComponent<SpriteRenderer>();
		movement.Enable();
	}

	void UpdateMovement() {
		var movementInput = movement.ReadValue<float>();
		if(movementInput == 0)
			return;
		rigidbody.velocity = transform.right * (speed * movementInput);
		renderer.flipX = movementInput < 0;
	}

	void Update() {
		UpdateMovement();
	}
}
