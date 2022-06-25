using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Protagonist : MonoBehaviour {
	NavMeshAgent agent;
	new SpriteRenderer renderer;
	Vector3 movement;
	[NonSerialized] public Transform controlBase;
	public Page page;

	void Start() {
		agent = GetComponent<NavMeshAgent>();
		agent.updateRotation = false;
		renderer = GetComponent<SpriteRenderer>();
		if(page == null)
			page = FindObjectOfType<Page>();
	}

	public void FaceTo(Vector3 position) {
		Vector3 euler = transform.rotation.eulerAngles;
		position -= transform.position;
		float eulerY = Mathf.Atan2(position.x, position.z);
		euler.y = eulerY * 180 / Mathf.PI - 180;
		transform.rotation = Quaternion.Euler(euler);
	}

	public void TeleportTo(Transform destination) {
		agent.enabled = false;
		transform.position = destination.position;
		agent.enabled = true;
	}

	public void OnMovement(InputValue value) {
		if(controlBase == null)
			return;
		Vector2 vec2 = value.Get<Vector2>();
		movement = Vector3.zero;
		movement += controlBase.right * vec2.x;
		movement += controlBase.forward * vec2.y;
		movement *= agent.speed;
		if(Mathf.Abs(vec2.x) > 0)
			renderer.flipX = vec2.x < 0;
	}

	void FixedUpdate() {
		var camera = page.storyboard?.soulCamera;
		if(camera != null)
			FaceTo(camera.transform.position);
		agent.velocity = movement;
	}
}
