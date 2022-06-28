using UnityEngine;

public abstract class Controller<T> : MonoBehaviour where T : Object {
	public T target = null;
	public virtual T Target {
		get => target;
		set => target = value;
	}
}
