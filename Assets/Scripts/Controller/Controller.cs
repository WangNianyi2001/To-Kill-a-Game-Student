using UnityEngine;

public abstract class Controller<T> : MonoBehaviour where T : Object {
	public T target;
	public virtual T Target {
		get => target;
		set => target = value;
	}

	public abstract void Step();

	void FixedUpdate() {
		Step();
	}
}
