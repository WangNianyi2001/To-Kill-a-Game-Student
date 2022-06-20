using UnityEngine;

public abstract class Controller<T> : MonoBehaviour where T : Object {
	public T target;

	public abstract void Step();

	void FixedUpdate() {
		Step();
	}
}
