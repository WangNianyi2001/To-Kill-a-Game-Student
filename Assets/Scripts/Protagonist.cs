using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Protagonist : MonoBehaviour {
	const float eps = .01f;

	NavMeshAgent agent;
	Vector3 movement;
	[NonSerialized] public Transform controlBase;
	public Page page;
	new Animation2DManager animation;

	void Start() {
		agent = GetComponent<NavMeshAgent>();
		agent.updateRotation = false;
		if(page == null)
			page = FindObjectOfType<Page>();
		animation = GetComponentInChildren<Animation2DManager>();
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

	public bool grantControl {
		get => agent.enabled;
		set {
			agent.enabled = value;
			if(!value)
				movement = Vector3.zero;
		}
	}

	public void OnMovement(InputValue value) {
		if(!grantControl || controlBase == null)
			return;
		Vector2 vec2 = value.Get<Vector2>();
		movement = Vector3.zero;
		movement += controlBase.right * vec2.x;
		movement += controlBase.forward * vec2.y;
		movement *= agent.speed;

		if(Math.Abs(vec2.x) > eps) {
			animation.gameObject.transform.localScale = new Vector3(vec2.x < 0 ? -1 : 1, 1, 1);
		}
	}

	void OnRenderObject() {
		var camera = page.storyboard?.soulCamera;
		if(camera != null)
			FaceTo(camera.transform.position);
	}

	bool idle = true;
	void FixedUpdate() {
		agent.velocity = movement;
		bool newIdle = movement.magnitude <= eps;
		if(newIdle != idle) {
			idle = newIdle;
			animation.Pause();
			animation.Play(idle ? "Idle" : "Walking");
		}
	}
}
