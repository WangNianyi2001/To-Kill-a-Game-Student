using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class OneTimeTrigger : MonoBehaviour {
	public UnityEvent action;

	void OnTriggerEnter(Collider other) {
		action?.Invoke();
		Destroy(this);
	}
}
